using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Models.Event;
using Reserve.Repositories;

namespace Reserve.Pages.Event;
[BindProperties]
public class ReserverDetailsModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? Id { get; set; }
    public CasualTicket? Ticket { get; set; }
    private readonly IEventRepository _eventRepository;
    public ReserverDetailsModel(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }
    public async Task OnGet()
    {
        Ticket = await _eventRepository.GetTicketByIdAsync(Id!);
    }
    public async Task<IActionResult> OnPost(Guid? deletedTicketId, Guid? eventId)
    {
        await _eventRepository.CancelReservationAsync(deletedTicketId, eventId);
        return RedirectToPage("CancelNotification");
    }

}
