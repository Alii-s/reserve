using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Core.Features.Queue;

namespace Reserve.Pages.Queue;

[BindProperties]
public class QueueTicketModel : PageModel
{
    private readonly IQueueRepository _queueRepository;

    public QueueEvent CurrentQueue { get; set; }
    public QueueTicket QueueTicket { get; set; }
    public QueueTicketModel(IQueueRepository queueRepository)
    {
        _queueRepository = queueRepository;
    }


    public async Task<IActionResult> OnGet(Guid QueueTicketId, Guid QueueId)
    {
        CurrentQueue = await _queueRepository.GetQueueEventByID(QueueId.ToString());
        QueueTicket = await _queueRepository.GetQueueTicketByID(QueueTicketId.ToString());
        return Page();
    }
}
