using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Reserve.Core.Features.Queue;
using FluentValidation;

namespace Reserve.Pages.Queue;

[BindProperties]
public class CreateQueueModel : PageModel
{
    private readonly IQueueRepository _queueRepository;
    private readonly IValidator<QueueEvent> _validator;
    [Required]
    public QueueEvent NewQueue { get; set; }

    public CreateQueueModel(IQueueRepository queueRepository, IValidator<QueueEvent> validator)
    {
        _queueRepository = queueRepository;
        _validator = validator;
    }

    public async Task<IActionResult> OnPost()
    {
        var validationResult = await _validator.ValidateAsync(NewQueue);

        if (!validationResult.IsValid)
        {
            return Page();
        }
        NewQueue.CurrentNumberServed = 1;
        NewQueue = await _queueRepository.Create(NewQueue);
        return RedirectToPage("QueueURL", new { id = NewQueue.Id });
    }
}
