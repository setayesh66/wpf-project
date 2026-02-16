namespace BeetleMovies.API.DTOs
{
    public class TaskDTO
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public DateTime StartTime { get; set; }
        public int Duration { get; set; }
    }
}