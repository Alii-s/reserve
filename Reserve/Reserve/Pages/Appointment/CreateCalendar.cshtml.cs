using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Core.Features.Appointment;
using System.Text.Json;

namespace Reserve.Pages.Appointment;
[BindProperties]
public class CreateCalendarModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string Id { get; set; }
    public AppointmentCalendar NewAppointmentCalendar { get; set; }
    public Availability NewAvailabilitySlot { get; set; }
    private readonly IAppointmentRepository _appointmentRepository;
    public CreateCalendarModel(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }
    public async Task<IActionResult> OnGet()
    {
        if(string.IsNullOrEmpty(Id))
        {
            return RedirectToPage("/Appointment/AppointmentError");
        }
        NewAppointmentCalendar = await _appointmentRepository.GetByIdAsync(Id);
        if(NewAppointmentCalendar is null)
        {
            return RedirectToPage("/Appointment/AppointmentError");
        }
        return Page();
    }
    
}
