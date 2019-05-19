using System;
using System.Net.Http;
using System.Threading.Tasks;
using inmemorytesting;
using InMemoryTesting.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace tests
{
    [Collection("Sequential")]
    public class MoviesControllerTests
    {
        public MoviesControllerTests()
        {
        }

        [Fact]
        public async Task Get()
        {
            // Arrange
            var connection = new SqliteConnection("Data Source=file:memdb1?mode=memory&cache=shared");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<MovieContext>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new MovieContext(options))
                {
                    context.Database.EnsureCreated();
                }

                var webHost = new WebHostBuilder().Configure(app => {
                    ;
                }).ConfigureAppConfiguration(config => {
                    ;
                }).ConfigureTestServices(services => {
                    services.AddDbContext<MovieContext>(o =>
                        o.UseSqlite("Data Source=file:memdb1?mode=memory&cache=shared"));
                    services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
                }).UseStartup<Startup>();
            
                var server = new TestServer(webHost);
                var client = server.CreateClient();

                // Act
                var response = await client.GetAsync("/api/movies");
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();

                // Assert
                Assert.Equal("2", responseString);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
