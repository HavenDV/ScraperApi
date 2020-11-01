﻿using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ScraperApi.IntegrationTests.Utilities
{
    internal static class BaseTests
    {
        public static async Task ApiTestAsync(Func<ScraperApi, CancellationToken, Task> action)
        {
            using var source = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            var cancellationToken = source.Token;

            var token = Environment.GetEnvironmentVariable("IPINFO_TOKEN") ??
                        throw new InvalidOperationException("token is null.");

            using var client = new HttpClient();
            var api = new ScraperApi(token, client);

            try
            {
                await action(api, cancellationToken).ConfigureAwait(false);
            }
            catch (ApiException exception)
            {
                if (exception.StatusCode != 403)
                {
                    throw;
                }
            }
        }

        public static async Task GeneralApiTestAsync(Func<ScraperApi, CancellationToken, Task<FullResponse>> action)
        {
            await ApiTestAsync(async (api, cancellationToken) =>
            {
                var response = await action(api, cancellationToken)
                    .ConfigureAwait(false);

                Assert.IsNotNull(response, nameof(response));
                Console.WriteLine(response.GetPropertiesText());
            });
        }

        public static async Task SingleApiTestAsync(Func<ScraperApi, CancellationToken, Task<string>> action)
        {
            await ApiTestAsync(async (api, cancellationToken) =>
            {
                var response = await action(api, cancellationToken)
                    .ConfigureAwait(false);

                Assert.IsNotNull(response, nameof(response));
                Console.WriteLine(response);
            });
        }
    }
}