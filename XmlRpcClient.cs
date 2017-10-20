using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Linq;

namespace Topdev.OpenSubtitles
{
    public class XmlRpcClient
    {
        private HttpClient _httpClient = new HttpClient();
        private readonly string _xmlRpcEndpointURL;

        public XmlRpcClient(string xmlRpcEndpointURL)
        {
            _xmlRpcEndpointURL = xmlRpcEndpointURL;
        }

        public T Invoke<T>(string methodName, params object[] parameters)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<methodCall>");
            sb.Append($"<methodName>{methodName}</methodName>");

            sb.Append("<params>");
            foreach (object obj in parameters)
            {
                sb.Append(SerializeParameter(obj));
            }
            sb.Append("</params></methodCall>");

            StringContent content = new StringContent(sb.ToString());

            HttpResponseMessage response = _httpClient.PostAsync(_xmlRpcEndpointURL, content).Result;
            if (response.IsSuccessStatusCode)
            {
                string responseString = response.Content.ReadAsStringAsync().Result;
                T o = DeserializeResponse<T>(responseString);
                return o;
            }
            else
            {
                throw new HttpRequestException(response.Content.ReadAsStringAsync().Result);
            }
        }

        private T DeserializeResponse<T>(string response)
        {
            XElement xmlElement = XElement.Load(new StringReader(response));

            XElement value = xmlElement.Element("params")
                .Element("param")
                .Element("value")
                .Elements()
                .First();

            return (T)ValueElement(value, typeof(T));
        }

        private string SerializeParameter(object obj)
        {
            return "<param><value>" + SerializeValue(obj) + "</value></param>";
        }

        private object ValueElement(XElement valueElement, Type type)
        {
            switch (valueElement.Name.LocalName)
            {
                case "string": return valueElement.Value;
                case "int": return Int32.Parse(valueElement.Value);
                case "struct": return DeserializeStruct(valueElement, type);
                case "array": return DeserializeArray(valueElement, type);
                default: throw new Exception("Unknown value type.");
            }
        }

        private object DeserializeStruct(XElement structElement, Type type)
        {
            IEnumerable<XElement> members = structElement.Elements("member");
            PropertyInfo[] props = type.GetProperties();
            object o = Activator.CreateInstance(type);

            foreach (XElement member in members)
            {
                string memberName = member.Element("name").Value;
                XElement memberValue = member.Element("value");
                XElement valueChild = memberValue.Elements().First();
                string valueType = valueChild.Name.LocalName;

                PropertyInfo prop;

                prop = props.FirstOrDefault(x => x.GetCustomAttributes<DataMemberAttribute>().Any(y => y.Name == memberName));

                if (prop == null)
                    prop = props.FirstOrDefault(x => x.Name == memberName);

                if (prop == null) // skip to next member
                    continue;

                switch (valueType)
                {
                    case "string": prop.SetValue(o, valueChild.Value); break;
                    case "int": prop.SetValue(o, Int32.Parse(valueChild.Value)); break;
                    case "double": prop.SetValue(o, Double.Parse(valueChild.Value, System.Globalization.CultureInfo.InvariantCulture)); break;
                    case "struct": prop.SetValue(o, DeserializeStruct(valueChild, prop.PropertyType)); break;
                    case "array": prop.SetValue(o, DeserializeArray(valueChild, prop.PropertyType)); break;
                }
            }

            return o;
        }

        private object DeserializeArray(XElement arrayElement, Type type)
        {
            List<XElement> arrayValues = arrayElement.Element("data").Elements("value").ToList();
            Type arrayItemType = type.GetElementType();
            Array array = Array.CreateInstance(arrayItemType, arrayValues.Count);

            for (int i = 0; i < arrayValues.Count; i++)
            {
                object arrayValur = ValueElement(arrayValues[i].Elements().First(), arrayItemType);
                array.SetValue(arrayValur, i);
            }

            return array;
        }

        private string SerializeValue(object o)
        {
            if (o.GetType().Namespace.StartsWith("System"))
            {
                string element = GetTypeElement(o.GetType());
                return $"<{element}>{o.ToString()}</{element}>";
            }
            else if (o.GetType().IsArray)
            {
                string arrayString = "<array><data>";
                foreach (object x in (IEnumerable<object>)o)
                {
                    arrayString += "<value>";
                    arrayString += SerializeComplexParameter(x);
                    arrayString += "</value>";
                }
                arrayString += "</data></array>";

                return arrayString;
            }
            else
            {

                return SerializeComplexParameter(o);
            }
        }

        private string GetTypeElement(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.String:
                    return "string";
                case TypeCode.Int32:
                    return "int";
                case TypeCode.Double:
                    return "double";
                default:
                    throw new Exception("Unknown simple type element");
            }
        }

        private string SerializeComplexParameter(object obj)
        {
            Type type = obj.GetType();
            PropertyInfo[] props = type.GetProperties();
            string str = "<struct>";

            foreach (PropertyInfo prop in props)
            {
                str += "<member>";
                Attribute attr = prop.GetCustomAttribute(typeof(DataMemberAttribute), true);
                string propertyName = attr == null ? prop.Name : ((DataMemberAttribute)attr).Name;
                str += "<name>" + propertyName + "</name>";
                str += "<value>" + SerializeValue(prop.GetValue(obj, null)) + "</value>";
                str += "</member>";
            }

            str += "</struct>";
            return str;
        }
    }
}