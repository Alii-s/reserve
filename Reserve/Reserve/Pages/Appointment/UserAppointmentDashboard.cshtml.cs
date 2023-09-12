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
        return Page();
    }
    public async Task<IActionResult> OnPost()
    {
        try
        {
            AppointmentDetails = await _appointmentRepository.GetAppointmentDetailsByIdAsync(Id);
            await _appointmentRepository.Reschedule(AppointmentDetails, SelectedDate);
            return RedirectToPage("/Appointment/AppointmentRescheduleNotification");
        }
        catch (Exception)
        {
            return RedirectToPage("/Appointment/AppointmentError");
        }
    }
}
