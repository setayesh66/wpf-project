using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BeetleMovies.API.Entities
{
    public class TaskItem 
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Title { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        [Range(1, 1440)] 
        public int Duration { get; set; } 

        public TaskItem() { }

        public TaskItem(int id, string title, DateTime startTime, int duration)
        {
            Id = id;
            Title = title;
            StartTime = startTime;
            Duration = duration;
        }
    }
}