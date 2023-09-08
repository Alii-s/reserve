using System.ComponentModel.DataAnnotations;
using FluentValidation;
namespace Reserve.Core.Features.Queue;
public class QueueEvent
{
    public Guid Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string OrganizerEmail { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public int CurrentNumberServed { get; set; }
    [Required]
    public int TicketCounter { get; set; }
}

public class QueueEventValidator : AbstractValidator<QueueEvent>
{
    public QueueEventValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
        RuleFor(x => x.OrganizerEmail)
            .NotEmpty().WithMessage("Organizer Email is required")
            .EmailAddress().WithMessage("Correct Email Address format is required");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
        RuleFor(x => x.CurrentNumberServed).NotNull().WithMessage("Current Number Served is required").GreaterThanOrEqualTo(0).WithMessage("Current Number Served must be 0 or greater");
        RuleFor(x => x.TicketCounter).NotNull().WithMessage("Ticket Counter is required").GreaterThanOrEqualTo(0).WithMessage("Ticket Counter must be 0 or greater");
    }
}