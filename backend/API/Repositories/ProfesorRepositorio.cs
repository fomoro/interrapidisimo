using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace API.Repositories
{
    public interface IProfesorRepository
    {
        List<Profesor> ObtenerProfesores();
        Profesor ObtenerProfesorPorId(int id);
    }

    public class ProfesorRepository : IProfesorRepository
    {
        private readonly AppDbContext _context;

        public ProfesorRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Profesor> ObtenerProfesores()
        {
            return _context.Profesores.ToList();
        }

        public Profesor ObtenerProfesorPorId(int id)
        {
            return _context.Profesores.Include(p => p.Materias).FirstOrDefault(p => p.Id == id);
        }
    }
}