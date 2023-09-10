using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Core.Features.Appointment;

namespace Reserve.Pages.Appointment;

public class EditSlotsModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string Id { get; set; }
    public List<Availability> AvailabilitySlots { get; set; } = new();
    public AppointmentCalendar AppointmentCalendar { get; set; }
    public List<Days> DaysList { get; set; } = new List<Days>();
    public Days Day { get; set; }
    private readonly IAppointmentRepository _appointmentRepository;
    public Availability NewAvailabilitySlot { get; set; }
    public EditSlotsModel(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }
    public async Task<IActionResult> OnGet()
    {
        DaysList = Enum.GetValues(typeof(Days)).Cast<Days>().ToList();
        AppointmentCalendar = await _appointmentRepository.GetByIdAsync(Id);
        if(AppointmentCalendar is null)
        {
            return RedirectToPage("/Appointment/AppointmentError");
        }
        AvailabilitySlots = await _appointmentRepository.GetSlotsFromCalendarIdAsync(Id);
        return Page();
    }
}
