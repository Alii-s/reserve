using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Models.Event;
using Reserve.Repositories;

namespace Reserve.Pages.Event
{
    public class EventDetailsModel : PageModel
    {
        private readonly IEventRepository _eventRepository;
        public CasualEvent DetailedEvent { get; set; }
        public EventDetailsModel(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }
        public async Task OnGet(string id)
        {
            DetailedEvent = await _eventRepository.GetById(id);
        }
    }
}