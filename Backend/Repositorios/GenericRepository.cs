using Castle.Core.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StockMeal.Backend.DBContext;
using System.Linq.Expressions;

namespace StockMeal.Backend.Repositorios
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly StockMealContext _context;
        protected readonly DbSet<T> _dbSet;
        protected readonly ILogger<GenericRepository<T>> _logger;

        public GenericRepository(StockMealContext context, ILogger<GenericRepository<T>> logger)
        {
            _context = context;
            _dbSet = _context.Set<T>();
            _logger = logger;
        }

        // ========================
        //       CONSULTAS
        // ========================

        public IQueryable<T> Query(bool asNoTracking = true, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            if (asNoTracking)
                query = query.AsNoTracking();

            if (includes != null)
                foreach (var inc in includes)
                    query = query.Include(inc);

            return query;
        }

        public async Task<T?> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id).ConfigureAwait(false);
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate,
            bool asNoTracking = true, params Expression<Func<T, object>>[] includes)
        {
            return await Query(asNoTracking, includes)
                .FirstOrDefaultAsync(predicate)
                .ConfigureAwait(false);
        }

        public async Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate,
            bool asNoTracking = true, params Expression<Func<T, object>>[] includes)
        {
            return await Query(asNoTracking, includes)
                .Where(predicate)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        // ========================
        //       ESCRITURA
        // ========================

        public async Task AddAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            try
            {
                await _dbSet.AddAsync(entity).ConfigureAwait(false);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Error al añadir la entidad del tipo {typeof(T).Name}", ex);
            }
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));

            try
            {
                await _dbSet.AddRangeAsync(entities).ConfigureAwait(false);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Error al añadir varias entidades del tipo {typeof(T).Name}", ex);
            }
        }

        public async Task UpdateAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            try
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Error al actualizar la entidad del tipo {typeof(T).Name}", ex);
            }
        }

        public async Task RemoveAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            try
            {
                var entityType = _context.Model.FindEntityType(typeof(T));
                var primaryKey = entityType?.FindPrimaryKey();

                if (primaryKey != null)
                {
                    var keyValues = primaryKey.Properties
                        .Select(p => p.PropertyInfo?.GetValue(entity))
                        .ToArray();

                    var trackedEntity = await _dbSet.FindAsync(keyValues).ConfigureAwait(false);
                    if (trackedEntity != null)
                    {
                        _dbSet.Remove(trackedEntity);
                    }
                    else
                    {
                        _dbSet.Remove(entity);
                    }
                }
                else
                {
                    _dbSet.Remove(entity);
                }

                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Error al eliminar la entidad del tipo {typeof(T).Name}", ex);
            }
        }

        public async Task RemoveByIdAsync(object id)
        {
            var entity = await GetByIdAsync(id).ConfigureAwait(false);
            if (entity == null) return;

            await RemoveAsync(entity).ConfigureAwait(false);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }

    public class DataAccessException : Exception
    {
        public DataAccessException(string message) : base(message) { }

        public DataAccessException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
