using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Core.Features.Appointment;

namespace Reserve.Pages.Appointment;

public class RescheduleDetailsModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string Id { get; set; }
    public AppointmentReschedule RescheduleDetails { get; set; }
    private readonly IAppointmentRepository _appointmentRepository;
    public RescheduleDetailsModel(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository; 
    }
    public async Task OnGet()
    {
        RescheduleDetails = await _appointmentRepository.GetRescheduleByIdAsync(Id);
    }
    public async Task<IActionResult> OnPostDecline()
    {
        RescheduleDetails = await _appointmentRepository.GetRescheduleByIdAsync(Id);
        await _appointmentRepository.DeclineRescheduling(RescheduleDetails.Id.ToString());
        TempData["reschedule-request"] = "Reschedule request recorded successfully";
        return RedirectToPage("UserAppointmentDashboard", new { id = Id });
    }
    public async Task<IActionResult> OnPostAccept()
    {
        RescheduleDetails = await _appointmentRepository.GetRescheduleByIdAsync(Id);
        await _appointmentRepository.AcceptRescheduling(RescheduleDetails.Id.ToString());
        TempData["reschedule-request"] = "Reschedule request recorded successfully";
        return RedirectToPage("UserAppointmentDashboard", new { id = Id });
    }
}
