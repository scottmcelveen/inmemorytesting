using System;
using System.Net.Http;
using System.Threading.Tasks;
using inmemorytesting;
using InMemoryTesting.Configuration;
using InMemoryTesting.Data;
using InMemoryTesting.Data.Entites;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace tests
{    public class MoviesControllerTests2 : IClassFixture<WebApplicationFactory<Startup>>
    {
        private WebApplicationFactory<Startup> factory;
        public MoviesControllerTests2(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }
        [Fact] //"Data Source=file:memdb2?mode=memory&cache=shared"
        public async Task Get()
        {
            using(var sqlite = new SqliteConnection("Data Source=file:memdb2?mode=memory&cache=shared"))
            {
                sqlite.Open();
                
                var testFactory = factory.WithWebHostBuilder(builder => {
                    builder.UseEnvironment("Testing");
                    builder.ConfigureTestServices(services => {
                        
                        var serviceProvider = services
                            .AddEntityFrameworkSqlite()
                            .AddDbContext<MovieContext>((sp,o) => {
                                o.UseSqlite(sqlite).UseInternalServiceProvider(sp);
                            })
                            .Configure<DatabaseConfiguration>(d => d.RootConnectionString = "Data Source=file:memdb2?mode=memory&cache=shared")
                            .BuildServiceProvider();

                        using(var scope = serviceProvider.CreateScope())
                        {
                            var context = scope.ServiceProvider.GetRequiredService<MovieContext>();
                            context.Database.EnsureCreated();
                        }
                    });
                });

                var client = testFactory.CreateClient();

                // Act
                var response = await client.PostAsJsonAsync("/api/movies", new Movie { Title = "TESTMOVIE", ReleaseYear = 2000 });
                response.EnsureSuccessStatusCode();
                var movie = await response.Content.ReadAsAsync<Movie>();
                
                // Assert
                Assert.Equal("TESTMOVIE", movie.Title);
            }
        }
    }
}
