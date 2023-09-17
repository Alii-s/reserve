using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reserve.Core.Features.Event;

public class CasualTicket
{
    public Guid Id { get; set; }
    public string? ReserverName { get; set; }
    public string? ReserverEmail { get; set; }
    public string? ReserverPhoneNumber { get; set; }
    public CasualEvent? CasualEvent { get; set; }
}
