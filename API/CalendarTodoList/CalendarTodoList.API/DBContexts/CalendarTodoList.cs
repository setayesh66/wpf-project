using Microsoft.EntityFrameworkCore;
using BeetleMovies.API.Entities;

namespace BeetleMovies.API.DBContexts
{
    public class BeetleMoviesContext(DbContextOptions<BeetleMoviesContext> options) : DbContext(options)
    {
        public DbSet<TaskItem> Tasks { get; set; } 
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskItem>().HasData( 
                new TaskItem { Id = 1, Title = "Dentist Appointment", StartTime = DateTime.Parse("2025-07-25T02:00:00"), Duration = 60 },
                new TaskItem { Id = 2, Title = "Advanced Programming Class", StartTime = DateTime.Parse("2025-10-19T05:00:00"), Duration = 50 },
                new TaskItem { Id = 3, Title = "Math", StartTime = DateTime.Parse("2025-01-22T06:00:00"), Duration = 30 },
                new TaskItem { Id = 4, Title = "Physics", StartTime = DateTime.Parse("2025-07-10T10:00:00"), Duration = 90 },
                new TaskItem { Id = 5, Title = "Gym", StartTime = DateTime.Parse("2025-03-13T03:00:00"), Duration = 20 },
                new TaskItem { Id = 6, Title = "Art Class", StartTime = DateTime.Parse("2025-11-05T04:00:00"), Duration = 40 },
                new TaskItem { Id = 7, Title = "Hang Out With Friends", StartTime = DateTime.Parse("2025-04-19T06:00:00"), Duration = 120 }
            );
        }
    }
}