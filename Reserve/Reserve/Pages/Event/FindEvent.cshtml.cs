using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Core.Features.Event;

namespace Reserve.Pages.Event;

public class FindEventModel : PageModel
{
    public List<CasualEventView> CasualEvents = new();
    private readonly IEventRepository _eventRepository;
    public FindEventModel(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }
    public async Task<IActionResult> OnGet()
    {
        CasualEvents = await _eventRepository.GetAllEvents();
        if(CasualEvents is null)
        {
            return RedirectToPage("/Event/EventError");
        }
        return Page();
    }
}
