using Microsoft.EntityFrameworkCore;
using MVC.Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Project.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        private readonly NorthwindContext _northwindContext;
        private readonly DbSet<TEntity> _dbSet;
        public Repository(NorthwindContext northwindContext)
        {
            _northwindContext = northwindContext;
            _dbSet = _northwindContext.Set<TEntity>();
        }

        public DbSet<TEntity> GetDbSet()
        {
            return _dbSet;
        }
        public async Task Create(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _northwindContext.SaveChangesAsync();
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
            _northwindContext.SaveChanges();
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity> GetById(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Update(TEntity entity)
        {
            _northwindContext.Entry(entity).State = EntityState.Modified;
            //_dbSet.Update(entity);
            _northwindContext.SaveChanges();
        }
    }
}
