using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Core.Features.Appointment;

namespace Reserve.Pages.Appointment;

public class EditSlotsModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string Id { get; set; }
    public List<Availability> AvailabilitySlots { get; set; } = new();
    public List<Availability> OpenSlots { get; set; } = new();
    public AppointmentCalendar AppointmentCalendar { get; set; }
    private readonly IAppointmentRepository _appointmentRepository;
    public Availability NewAvailabilitySlot { get; set; }
    public List<DateTime> FreeSlots { get; set; } = new();
    [BindProperty]
    public DateTime SelectedDate { get; set; }

    [BindProperty]
    public int SelectedSlotIndex { get; set; }
    public EditSlotsModel(IAppointmentRepository appointmentRepository)
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
        AvailabilitySlots = await _appointmentRepository.GetSlotsFromCalendarIdAsync(Id);
        OpenSlots = await _appointmentRepository.GetOpenSlotsAsync(Id);
        FreeSlots = OpenSlots.Select(x => x.StartTime).ToList();
        return Page();
    }

    public async Task<IActionResult> OnPost()
    { //there has to be a better way to do this
        AvailabilitySlots = await _appointmentRepository.GetSlotsFromCalendarIdAsync(Id);
        OpenSlots = await _appointmentRepository.GetOpenSlotsAsync(Id);
        AppointmentDetails appointmentDetails = await _appointmentRepository.GetAppointmentDetailsByAvailabilityId(AvailabilitySlots[SelectedSlotIndex].Id.ToString());
        Availability RequestedAppointment = OpenSlots.FirstOrDefault(slot => slot.StartTime.Date == SelectedDate.Date);
        AppointmentReschedule appointmentReschedule = new AppointmentReschedule
        {
            IsAccepted = false,
            OriginalAppointment = appointmentDetails,
            RequestedTime = RequestedAppointment
        };
        await _appointmentRepository.CreateAppointmentReschedule(appointmentReschedule);
        return RedirectToPage("EditSlots", new {id = Id});
    }
}
