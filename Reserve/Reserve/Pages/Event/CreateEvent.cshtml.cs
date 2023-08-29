using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Core.Features.Event;
using Reserve.Core.Features.MailService;
using Reserve.Repositories;
using System;
using System.Globalization;
using static Reserve.Helpers.ImageHelper;
using static Reserve.Core.Features.MailService.MailFormats;

namespace Reserve.Pages.Event;
[BindProperties]
public class CreateEventModel : PageModel
{
    public CasualEvent? NewEvent { get; set; }
    private readonly IEventRepository _eventRepository;
    private readonly IValidator<CasualEvent> _validator;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IEmailService _emailService;
    public CreateEventModel(IEventRepository eventRepository, IWebHostEnvironment webHostEnvironment, IValidator<CasualEvent> validator, IEmailService emailService)
    {
        _eventRepository = eventRepository;
        _webHostEnvironment = webHostEnvironment;
        _validator = validator;
        _emailService = emailService;
    }
    public void OnGet()
    {
    }
    public async Task<IActionResult> OnPost()
    {
        if(NewEvent is null)
        {
            return RedirectToPage("EventError");
        }
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
            if (NewEvent is not null) {
                MailRequest mailRequest = new MailRequest
                {
                    ToEmail = NewEvent.OrganizerEmail,
                    Subject = "Event Created",
                    Body = EventCreationNotification(NewEvent.Id.ToString())
                };
                await _emailService.SendEmailAsync(mailRequest);
                return RedirectToPage("CreationNotification", new { id = NewEvent.Id });
            }
            else
            {
                return RedirectToPage("EventError");
            }
        }
        else
        {
            result.AddToModelState(this.ModelState, "NewEvent");
        }
        return Page();
    }
}
