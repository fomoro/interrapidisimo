using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using API.Models;

namespace API.Repositories
{
    public interface IProfesorRepositorio : IRepositorioGenerico<Profesor>
    {
        // Aquí puedes agregar métodos específicos para Profesor si es necesario
    }

    public class ProfesorRepositorio : RepositorioGenerico<Profesor>, IProfesorRepositorio
    {
        private readonly AppDbContext _context;

        public ProfesorRepositorio(AppDbContext context) : base(context)
        {
            _context = context;
        }

        // Implementa métodos específicos para Profesor si es necesario
    }
}