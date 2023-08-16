using System.ComponentModel.DataAnnotations;

namespace Reserve.Models.Queue;

public class QueueTicket
{
    public Guid Id { get; set; }
    [Required]
    public string CustomerName { get; set; }
    [Required]
    public string CustomerEmail { get; set; }
    [Required]
    public string CustomerPhoneNumber { get; set; }
    [Required]
    public int QueueNumber { get; set; }
    [Required]
    public Guid QueueEventId { get; set; }
}
