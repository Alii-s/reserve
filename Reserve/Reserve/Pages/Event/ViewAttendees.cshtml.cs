using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Core.Features.Event;
using Reserve.Repositories;

namespace Reserve.Pages.Event;

public class ViewAttendeesModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? Id { get; set; }
    public List<CasualTicket?> Attendees { get; set; } = new();
    public CasualEvent? Event { get; set; }
    private readonly IEventRepository _eventRepository;

    public ViewAttendeesModel(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }
    public async Task OnGet()
    {
        Event = await _eventRepository.GetByIdAsync(Id!);
        Attendees = await _eventRepository.GetAttendeesAsync(Id!);
    }
}
