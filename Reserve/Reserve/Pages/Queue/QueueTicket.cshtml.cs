using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Core.Features.Queue;
using Reserve.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Reserve.Pages.Queue;

[BindProperties]
public class QueueTicketModel : PageModel
{
    private readonly IQueueRepository _queueRepository;
    public string CurrentUrl { get; set; }
    public QueueEventView CurrentQueueView { get; set; }
    public QueueTicketView QueueTicketView { get; set; }
    public QueueTicketModel(IQueueRepository queueRepository)
    {
        _queueRepository = queueRepository;
    }


    public async Task<IActionResult> OnGet(string QueueTicketId, string QueueId)
    {
        Guid QueueTicketid = GuidShortener.RestoreGuid(QueueTicketId);
        Guid Queueid = GuidShortener.RestoreGuid(QueueId);
        CurrentUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";
        QueueEvent CurrentQueue = await _queueRepository.GetQueueEventByID(Queueid.ToString());
        CurrentQueueView = new QueueEventView()
        {
            Id = CurrentQueue.Id,
            Title = CurrentQueue.Title,
            OrganizerEmail = CurrentQueue.OrganizerEmail,
            Description = CurrentQueue.Description,
            CurrentNumberServed = CurrentQueue.CurrentNumberServed,
            TicketCounter = CurrentQueue.TicketCounter,
            LastReset = CurrentQueue.LastReset
        };

        QueueTicket QueueTicket = await _queueRepository.GetQueueTicketByID(QueueTicketid.ToString());
        QueueTicketView = new QueueTicketView()
        {
            Id = QueueTicket.Id,
            CustomerName = QueueTicket.CustomerName,
            QueueNumber = QueueTicket.QueueNumber,
            QueueEvent = CurrentQueue,
            Status = QueueTicket.Status
        };
        return Page();
    }
}
