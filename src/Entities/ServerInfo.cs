using System.Runtime.Serialization;

namespace Topdev.OpenSubtitles.Client
{
    [DataContract]
    public class ServerInfo
    {
        [DataMember(Name = "xmlrpc_version")]
        public string XmlRpcVersion { get; set; }
    }
}