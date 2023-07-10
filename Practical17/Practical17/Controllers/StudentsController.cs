using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using Practical17.Data;
using Practical17.Models;

namespace Practical17.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IRepository<StudentModel> _studentRepository;

        public StudentsController(IRepository<StudentModel> studentRepository)
        {
            _studentRepository = studentRepository;
        }

        // GET: Students
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var students = await _studentRepository.GetAll();

            if(students != null)
            {
                return View(students);
            }

            return Problem("Entity set 'MyDBContext.StudentModel'  is null.");
        }

        // GET: Students/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var student = await _studentRepository.Get(id);

            if (student == null)
            {
                return NotFound();
            }
            
            return View(student);
        }

        // GET: Students/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EnrollmentNumber,FirstName,LastName,Email,DateOfBirth,Gender,Address")] StudentModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _studentRepository.Add(model);
                    if(result)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Oops something went wrong!, please try again.");
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return View(model);
        }

        // GET: Students/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var student = await _studentRepository.Get(id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EnrollmentNumber,FirstName,LastName,Email,DateOfBirth,Gender,Address")] StudentModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var result = _studentRepository.Update(id, model);
                    if(await result)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Oops something went wrong!, please try again.");
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return View(model);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _studentRepository.Get(id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = _studentRepository.Get(id);
            if (student != null)
            {
                var result = await _studentRepository.Delete(id);
                if(result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Oops something went wrong!, please try again.");
                }
            }

            return NotFound();
        }

        //private bool StudentModelExists(int id)
        //{
        //    return (_context.Students?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
