using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Topdev.OpenSubtitles.Client.Tests
{
    [TestClass]
    public class XmlRcpSerializerTests
    {
        [TestMethod]
        public void TestSerializeStringParameter()
        {
            var stringParameter = XmlRpcSerializer.SerializeParameter("asd");
            Assert.AreEqual("<param><value><string>asd</string></value></param>", stringParameter);
        }

        [TestMethod]
        public void TestSerializeIntParameter()
        {
            var intParameter = XmlRpcSerializer.SerializeParameter(123);
            Assert.AreEqual("<param><value><int>123</int></value></param>", intParameter);
        }

        [TestMethod]
        public void TestSerializeDoubleParameter()
        {
            var doubleParameter = XmlRpcSerializer.SerializeParameter(123.2);
            Assert.AreEqual("<param><value><double>123.2</double></value></param>", doubleParameter);
        }

        [TestMethod]
        public void TestSerializeComplexParameter()
        {
            var dummy = new ServerInfo()
            {
                XmlRpcVersion = "1.0.0"
            };
            var complexParameter = XmlRpcSerializer.SerializeParameter(dummy);
            Assert.AreEqual(@"<param><value><struct><member><name>xmlrpc_version</name><value><string>1.0.0</string></value></member></struct></value></param>", 
                complexParameter);
        }

        [TestMethod]
        public void TestSerializeArrayParameter()
        {
            var arrayParameter = XmlRpcSerializer.SerializeParameter(new int[] { 1, 2 });
            Assert.AreEqual(@"<param><value><array><data><value><int>1</int></value><value><int>2</int></value></data></array></value></param>", 
                arrayParameter);
        }

        [TestMethod]
        public void TestSerializeUnknownParameter() 
        {
            Assert.ThrowsException<UnknownTypeException>(() => {
                var floatParameter = XmlRpcSerializer.SerializeParameter(123.2F);
            });
        }
    }
}
