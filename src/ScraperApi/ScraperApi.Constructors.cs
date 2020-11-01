using System;
using System.Net.Http;
using System.Text;

namespace ScraperApi
{
    /// <summary>
    /// Class providing methods for API access.
    /// </summary>
    public partial class ScraperApi
    {
        #region Properties

        private string? ApiKey { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Sets the selected token as a default header for the HttpClient.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="httpClient"></param>
        public ScraperApi(string apiKey, HttpClient httpClient) : this(httpClient)
        {
            ApiKey = apiKey;
        }

        #endregion

        #region Methods

        // ReSharper disable UnusedParameterInPartialMethod
        partial void PrepareRequest(HttpClient client, HttpRequestMessage request, StringBuilder urlBuilder)
        {
            var apiKey = ApiKey ?? throw new InvalidOperationException("ApiKey is null");

            urlBuilder.Append(urlBuilder.ToString().Contains("?") 
                ? "&" 
                : "?");
            urlBuilder.Append($"api_key={Uri.EscapeDataString(apiKey)}");
        }

        #endregion
    }
}