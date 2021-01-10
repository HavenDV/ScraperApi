using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace ScraperApi
{
    /// <summary>
    /// Class providing methods for API access.
    /// </summary>
    public partial class ScraperApiClient
    {
        #region Constants

        private const string Sdk = "csharp";

        #endregion

        #region Static methods

        private static string MakeProxyUserName(
            bool? render,
            string? countryCode,
            long? sessionNumber,
            bool? keepHeaders,
            bool? premium,
            Device_type? deviceType = null,
            bool? autoParse = null)
        {
            var value = "scraperapi.scraper_sdk=csharp";
            value = render == true ? value + ".render=true" : value;
            value = countryCode != null ? value + $".country_code={countryCode}" : value;
            value = sessionNumber != null ? value + $".session_number={sessionNumber}" : value;
            value = keepHeaders == true ? value + ".keep_headers=true" : value;
            value = premium == true ? value + ".premium=true" : value;
            value = deviceType != null ? value + ".device_type=mobile" : value;
            value = autoParse == true ? value + ".autoparse=true" : value;

            return value;
        }

        /// <summary>
        /// Returns the scraperapi.com web proxy.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="render">If you are crawling a page that requires you to render the javascript on the page, we can fetch these pages using a headless browser. This feature is only available on the Business and Enterprise plans. To render javascript, simply set render=true and we will use a headless Google Chrome instance to fetch the page.</param>
        /// <param name="keepHeaders">If you would like to keep the original request headers in order to pass through custom headers (user agents, cookies, etc.), simply set keep_headers=true. Only use this feature in order to get customized results, do not use this feature in order to avoid blocks, we handle that internally.</param>
        /// <param name="sessionNumber">To reuse the same proxy for multiple requests, simply use the &amp;session_number= flag (e.g. session_number=123). The value of session can be any integer, simply send a new integer to create a new session (this will allow you to continue using the same proxy for each request with that session number). Sessions expire 15 minutes after the last usage.</param>
        /// <param name="countryCode">To ensure your requests come from the United States, please use the country_code= flag (e.g. country_code=us). United States (us) geotargeting is available on the Startup plan and higher. Business plan customers also have access to Canada (ca), United Kingdom (uk), Germany (de), France (fr), Spain (es), Brazil (br), Mexico (mx), India (in), Japan (jp), China (cn), and Australia (au). Other countries are available to Enterprise customers upon request.</param>
        /// <param name="premium">Our standard proxy pools include millions of proxies from over a dozen ISPs, and should be sufficient for the vast majority of scraping jobs. However, for a few particularly difficult to scrape sites, we also maintain a private internal pool of residential and mobile IPs. This pool is only available to users on the Business plan or higher. Requests through our premium residential and mobile pool are charged at 10 times the normal rate (every successful request will count as 10 API calls against your monthly limit), each request that uses both rendering javascript and our premium pool will be charged at 25 times the normal rate (every successful request will count as 25 API calls against your monthly limit). To send a request through our premium proxy pool, please use the premium=true flag.</param>
        /// <param name="deviceType">Undocumented.</param>
        /// <param name="autoParse">Undocumented.</param>
        /// <returns></returns>
        public static IWebProxy GetProxy(
            string apiKey,
            bool? render = null,
            bool? keepHeaders = null,
            long? sessionNumber = null,
            string? countryCode = null,
            bool? premium = null,
            Device_type? deviceType = null,
            bool? autoParse = null)
        {
            return new WebProxy
            {
                Address = new Uri("http://proxy-server.scraperapi.com:8001/"),
                Credentials = new NetworkCredential(MakeProxyUserName(render, countryCode, sessionNumber, keepHeaders, premium, deviceType, autoParse), apiKey),
            };
        }

        /// <summary>
        /// Returns http client handler with the scraperapi.com proxy.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="render">If you are crawling a page that requires you to render the javascript on the page, we can fetch these pages using a headless browser. This feature is only available on the Business and Enterprise plans. To render javascript, simply set render=true and we will use a headless Google Chrome instance to fetch the page.</param>
        /// <param name="keepHeaders">If you would like to keep the original request headers in order to pass through custom headers (user agents, cookies, etc.), simply set keep_headers=true. Only use this feature in order to get customized results, do not use this feature in order to avoid blocks, we handle that internally.</param>
        /// <param name="sessionNumber">To reuse the same proxy for multiple requests, simply use the &amp;session_number= flag (e.g. session_number=123). The value of session can be any integer, simply send a new integer to create a new session (this will allow you to continue using the same proxy for each request with that session number). Sessions expire 15 minutes after the last usage.</param>
        /// <param name="countryCode">To ensure your requests come from the United States, please use the country_code= flag (e.g. country_code=us). United States (us) geotargeting is available on the Startup plan and higher. Business plan customers also have access to Canada (ca), United Kingdom (uk), Germany (de), France (fr), Spain (es), Brazil (br), Mexico (mx), India (in), Japan (jp), China (cn), and Australia (au). Other countries are available to Enterprise customers upon request.</param>
        /// <param name="premium">Our standard proxy pools include millions of proxies from over a dozen ISPs, and should be sufficient for the vast majority of scraping jobs. However, for a few particularly difficult to scrape sites, we also maintain a private internal pool of residential and mobile IPs. This pool is only available to users on the Business plan or higher. Requests through our premium residential and mobile pool are charged at 10 times the normal rate (every successful request will count as 10 API calls against your monthly limit), each request that uses both rendering javascript and our premium pool will be charged at 25 times the normal rate (every successful request will count as 25 API calls against your monthly limit). To send a request through our premium proxy pool, please use the premium=true flag.</param>
        /// <param name="deviceType">Undocumented.</param>
        /// <param name="autoParse">Undocumented.</param>
        /// <returns></returns>
        public static HttpClientHandler GetProxyHttpClientHandler(
            string apiKey,
            bool? render = null,
            bool? keepHeaders = null,
            long? sessionNumber = null,
            string? countryCode = null,
            bool? premium = null,
            Device_type? deviceType = null,
            bool? autoParse = null)
        {
            return new HttpClientHandler
            {
                Proxy = GetProxy(apiKey, render, keepHeaders, sessionNumber, countryCode, premium, deviceType, autoParse),
#if !NET45
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true,
#endif
            };
        }

        /// <summary>
        /// Returns http client with the scraperapi.com proxy.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="render">If you are crawling a page that requires you to render the javascript on the page, we can fetch these pages using a headless browser. This feature is only available on the Business and Enterprise plans. To render javascript, simply set render=true and we will use a headless Google Chrome instance to fetch the page.</param>
        /// <param name="keepHeaders">If you would like to keep the original request headers in order to pass through custom headers (user agents, cookies, etc.), simply set keep_headers=true. Only use this feature in order to get customized results, do not use this feature in order to avoid blocks, we handle that internally.</param>
        /// <param name="sessionNumber">To reuse the same proxy for multiple requests, simply use the &amp;session_number= flag (e.g. session_number=123). The value of session can be any integer, simply send a new integer to create a new session (this will allow you to continue using the same proxy for each request with that session number). Sessions expire 15 minutes after the last usage.</param>
        /// <param name="countryCode">To ensure your requests come from the United States, please use the country_code= flag (e.g. country_code=us). United States (us) geotargeting is available on the Startup plan and higher. Business plan customers also have access to Canada (ca), United Kingdom (uk), Germany (de), France (fr), Spain (es), Brazil (br), Mexico (mx), India (in), Japan (jp), China (cn), and Australia (au). Other countries are available to Enterprise customers upon request.</param>
        /// <param name="premium">Our standard proxy pools include millions of proxies from over a dozen ISPs, and should be sufficient for the vast majority of scraping jobs. However, for a few particularly difficult to scrape sites, we also maintain a private internal pool of residential and mobile IPs. This pool is only available to users on the Business plan or higher. Requests through our premium residential and mobile pool are charged at 10 times the normal rate (every successful request will count as 10 API calls against your monthly limit), each request that uses both rendering javascript and our premium pool will be charged at 25 times the normal rate (every successful request will count as 25 API calls against your monthly limit). To send a request through our premium proxy pool, please use the premium=true flag.</param>
        /// <param name="deviceType">Undocumented.</param>
        /// <param name="autoParse">Undocumented.</param>
        /// <returns></returns>
        public static HttpClient GetProxyHttpClient(
            string apiKey,
            bool? render = null,
            bool? keepHeaders = null,
            long? sessionNumber = null,
            string? countryCode = null,
            bool? premium = null,
            Device_type? deviceType = null,
            bool? autoParse = null)
        {
            return new HttpClient(
                GetProxyHttpClientHandler(apiKey, render, keepHeaders, sessionNumber, countryCode, premium, deviceType, autoParse), 
                true);
        }

        #endregion

        #region Properties

        private string? ApiKey { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Sets the selected token as a default header for the HttpClient.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="httpClient"></param>
        public ScraperApiClient(string apiKey, HttpClient httpClient) : this(httpClient)
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

            if (request.Headers.TryGetValues("scraper_api_post_put_body", out var bodyValues))
            {
                var json = bodyValues.FirstOrDefault();
                if (json != null && !string.IsNullOrWhiteSpace(json))
                {
                    var formData = JsonConvert.DeserializeObject<IEnumerable<KeyValuePair<string, string>>>(json);

                    request.Headers.Remove("scraper_api_post_put_body");

                    request.Content.Dispose();
                    request.Content = new FormUrlEncodedContent(formData);
                }
            }

            if (request.Headers.TryGetValues("scraper_api_headers", out var headersValues))
            {
                var json = headersValues.FirstOrDefault();
                if (json != null && !string.IsNullOrWhiteSpace(json))
                {
                    var headers = JsonConvert.DeserializeObject<IEnumerable<KeyValuePair<string, string>>>(json);

                    request.Headers.Remove("scraper_api_headers");
                    foreach (var pair in headers)
                    {
                        request.Headers.TryAddWithoutValidation(pair.Key, pair.Value);
                    }
                }
            }
        }

        /// <summary>Scrapes the url.</summary>
        /// <param name="url">The url you would like to scrape.</param>
        /// <param name="render">If you are crawling a page that requires you to render the javascript on the page, we can fetch these pages using a headless browser. This feature is only available on the Business and Enterprise plans. To render javascript, simply set render=true and we will use a headless Google Chrome instance to fetch the page.</param>
        /// <param name="keepHeaders">If you would like to keep the original request headers in order to pass through custom headers (user agents, cookies, etc.), simply set keep_headers=true. Only use this feature in order to get customized results, do not use this feature in order to avoid blocks, we handle that internally.</param>
        /// <param name="headers">If you would like to keep the original request headers in order to pass through custom headers (user agents, cookies, etc.), simply set keep_headers=true. Only use this feature in order to get customized results, do not use this feature in order to avoid blocks, we handle that internally.</param>
        /// <param name="sessionNumber">To reuse the same proxy for multiple requests, simply use the &amp;session_number= flag (e.g. session_number=123). The value of session can be any integer, simply send a new integer to create a new session (this will allow you to continue using the same proxy for each request with that session number). Sessions expire 15 minutes after the last usage.</param>
        /// <param name="countryCode">To ensure your requests come from the United States, please use the country_code= flag (e.g. country_code=us). United States (us) geotargeting is available on the Startup plan and higher. Business plan customers also have access to Canada (ca), United Kingdom (uk), Germany (de), France (fr), Spain (es), Brazil (br), Mexico (mx), India (in), Japan (jp), China (cn), and Australia (au). Other countries are available to Enterprise customers upon request.</param>
        /// <param name="premium">Our standard proxy pools include millions of proxies from over a dozen ISPs, and should be sufficient for the vast majority of scraping jobs. However, for a few particularly difficult to scrape sites, we also maintain a private internal pool of residential and mobile IPs. This pool is only available to users on the Business plan or higher. Requests through our premium residential and mobile pool are charged at 10 times the normal rate (every successful request will count as 10 API calls against your monthly limit), each request that uses both rendering javascript and our premium pool will be charged at 25 times the normal rate (every successful request will count as 25 API calls against your monthly limit). To send a request through our premium proxy pool, please use the premium=true flag.</param>
        /// <param name="deviceType">Undocumented.</param>
        /// <param name="autoParse">Undocumented.</param>
        /// <param name="proxyMode">https://www.scraperapi.com/documentation#proxy-mode</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Text response.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async System.Threading.Tasks.Task<string> GetAsync(
            string url, 
            bool? render = null,
            IEnumerable<KeyValuePair<string, string>>? headers = null,
            bool? keepHeaders = null,
            long? sessionNumber = null, 
            string? countryCode = null, 
            bool? premium = null,
            Device_type? deviceType = null,
            bool? autoParse = null,
            bool proxyMode = false,
            CancellationToken cancellationToken = default)
        {
            keepHeaders = headers != null ? true : keepHeaders;
            if (proxyMode)
            {
                var apiKey = ApiKey ?? throw new InvalidOperationException("ApiKey is null");

                using var client = GetProxyHttpClient(apiKey, render, keepHeaders, sessionNumber, countryCode, premium, deviceType, autoParse);
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                foreach (var pair in headers ?? new List<KeyValuePair<string, string>>())
                {
                    request.Headers.TryAddWithoutValidation(pair.Key, pair.Value);
                }
                var response = await client.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }

            var headersJson = headers != null ? JsonConvert.SerializeObject(headers.ToList()) : null;

            return await GetCoreAsync(url, render, keepHeaders, sessionNumber, countryCode, premium, deviceType, Sdk, autoParse, headersJson, cancellationToken).ConfigureAwait(false);
        }

        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Scrapes the url.</summary>
        /// <param name="url">The url you would like to scrape.</param>
        /// <param name="formData">Form data</param>
        /// <param name="render">If you are crawling a page that requires you to render the javascript on the page, we can fetch these pages using a headless browser. This feature is only available on the Business and Enterprise plans. To render javascript, simply set render=true and we will use a headless Google Chrome instance to fetch the page.</param>
        /// <param name="keepHeaders">If you would like to keep the original request headers in order to pass through custom headers (user agents, cookies, etc.), simply set keep_headers=true. Only use this feature in order to get customized results, do not use this feature in order to avoid blocks, we handle that internally.</param>
        /// <param name="headers">If you would like to keep the original request headers in order to pass through custom headers (user agents, cookies, etc.), simply set keep_headers=true. Only use this feature in order to get customized results, do not use this feature in order to avoid blocks, we handle that internally.</param>
        /// <param name="sessionNumber">To reuse the same proxy for multiple requests, simply use the &amp;session_number= flag (e.g. session_number=123). The value of session can be any integer, simply send a new integer to create a new session (this will allow you to continue using the same proxy for each request with that session number). Sessions expire 15 minutes after the last usage.</param>
        /// <param name="countryCode">To ensure your requests come from the United States, please use the country_code= flag (e.g. country_code=us). United States (us) geotargeting is available on the Startup plan and higher. Business plan customers also have access to Canada (ca), United Kingdom (uk), Germany (de), France (fr), Spain (es), Brazil (br), Mexico (mx), India (in), Japan (jp), China (cn), and Australia (au). Other countries are available to Enterprise customers upon request.</param>
        /// <param name="premium">Our standard proxy pools include millions of proxies from over a dozen ISPs, and should be sufficient for the vast majority of scraping jobs. However, for a few particularly difficult to scrape sites, we also maintain a private internal pool of residential and mobile IPs. This pool is only available to users on the Business plan or higher. Requests through our premium residential and mobile pool are charged at 10 times the normal rate (every successful request will count as 10 API calls against your monthly limit), each request that uses both rendering javascript and our premium pool will be charged at 25 times the normal rate (every successful request will count as 25 API calls against your monthly limit). To send a request through our premium proxy pool, please use the premium=true flag.</param>
        /// <param name="deviceType">Undocumented.</param>
        /// <param name="autoParse">Undocumented.</param>
        /// <returns>Json response.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async System.Threading.Tasks.Task<string> PutAsync(
            string url, 
            IEnumerable<KeyValuePair<string, string>> formData, 
            bool? render = null,
            IEnumerable<KeyValuePair<string, string>>? headers = null,
            bool? keepHeaders = null,
            long? sessionNumber = null, 
            string? countryCode = null, 
            bool? premium = null, 
            Device_type? deviceType = null,
            bool? autoParse = null,
            CancellationToken cancellationToken = default)
        {
            var headersJson = headers != null ? JsonConvert.SerializeObject(headers.ToList()) : null;
            keepHeaders = headers != null ? true : keepHeaders;

            var formDataJson = JsonConvert.SerializeObject(formData.ToList());

            return await PutCoreAsync(url, render, keepHeaders, sessionNumber, countryCode, premium, deviceType, Sdk, autoParse, headersJson, formDataJson, cancellationToken).ConfigureAwait(false);
        }


        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <summary>Scrapes the url.</summary>
        /// <param name="url">The url you would like to scrape.</param>
        /// <param name="formData">Form data</param>
        /// <param name="render">If you are crawling a page that requires you to render the javascript on the page, we can fetch these pages using a headless browser. This feature is only available on the Business and Enterprise plans. To render javascript, simply set render=true and we will use a headless Google Chrome instance to fetch the page.</param>
        /// <param name="keepHeaders">If you would like to keep the original request headers in order to pass through custom headers (user agents, cookies, etc.), simply set keep_headers=true. Only use this feature in order to get customized results, do not use this feature in order to avoid blocks, we handle that internally.</param>
        /// <param name="headers">If you would like to keep the original request headers in order to pass through custom headers (user agents, cookies, etc.), simply set keep_headers=true. Only use this feature in order to get customized results, do not use this feature in order to avoid blocks, we handle that internally.</param>
        /// <param name="sessionNumber">To reuse the same proxy for multiple requests, simply use the &amp;session_number= flag (e.g. session_number=123). The value of session can be any integer, simply send a new integer to create a new session (this will allow you to continue using the same proxy for each request with that session number). Sessions expire 15 minutes after the last usage.</param>
        /// <param name="countryCode">To ensure your requests come from the United States, please use the country_code= flag (e.g. country_code=us). United States (us) geotargeting is available on the Startup plan and higher. Business plan customers also have access to Canada (ca), United Kingdom (uk), Germany (de), France (fr), Spain (es), Brazil (br), Mexico (mx), India (in), Japan (jp), China (cn), and Australia (au). Other countries are available to Enterprise customers upon request.</param>
        /// <param name="premium">Our standard proxy pools include millions of proxies from over a dozen ISPs, and should be sufficient for the vast majority of scraping jobs. However, for a few particularly difficult to scrape sites, we also maintain a private internal pool of residential and mobile IPs. This pool is only available to users on the Business plan or higher. Requests through our premium residential and mobile pool are charged at 10 times the normal rate (every successful request will count as 10 API calls against your monthly limit), each request that uses both rendering javascript and our premium pool will be charged at 25 times the normal rate (every successful request will count as 25 API calls against your monthly limit). To send a request through our premium proxy pool, please use the premium=true flag.</param>
        /// <param name="deviceType">Undocumented.</param>
        /// <param name="autoParse">Undocumented.</param>
        /// <returns>Json response.</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async System.Threading.Tasks.Task<string> PostAsync(
            string url,
            IEnumerable<KeyValuePair<string, string>> formData,
            bool? render = null,
            IEnumerable<KeyValuePair<string, string>>? headers = null, 
            bool? keepHeaders = null,
            long? sessionNumber = null, 
            string? countryCode = null, 
            bool? premium = null,
            Device_type? deviceType = null,
            bool? autoParse = null,
            CancellationToken cancellationToken = default)
        {
            var headersJson = headers != null ? JsonConvert.SerializeObject(headers.ToList()) : null;
            keepHeaders = headers != null ? true : keepHeaders;

            var formDataJson = JsonConvert.SerializeObject(formData.ToList());

            return await PostCoreAsync(url, render, keepHeaders, sessionNumber, countryCode, premium, deviceType, Sdk, autoParse, headersJson, formDataJson, cancellationToken).ConfigureAwait(false);
        }

        #endregion
    }
}