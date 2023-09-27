using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Core.Features.Event;
using Reserve.Helpers;

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
        CasualTicket? ticket = await _eventRepository.GetTicketByIdAsync(GuidShortener.RestoreGuid(Id!).ToString());
        if(ticket is null)
        {
            return RedirectToPage("EventError");
        }
        Ticket = new CasualTicketView
        {
            Id = ticket.Id,
            ReserverName = ticket.ReserverName,
            ReserverEmail = ticket.ReserverEmail,
            ReserverPhoneNumber = ticket.ReserverPhoneNumber,
            CasualEvent = ticket.CasualEvent,
        };
        return Page();
    }
}
