using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Core.Features.Queue;
using System.ComponentModel.DataAnnotations;

namespace Reserve.Pages.Queue;

[BindProperties]
public class QueueRegistrationModel : PageModel
{
    private readonly IQueueRepository _queueRepository;
    private readonly IValidator<QueueTicket> _validator;
    [Required]
    public QueueTicket NewQueueTicket { get; set; }
    public Guid QueueEventId { get; set; }

    public QueueRegistrationModel(IQueueRepository queueRepository, IValidator<QueueTicket> validator)
    {
        _queueRepository = queueRepository;
        _validator = validator;
        NewQueueTicket = new QueueTicket();
    }

    public void OnGet(Guid id)
    {
        NewQueueTicket.QueueEventId = id;
    }

    public async Task<IActionResult> OnPost()
    {
        QueueEventId = NewQueueTicket.QueueEventId;
        var validationResult = await _validator.ValidateAsync(NewQueueTicket);

        if (!validationResult.IsValid)
        {
            return Page();
        }
        NewQueueTicket.QueueNumber = await _queueRepository.GetNextQueueNumber(NewQueueTicket.QueueEventId.ToString());
        await _queueRepository.IncrementTicketCounter(NewQueueTicket.QueueEventId.ToString());
        NewQueueTicket = await _queueRepository.RegisterCustomer(NewQueueTicket);
        NewQueueTicket.QueueEventId = QueueEventId;
        return RedirectToPage("QueueTicket", new { QueueTicketId = NewQueueTicket.Id, QueueId = NewQueueTicket.QueueEventId });
    }

}
