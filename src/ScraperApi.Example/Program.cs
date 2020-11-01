using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ScraperApi.Example
{
    internal static class Program
    {
        private static async Task Main()
        {
            using var client = new HttpClient();
            var api = new ScraperApi(client);

            var response = await api.GetCurrentInformationAsync();

            Console.WriteLine($"City: {response.City}");
        }
    }
}