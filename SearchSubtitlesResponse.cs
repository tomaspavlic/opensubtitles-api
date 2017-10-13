using System.Runtime.Serialization;

namespace Topdev.OpenSubtitles
{
    public class SearchSubtitlesResponse
    {
        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "data")]
        public SearchSubtitles[] Data { get; set; }
    }
}