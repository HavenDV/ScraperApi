using System;
using System.Threading.Tasks;
using ScraperApi.IntegrationTests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ScraperApi.IntegrationTests
{
    [TestClass]
    public class GeneralTests
    {
        [TestMethod]
        public async Task GetCurrentInformationTest() => await BaseTests.TextTestAsync(
            (api, cancellationToken) => api.GetAsync("http://httpbin.org/ip", cancellationToken: cancellationToken));

        [TestMethod]
        public async Task GetInformationByIpTest() => await BaseTests.AccountTestAsync(
            (api, cancellationToken) => api.GetAccountInformationAsync(cancellationToken));
    }
}
