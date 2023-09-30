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
    public AppointmentReschedule AppointmentReschedule { get; set; }
    public UserAppointmentDashboardModel(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }
    public async Task<IActionResult> OnGet()
    {
        if (string.IsNullOrEmpty(Id))
        {
            return RedirectToPage("AppointmentError");
        }
        AppointmentDetails = await _appointmentRepository.GetAppointmentDetailsByIdAsync(Id);
        if (AppointmentDetails is null)
        {
            return RedirectToPage("AppointmentError");
        }
        if (HttpContext.Request.Cookies["check"] is not null)
        {
            TempData["check"] = HttpContext.Request.Cookies["check"];
            HttpContext.Response.Cookies.Delete("check");
        }
        List<Availability> availableSlots = await _appointmentRepository.GetFreeSlotsOfCalendarByIdAsync(AppointmentDetails);
        if(availableSlots is null)
        {
            return RedirectToPage("AppointmentError");
        }
        FreeSlots = availableSlots.Select(x => x.StartTime).ToList();
        AppointmentReschedule = await _appointmentRepository.GetRescheduleByIdAsync(Id);
        return Page();
    }
    public async Task<IActionResult> OnPost()
    {
        AppointmentDetails = await _appointmentRepository.GetAppointmentDetailsByIdAsync(Id);
        if(AppointmentDetails.AppointmentStatus == AppointmentState.Done)
        {
            TempData["error"] = "This appointment has already been completed.";
            return RedirectToPage("UserAppointmentDashboard", new {id = Id });
        }
        AppointmentReschedule appointmentReschedule = await _appointmentRepository.GetRescheduleByIdAsync(Id);
        if (appointmentReschedule is not null)
        {
            TempData["check"] = "Please check notifications before making this action";
            return RedirectToPage("UserAppointmentDashboard", new { id = Id });
        }
        if(AppointmentDetails is null)
        {
            return RedirectToPage("AppointmentError");
        }
        await _appointmentRepository.Reschedule(AppointmentDetails, SelectedDate);
        return RedirectToPage("AppointmentRescheduleNotification", new {id = Id });
    }
}
