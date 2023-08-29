using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Core.Features.Event;
using Reserve.Repositories;


namespace Reserve.Pages.Event;
[BindProperties]
public class ReserveEventModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? Id { get; set; }

    public CasualEvent? Event { get; set; }
    public CasualTicket Ticket { get; set; } = new();
    private readonly IEventRepository _eventRepository;
    private readonly IValidator<CasualTicket> _validator;
    public ReserveEventModel(IEventRepository eventRepository, IValidator<CasualTicket> validator)
    {
        _eventRepository = eventRepository;
        _validator = validator;
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
        if (result.IsValid)
        {
            Ticket = (await _eventRepository.AddReserverAsync(Ticket))!;
            return RedirectToPage("ReservationNotification", new { id = Ticket.Id });
        }
        else
        {
            result.AddToModelState(this.ModelState, "Ticket");
        }
        return Page();
    }

}
