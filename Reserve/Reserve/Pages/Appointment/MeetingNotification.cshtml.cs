using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Reserve.Pages.Appointment;

public class MeetingNotificationModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string Id { get; set; }
    public void OnGet()
    {
    }
}
