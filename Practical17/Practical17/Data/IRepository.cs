namespace Practical17.Data;

public interface IRepository<T> where T : class
{
    Task<List<T>> GetAll();

    Task<T> Get(int id);

    Task<bool> Add(T entity);

    Task<bool> Update(int id, T entity);
    
    Task<bool> Delete(int id);
}
