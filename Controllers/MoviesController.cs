using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InMemoryTesting.Data;
using InMemoryTesting.Data.Entites;
using Microsoft.AspNetCore.Mvc;

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
            return movieContext.Movies.Count();
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
