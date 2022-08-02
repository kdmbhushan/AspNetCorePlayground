using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SendEmailFromAspNetCoreMvc.Configuration;
using SendEmailFromAspNetCoreMvc.Models;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace SendEmailFromAspNetCoreMvc.Controllers
{
    public class EmailController : Controller
    {
        private readonly SMTPSettings _smtpSettings;

        public EmailController(IConfiguration configuration)
        {
            _smtpSettings = new SMTPSettings();
            configuration.GetSection("SMTP_Settings").Bind(_smtpSettings);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendEmail([FromForm] EmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (MailMessage mm = new MailMessage(_smtpSettings.Email, model.To))
                    {
                        mm.Subject = model.Subject;
                        mm.Body = model.Body;

                        if (!string.IsNullOrEmpty(model.CC))
                            mm.CC.Add(model.CC);

                        if (model.Attachment != null)
                        {
                            string fileName = Path.GetFileName(model.Attachment.FileName);
                            mm.Attachments.Add(new Attachment(model.Attachment.OpenReadStream(), fileName));
                        }
                        mm.IsBodyHtml = false;
                        using (SmtpClient smtp = new SmtpClient())
                        {
                            smtp.Host = _smtpSettings.Host;
                            smtp.EnableSsl = true;
                            NetworkCredential NetworkCred = new NetworkCredential(_smtpSettings.Email, _smtpSettings.Password);
                            smtp.UseDefaultCredentials = true;
                            smtp.Credentials = NetworkCred;
                            smtp.Port = _smtpSettings.Port;
                            smtp.Send(mm);

                            ViewBag.Message = "Email Sent Successfully";
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    ViewBag.ErrorMessage = ex.Message;
                }
            }
            return View("Index", model);
        }
    }
}
