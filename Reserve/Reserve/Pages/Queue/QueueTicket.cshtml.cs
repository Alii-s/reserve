using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Models.Queue;
using Reserve.Repositories;

namespace Reserve.Pages.Queue;

[BindProperties]
public class QueueTicketModel : PageModel
{
    private readonly IQueueRepository _queueRepository;

    public QueueTicketModel(IQueueRepository queueRepository)
    {
        _queueRepository = queueRepository;
    }
    public Guid QueueId { get; set; } 
    public Guid QueueTicketId { get; set; }
    public QueueEvent CurrentQueue { get; set; }

    public async Task<IActionResult> OnGet()
    {
        CurrentQueue = await _queueRepository.GetByID(QueueId.ToString());
        return Page();
    }
}
