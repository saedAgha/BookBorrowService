using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;

namespace BooksBorrowService.IntegrationTests.Helpers
{
    public static class WebApplicationFactoryExtensions
    {
        public static HttpClient CreateClient<T>(this WebApplicationFactory<T> factory) where T : class
        {
            var client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            return client;
        }
    }
}