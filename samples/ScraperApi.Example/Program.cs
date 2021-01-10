using System;
using System.Net.Http;
using System.Threading.Tasks;
using ScraperApi;

namespace Examples
{
    internal static class Program
    {
        private static async Task Main()
        {
            using var client = new HttpClient();
            var apiKey = Environment.GetEnvironmentVariable("SCRAPER_API_TOKEN") ?? string.Empty;
            var api = new ScraperApiClient(apiKey, client);

            var result = await api.GetAsync("http://httpbin.org/ip");

            Console.WriteLine($"Result: {result}");
        }
    }
}