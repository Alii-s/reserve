using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Core.Features.Appointment;

namespace Reserve.Pages.Appointment;
[BindProperties]
public class CreateAppointmentCalendarModel : PageModel
{
    public AppointmentCalendar NewAppointmentCalendar { get; set; }
    private readonly IAppointmentRepository _appointmentRepository;
    public CreateAppointmentCalendarModel(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }
    public void OnGet()
    {        
    }
    public async Task<IActionResult> OnPost()
    {
        if(ModelState.IsValid)
        {
            NewAppointmentCalendar = await _appointmentRepository.CreateAppointmentInfo(NewAppointmentCalendar);
            return RedirectToPage("/Appointment/CreateCalendar", new {id = NewAppointmentCalendar.Id});
        }
        return Page();
    }
}
