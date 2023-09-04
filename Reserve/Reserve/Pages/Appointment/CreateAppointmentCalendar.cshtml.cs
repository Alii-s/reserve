using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Core.Features.Appointment;

namespace Reserve.Pages.Appointment;
[BindProperties]
public class CreateAppointmentCalendarModel : PageModel
{
    public AppointmentCalendar NewAppointmentCalendar { get; set; }
    public List<Availability> AvailabilitySlots { get; set; } = new List<Availability>();
    public Availability NewAvailabilitySlot { get; set; }
    public List<Days> DaysList { get; set; } = new List<Days>();
    public Days SelectedDay { get; set; }

    public void OnGet()
    {
        DaysList = Enum.GetValues(typeof(Days)).Cast<Days>().ToList();
    }
}
