using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Core.Features.Event;

namespace Reserve.Pages.Event;
[BindProperties]
public class ReserverDetailsModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? Id { get; set; }
    public CasualTicketView? Ticket { get; set; }
    private readonly IEventRepository _eventRepository;
    public ReserverDetailsModel(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }
    public async Task<IActionResult> OnGet()
    {
        Ticket = await _eventRepository.GetTicketByIdAsync(Id!);
        if(Ticket is null)
        {
            return RedirectToPage("EventError");
        }
        return Page();
    }
}
