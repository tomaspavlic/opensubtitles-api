using System.Net.Http;
using System.Threading.Tasks;

namespace Topdev.OpenSubtitles.Client
{
    public class XmlRpcClient
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _xmlRpcEndpointURL;

        public XmlRpcClient(string xmlRpcEndpointURL)
        {
            _xmlRpcEndpointURL = xmlRpcEndpointURL;
        }

        public async Task<T> InvokeAsync<T>(string methodName, params object[] parameters) where T : Response
        {
            var methodCall = XmlRpcSerializer.SerializeRequest(methodName, parameters);
            var content = new StringContent(methodCall);

            var response = await _httpClient.PostAsync(_xmlRpcEndpointURL, content);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var o = XmlRpcSerializer.DeserializeResponse<T>(responseString);

                if (o.Status != "200 OK")
                    throw new RpcException(o.Status);

                return o;
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(error);
            }
        }
    }
}