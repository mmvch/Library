using Library.DAL.Contexts;
using Library.Domain.DTO;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Library.DAL.Repositories
{
	public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
	{
		private readonly Context _context;
		private readonly DbSet<TEntity> _dbSet;

		public Repository(Context context)
		{
			_context = context;
			_dbSet = context.Set<TEntity>();
		}

		public async Task<PartialData<TEntity>> GetItemListAsync(Expression<Func<TEntity, bool>>? predicate,
			int? skip, int? take, params Expression<Func<TEntity, object?>>[] includes)
		{
			IQueryable<TEntity> query = predicate == null ? _dbSet : _dbSet.Where(predicate);
			int totalAmount = await query.CountAsync();

			foreach (var include in includes)
			{
				query = query.Include(include);
			}

			return new PartialData<TEntity>()
			{
				Data = await query.Skip(skip ?? 0).Take(take ?? totalAmount).ToListAsync(),
				TotalAmount = totalAmount
			};
		}

		public async Task<TEntity?> GetItemAsync(TKey id)
		{
			return await _dbSet.FindAsync(id);
		}

		public async Task<TEntity> GetItemAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object?>>[] includes)
		{
			IQueryable<TEntity> query = _dbSet;

			foreach (var include in includes)
			{
				query = query.Include(include);
			}

			return await query.FirstAsync(predicate);
		}

		public async Task CreateAsync(TEntity entity)
		{
			await _dbSet.AddAsync(entity);
		}

		public void Update(TEntity entity)
		{
			_dbSet.Attach(entity);
			_context.Entry(entity).State = EntityState.Modified;
		}

		public async Task DeleteAsync(TKey id)
		{
			TEntity? entity = await _dbSet.FindAsync(id);

			if (entity != null)
			{
				_dbSet.Remove(entity);
			}
		}

		public async Task SaveAsync()
		{
			await _context.SaveChangesAsync();
		}

		#region dispose
		private bool disposed = false;

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					_context.Dispose();
				}
			}

			disposed = true;
		}
		#endregion
	}
}
