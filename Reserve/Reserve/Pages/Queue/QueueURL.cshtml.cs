using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Reserve.Pages.Queue;

public class QueueURLModel : PageModel
{
    public Guid QueueId { get; set; }

    public void OnGet(Guid id)
    {
        QueueId = id;
    }
}
