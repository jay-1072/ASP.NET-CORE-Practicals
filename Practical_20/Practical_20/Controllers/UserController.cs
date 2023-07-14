using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practical_20.Interfaces;
using Practical_20.Models;

namespace Practical_20.Controllers
{
	public class UserController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IRepository<User> _repository;
		private readonly ILogger<UserController> _logger;

		public UserController(IUnitOfWork unitOfWork, ILogger<UserController> logger)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
			_repository = _unitOfWork.GetRepository<User>();
		}

		public async Task<IActionResult> Index()
		{
			_logger.LogInformation("User Data!");
			return _repository.GetAll() != null ?
						View(_repository.GetAll().ToList()) :
						Problem("Entity set 'DatabaseContext.Users'  is null.");
		}

		public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _repository.GetAll() == null)
			{
				return NotFound();
			}

			var user = _repository.GetById(id ?? 0);
			if (user == null)
			{
				return NotFound();
			}

			_logger.LogInformation("Student data information requested!");

			return View(user);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Name,Email,Password")] User user)
		{
			if (ModelState.IsValid)
			{
				_repository.Insert(user);
				await _unitOfWork.SaveChangesAsync();
				_logger.LogInformation("User created!");

				return RedirectToAction(nameof(Index));
			}
			return View(user);
		}

		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null || _repository.GetAll() == null)
			{
				return NotFound();
			}

			var user = _repository.GetById(id ?? 0);
			if (user == null)
			{
				return NotFound();
			}
			return View(user);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Password")] User user)
		{
			if (id != user.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_repository.Update(user);
					_logger.LogInformation("User data edited!");
					await _unitOfWork.SaveChangesAsync();

				}
				catch (DbUpdateConcurrencyException)
				{
					if (!UserExists(user.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			return View(user);
		}

		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null || _repository.GetAll() == null)
			{
				return NotFound();
			}

			var user = _repository.GetById(id ?? 0);
			if (user == null)
			{
				return NotFound();
			}

			return View(user);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (_repository.GetAll() == null)
			{
				return Problem("Entity set 'DatabaseContext.Users'  is null.");
			}
			var user = _repository.GetById(id);
			if (user != null)
			{
				_repository.Delete(user);

			}

			await _unitOfWork.SaveChangesAsync();
			_logger.LogInformation("User deleted!");

			return RedirectToAction(nameof(Index));
		}

		private bool UserExists(int id)
		{
			return (_repository.GetAll()?.Any(e => e.Id == id)).GetValueOrDefault();
		}
	}
}