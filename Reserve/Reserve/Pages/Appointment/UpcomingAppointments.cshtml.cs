using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Core.Features.Appointment;

namespace Reserve.Pages.Appointment;

public class UpcomingAppointmentsModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string Id { get; set; }
    public List<AppointmentDetails> Appointments { get; set; } = new();
    public Availability NewAvailabilitySlot { get; set; }
    private readonly IAppointmentRepository _appointmentRepository;
    public UpcomingAppointmentsModel(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }
    public async Task<IActionResult> OnGet()
    {
        if(string.IsNullOrEmpty(Id))
        {
            return RedirectToPage("/Appointment/AppointmentError");
        }
        Appointments = await _appointmentRepository.GetAppointmentDetailsForCalendarAsync(Id);
        if(Appointments is null)
        {
            return RedirectToPage("/Appointment/AppointmentError");
        }
        if (HttpContext.Request.Cookies["error"] is not null)
        {
            TempData["error"] = HttpContext.Request.Cookies["error"];
            HttpContext.Response.Cookies.Delete("error");
        }
        if (HttpContext.Request.Cookies["success"] is not null)
        {
            TempData["success"] = HttpContext.Request.Cookies["success"];
            HttpContext.Response.Cookies.Delete("success");
        }
        return Page();
    }
}
