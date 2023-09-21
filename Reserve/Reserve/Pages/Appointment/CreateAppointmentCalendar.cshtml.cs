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
            AppointmentCalendar calendar = await _appointmentRepository.GetCalendarFromEmail(NewAppointmentCalendar.Email);
            if(calendar is not null)
            {
                ModelState.AddModelError("NewAppointmentCalendar.Email", "This email is already in use");
                return Page();
            }
            NewAppointmentCalendar = await _appointmentRepository.CreateAppointmentInfoAsync(NewAppointmentCalendar);
            if(NewAppointmentCalendar is null)
            {
                return RedirectToPage("/Appointment/AppointmentError");
            }
            return RedirectToPage("/Appointment/CreateCalendar", new {id = NewAppointmentCalendar.Id});
        }
        return Page();
    }
}
