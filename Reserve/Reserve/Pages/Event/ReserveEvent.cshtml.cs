using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Core.Features.Event;
using Reserve.Core.Features.MailService;
using Reserve.Helpers;
using static Reserve.Core.Features.MailService.MailFormats;


namespace Reserve.Pages.Event;
[BindProperties]
public class ReserveEventModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    [ValidateNever]
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
        CasualEvent? eventToReserve = await _eventRepository.GetByIdAsync(GuidShortener.RestoreGuid(Id!).ToString());
        if(eventToReserve is null)
        {
            return RedirectToPage("EventError");
        }
        Event = new CasualEventView
        {
            Id = eventToReserve.Id,
            Title = eventToReserve.Title,
            OrganizerName = eventToReserve.OrganizerName,
            OrganizerEmail = eventToReserve.OrganizerEmail,
            Description = eventToReserve.Description,
            StartDate = eventToReserve.StartDate,
            EndDate = eventToReserve.EndDate,
            ImageUrl = eventToReserve.ImageUrl,
            Opened = eventToReserve.Opened,
            MaximumCapacity = eventToReserve.MaximumCapacity,
            CurrentCapacity = eventToReserve.CurrentCapacity,
        };
        return Page();
    }
    public async Task<IActionResult> OnPost()
    {
        Id = GuidShortener.RestoreGuid(Id).ToString();
        CasualEvent? eventToReserve = await _eventRepository.GetByIdAsync(Id);
        if(eventToReserve is null)
        {
            return RedirectToPage("EventError");
        }
        Event = new CasualEventView
        {
            Id = eventToReserve.Id,
            Title = eventToReserve.Title,
            OrganizerName = eventToReserve.OrganizerName,
            OrganizerEmail = eventToReserve.OrganizerEmail,
            Description = eventToReserve.Description,
            StartDate = eventToReserve.StartDate,
            EndDate = eventToReserve.EndDate,
            ImageUrl = eventToReserve.ImageUrl,
            Opened = eventToReserve.Opened,
            MaximumCapacity = eventToReserve.MaximumCapacity,
            CurrentCapacity = eventToReserve.CurrentCapacity,
        };
        Ticket.CasualEvent = eventToReserve;
        ValidationResult result = await _validator.ValidateAsync(Ticket);
        if (!result.IsValid)
        {
            result.AddToModelState(this.ModelState, "Ticket");
        }
        else
        {
            CasualTicket ticket = new CasualTicket
            {
                ReserverName = Ticket.ReserverName,
                ReserverEmail = Ticket.ReserverEmail,
                CasualEvent = Ticket.CasualEvent,
                ReserverPhoneNumber = Ticket.ReserverPhoneNumber
            };
            var alreadyReserved = (await _eventRepository.CheckIfAlreadyReserved(ticket)).ToList();
            if (alreadyReserved.Count != 0)
            {
                ModelState.AddModelError("Ticket.ReserverEmail", "This email is already reserved");
            }
            ModelState.Remove("Id");
            if (ModelState.IsValid)
            {
                ticket = (await _eventRepository.AddReserverAsync(ticket))!;
                if (Ticket is not null)
                {
                    MailRequest mailRequest = new MailRequest
                    {
                        ToEmail = Ticket.ReserverEmail,
                        Subject = "Reservation Successful",
                        Body = ReservationSuccessfulNotification(ticket.Id.ToString())
                    };
                    //await _emailService.SendEmailAsync(mailRequest);
                    return RedirectToPage("ReservationNotification", new { id = GuidShortener.ShortenGuid(ticket.Id )});
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
