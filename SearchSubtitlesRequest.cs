using System.Runtime.Serialization;

namespace Topdev.OpenSubtitles
{
    [DataContract]
    public class SearchSubtitlesRequest
    {
        [DataMember(Name = "sublanguageid")]
        public string SublanguageId { get; set; }

        [DataMember(Name = "moviehash")]
        public string MovieHash { get; set; }

        [DataMember(Name = "moviebytesize")]
        public double MoevieByteSize { get; set; }

        [DataMember(Name = "query")]
        public string Query { get; set; }
        
        [DataMember(Name = "imdbid")]
        public string IMDBId { get; set; }
        
        [DataMember(Name = "tag")]
        public string Tag { get; set; }
    }
}