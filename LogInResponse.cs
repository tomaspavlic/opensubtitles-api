using System.Runtime.Serialization;

namespace Topdev.OpenSubtitles
{
    [DataContract]
    public class LogInResponse
    {
        [DataMember(Name = "token")]
        public string Token { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "seconds")]
        public double Seconds { get; set; }
    }
}