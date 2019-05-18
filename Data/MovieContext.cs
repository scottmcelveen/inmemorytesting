using System;
using InMemoryTesting.Data.Entites;
using Microsoft.EntityFrameworkCore;

namespace InMemoryTesting.Data
{
    public class MovieContext : DbContext
    {
        public MovieContext(DbContextOptions<MovieContext> options)
        : base(options)
        { }
        public DbSet<Movie> Movies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>().HasData(new[]
            {
                new Movie
                {
                    Id = new Guid("11111111-1111-1111-1111-111111111111"),
                    Title = "The Matrix",
                    ReleaseYear = 1999
                }
            });
        }
    }
}