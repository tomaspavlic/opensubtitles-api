using System.Runtime.Serialization;

namespace Topdev.OpenSubtitles.Client
{
    public class SearchSubtitlesResponse
    {
        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "data")]
        public Subtitles[] Data { get; set; }
    }
}