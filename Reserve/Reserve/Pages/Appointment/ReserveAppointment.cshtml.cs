using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Core.Features.Appointment;

namespace Reserve.Pages.Appointment;

public class ReserveAppointmentModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string Id { get; set; }
    public AppointmentCalendar AppointmentCalendar { get; set; }
    
    private readonly IAppointmentRepository _appointmentRepository;
    public List<Availability> AvailabilitySlots { get; set; } = new();  
    public ReserveAppointmentModel(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }
    public async Task<IActionResult> OnGet()
    {
        if(string.IsNullOrEmpty(Id))
        {
            return RedirectToPage("/Appointment/AppointmentError");
        }
        AppointmentCalendar = await _appointmentRepository.GetByIdAsync(Id);
        if(AppointmentCalendar is null)
        {
            return RedirectToPage("/Appointment/AppointmentError");
        }
        AvailabilitySlots = await _appointmentRepository.GetOpenSlotsAsync(Id);
        if(AvailabilitySlots is null)
        {
            return RedirectToPage("/Appointment/AppointmentError");
        }
        return Page();
    }
}
