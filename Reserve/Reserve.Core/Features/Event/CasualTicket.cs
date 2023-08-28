using FluentValidation;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reserve.Core.Features.Event;

public class CasualTicket
{
    public Guid Id { get; set; }
    public string? ReserverName { get; set; }
    public string? ReserverEmail { get; set; }
    public string? ReserverPhoneNumber { get; set; }
    public CasualEvent? CasualEvent { get; set; }
}

public class CasualTicketValidator : AbstractValidator<CasualTicket>
{
    public CasualTicketValidator()
    {
        RuleFor(x => x.ReserverName).NotEmpty().WithMessage("Reserver Name is required");
        RuleFor(x => x.ReserverEmail).EmailAddress().WithMessage("Correct Email Address format is required");
        RuleFor(x => x.ReserverPhoneNumber).NotEmpty().WithMessage("Reserver Phone Number is required");
    }
}