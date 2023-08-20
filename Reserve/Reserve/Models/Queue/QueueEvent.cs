using System.ComponentModel.DataAnnotations;

namespace Reserve.Models.Queue;

public class QueueEvent
{
    public Guid Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string OrganizerEmail { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public int CurrentNumberServed { get; set; }
}
