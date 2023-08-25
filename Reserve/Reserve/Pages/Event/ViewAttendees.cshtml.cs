using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Models.Event;
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
        Event = await _eventRepository.GetById(Id!);
        Attendees = await _eventRepository.GetAttendees(Id!);
    }
}
