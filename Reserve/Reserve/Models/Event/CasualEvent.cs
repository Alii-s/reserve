using System.ComponentModel.DataAnnotations;

namespace Reserve.Models.Event;

public class CasualEvent
{
    public Guid Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    [Display(Name = "Organizer Name")]
    public string OrganizerName { get; set; }
    [Required]
    [Display(Name = "Organizer Email")]
    public string OrganizerEmail { get; set; }
    [Required]
    [Display(Name = "Maximum Capacity")]
    public int MaximumCapacity { get; set; }
    [Required]
    public string Location { get; set; }
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
    [Required]
    public string[] Tags { get; set; }
    [Required]
    public int CurrentCapacity { get; set; }
    [Required]
    public string Description { get; set; }
}
