using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Core.Features.Appointment;

namespace Reserve.Pages.Appointment;
[BindProperties]
public class UserAppointmentDashboardModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string Id { get; set; }
    public AppointmentDetails AppointmentDetails { get; set; }
    private readonly IAppointmentRepository _appointmentRepository;
    public List<DateTime> FreeSlots { get; set; } = new();
    public DateTime SelectedDate { get; set; }
    public List<AppointmentReschedule> AppointmentReschedules { get; set; } = new();
    public UserAppointmentDashboardModel(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }
    public async Task<IActionResult> OnGet()
    {
        if (string.IsNullOrEmpty(Id))
        {
            return RedirectToPage("/Appointment/AppointmentError");
        }
        AppointmentDetails = await _appointmentRepository.GetAppointmentDetailsByIdAsync(Id);
        if (AppointmentDetails is null)
        {
            return RedirectToPage("/Appointment/AppointmentError");
        }
        List<Availability> availableSlots = await _appointmentRepository.GetFreeSlotsOfCalendarByIdAsync(AppointmentDetails);
        if(availableSlots is null)
        {
            return RedirectToPage("/Appointment/AppointmentError");
        }
        FreeSlots = availableSlots.Select(x => x.StartTime).ToList();
        AppointmentReschedules = await _appointmentRepository.GetReschedulesByIdAsync(Id);
        return Page();
    }
    public async Task<IActionResult> OnPost()
    {
        try
        {
            AppointmentDetails = await _appointmentRepository.GetAppointmentDetailsByIdAsync(Id);
            await _appointmentRepository.Reschedule(AppointmentDetails, SelectedDate);
            return RedirectToPage("/Appointment/AppointmentRescheduleNotification", new {id = Id });
        }
        catch (Exception)
        {
            return RedirectToPage("/Appointment/AppointmentError");
        }
    }
    public async Task<IActionResult> OnPostAcceptRescheduleAsync(Guid rescheduleId)
    {
        AppointmentReschedules = await _appointmentRepository.GetReschedulesByIdAsync(Id);
        AppointmentDetails = await _appointmentRepository.GetAppointmentDetailsByIdAsync(Id);
        var rescheduleToAccept = AppointmentReschedules.FirstOrDefault(r => r.Id == rescheduleId);
        await _appointmentRepository.Reschedule(AppointmentDetails, rescheduleToAccept.RequestedTime.StartTime);
        await _appointmentRepository.DeleteAppointmentReschedule(rescheduleId.ToString());
        return RedirectToPage("/Appointment/UserAppointmentDashboard", new { id = Id });
    }

    public async Task<IActionResult> OnPostRejectRescheduleAsync(Guid rescheduleId)
    {
        AppointmentReschedules = await _appointmentRepository.GetReschedulesByIdAsync(Id);
        var rescheduleToReject = AppointmentReschedules.FirstOrDefault(r => r.Id == rescheduleId);

        if (rescheduleToReject != null)
        {
            AppointmentReschedules.Remove(rescheduleToReject);
            await _appointmentRepository.DeleteAppointmentReschedule(rescheduleId.ToString());
        }

        return RedirectToPage("/Appointment/UserAppointmentDashboard", new { id = Id });
    }


}
