using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InMemoryTesting.Data;
using InMemoryTesting.Data.Entites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace inmemorytesting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovieContext movieContext;

        public MoviesController(MovieContext movieContext)
        {
            this.movieContext = movieContext;           
        }

        // GET api/movies
        [HttpGet]
        public ActionResult<int> Get()
        {
            Int64 sqlCount = 0;
            var sqlite = new SqliteConnection("Data Source=file:memdb1?mode=memory&cache=shared");
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
