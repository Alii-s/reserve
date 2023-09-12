using FluentValidation;
using FluentValidation.AspNetCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Reserve.Core.Features.Appointment;

public class AppointmentDetails
{
    public Guid Id { get; set; }
    public string ReserverName { get; set; }
    public string ReserverEmail { get; set; }
    public string ReserverPhoneNumber { get; set; }
    public Availability Slot { get; set; }
}

public class AppointmentDetailsValidator: AbstractValidator<AppointmentDetails>
{
    public AppointmentDetailsValidator()
    {
        RuleFor(x => x.ReserverName).NotNull().NotEmpty().WithMessage("Please enter your name.");
        RuleFor(x => x.ReserverEmail).NotNull().EmailAddress().WithMessage("Please enter valid email format").NotEmpty().WithMessage("Please enter your email.");
        RuleFor(x => x.ReserverPhoneNumber).NotEmpty().WithMessage("Please enter your phone number.");
        RuleFor(x => x.Slot.Available).Equal(true).WithMessage("This slot has already been reserved.");
    }

}
