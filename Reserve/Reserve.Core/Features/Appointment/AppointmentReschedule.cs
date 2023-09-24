using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reserve.Core.Features.Appointment;

public class AppointmentReschedule
{
    public Guid Id { get; set; }
    public AppointmentDetails OriginalAppointment { get; set; }
    public Availability RequestedTime { get; set; }
    public RescheduleState RescheduleStatus { get; set; }
}

public enum RescheduleState{
    Pending,
    Accepted,
    Declined
}