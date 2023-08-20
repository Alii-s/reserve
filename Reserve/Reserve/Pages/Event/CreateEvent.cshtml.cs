using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reserve.Models.Event;
using Reserve.Repositories;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using static Reserve.Helpers.DateTimeHelper;

namespace Reserve.Pages.Event;
[BindProperties]
public class CreateEventModel : PageModel
{
    [Required]
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
    public CreateEventModel(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }
    public void OnGet()
    {
    }
    public async Task<IActionResult> OnPost()
    {
        NewEvent.StartDate = DateTimeBuilder(StartDate, StartTime);
        NewEvent.EndDate = DateTimeBuilder(EndDate, EndTime);
        if (ModelState.IsValid)
        {
            await _eventRepository.Create(NewEvent);
            return RedirectToPage("");
        }
        return Page();
    }
}
