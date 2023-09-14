using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Core.Features.Queue;
using System.Text;

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
        public async Task<IActionResult> OnPostMarkAsReservedAsync(string Id, int queueNumber)
        {
            // Fetch the updated attendees data
            Attendees = await _queueRepository.GetAttendees(Id.ToString());
            var attendee = Attendees.FirstOrDefault(a => a.QueueNumber == queueNumber);
            var validationResult = await _validator.ValidateAsync(attendee);

            if (!validationResult.IsValid)
            {
                return new JsonResult(new { success = false });
            }

            await _queueRepository.MarkAsReserved(attendee, queueNumber);

            // Get the updated attendees HTML content
            Attendees = await _queueRepository.GetAttendees(Id.ToString());
            var updatedHtml = RenderAttendeesTableBody(Attendees);

            // Return a JSON result with success and updated HTML content
            return new JsonResult(new { success = true, html = updatedHtml });
        }


        // Helper method to render the attendees table body
        private string RenderAttendeesTableBody(List<QueueTicket> attendees)
        {
            var sb = new StringBuilder();

            foreach (var attendee in attendees)
            {
                // Render each table row here and append it to the StringBuilder
                sb.Append("<tr>");
                sb.Append("<td>").Append(attendee.QueueNumber).Append("</td>");
                sb.Append("<td>").Append(attendee.CustomerName).Append("</td>");
                sb.Append("<td>").Append(attendee.CustomerPhoneNumber).Append("</td>");
                sb.Append("<td>");
                sb.Append("<form method='post' id='markAsReservedForm-" + attendee.QueueNumber + "'>");
                sb.Append("<input type='hidden'  name='Id' value='" + Id + "' />");
                sb.Append("<input type='hidden' name='queueNumber' value='" + attendee.QueueNumber + "' />");
                sb.Append("<button type='button' class='btn reserve-blue-button' onclick='markAsReserved(" + attendee.QueueNumber + ")'>Mark as reserved</button>");
                sb.Append("</form>");
                sb.Append("</td>");
                sb.Append("</tr>");
            }

            return sb.ToString();
        }




        public async Task<IActionResult> OnPostResetAsync()
        {
            await _queueRepository.ResetQueue(Id.ToString());
            Attendees = await _queueRepository.GetAttendees(Id.ToString());
            var updatedHtml = RenderAttendeesTableBody(Attendees);
            return new JsonResult(new { success = true, html = updatedHtml });
        }
    }
}
