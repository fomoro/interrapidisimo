using API.Data;
using API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace API.Repositories
{
    public interface IEstudianteRepository
    {
        Task<Estudiante> CrearEstudianteAsync(Estudiante estudiante);
        Task<List<Estudiante>> ObtenerEstudiantesAsync();
        Task<Estudiante> ObtenerEstudiantePorIdAsync(int id);
        Task<bool> EliminarEstudianteAsync(int id);
        Task<bool> TieneMateriasInscritas(int id);
        Task<Estudiante> ActualizarEstudianteAsync(Estudiante estudiante);
        Task<List<Registro>> ObtenerRegistrosPorEstudianteIdAsync(int estudianteId);
        Task<List<Registro>> ObtenerRegistrosPorMateriasAsync(List<int> materiasIds);

        Task<List<int>> ObtenerMateriasEstudianteAsync(int estudianteId);
        Task<List<Registro>> ObtenerRegistrosConEstudianteMateriaProfesorAsync();
    }

    public class EstudianteRepository : IEstudianteRepository
    {
        private readonly AppDbContext _context;

        public EstudianteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Estudiante> CrearEstudianteAsync(Estudiante estudiante)
        {
            _context.Estudiantes.Add(estudiante);
            await _context.SaveChangesAsync();
            return estudiante;
        }

        public async Task<List<Estudiante>> ObtenerEstudiantesAsync()
        {
            return await _context.Estudiantes.ToListAsync();
        }

        public async Task<Estudiante> ObtenerEstudiantePorIdAsync(int id)
        {
            return await _context.Estudiantes
                .Include(e => e.Registros)
                .ThenInclude(r => r.Materia)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<bool> EliminarEstudianteAsync(int id)
        {
            var estudiante = await _context.Estudiantes.FindAsync(id);
            if (estudiante == null) return false;

            _context.Estudiantes.Remove(estudiante);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> TieneMateriasInscritas(int id)
        {
            return await _context.Registros.AnyAsync(r => r.EstudianteId == id);
        }

        public async Task<Estudiante> ActualizarEstudianteAsync(Estudiante estudiante)
        {
            _context.Estudiantes.Update(estudiante);
            await _context.SaveChangesAsync();
            return estudiante;
        }

        public async Task<List<Registro>> ObtenerRegistrosPorEstudianteIdAsync(int estudianteId)
        {
            return await _context.Registros
                .Include(r => r.Materia)
                .Where(r => r.EstudianteId == estudianteId)
                .ToListAsync();
        }

        public async Task<List<Registro>> ObtenerRegistrosPorMateriasAsync(List<int> materiasIds)
        {
            return await _context.Registros
                .Include(r => r.Estudiante)
                .Include(r => r.Materia)
                .Where(r => materiasIds.Contains(r.MateriaId))
                .ToListAsync();
        }


        public async Task<List<int>> ObtenerMateriasEstudianteAsync(int estudianteId)
        {
            return await _context.Registros
                .Where(r => r.EstudianteId == estudianteId)
                .Select(r => r.MateriaId)
                .ToListAsync();
        }

        public async Task<List<Registro>> ObtenerRegistrosConEstudianteMateriaProfesorAsync()
        {
            return await _context.Registros
                .Include(r => r.Estudiante)
                .Include(r => r.Materia)
                    .ThenInclude(m => m.Profesor)
                .ToListAsync();
        }

    }
}