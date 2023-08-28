using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Core.Features.Event;
using Reserve.Repositories;
using System;
using System.Globalization;
using static Reserve.Helpers.ImageHelper;

namespace Reserve.Pages.Event;
[BindProperties]
public class CreateEventModel : PageModel
{
    public CasualEvent? NewEvent { get; set; }
    private readonly IEventRepository _eventRepository;
    private readonly IValidator<CasualEvent> _validator;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public CreateEventModel(IEventRepository eventRepository, IWebHostEnvironment webHostEnvironment, IValidator<CasualEvent> validator)
    {
        _eventRepository = eventRepository;
        _webHostEnvironment = webHostEnvironment;
        _validator = validator;
    }
    public void OnGet()
    {
    }
    public async Task<IActionResult> OnPost()
    {
        IFormFile? imageFile = Request.Form.Files.FirstOrDefault();
        ValidationResult result = await _validator.ValidateAsync(NewEvent);
        if(imageFile is not null)
        {
            NewEvent.ImageUrl = @"\images\event\" + imageFile.FileName;
        }
        if (result.IsValid)
        {
            if (imageFile is not null)
            {
                NewEvent.ImageUrl = SaveImage(imageFile, _webHostEnvironment);
            }
            NewEvent = await _eventRepository.CreateAsync(NewEvent);
            return RedirectToPage("CreationNotification", new { id = NewEvent.Id });
        }
        else
        {
            result.AddToModelState(this.ModelState, "NewEvent");
        }
        return Page();
    }
}
