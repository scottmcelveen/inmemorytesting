using System;
using System.Net.Http;
using System.Threading.Tasks;
using inmemorytesting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace tests
{
    public class MoviesControllerTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public MoviesControllerTests()
        {
            // Arrange
            _server = new TestServer(new WebHostBuilder()
               .UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task Get()
        {
            // Act
            var response = await _client.GetAsync("/api/movies");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            // Assert
            Assert.Equal("1", responseString);
        }
    }
}
