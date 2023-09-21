using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reserve.Core.Features.Event;

public class CasualTicketInput
{
    public Guid Id { get; set; }
    public string? ReserverName { get; set; }
    public string? ReserverEmail { get; set; }
    public string? ReserverPhoneNumber { get; set; }
    public CasualEvent? CasualEvent { get; set; }
}

public class CasualTicketInputValidator : AbstractValidator<CasualTicketInput>
{
    public CasualTicketInputValidator()
    {
        RuleFor(x => x.ReserverName).NotEmpty().WithMessage("Reserver Name is required");
        RuleFor(x => x.ReserverEmail).NotEmpty().WithMessage("Email Address required").EmailAddress().WithMessage("Correct Email Address format is required");
        RuleFor(x => x.ReserverPhoneNumber).NotEmpty().WithMessage("Reserver Phone Number is required");
        RuleFor(x => x.CasualEvent).NotNull().Must(eventObj => eventObj?.Opened == true).WithMessage("Event is closed");
        RuleFor(x => x.CasualEvent).NotNull().Must(eventObj => eventObj?.CurrentCapacity < eventObj?.MaximumCapacity).WithMessage("Event is full");
    }
}
