using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace IntellisenseForMemes.DAL
{
    public interface IRepository<T, TK>
        where T : class
    {
        /// <summary>
        /// Get entity by primary key.
        /// </summary>
        T GetById(TK id);

        /// <summary>
        /// Get entity by primary key.
        /// </summary>
        Task<T> GetByIdAsync(TK id);

        /// <summary>
        /// Add entity to storage.
        /// </summary>
        EntityEntry<T> Create(T newEntity);

        /// <summary>
        /// Add entities to storage.
        /// </summary>
        void CreateMany(List<T> newEntities);

        /// <summary>
        /// Update entity in storage.
        /// </summary>
        void Update(T entity);

        /// <summary>
        /// Remove entity from storage.
        /// </summary>
        EntityEntry<T> Delete(TK id);

        /// <summary>
        /// Remove entity from storage.
        /// </summary>
        EntityEntry<T> Delete(T entity);

        /// <summary>
        /// Remove entities from storage.
        /// </summary>
        void DeleteMany(List<TK> ids);

        /// <summary>
        /// Remove entities from storage.
        /// </summary>
        void DeleteMany(List<T> entites);

        IEnumerable<T> GetAll();

        /// <summary>
        /// Save all changes.
        /// </summary>
        void Save();

        Task<int> SaveAsync();

        IQueryable<T> AsQueryable();
    }

    /// <summary>
    /// Default repository interface for entities with numerical primary key.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> : IRepository<T, int> where T : class
    {

    }
}
