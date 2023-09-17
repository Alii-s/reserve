using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reserve.Core.Features.Queue;

public class QueueTicketInput
{
    public Guid Id { get; set; }
    [Required]
    public string CustomerName { get; set; }
    [Required]
    public string CustomerPhoneNumber { get; set; }
    [Required]
    public int QueueNumber { get; set; }
    [Required]
    public QueueEvent QueueEvent { get; set; }
}
