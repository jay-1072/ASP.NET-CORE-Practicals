using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Practical19_API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Practical19_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;
    private ApplicationUser _user;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IMapper mapper,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
        _config = configuration;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> RegisterUser(RegisterModel model)
    {
        var newUser = _mapper.Map<ApplicationUser>(model);

        var result = await _userManager.CreateAsync(newUser, model.Password);

        if (result.Succeeded)
        {
            _userManager.AddToRoleAsync(newUser, "User").Wait();
            return Created(string.Empty, string.Empty);
        }
        return Problem(result.Errors.First().Description, null, 500);
    }

    [HttpPost("SignIn")]
    public async Task<IActionResult> SignIn([FromBody] LoginModel model)
    {
        if (ModelState.IsValid)
        {
            var signIn = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            if (signIn.Succeeded)
            {
                _user = await _userManager.FindByEmailAsync(model.Email);
                var token = GenerateJSONWebToken(model);

                return Ok(new { Token = token, User = _user });
            }
            else
            {
                return Unauthorized();
            }
        }
        return BadRequest();
    }

    [HttpPost("SignOut")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("AllUser")]
    public IActionResult AllUser()
    {
        try
        {
            var result = _userManager.Users;
            return Ok(new { result });
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error! please try again");
        }
    }

    private async Task<string> GenerateJSONWebToken(LoginModel userInfo)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, _user.Email!)
        };

        var roles = await _userManager.GetRolesAsync(_user);
        foreach(var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:ValidIssuer"],
            audience: _config["JWT:ValidAudience"],
            claims: claims,
            //notBefore: DateTime.Now.AddDays(1),
            expires: DateTime.Now.AddMinutes(1),
            signingCredentials : credentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();

        var jwt = tokenHandler.WriteToken(token);
        return jwt;
    }
}