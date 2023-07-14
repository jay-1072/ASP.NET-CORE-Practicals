using Microsoft.AspNetCore.Mvc;

namespace Practical_20.ErrorHandlerController;

public class ErrorController : Controller
{
    public IActionResult NotFoundEx()
    {
        return View();
    }
    public IActionResult Ambiguous()
    {
        return View();
    }
    public IActionResult BadRequestEx()
    {
        return View();
    }
    public IActionResult InternalServerError()
    {
        return View();
    }
    public IActionResult LoopDetected()
    {
        return View();
    }
    public IActionResult UnAuthorized()
    {
        return View();
    }
}