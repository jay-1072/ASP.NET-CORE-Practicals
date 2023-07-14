using Microsoft.AspNetCore.Mvc;

namespace Practical_20.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult BadRequestEx()
    {
        return StatusCode(400);
    }

    public IActionResult UnAuthorizeError()
    {
        return StatusCode(401);
    }

    public IActionResult NotFoundExample()
    {
        return StatusCode(404);
    }

    public IActionResult AmbiguityExample()
    {
        return StatusCode(300);
    }

    public IActionResult LoopEx()
    {
        return StatusCode(508);
    }

    public IActionResult InternalServerErrorEx()
    {
        return StatusCode(500);
    }
}