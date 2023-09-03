using EdgeDB;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reserve.Core.Features.Event;

public class CasualEventInput
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? OrganizerName { get; set; }
    public string? OrganizerEmail { get; set; }
    public int? MaximumCapacity { get; set; }
    public int CurrentCapacity { get; set; }
    public string? Location { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool Opened { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
}

public class CasualEventInputValidator : AbstractValidator<CasualEventInput>
{
    public CasualEventInputValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
        RuleFor(x => x.OrganizerName).NotEmpty().WithMessage("Organizer Name is required");
        RuleFor(x => x.OrganizerEmail).NotEmpty().WithMessage("Email Address is required").EmailAddress().WithMessage("Correct Email Address format is required");
        RuleFor(x => x.MaximumCapacity).Cascade(CascadeMode.Stop).NotNull().WithMessage("Maximum Capacity is required").NotEqual(0).WithMessage("Capacity can't be zero");
        RuleFor(x => x.MaximumCapacity).GreaterThanOrEqualTo(x => x.CurrentCapacity).WithMessage("Current Capacity cannot be greater than Maximum Capacity");
        RuleFor(x => x.Location).NotEmpty().WithMessage("Location is required");
        RuleFor(x => x.EndDate).NotNull().WithMessage("End Date is required");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
        RuleFor(x => x.ImageUrl).Must(HaveValidImageExtension).WithMessage("Image must be a jpeg, jpg or png.");
    }
    private bool HaveValidImageExtension(string? imageUrl)
    {
        if (!string.IsNullOrEmpty(imageUrl))
        {
            string[] validExtensions = { ".jpeg", ".jpg", ".png" };
            string extension = Path.GetExtension(imageUrl).ToLower();
            return validExtensions.Contains(extension);
        }
        return true;
    }
}

