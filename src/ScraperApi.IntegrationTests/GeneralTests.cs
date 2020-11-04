using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ScraperApi.IntegrationTests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ScraperApi.IntegrationTests
{
    [TestClass]
    public class GeneralTests
    {
        [TestMethod]
        public async Task GetAsyncTest() => await BaseTests.TextTestAsync(
            (api, cancellationToken) => api.GetAsync("http://httpbin.org/ip", cancellationToken: cancellationToken));

        /*
        [TestMethod]
        public async Task ProxyGetAsyncTest() => await BaseTests.TextTestAsync(
            (api, cancellationToken) => api.GetAsync("http://httpbin.org/ip", cancellationToken: cancellationToken),
            () => ScraperApiClient.GetProxyHttpClient(Environment.GetEnvironmentVariable("SCRAPER_API_TOKEN") ?? string.Empty));
        */

        [TestMethod]
        public async Task PremiumGetAsyncTest() => await BaseTests.TextTestAsync(
            (api, cancellationToken) => api.GetAsync("http://httpbin.org/ip", premium: true, cancellationToken: cancellationToken));

        [TestMethod]
        public async Task SessionsGetAsyncTest() => await BaseTests.TextTestAsync(
            async (api, cancellationToken) =>
            {
                const long id = long.MaxValue;
                var response1 = await api.GetAsync("http://httpbin.org/ip", sessionNumber: id,
                    cancellationToken: cancellationToken);

                var response2 = await api.GetAsync("http://httpbin.org/ip", sessionNumber: id,
                    cancellationToken: cancellationToken);

                Assert.AreEqual(response1, response2, "Responses are not equal");

                return response1 + Environment.NewLine + response2;
            });

        [TestMethod]
        public async Task CustomHeadersGetAsyncTest() => await BaseTests.TextTestAsync(
            async (api, cancellationToken) =>
            {
                var response = await api.GetAsync("http://httpbin.org/anything", headers: new Dictionary<string, string>
                {
                    {"X-MyHeader", "123"},
                }, cancellationToken: cancellationToken);

                Assert.IsTrue(response.Contains("\"X-Myheader\": \"123\""));

                return response;
            });

        [TestMethod]
        public async Task PutAsyncTest() => await BaseTests.TextTestAsync(
            async (api, cancellationToken) =>
            {
                var response = await api.PutAsync("http://httpbin.org/anything", new Dictionary<string, string>
                {
                    { "foo", "bar" },
                }, cancellationToken: cancellationToken);

                Assert.IsTrue(response.Contains("\"data\": \"{\\\"foo\\\":\\\"bar\\\"}\""));
                Assert.IsTrue(response.Contains("\"form\": {}"));
                Assert.IsTrue(response.Contains("\"json\": {\n    \"foo\": \"bar\"\n  }"));

                return response;
            });

        [TestMethod]
        public async Task PostAsyncTest() => await BaseTests.TextTestAsync(
            async (api, cancellationToken) =>
            {
                var response = await api.PostAsync("http://httpbin.org/anything", new Dictionary<string, string>
                {
                    { "foo", "bar" },
                }, cancellationToken: cancellationToken);

                Assert.IsTrue(response.Contains("\"data\": \"\""));
                Assert.IsTrue(response.Contains("\"form\": {\n    \"foo\": \"bar\"\n  }"));
                Assert.IsTrue(response.Contains("\"json\": null"));

                return response;
            });

        [TestMethod]
        public async Task GetAccountInformationTest() => await BaseTests.AccountInformationTestAsync(
            async (api, cancellationToken) =>
            {
                var information = await api.GetAccountInformationAsync(cancellationToken);

                Assert.IsTrue(information.RequestCount > 0, nameof(information.RequestCount));
                Assert.IsTrue(information.RequestLimit > 0, nameof(information.RequestLimit));
                Assert.IsTrue(information.ConcurrencyLimit > 0, nameof(information.ConcurrencyLimit));

                return information;
            });
    }
}
