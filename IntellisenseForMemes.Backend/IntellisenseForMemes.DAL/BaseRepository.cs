using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace IntellisenseForMemes.DAL
{
    public class BaseRepository<T, TK> : IRepository<T, TK>
        where T : class
    {
        #region - Fields -

        /// <summary>
        /// Storage access context.
        /// </summary>
        protected readonly IntellisenseDbContext DbContext;

        /// <summary>
        /// Set which contains T entities.
        /// </summary>
        protected DbSet<T> Set => DbContext.Set<T>();

        #endregion // Fields  

        //*********************************************************************

        public BaseRepository(IntellisenseDbContext dbContext)
        {
            DbContext = dbContext;
        }

        //*********************************************************************

        #region Implementation of IRepository<T>

        /// <summary>
        /// Get entity by primary key.
        /// </summary>
        public T GetById(TK id)
        {
            return DbContext.Set<T>().Find(id);
        }

        /// <summary>
        /// Get entity by primary key.
        /// </summary>
        public async Task<T> GetByIdAsync(TK id)
        {
            return await DbContext.Set<T>().FindAsync(id);
        }

        /// <summary>
        /// Add entity to storage.
        /// </summary>
        public EntityEntry<T> Create(T newEntity)
        {
            return DbContext.Set<T>().Add(newEntity);
        }


        /// <summary>
        /// Add entities to storage.
        /// </summary>
        public void CreateMany(List<T> newEntities)
        {
            DbContext.Set<T>().AddRange(newEntities);
        }


        /// <summary>
        /// Update entity in storage.
        /// </summary>
        public void Update(T entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Remove entity from storage.
        /// </summary>
        public virtual EntityEntry<T> Delete(TK id)
        {
            var dbSet = DbContext.Set<T>();
            var entity = dbSet.Find(id);

            if (entity == null)
                throw new KeyNotFoundException();

            return dbSet.Remove(entity);
        }

        /// <summary>
        /// Remove entity from storage.
        /// </summary>
        public virtual EntityEntry<T> Delete(T entity)
        {
            var dbSet = DbContext.Set<T>();

            return dbSet.Remove(entity);
        }

        /// <summary>
        /// Remove entities from storage.
        /// </summary>
        public void DeleteMany(List<TK> ids)
        {
            foreach (var id in ids)
            {
                Delete(id);
            }
        }

        /// <summary>
        /// Remove entities from storage.
        /// </summary>
        public void DeleteMany(List<T> entities)
        {
            DbContext.Set<T>().RemoveRange(entities);
        }

        public IEnumerable<T> GetAll()
        {
            return DbContext.Set<T>();
        }

        /// <summary>
        /// Save all changes.
        /// </summary>
        public void Save()
        {
            DbContext.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await DbContext.SaveChangesAsync();
        }

        public virtual IQueryable<T> AsQueryable()
        {
            return Set.AsQueryable();
        }

        #endregion
    }

    public class BaseRepository<T> : BaseRepository<T, int>, IRepository<T> where T : class
    {
        public BaseRepository(IntellisenseDbContext dbContext) : base(dbContext)
        {
        }
    }
}
