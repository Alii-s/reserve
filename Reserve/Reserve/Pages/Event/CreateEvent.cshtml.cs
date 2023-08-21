using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Models.Event;
using Reserve.Repositories;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using static Reserve.Helpers.DateTimeHelper;
using static Reserve.Helpers.ImageHelper;

namespace Reserve.Pages.Event;
[BindProperties]
public class CreateEventModel : PageModel
{
    public CasualEvent NewEvent { get; set; }
    [Required]
    [Display(Name = "Start Date")]
    public string StartDate { get; set; }
    [Required]
    [Display(Name = "Start Time")]
    public string StartTime { get; set; }
    [Required]
    [Display(Name = "End Date")]
    public string EndDate { get; set; }
    [Required]
    [Display(Name = "End Time")]
    public string EndTime { get; set; }
    private readonly IEventRepository _eventRepository;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public CreateEventModel(IEventRepository eventRepository, IWebHostEnvironment webHostEnvironment)
    {
        _eventRepository = eventRepository;
        _webHostEnvironment = webHostEnvironment;
    }
    public void OnGet()
    {
    }
    public async Task<IActionResult> OnPost()
    {
        NewEvent.StartDate = DateTimeBuilder(StartDate, StartTime);
        NewEvent.EndDate = DateTimeBuilder(EndDate, EndTime);
        IFormFile? imageFile = Request.Form.Files.FirstOrDefault();
        if (imageFile is not null)
        {
            string imageExtension = Path.GetExtension(imageFile.FileName);
            if (imageExtension == ".jpg" || imageExtension == ".png" || imageExtension == ".jpeg")
            {
                NewEvent.ImageUrl = SaveImage(imageFile, _webHostEnvironment);
            }
            else
            {
                ModelState.AddModelError("NewEvent.ImageUrl", "Image must be in .jpg, .png, or .jpeg format");
            }
        }
        if (ModelState.IsValid)
        {
            NewEvent = await _eventRepository.Create(NewEvent);
            return RedirectToPage("CreationNotification", new {id = NewEvent.Id});
        }
        return Page();
    }
}
