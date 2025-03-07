using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public interface IRepositorioGenerico<T> where T : class
    {
        Task<IEnumerable<T>> ObtenerTodosAsync();
        Task<T> ObtenerAsync(int id);
        Task<T> AgregarAsync(T entity);
        Task<bool> ActualizarAsync(T entity);
        Task<bool> EliminarAsync(int id);
    }

    public class RepositorioGenerico<T> : IRepositorioGenerico<T> where T : class
    {
        private readonly AppDbContext _context;

        public RepositorioGenerico(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> ObtenerTodosAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> ObtenerAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> AgregarAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> ActualizarAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
            {
                return false;
            }
            _context.Set<T>().Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}