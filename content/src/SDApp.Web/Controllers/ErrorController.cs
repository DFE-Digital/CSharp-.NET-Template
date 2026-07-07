using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace SDApp.Web.Controllers;

public class ErrorController : Controller
{
    [Route("error")]
    public IActionResult Error(int? code)
    {
        var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        var statusCodeReExecuteFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

        if (exceptionHandlerPathFeature is null && statusCodeReExecuteFeature is null)
        {
            code = 404;
        }

        var viewName = code switch
        {
            404 => "NotFound",
            _ => "Error"
        };

        var result = View(viewName);
        result.StatusCode = code;
        return result;
    }
}