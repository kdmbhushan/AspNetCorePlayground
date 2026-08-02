using Microsoft.AspNetCore.Mvc;
using SendingEmail.Models;
using SendingEmail.Services;

namespace SendingEmail.Controllers;

public class EmailController : Controller
{
    private readonly IEmailSender _emailSender;

    public EmailController(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SendEmail(EmailViewModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _emailSender.SendEmailAsync(model);
                ViewBag.Message = "Email Sent Successfully";
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
        }
        return View("Index", model);
    }
}
