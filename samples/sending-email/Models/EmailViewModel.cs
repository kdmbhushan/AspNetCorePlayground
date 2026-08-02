using Microsoft.AspNetCore.Http;

namespace SendingEmail.Models;

public class EmailViewModel
{
    [Required(ErrorMessage = "Recipient email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [Display(Name = "To")]
    public string To { get; set; } = string.Empty;

    [Required(ErrorMessage = "Subject is required")]
    [Display(Name = "Subject")]
    public string Subject { get; set; } = string.Empty;

    [Required(ErrorMessage = "Message body is required")]
    [Display(Name = "Message")]
    public string Body { get; set; } = string.Empty;

    [Display(Name = "CC")]
    public string? CC { get; set; }

    [Display(Name = "Attachment")]
    public IFormFile? Attachment { get; set; }
}
