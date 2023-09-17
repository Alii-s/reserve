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
    public QueueEventInput NewQueue { get; set; }

    public CreateQueueModel(IQueueRepository queueRepository, IValidator<QueueEvent> validator)
    {
        _queueRepository = queueRepository;
        _validator = validator;
    }

    public async Task<IActionResult> OnPost()
    {
        var newQueueEvent = new QueueEvent
        {
            Title = NewQueue.Title,
            OrganizerEmail = NewQueue.OrganizerEmail,
            Description = NewQueue.Description,
            CurrentNumberServed = 1,
            TicketCounter = 0,
            LastReset = DateTime.UtcNow
        };
        var validationResult = await _validator.ValidateAsync(newQueueEvent);

        if (!validationResult.IsValid)
        {
            return Page();
        }
        newQueueEvent = await _queueRepository.Create(newQueueEvent);
        return RedirectToPage("QueueURL", new { id = newQueueEvent.Id });
    }
}
