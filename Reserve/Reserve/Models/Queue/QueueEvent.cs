using System.ComponentModel.DataAnnotations;

namespace Reserve.Models.Queue;

public class QueueEvent
{
    public Guid Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string OrganizerName { get; set; }
    [Required]
    public string OrganizerEmail { get; set; }
    [Required]
    public int MaximumCapacity { get; set; }
    [Required]
    public string Location { get; set; }
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
    [Required]
    public List<string> Tags { get; set; }
    [Required]
    public int CurrentCapacity { get; set; }
}
