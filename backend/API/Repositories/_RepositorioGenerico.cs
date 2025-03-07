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
}