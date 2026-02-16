using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BeetleMovies.API.DTOs
{
    public class TaskFiltersDTO
    {
        [FromQuery(Name = "day")]
        public DateOnly? Day { get; set; }
        [FromQuery(Name = "startTime")]
        public DateTime? StartTime { get; set; }

        [FromQuery(Name = "endTime")]
        public DateTime? EndTime { get; set; }
    }
}