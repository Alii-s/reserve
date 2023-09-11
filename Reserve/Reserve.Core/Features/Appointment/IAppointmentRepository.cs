using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reserve.Core.Features.Appointment;

public interface IAppointmentRepository
{
    Task CreateAppointmentCalendarAsync(AppointmentCalendar appointmentCalendar, string availabilitySlots);
    Task<AppointmentCalendar?> GetByIdAsync(string id);
    Task<List<Availability>> GetSlotsFromCalendarIdAsync(string id);
    Task<AppointmentCalendar> CreateAppointmentInfoAsync(AppointmentCalendar appointmentCalendar);
    Task DeleteAppointmentSlotAsync(string id);
    Task<Availability> AddAppointmentSlotAsync(string id, Availability newSlot);
    Task<List<Availability>> GetOpenSlotsAsync(string id);
    Task<Availability> GetSlotByIdAsync(string id);
    Task<AppointmentDetails> CreateAppointmentMeetingAsync(AppointmentDetails appointmentDetails);
    Task<List<AppointmentDetails>> GetAppointmentDetailsForCalendarAsync(string id);
    Task<AppointmentDetails> GetAppointmentDetailsByIdAsync(string id);
    Task CancelAppointmentAsync(AppointmentDetails cancelledAppointment);
    Task<List<AppointerNotifications>> GetAppointmentNotificationsForCalendarAsync(string id);
    Task DeleteNotification(string id);
}
