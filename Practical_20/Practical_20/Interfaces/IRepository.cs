namespace Practical_20.Interfaces;

public interface IRepository<T> where T : class
{
	T GetById(int id);

	void Insert(T entity);

	void Update(T entity);

	void Delete(T entity);

	IEnumerable<T> GetAll();
}
