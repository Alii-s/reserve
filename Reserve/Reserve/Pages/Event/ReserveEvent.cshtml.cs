using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Core.Features.Event;
using Reserve.Core.Features.MailService;
using static Reserve.Core.Features.MailService.MailFormats;


namespace Reserve.Pages.Event;
[BindProperties]
public class ReserveEventModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? Id { get; set; }

    public CasualEventView? Event { get; set; }
    public CasualTicketInput Ticket { get; set; } = new();
    private readonly IEventRepository _eventRepository;
    private readonly IValidator<CasualTicketInput> _validator;
    private readonly IEmailService _emailService;
    public ReserveEventModel(IEventRepository eventRepository, IValidator<CasualTicketInput> validator, IEmailService emailService)
    {
        _eventRepository = eventRepository;
        _validator = validator;
        _emailService = emailService;
    }
    public async Task<IActionResult> OnGet()
    {
        Event = await _eventRepository.GetByIdAsync(Id!);
        if(Event is null)
        {
            return RedirectToPage("EventError");
        }
        return Page();
    }
    public async Task<IActionResult> OnPost()
    {
        Event = await _eventRepository.GetByIdAsync(Id!);
        if(Event is null)
        {
            return RedirectToPage("EventError");
        }
        Ticket.CasualEvent = Event;
        ValidationResult result = await _validator.ValidateAsync(Ticket);
        if (!result.IsValid)
        {
            result.AddToModelState(this.ModelState, "Ticket");
        }
        else
        {
            var alreadyReserved = (await _eventRepository.CheckIfAlreadyReserved(Ticket)).ToList();
            if (alreadyReserved.Count != 0)
            {
                ModelState.AddModelError("Ticket.ReserverEmail", "This email is already reserved");
            }
            if (ModelState.IsValid)
            {
                Ticket = (await _eventRepository.AddReserverAsync(Ticket))!;
                if (Ticket is not null)
                {
                    MailRequest mailRequest = new MailRequest
                    {
                        ToEmail = Ticket.ReserverEmail,
                        Subject = "Reservation Successful",
                        Body = ReservationSuccessfulNotification(Ticket.Id.ToString())
                    };
                    //await _emailService.SendEmailAsync(mailRequest);
                    return RedirectToPage("ReservationNotification", new { id = Ticket.Id });
                }
                else
                {
                    return RedirectToPage("EventError");
                }
            }
        }
        return Page();
    }

}
