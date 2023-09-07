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
    public List<Days> DaysList { get; set; } = new List<Days>();
    public Days SelectedDay { get; set; }
    private readonly IAppointmentRepository _appointmentRepository;
    public CreateCalendarModel(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }
    public async Task OnGet(AppointmentCalendar appointmentCalendar)
    {
        NewAppointmentCalendar = await _appointmentRepository.GetByIdAsync(Id);
        DaysList = Enum.GetValues(typeof(Days)).Cast<Days>().ToList();
    }
    
}
