using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Reserve.Core.Features.Queue;

namespace Reserve.Pages.Queue;

[BindProperties]
public class CreateQueueModel : PageModel
{
    private readonly IQueueRepository _queueRepository;
    [Required]
    public QueueEvent NewQueue { get; set; }

    public CreateQueueModel(IQueueRepository queueRepository)
    {
        _queueRepository = queueRepository;
    }

    public async Task<IActionResult> OnPost()
    {
        NewQueue.CurrentNumberServed = 1;
        NewQueue = await _queueRepository.Create(NewQueue);
        return RedirectToPage("QueueURL", new { id = NewQueue.Id });
    }
}
