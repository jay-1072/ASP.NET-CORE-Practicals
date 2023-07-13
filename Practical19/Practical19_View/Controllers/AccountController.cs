using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using Practical19_View.Models;
using Practical19_View.ViewModels;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Practical19_View.Controllers;

public class AccountController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;

    public AccountController(HttpClient httpClient, IMapper mapper, IConfiguration config)
    {
        _httpClient = httpClient;
        _mapper = mapper;
        _config = config;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if(ModelState.IsValid)
        {
            var dataModel = _mapper.Map<RegisterModel>(model);
            var jsonData = JsonConvert.SerializeObject(dataModel);

            var response = await _httpClient.PostAsync(_config["AuthenticationAPI:signUp"], new StringContent(jsonData, mediaType: MediaTypeWithQualityHeaderValue.Parse("application/json")));
            
            if (response.StatusCode == HttpStatusCode.Created)
            {
                return RedirectToAction(nameof(Login));
            }
            return BadRequest(response);
        }
        return View(model);
    }

    [HttpGet]   
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if(ModelState.IsValid)
        {
            var dataModel = _mapper.Map<LoginModel>(model);
            var jsonData = JsonConvert.SerializeObject(dataModel);

            var response = await _httpClient.PostAsync(_config["AuthenticationAPI:signIn"], new StringContent(jsonData, mediaType: MediaTypeWithQualityHeaderValue.Parse("application/json")));
            if(response.StatusCode == HttpStatusCode.OK)
            {
                dynamic content = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);
                var Token = content!.token.ToString();
                var User = content!.user.email.ToString();

                Response.Cookies.Append("X-AccessToken", Token, new CookieOptions() { HttpOnly = true });
                Response.Cookies.Append("X-Username", User, new CookieOptions() { HttpOnly = true });

                return View("Temp");
            }
            ModelState.AddModelError("", "Invalid Email or Password.");
        }
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> AllUser()
    {
        var parsedResult = JObject.Parse(Request.Cookies["X-AccessToken"]!.ToString());
        var kvp = parsedResult.Cast<KeyValuePair<string, JToken>>().ToList();
        var token = kvp[0].Value.ToString();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.PostAsync(_config["AuthenticationAPI:allUser"], null);
        
        if (response.StatusCode == HttpStatusCode.OK)
        {
            dynamic content = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);
            return View("AllUser", content.result);
        }

        if (Request.Cookies["X-Username"] == null)
        {
            return RedirectToAction(nameof(Login), "Account");
        }

        return View("~/Views/Shared/UnAuthorized.cshtml");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        var response = await _httpClient.PostAsync(_config["AuthenticationAPI:signOut"], null);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            Response.Cookies.Delete("X-Username");
            Response.Cookies.Delete("X-AccessToken");
            return RedirectToAction(nameof(Login));
        }
        return Problem("Oops, something went wrong please try again!");
    }
}