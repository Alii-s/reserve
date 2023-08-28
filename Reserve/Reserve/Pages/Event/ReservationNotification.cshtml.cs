using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Reserve.Pages.Event;

public class ReservationNotificationModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? Id { get; set; }
    public void OnGet()
    {
    }
}
