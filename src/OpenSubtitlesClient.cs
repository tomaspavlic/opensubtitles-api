using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Topdev.OpenSubtitles.Client.Tests")]
namespace Topdev.OpenSubtitles.Client
{    
    public class OpenSubtitlesClient
    {
        private readonly XmlRpcClient _rcpClient;
        private string _token;

        public OpenSubtitlesClient()
        {
            _rcpClient = new XmlRpcClient("http://api.opensubtitles.org:80/xml-rpc");
        }

        public async Task LogInAsync(string language, string agent, string username, string password)
        {
            var logIn = await _rcpClient.InvokeAsync<LogInResponse>("LogIn", username, password, language, agent);
            _token = logIn.Token;
        }

        internal async Task<Subtitles[]> SearchSubtitlesAsync(
            string languages, 
            string movieHash = "",  
            string query = "", 
            string imdbId = "", 
            string tag = "")
        {
            var searchRequests = new SearchSubtitlesRequest[] {
                new SearchSubtitlesRequest() {
                    SublanguageId = languages,
                    MovieHash = movieHash,
                    Query = query,
                    IMDBId = imdbId,
                    Tag = tag }
            };

            if (_token == null)
                throw new Exception("You are not logged in.");

            var search = await _rcpClient.InvokeAsync<SearchSubtitlesResponse>("SearchSubtitles", _token, searchRequests);

            return search.Data;
        }

        /// <summary>
        /// Downloads subtitles from opensubtitles and save it with same name as file input.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileName"></param>
        /// <param name="subtitleExtension"></param>
        public async Task DownloadSubtitleAsync(Subtitles sub, string subtitleFilePath = null)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStreamAsync(sub.SubDownloadLink);

            try
            {
                using (var fs = new FileStream(subtitleFilePath ?? sub.SubFileName, FileMode.Create, FileAccess.Write))
                using (var gzip = new GZipStream(response, CompressionMode.Decompress))
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

        public Task<Subtitles[]> FindSubtitlesAsync(SearchMethod method, string searchValue, string language)
        {
            switch (method)
            {
                case SearchMethod.MovieHash:
                    var movieHash = MovieHasher.ComputeMovieHash(searchValue);
                    return SearchSubtitlesAsync(language, MovieHasher.ToHexadecimal(movieHash));
                case SearchMethod.Query:
                    return SearchSubtitlesAsync(language, string.Empty, searchValue);
                case SearchMethod.IMDBId:
                    return SearchSubtitlesAsync(language, string.Empty, string.Empty, searchValue);
                case SearchMethod.Tag:
                    return SearchSubtitlesAsync(language, string.Empty, string.Empty, string.Empty, searchValue);
                default: return null;
            }
        }
    }
}