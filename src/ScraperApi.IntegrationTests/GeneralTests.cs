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

        [TestMethod]
        public async Task PremiumGetAsyncTest() => await BaseTests.TextTestAsync(
            (api, cancellationToken) => api.GetAsync("http://httpbin.org/ip", premium: true, cancellationToken: cancellationToken));

        [TestMethod]
        public async Task SessionsGetAsyncTest() => await BaseTests.TextTestAsync(
            async (api, cancellationToken) =>
            {
                const string? id = "123";
                var response1 = await api.GetAsync("http://httpbin.org/ip", sessionNumber: id,
                    cancellationToken: cancellationToken);

                var response2 = await api.GetAsync("http://httpbin.org/ip", sessionNumber: id,
                    cancellationToken: cancellationToken);

                Assert.AreEqual(response1, response2, "Responses are not equal");

                return response1 + Environment.NewLine + response2;
            });

        [TestMethod]
        public async Task CustomHeadersGetAsyncTest() => await BaseTests.TextTestAsync(
            (api, cancellationToken) => api.GetAsync("http://httpbin.org/anything", headers: new Dictionary<string, string>
            {
                { "X-MyHeader", "123" },
            }, cancellationToken: cancellationToken));

        [TestMethod]
        public async Task PutAsyncTest() => await BaseTests.TextTestAsync(
            (api, cancellationToken) => api.PutAsync("http://httpbin.org/anything", new Dictionary<string, string>
            {
                { "foo", "bar" },
            }, cancellationToken: cancellationToken));

        [TestMethod]
        public async Task PostAsyncTest() => await BaseTests.TextTestAsync(
            (api, cancellationToken) => api.PostAsync("http://httpbin.org/anything", new Dictionary<string, string>
            {
                { "foo", "bar" },
            }, cancellationToken: cancellationToken));

        [TestMethod]
        public async Task GetAccountInformationTest() => await BaseTests.AccountInformationTestAsync(
            (api, cancellationToken) => api.GetAccountInformationAsync(cancellationToken));
    }
}
