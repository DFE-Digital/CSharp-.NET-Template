using Microsoft.AspNetCore.Mvc;

namespace SDApp.Web.Controllers;

public class HomeController : Controller
{
    [HttpGet("")]
    public IActionResult Index() => View();
}