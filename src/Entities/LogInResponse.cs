using System.Runtime.Serialization;

namespace Topdev.OpenSubtitles.Client
{
    [DataContract]
    public class LogInResponse : Response
    {
        [DataMember(Name = "token")]
        public string Token { get; set; }

        [DataMember(Name = "seconds")]
        public double Seconds { get; set; }
    }
}