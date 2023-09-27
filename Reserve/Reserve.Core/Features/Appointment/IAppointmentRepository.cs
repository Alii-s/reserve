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
    Task<List<Availability>> GetFreeSlotsOfCalendarByIdAsync(AppointmentDetails appointment);
    Task Reschedule(AppointmentDetails appointment, DateTime newSlot);
    Task FinishAppointment(string id);
    Task<AppointmentReschedule> GetRescheduleByIdAsync(string id);
    Task<AppointmentDetails> GetAppointmentDetailsByAvailabilityId(string id);
    Task CreateAppointmentReschedule(AppointmentReschedule appointmentReschedule);
    Task DeleteAppointmentReschedule(string id);
    Task<AppointmentCalendar> GetCalendarFromEmail(string email);
    Task<List<AppointmentDetails>> GetAppointmentsOfCalendar(string id);
    Task<List<Availability>> GetFreeSlotsForCalendarView(string id);
    Task DeclineRescheduling(string id);
    Task AcceptRescheduling(string id);
    Task<List<AppointmentReschedule>> GetAllRequestsForCalendar(string id);
    Task DeleteRequest(string id);
    Task<List<Availability>> GetPendingSlots(string id);
    Task<AppointmentReschedule> GetRequestByIdAsync(string id);
    Task<List<AppointmentDetails>> GetDoneAppointmentsByCalendarId(string id);
}
