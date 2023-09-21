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
        List<CasualEvent> allEvents = await _eventRepository.GetAllEvents();
        if(CasualEvents is null)
        {
            return RedirectToPage("/Event/EventError");
        }
        foreach (var casualEvent in allEvents)
        {
            CasualEvents.Add(new CasualEventView
            {
                Id = casualEvent.Id,
                Title = casualEvent.Title,
                OrganizerName = casualEvent.OrganizerName,
                OrganizerEmail = casualEvent.OrganizerEmail,
                Description = casualEvent.Description,
                StartDate = casualEvent.StartDate,
                EndDate = casualEvent.EndDate,
                ImageUrl = casualEvent.ImageUrl,
                Opened = casualEvent.Opened,
                MaximumCapacity = casualEvent.MaximumCapacity,
                CurrentCapacity = casualEvent.CurrentCapacity,
            });
        }
        return Page();
    }
}
