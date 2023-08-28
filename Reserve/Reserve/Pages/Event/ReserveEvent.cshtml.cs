using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Models.Event;
using Reserve.Repositories;

namespace Reserve.Pages.Event;
[BindProperties]
public class ReserveEventModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? Id { get; set; }

    public CasualEvent? Event { get; set; }
    public CasualTicket Ticket { get; set; } = new();
    private readonly IEventRepository _eventRepository;
    public ReserveEventModel(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }
    public async Task OnGet()
    {
        Event = await _eventRepository.GetByIdAsync(Id!);
    }
    public async Task<IActionResult> OnPost()
    {
        if (Event == null)
        {
            return NotFound();
        }
        else
        {
            Ticket.CasualEvent = Event;
            if (Event.CurrentCapacity >= Event.MaximumCapacity)
            {
                ModelState.AddModelError("Event.CurrentCapacity", "This event is full");
            }
            if (Event.Opened == false)
            {
                ModelState.AddModelError("Event.Opened", "This event's reservation is closed");
            }
            if (ModelState.IsValid)
            {
                Ticket = (await _eventRepository.AddReserverAsync(Ticket))!;
                if (Ticket is not null)
                {
                    return RedirectToPage("ReservationNotification", new { id = Ticket.Id });
                }
                else
                {
                    return RedirectToPage("./Error");
                }
            }
        }
        return Page();
    }

}
