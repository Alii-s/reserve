using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Core.Features.Queue;

namespace Reserve.Pages.Queue
{
    public class QueueAdminModel : PageModel
    {
        private readonly IQueueRepository _queueRepository;
        public List<QueueTicket> Attendees { get; set; } = new List<QueueTicket>();
        public QueueAdminModel(IQueueRepository queueRepository)
        {
            _queueRepository = queueRepository;
        }
        public async Task<IActionResult> OnGet(Guid id)
        {
            Attendees = await _queueRepository.GetAttendees(id.ToString());
            return Page();
        }
    }
}
