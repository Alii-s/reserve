using FluentValidation;
using System.ComponentModel.DataAnnotations;
namespace Reserve.Core.Features.Queue;

public class QueueTicket
{
    public Guid Id { get; set; }
    [Required]
    public string CustomerName { get; set; }
    [Required]
    public string CustomerPhoneNumber { get; set; }
    [Required]
    public int QueueNumber { get; set; }
    [Required]
    public QueueEvent QueueEvent { get; set; }
    public string? Status { get; set; }
}

public class QueueTicketValidator : AbstractValidator<QueueTicket>
{
    public QueueTicketValidator()
    {
        RuleFor(x => x.CustomerName).NotEmpty().WithMessage("Customer Name is required");
        RuleFor(x => x.CustomerPhoneNumber).NotEmpty().WithMessage("Customer Phone Number is required");
        RuleFor(x => x.QueueNumber).NotNull().WithMessage("Queue Number is required").GreaterThanOrEqualTo(0).WithMessage("Queue Number must be 0 or greater");
        RuleFor(x => x.QueueEvent).NotNull().WithMessage("Queue Event doesnt exist");
    }
}