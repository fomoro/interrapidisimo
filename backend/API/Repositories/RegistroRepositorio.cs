using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using API.Models;

namespace API.Repositories
{
    public interface IRegistroRepositorio : IRepositorioGenerico<Registro>
    {
        // Aquí puedes agregar métodos específicos para Registro si es necesario
    }

    public class RegistroRepositorio : RepositorioGenerico<Registro>, IRegistroRepositorio
    {
        private readonly AppDbContext _context;

        public RegistroRepositorio(AppDbContext context) : base(context)
        {
            _context = context;
        }

        // Implementa métodos específicos para Registro si es necesario
    }
}