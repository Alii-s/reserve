using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Models.Queue;
using Reserve.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Reserve.Pages.Queue;

[BindProperties]
public class QueueRegistrationModel : PageModel
{
    private readonly IQueueRepository _queueRepository;
    [Required]
    public QueueTicket NewQueueTicket { get; set; }

    public void OnGet(Guid id)
    {
        NewQueueTicket.QueueEventId = id;
    }

    public async Task<IActionResult> OnPost()
    {
        if(ModelState.IsValid)
        {
            NewQueueTicket.QueueNumber = await _queueRepository.GetNextQueueNumber(NewQueueTicket.QueueEventId.ToString());
            await _queueRepository.RegisterCustomer(NewQueueTicket);
            return RedirectToPage("QueueTicket", new { id = NewQueueTicket.Id });
        }
        return Page();
    }
}
