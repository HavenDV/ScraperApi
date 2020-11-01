using System.Net.Http;
using System.Net.Http.Headers;

namespace ScraperApi
{
    /// <summary>
    /// Class providing methods for API access.
    /// </summary>
    public partial class ScraperApi
    {
        /// <summary>
        /// Sets the selected token as a default header for the HttpClient.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="httpClient"></param>
        public ScraperApi(string token, HttpClient httpClient) : this(httpClient)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}