using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace BooksBorrowService.IntegrationTests.Helpers
{
    public abstract class TestBase : IClassFixture<TestApplicationFactory<FakeStartup>>
    {
        protected WebApplicationFactory<FakeStartup> Factory { get; }

        public TestBase(TestApplicationFactory<FakeStartup> factory)
        {
            Factory = factory;            
        }
    }
}