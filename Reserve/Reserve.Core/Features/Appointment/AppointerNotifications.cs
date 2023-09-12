using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reserve.Core.Features.Appointment;

public class AppointerNotifications
{
    public Guid Id { get; set; }
    public string ReserverName { get; set; }
    public string ReserverEmail { get; set; }
    public string ReserverPhoneNumber { get; set; }
    public string NotificationType { get; set; }
    public DateTime NewSlot { get; set; }
    public Availability Slot { get; set; }
}
