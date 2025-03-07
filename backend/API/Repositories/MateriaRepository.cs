// API/Repositories/MateriaRepository.cs
using API.Models;
using API.Data;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public interface IMateriaRepository
    {
        List<Materia> ObtenerMaterias();
        Materia ObtenerMateriaPorId(int id);
    }

    public class MateriaRepository : IMateriaRepository
    {
        private readonly AppDbContext _context;

        public MateriaRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Materia> ObtenerMaterias()
        {
            return _context.Materias
                .Include(m => m.Profesor)
                .ToList();
        }

        public Materia ObtenerMateriaPorId(int id)
        {
            return _context.Materias
                .Include(m => m.Profesor)
                .FirstOrDefault(m => m.Id == id);
        }
    }
}