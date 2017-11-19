using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketAPI.Models;

namespace TicketAPI.Repository
{
    public class Repository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly AppContextDB _context;

        public Repository(AppContextDB context)
        {
            _context = context;
        }

        public async Task Add(TEntity o)
        {
            await _context.Set<TEntity>().AddAsync(o);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var itemToDelete = await _context.Set<TEntity>().FindAsync();
            if(itemToDelete != null)
            {
                _context.Remove(itemToDelete);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<TEntity> Get(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task<List<TEntity>> getAll()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<List<TEntity>> paginate(int perPage, int page = 1)
        {
            return await _context.Set<TEntity>().Take(perPage)
                                .Skip((page - 1) * perPage)
                                .ToListAsync();
        }

        public async Task Update(int primary, TEntity o)
        {
            var itemToUpdate = await _context.Set<TEntity>().FindAsync(primary);
            if (itemToUpdate != null)
            {
                itemToUpdate = o;
                await _context.SaveChangesAsync();
            }
        }
    }
}
