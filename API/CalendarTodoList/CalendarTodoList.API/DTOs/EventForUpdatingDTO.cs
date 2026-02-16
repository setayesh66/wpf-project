
using System.ComponentModel.DataAnnotations;
public class TaskForUpdatingDTO
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public int Duration { get; set; }
}