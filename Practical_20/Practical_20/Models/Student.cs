using Practical_20.Interfaces;

namespace Practical_20.Models;

public class Student : IAuditable
{
	public int Id { get; set; }

	public string Name { get; set; }

	public string Email { get; set; }

}
