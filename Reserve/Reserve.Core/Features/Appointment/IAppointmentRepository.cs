using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reserve.Core.Features.Appointment;

public interface IAppointmentRepository
{
    Task CreateAppointmentCalendar(AppointmentCalendar appointmentCalendar, string availabilitySlots);
    Task<AppointmentCalendar?> GetByIdAsync(string Id);
    Task<List<Availability>> GetSlotsFromCalendarIdAsync(string Id);
    Task<AppointmentCalendar> CreateAppointmentInfo(AppointmentCalendar appointmentCalendar);
    Task DeleteAppointmentSlot(string Id);
}
