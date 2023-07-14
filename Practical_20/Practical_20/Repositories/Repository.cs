using Microsoft.EntityFrameworkCore;
using Practical_20.Data;
using Practical_20.Interfaces;

namespace Practical_20.UnitOfWork;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
	private DbSet<TEntity> _dbSet;

	public Repository(DatabaseContext context)
	{
		this._dbSet = context.Set<TEntity>();
	}

	public void Delete(TEntity entity)
	{
		_dbSet.Remove(entity);
	}

	public IEnumerable<TEntity> GetAll()
	{
		return _dbSet;
	}

	public TEntity GetById(int id)
	{
		return _dbSet.Find(id);
	}

	public void Insert(TEntity entity)
	{
		_dbSet.Add(entity);
	}

	public void Update(TEntity entity)
	{
		_dbSet.Update(entity);
	}
}