using Microsoft.AspNetCore.Mvc;

namespace Practical16.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SampleController : ControllerBase
{
    private readonly ILogger<SampleController> _logger;

    public SampleController(ILogger<SampleController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task Get()
    {
        _logger.LogInformation("Hello World API Print From End Point : " + HttpContext.Request.Path);
        _logger.LogInformation("Hello World API Request Method : " + HttpContext.Request.Method);
        _logger.LogInformation("Hello World API Request Host Name : " + HttpContext.Request.Host);
        _logger.LogInformation("Hello World API Request is Https( Use SSL ) ? : " + HttpContext.Request.IsHttps);
        _logger.LogInformation("Hello World API Request Protocol : " + HttpContext.Request.Protocol);
        await HttpContext.Response.WriteAsync("Hello World");
    }
}