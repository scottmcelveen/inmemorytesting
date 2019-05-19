using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InMemoryTesting.Configuration;
using InMemoryTesting.Data;
using InMemoryTesting.Data.Entites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace inmemorytesting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovieContext movieContext;
        private readonly DatabaseConfiguration configuration;

        public MoviesController(MovieContext movieContext, IOptions<DatabaseConfiguration> configuration)
        {
            this.movieContext = movieContext;           
            this.configuration = configuration.Value;
        }

        // GET api/movies
        [HttpGet]
        public ActionResult<int> Get()
        {
            Int64 sqlCount = 0;
            var sqlite = new SqliteConnection(configuration.RootConnectionString);
            sqlite.Open();
            var command = sqlite.CreateCommand();
            command.CommandText = "select COUNT(Id) from movies";
            var result = command.ExecuteScalar();
            var total =  movieContext.Movies.Count() + Convert.ToInt32(result);
            return total;
        }

        // POST api/movies
        [HttpPost]
        public ActionResult Post(Movie movie)
        {
            movieContext.Movies.Add(movie);
            movieContext.SaveChanges();
            return Ok(movie);
        }
    }
}
