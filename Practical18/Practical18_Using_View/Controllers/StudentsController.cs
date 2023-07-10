using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Practical18_Using_View.Data;
using Practical18_Using_View.DataModels;
using Practical18_Using_View.ViewModels;
using System.Net;
using System.Net.Http.Headers;

namespace Practical18_Using_View.Controllers
{
	public class StudentsController : Controller
	{
		private readonly MyContext _context;
		private readonly HttpClient _httpClient;
		private readonly IMapper _mapper;

		public StudentsController(MyContext context, HttpClient httpClient, IMapper mapper)
		{
			_context = context;
			_httpClient = httpClient;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var response = await _httpClient.GetAsync("https://localhost:7098/api/Students");

			if (response.StatusCode == HttpStatusCode.OK)
			{
				var jsonResult = await response.Content.ReadAsStringAsync();
				var result = JsonConvert.DeserializeObject<List<StudentViewModel>>(jsonResult);
				return View(result);
			}
			return Problem("Entity set 'MyContext.Students'  is null.");
		}

		[HttpGet]
		public async Task<IActionResult> Details(int id)
		{
			var response = await _httpClient.GetAsync($"https://localhost:7098/api/Students/{id}");

			if (response.StatusCode == HttpStatusCode.OK)
			{
				var jsonResult = await response.Content.ReadAsStringAsync();
				var result = JsonConvert.DeserializeObject<StudentViewModel>(jsonResult);
				return View(result);
			}
			return NotFound();
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,EnrollmentNumber,FirstName,LastName,Email,DateOfBirth,Gender")] StudentViewModel Students)
		{
			if (ModelState.IsValid)
			{
				var dataModel = _mapper.Map<StudentModel>(Students);
				var jsonData = JsonConvert.SerializeObject(dataModel);

				var response = await _httpClient.PostAsync("https://localhost:7098/api/Students", new StringContent(jsonData, mediaType: MediaTypeWithQualityHeaderValue.Parse("application/json")));

				if (response.StatusCode == HttpStatusCode.Created)
				{
					return RedirectToAction(nameof(Index));
				}
				return BadRequest(response);
			}
			return View(Students);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var response = await _httpClient.GetAsync($"https://localhost:7098/api/Students/{id}");

			if (response.StatusCode == HttpStatusCode.OK)
			{
				var jsonResult = await response.Content.ReadAsStringAsync();
				var result = JsonConvert.DeserializeObject<StudentViewModel>(jsonResult);
				return View(result);
			}
			return NotFound();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,EnrollmentNumber,FirstName,LastName,Email,DateOfBirth,Gender")] StudentViewModel Students)
		{
			if (id != Students.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					var dataModel = _mapper.Map<StudentModel>(Students);
					var jsonData = JsonConvert.SerializeObject(dataModel);

					var response = await _httpClient.PutAsync($"https://localhost:7098/api/Students/{id}", new StringContent(jsonData, mediaType: MediaTypeWithQualityHeaderValue.Parse("application/json")));

					if (response.StatusCode == HttpStatusCode.NoContent)
					{
						return RedirectToAction(nameof(Index));
					}
					return Problem($"{response.StatusCode} Problem occured during editing."); ;
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!StudentsExists(Students.Id))
					{
						return NotFound();
					}
					throw;
				}
			}
			return View(Students);
		}

		[HttpGet]
		public async Task<IActionResult> Delete(int id)
		{
			var response = await _httpClient.GetAsync($"https://localhost:7098/api/Students/{id}");

			if (response.StatusCode == HttpStatusCode.OK)
			{
				var jsonResult = await response.Content.ReadAsStringAsync();
				var result = JsonConvert.DeserializeObject<StudentViewModel>(jsonResult);
				return View(result);
			}
			return NotFound();
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			try
			{
				var response = await _httpClient.DeleteAsync($"https://localhost:7098/api/Students/{id}");

				if (response.StatusCode == HttpStatusCode.NoContent)
				{
					return RedirectToAction(nameof(Index));
				}
				return Problem($"{response.StatusCode} Problem occured during editing."); ;
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!StudentsExists(id))
				{
					return NotFound();
				}
				throw;
			}
		}

		private bool StudentsExists(int id)
		{
			return (_context.Students?.Any(e => e.Id == id)).GetValueOrDefault();
		}
	}
}
