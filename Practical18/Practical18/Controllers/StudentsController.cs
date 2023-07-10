using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practical18.Data;
using Practical18.Models;

namespace Practical18.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class StudentsController : ControllerBase
	{
		private readonly StudentContext _context;

		public StudentsController(StudentContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Student>>> GetStudent()
		{
			if (_context.Student == null)
			{
				return NotFound();
			}
			return await _context.Student.ToListAsync();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Student>> GetStudent(int id)
		{
			if (_context.Student == null)
			{
				return NotFound();
			}
			var student = await _context.Student.FindAsync(id);

			if (student == null)
			{
				return NotFound();
			}

			return student;
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutStudent(int id, Student student)
		{
			if (id != student.Id)
			{
				return BadRequest();
			}

			_context.Entry(student).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!StudentExists(id))
				{
					return NotFound();
				}
				throw;
			}

			return NoContent();
		}

		[HttpPost]
		public async Task<ActionResult<Student>> PostStudent(Student student)
		{
			if (_context.Student == null)
			{
				return Problem("Entity set 'StudentContext.Student'  is null.");
			}
		
			_context.Student.Add(student);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetStudent", new { id = student.Id }, student);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteStudent(int id)
		{
			if (_context.Student == null)
			{
				return NotFound();
			}
			var student = await _context.Student.FindAsync(id);
			if (student == null)
			{
				return NotFound();
			}

			_context.Student.Remove(student);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool StudentExists(int id)
		{
			return (_context.Student?.Any(e => e.Id == id)).GetValueOrDefault();
		}
	}
}
