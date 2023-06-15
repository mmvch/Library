using Library.Domain.DTO;
using System.Linq.Expressions;

namespace Library.DAL.Repositories
{
	public interface IRepository<TEntity, TKey> : IDisposable where TEntity : class
	{
		Task<PartialData<TEntity>> GetItemListAsync(Expression<Func<TEntity, bool>>? predicate,
			int? skip, int? take, params Expression<Func<TEntity, object?>>[] includes);
		Task<TEntity?> GetItemAsync(TKey id);
		Task<TEntity> GetItemAsync(Expression<Func<TEntity, bool>> predicate,
			params Expression<Func<TEntity, object?>>[] includes);
		Task CreateAsync(TEntity entity);
		void Update(TEntity entity);
		Task DeleteAsync(TKey id);
		Task SaveAsync();
	}
}
