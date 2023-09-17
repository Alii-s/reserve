using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reserve.Core.Features.Event;

public class CasualEvent
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? OrganizerName { get; set; }
    public string? OrganizerEmail { get; set; }
    public int? MaximumCapacity { get; set; }
    public int CurrentCapacity { get; set; }
    public string? Location { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool Opened { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
}
