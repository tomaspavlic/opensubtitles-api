using System.Runtime.Serialization;

namespace Topdev.OpenSubtitles.Client
{
    public class Response
    {
        [DataMember(Name = "status")]
        public string Status { get; set; }
    }
}