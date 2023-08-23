using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Reserve.Models.Event;

public class CasualTicket
{
    public Guid Id { get; set; }
    [Required]
    [Display(Name = "Reserver Name")]
    public string? ReserverName { get; set; }
    [Required]
    [Display(Name = "Reserver Email")]
    public string? ReserverEmail { get; set; }
    [Required]
    [Display(Name = "Reserver Phone Number")]
    public string? ReserverPhoneNumber { get; set; }
    [ValidateNever]
    public CasualEvent? CasualEvent { get; set; }

}
