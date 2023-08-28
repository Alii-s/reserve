using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Models.Event;
using Reserve.Repositories;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using static Reserve.Helpers.DateTimeHelper;
using static Reserve.Helpers.ImageHelper;

namespace Reserve.Pages.Event;
[BindProperties]
public class EditEventModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? Id { get; set; }
    public CasualEvent? EditEvent { get; set; }
    private readonly IEventRepository _eventRepository;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public EditEventModel(IEventRepository eventRepository, IWebHostEnvironment webHostEnvironment)
    {
        _eventRepository = eventRepository;
        _webHostEnvironment = webHostEnvironment;

    }
    public async Task OnGet()
    {
        EditEvent = await _eventRepository.GetByIdAsync(Id!);
    }
    public async Task<IActionResult> OnPost()
    {
        if(EditEvent.MaximumCapacity < EditEvent.CurrentCapacity)
        {
            ModelState.AddModelError("EditEvent.MaximumCapacity", "Maximum capacity cannot be less than Current Capacity");
        }
        IFormFile? imageFile = Request.Form.Files.FirstOrDefault();
        if (imageFile is not null && imageFile.FileName != EditEvent.ImageUrl)
        {
            string imageExtension = Path.GetExtension(imageFile.FileName);
            if (imageExtension == ".jpg" || imageExtension == ".png" || imageExtension == ".jpeg")
            {
                EditEvent.ImageUrl = SaveImage(imageFile, _webHostEnvironment);
            }
            else
            {
                ModelState.AddModelError("EditEvent.ImageUrl", "Image must be in .jpg, .png, or .jpeg format");
            }
        }
        if (ModelState.IsValid)
        {
            await _eventRepository.UpdateAsync(EditEvent);
            return RedirectToPage("ViewAttendees", new { id = Id });
        }
        return Page();
    }
}
