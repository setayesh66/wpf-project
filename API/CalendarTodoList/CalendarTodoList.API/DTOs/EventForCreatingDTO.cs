using System.ComponentModel.DataAnnotations;

namespace BeetleMovies.API.DTOs
{
    public class TaskForCreatingDTO
    {
        public required string Title { get; set; }
        public DateTime StartTime { get; set; }
        public int Duration { get; set; }
    }
}