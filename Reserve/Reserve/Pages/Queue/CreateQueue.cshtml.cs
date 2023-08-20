using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Models.Queue;
using System.ComponentModel.DataAnnotations;

namespace Reserve.Pages.Queue
{
    public class CreateQueueModel : PageModel
    {
        [Required]
        public QueueEvent NewQueue { get; set; }

        public void OnGet()
        {
        }
    }
}
