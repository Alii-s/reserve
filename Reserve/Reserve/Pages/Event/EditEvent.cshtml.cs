using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Core.Features.Event;
using Reserve.Helpers;
using System.Globalization;
using static Reserve.Helpers.DateTimeHelper;
using static Reserve.Helpers.ImageHelper;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Reserve.Pages.Event;
[BindProperties]
public class EditEventModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? Id { get; set; }
    public CasualEventInput? EditEvent { get; set; }
    private readonly IEventRepository _eventRepository;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IValidator<CasualEventInput> _validator;
    public EditEventModel(IEventRepository eventRepository, IWebHostEnvironment webHostEnvironment, IValidator<CasualEventInput> validator)
    {
        _eventRepository = eventRepository;
        _webHostEnvironment = webHostEnvironment;
        _validator = validator;
    }
    public async Task<IActionResult> OnGet()
    {
        CasualEvent? editEvent = await _eventRepository.GetByIdForEditAsync(GuidShortener.RestoreGuid(Id!).ToString());
        if(editEvent is null)
        {
            return RedirectToPage("EventError");
        }
        EditEvent = new CasualEventInput {
            Id = editEvent.Id,
            Title = editEvent.Title,
            OrganizerName = editEvent.OrganizerName,
            OrganizerEmail = editEvent.OrganizerEmail,
            MaximumCapacity = editEvent.MaximumCapacity,
            Location = editEvent.Location,
            StartDate = editEvent.StartDate,
            EndDate = editEvent.EndDate,
            Opened = editEvent.Opened,
            Description = editEvent.Description,
            ImageUrl = editEvent.ImageUrl,
            CurrentCapacity = editEvent.CurrentCapacity
        };
        return Page();
    }
    public async Task<IActionResult> OnPost()
    {
        if(EditEvent is null)
        {
            return RedirectToPage("EventError");
        }
        IFormFile? imageFile = Request.Form.Files.FirstOrDefault();
        if (imageFile is not null)
        {
            EditEvent.ImageUrl = @"\images\event\" + imageFile.FileName;
            ValidationResult result = await _validator.ValidateAsync(EditEvent);
            if (result.IsValid)
            {
                EditEvent.ImageUrl = SaveImage(imageFile, _webHostEnvironment);
                string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/event"));
                string fullPath = Path.Combine(path, Path.GetFileName(EditEvent.ImageUrl));
                if (imageFile.Length >= 3 * 1024 * 1024)
                {
                    using (Image image = Image.Load(imageFile.OpenReadStream()))
                    {
                        int width = image.Width / 2;
                        int height = image.Height / 2;
                        image.Mutate(x => x.Resize(width, height));
                        image.Save(fullPath);
                    }
                }
                CasualEvent? editEvent = new CasualEvent
                {
                    Id = GuidShortener.RestoreGuid(Id!),
                    Title = EditEvent.Title,
                    OrganizerName = EditEvent.OrganizerName,
                    OrganizerEmail = EditEvent.OrganizerEmail,
                    MaximumCapacity = EditEvent.MaximumCapacity,
                    CurrentCapacity = EditEvent.CurrentCapacity,
                    Location = EditEvent.Location,
                    StartDate = EditEvent.StartDate,
                    EndDate = EditEvent.EndDate,
                    Opened = EditEvent.Opened,
                    Description = EditEvent.Description,
                    ImageUrl = EditEvent.ImageUrl
                };
                await _eventRepository.UpdateAsync(editEvent);
                return RedirectToPage("ViewAttendees", new { id = Id });
            }
            else
            {
                result.AddToModelState(this.ModelState, "EditEvent");
            }
        }
        else
        {
            ValidationResult result = await _validator.ValidateAsync(EditEvent);
            if (result.IsValid)
            {
                CasualEvent? editEvent = new CasualEvent
                {
                    Id = GuidShortener.RestoreGuid(Id!),
                    Title = EditEvent.Title,
                    OrganizerName = EditEvent.OrganizerName,
                    OrganizerEmail = EditEvent.OrganizerEmail,
                    MaximumCapacity = EditEvent.MaximumCapacity,
                    CurrentCapacity = EditEvent.CurrentCapacity,
                    Location = EditEvent.Location,
                    StartDate = EditEvent.StartDate,
                    EndDate = EditEvent.EndDate,
                    Opened = EditEvent.Opened,
                    Description = EditEvent.Description,
                    ImageUrl = EditEvent.ImageUrl
                };
                await _eventRepository.UpdateAsync(editEvent);
                return RedirectToPage("ViewAttendees", new { id = Id });
            }
            else
            {
                result.AddToModelState(this.ModelState, "EditEvent");
            }
        }
        return Page();
    }
}
