using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Core.Features.Appointment;


namespace Reserve.Pages.Appointment;
[BindProperties]
public class ReserverDetailsModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string Id { get; set; }
    public Availability AvailabilitySlot { get; set; }
    public AppointmentDetails Appointment { get; set; }
    private readonly IAppointmentRepository _appointmentRepository;
    private IValidator<AppointmentDetails> _validator;
    public ReserverDetailsModel(IAppointmentRepository appointmentRepository, IValidator<AppointmentDetails> validator)
    {
        _appointmentRepository = appointmentRepository;
        _validator = validator;
    }
    public async Task<IActionResult> OnGet()
    {
        AvailabilitySlot = await _appointmentRepository.GetSlotByIdAsync(Id);
        if(AvailabilitySlot is null)
        {
            return RedirectToPage("/appointment-error");
        }
        return Page();
    }
    public async Task<IActionResult> OnPost()
    {
        AvailabilitySlot = await _appointmentRepository.GetSlotByIdAsync(Id);
        Appointment.Slot = AvailabilitySlot;
        Appointment.Slot.AppointmentCalendar = AvailabilitySlot.AppointmentCalendar;
        ValidationResult result = await _validator.ValidateAsync(Appointment);
        if (!result.IsValid)
        {
            result.AddToModelState(this.ModelState, "Appointment");
            return Page();
        }
        else
        {
            Appointment = await _appointmentRepository.CreateAppointmentMeetingAsync(Appointment);
            if(Appointment is null)
            {
                return RedirectToPage("AppointmentError");
            }
            return RedirectToPage("MeetingNotification", new {id = Appointment.Id});
        }
    }
}
