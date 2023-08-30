using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Core.Features.Queue;

namespace Reserve.Pages.Queue;

[BindProperties]
public class QueueTicketModel : PageModel
{
    private readonly IQueueRepository _queueRepository;
    private readonly IValidator<QueueTicketModel> _validator;

    public QueueTicketModel(IQueueRepository queueRepository)
    {
        _queueRepository = queueRepository;
    }
    public Guid QueueId { get; set; } 
    public Guid QueueTicketId { get; set; }
    public QueueEvent CurrentQueue { get; set; }

    public QueueTicketModel(IQueueRepository queueRepository, IValidator<QueueTicketModel> validator)
    {
        _queueRepository = queueRepository;
        _validator = validator;
    }


    public async Task<IActionResult> OnGet()
    {
        var validationResult = await _validator.ValidateAsync(this);

        if (!validationResult.IsValid)
        {
            return Page();
        }

        CurrentQueue = await _queueRepository.GetByID(QueueId.ToString());
        return Page();
    }
}
