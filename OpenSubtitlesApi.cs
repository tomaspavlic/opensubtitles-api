using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;

namespace Topdev.OpenSubtitles
{
    public class OpenSubtitlesApi
    {
        private XmlRpcClient _rcpClient;
        private string _token;

        public OpenSubtitlesApi()
        {
            _rcpClient = new XmlRpcClient("http://api.opensubtitles.org:80/xml-rpc");
        }

        public void LogIn(string language, string agent)
        {
            LogInResponse logIn = _rcpClient.Invoke<LogInResponse>("LogIn", string.Empty, string.Empty, language, agent);
            _token = logIn.Token;
        }

        private Subtitles[] SearchSubtitles(string movieHash = "", string languages = "", string query = "")
        {
            SearchSubtitlesRequest[] searchRequests = new SearchSubtitlesRequest[] {
                new SearchSubtitlesRequest() {
                    SublanguageId = languages,
                    MovieHash = movieHash,
                    Query = query }
            };

            SearchSubtitlesResponse search = _rcpClient.Invoke<SearchSubtitlesResponse>("SearchSubtitles", _token, searchRequests);

            return search.Data;
        }

        /// <summary>
        /// Downloads subtitles from opensubtitles and save it with same name as file input.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileName"></param>
        /// <param name="subtitleExtension"></param>
        public void DownloadSubtitle(Subtitles sub, string subtitleFilePath = null)
        {
            HttpClient httpClient = new HttpClient();
            Stream response = httpClient.GetStreamAsync(sub.SubDownloadLink).Result;
            try
            {
                using (FileStream fs = new FileStream(subtitleFilePath ?? sub.SubFileName, FileMode.Create, FileAccess.Write))
                using (GZipStream gzip = new GZipStream(response, CompressionMode.Decompress))
                {
                    gzip.CopyTo(fs);
                    gzip.Flush();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Can not save file.");
            }
        }

        public Subtitles[] FindSubtitles(SearchMethod method, string file, string language)
        {
            switch (method)
            {
                case SearchMethod.MovieHash:
                    byte[] movieHash = MovieHasher.ComputeMovieHash(file);
                    return SearchSubtitles(MovieHasher.ToHexadecimal(movieHash), language);
                case SearchMethod.Query:
                    return SearchSubtitles("", language, file);
                default: return null;
            }
        }
    }
}