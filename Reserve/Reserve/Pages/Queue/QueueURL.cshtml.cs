using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using Reserve.Helpers;

namespace Reserve.Pages.Queue;

public class QueueURLModel : PageModel
{
    public Guid QueueId { get; set; }
    public string ForURL { get; set; }

    public void OnGet(string id)
    {
        ForURL = id;
        QueueId = GuidShortener.RestoreGuid(id);
    }
}