using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Core.Features.Event;
using System.Globalization;
using static Reserve.Helpers.DateTimeHelper;
using static Reserve.Helpers.ImageHelper;

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
        EditEvent = await _eventRepository.GetByIdForEditAsync(Id!);
        if(EditEvent is null)
        {
            return RedirectToPage("EventError");
        }
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
                await _eventRepository.UpdateAsync(EditEvent);
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
                await _eventRepository.UpdateAsync(EditEvent);
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
