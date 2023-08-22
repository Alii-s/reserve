using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Models.Queue;
using Reserve.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Reserve.Pages.Queue
{
    public class CreateQueueModel : PageModel
    {
        private readonly IQueueRepository _queueRepository;
        [Required]
        public QueueEvent NewQueue { get; set; }

       public CreateQueueModel(IQueueRepository queueRepository)
        {
            _queueRepository = queueRepository;
        }

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPost()
        {
            NewQueue.CurrentNumberServed = 1;
            NewQueue = await _queueRepository.Create(NewQueue);
            return Page();
        }
    }
}
