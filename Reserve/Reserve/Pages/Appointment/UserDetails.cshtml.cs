using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Core.Features.Appointment;

namespace Reserve.Pages.Appointment;

public class UserDetailsModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string Id { get; set; }
    public AppointmentDetails AppointmentDetails { get; set; }
    private readonly IAppointmentRepository _appointmentRepository;
    public List<Availability> FreeSlots { get; set; } = new();
    [BindProperty]
    public string SelectedSlot { get; set; }
    public UserDetailsModel(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }
    public async Task<IActionResult> OnGet()
    {
        if(string.IsNullOrEmpty(Id))
        {
            return RedirectToPage("AppointmentError");
        }
        AppointmentDetails = await _appointmentRepository.GetAppointmentDetailsByIdAsync(Id);
        if(AppointmentDetails is null)
        {
            return RedirectToPage("AppointmentError");
        }
        FreeSlots = await _appointmentRepository.GetFreeSlotsOfCalendarByIdAsync(AppointmentDetails);
        if (HttpContext.Request.Cookies["error"] is not null)
        {
            TempData["error"] = HttpContext.Request.Cookies["error"];
            HttpContext.Response.Cookies.Delete("error");
        }
        return Page();
    }
    public async Task<IActionResult> OnPost()
    {
        try
        {
            AppointmentDetails = await _appointmentRepository.GetAppointmentDetailsByIdAsync(Id);
            if(AppointmentDetails.AppointmentStatus == AppointmentState.Done)
            {
                TempData["error"] = "This appointment has already been completed.";
                return RedirectToPage("UserDetails", new {id = Id });
            }
            var result = await _appointmentRepository.GetRescheduleByIdAsync(Id);
            if(result is not null)
            {
                TempData["error"] = "You have already requested to reschedule this appointment.";
                return RedirectToPage("UserDetails", new {id = Id });
            }
            await _appointmentRepository.CreateAppointmentReschedule(new AppointmentReschedule
            {
                OriginalAppointment = AppointmentDetails,
                RequestedTime = await _appointmentRepository.GetSlotByIdAsync(SelectedSlot),
                RescheduleStatus = RescheduleState.Pending
            });
            TempData["success"] = "Your request has been sent to the user.";
            return RedirectToPage("UpcomingAppointments", new { id = AppointmentDetails.Slot.AppointmentCalendar.Id });
        }
        catch(Exception e)
        {
            TempData["error"] = "Something went wrong. Please try again later.";
            return RedirectToPage("UpcomingAppointments", new { id = AppointmentDetails.Slot.AppointmentCalendar.Id });
        }
    }
}
