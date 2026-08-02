using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Configuration.Controllers;

public class HomeController : Controller
{
    private readonly CompanySettings _companySettings;

    public HomeController(IOptions<CompanySettings> companySettings)
    {
        _companySettings = companySettings.Value;
    }

    public IActionResult CompanyDetails()
    {
        return View(_companySettings);
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
