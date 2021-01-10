using System.Runtime.Serialization;

namespace Topdev.OpenSubtitles.Client
{
    public class SearchSubtitlesResponse : Response
    {
        [DataMember(Name = "data")]
        public Subtitles[] Data { get; set; }
    }
}