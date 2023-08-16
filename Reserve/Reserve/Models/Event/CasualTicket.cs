using System.ComponentModel.DataAnnotations;

namespace Reserve.Models.Event;

public class CasualTicket
{
    public Guid Id { get; set; }
    [Required]
    public string ReserverName { get; set; }
    [Required]
    public string ReserverEmail { get; set; }
    [Required]
    public Guid CasualEventId { get; set; }

}
