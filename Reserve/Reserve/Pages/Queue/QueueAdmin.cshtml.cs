using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Core.Features.Queue;

namespace Reserve.Pages.Queue
{
    public class QueueAdminModel : PageModel
    {
        private readonly IValidator<QueueTicket> _validator;
        [BindProperty]
        public Guid Id { get; set; }
        private readonly IQueueRepository _queueRepository;
        public List<QueueTicket> Attendees { get; set; } = new List<QueueTicket>();
        public QueueAdminModel(IQueueRepository queueRepository, IValidator<QueueTicket> validator)
        {
            _queueRepository = queueRepository;
            _validator = validator;
        }
        public async Task<IActionResult> OnGet(Guid id)
        {
            Id = id;
            Attendees = await _queueRepository.GetAttendees(id.ToString());
            return Page();
        }
        public async Task<IActionResult> OnPostMarkAsReservedAsync(int queueNumber)
        {
            Attendees = await _queueRepository.GetAttendees(Id.ToString());
            var attendee = Attendees.FirstOrDefault(a => a.QueueNumber == queueNumber);
            var validationResult = await _validator.ValidateAsync(attendee);

            if (!validationResult.IsValid)
            {
                return RedirectToPage("/Queue/QueueAdmin", new { id = Id });
            }

            await _queueRepository.MarkAsReserved(attendee, queueNumber);
            return RedirectToPage("/Queue/QueueAdmin", new { id = Id });
        }

        public async Task<IActionResult> OnPostResetAsync()
        {
            await _queueRepository.ResetQueue(Id.ToString());
            return RedirectToPage("/Queue/QueueAdmin", new { id = Id });
        }
    }
}
