using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Core.Features.Appointment;

namespace Reserve.Pages.Appointment;

public class FreeSlotModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string Id { get; set; }
    private readonly IAppointmentRepository _appointmentRepository;
    public Availability FreeSlot { get; set; }
    public FreeSlotModel(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }
    public async Task<IActionResult> OnGet()
    {
        if(string.IsNullOrEmpty(Id))
        {
            return RedirectToPage("/Appointment/AppointmentError");
        }
        FreeSlot = await _appointmentRepository.GetSlotByIdAsync(Id);
        if(FreeSlot is null)
        {
            return RedirectToPage("/Appointment/AppointmentError");
        }
        return Page();

    }
}
