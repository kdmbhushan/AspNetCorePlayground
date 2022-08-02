using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SendEmailFromAspNetCoreMvc.Models
{
    public class EmailViewModel
    {
        [Required(ErrorMessage = "To Field is required.")]
        [Display(Name = "To")]
        public string To { get; set; }

        [Display(Name = "CC")]
        public string CC { get; set; }

        [Required(ErrorMessage = "Subject is required.")]
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Body is required.")]
        [Display(Name = "Body")]
        public string Body { get; set; }

        [Display(Name = "Attachment")]
        public IFormFile Attachment { get; set; }
    }
}
