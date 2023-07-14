using Practical_20.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Practical_20.Models;

public class User : IAuditable
{
	public int Id { get; set; }

	public string Name { get; set; }

	public string Email { get; set; }

	[DataType(DataType.Password)]
	public string Password { get; set; }
}
