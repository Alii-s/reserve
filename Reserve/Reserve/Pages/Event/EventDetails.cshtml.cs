using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Core.Features.Event;

namespace Reserve.Pages.Event;

public class EventDetailsModel : PageModel
{
    private readonly IEventRepository _eventRepository;
    public CasualEventView? DetailedEvent { get; set; }
    public EventDetailsModel(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }
    public async Task<IActionResult> OnGet(string id)
    {
        DetailedEvent = await _eventRepository.GetByIdAsync(id);
        if(DetailedEvent is null)
        {
            return RedirectToPage("EventError");
        }
        return Page();
    }
}
