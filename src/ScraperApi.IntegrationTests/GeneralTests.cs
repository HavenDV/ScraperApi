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
