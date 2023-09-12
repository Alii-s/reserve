using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Core.Features.Appointment;

namespace Reserve.Pages.Appointment;

public class UpcomingAppointmentsModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string Id { get; set; }
    public List<AppointmentDetails> Appointments { get; set; } = new();
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
        return Page();
    }
}
