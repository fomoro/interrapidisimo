using API.Models;
using API.ViewModels;
using API.Mapping;
using API.Data;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public interface IEstudianteService
    {
        EstudianteViewModel CrearEstudiante(EstudianteViewModel estudianteViewModel);
        List<EstudianteViewModel> ObtenerEstudiantes();
        EstudianteDetalleViewModel ObtenerEstudiantePorId(int id);
        bool EliminarEstudiante(int id);
        EstudianteViewModel ActualizarEstudiante(int id, EstudianteViewModel estudianteViewModel);
        Task<List<CompaneroViewModel>> ObtenerCompanerosPorEstudianteId(int estudianteId);
        Task<List<VisibilidadViewModel>> ObtenerVisibilidadEstudiantes(int estudianteId);


    }

    public class EstudianteService : IEstudianteService
    {
        private readonly AppDbContext _context;

        public EstudianteService(AppDbContext context)
        {
            _context = context;
        }

        public EstudianteViewModel CrearEstudiante(EstudianteViewModel estudianteViewModel)
        {
            var estudiante = EstudianteMapping.ToModel(estudianteViewModel);
            _context.Estudiantes.Add(estudiante);
            _context.SaveChanges();

            return EstudianteMapping.ToViewModel(estudiante);
        }

        public List<EstudianteViewModel> ObtenerEstudiantes()
        {
            var estudiantes = _context.Estudiantes.ToList();
            return estudiantes.Select(e => EstudianteMapping.ToViewModel(e)).ToList();
        }

        public EstudianteDetalleViewModel ObtenerEstudiantePorId(int id)
        {
            var estudiante = _context.Estudiantes
                .FirstOrDefault(e => e.Id == id);

            if (estudiante == null) return null;

            var materias = _context.Registros
                .Where(r => r.EstudianteId == id)
                .Join(_context.Materias,
                    registro => registro.MateriaId,
                    materia => materia.Id,
                    (registro, materia) => materia.Nombre)
                .ToList();

            return EstudianteMapping.ToDetalleViewModel(estudiante, materias);
        }

        public bool EliminarEstudiante(int id)
        {
            var estudiante = _context.Estudiantes.FirstOrDefault(e => e.Id == id);
            if (estudiante == null) return false;

            // Verificar si el estudiante tiene materias inscritas
            var tieneMaterias = _context.Registros.Any(r => r.EstudianteId == id);
            if (tieneMaterias)
            {
                throw new InvalidOperationException("No se puede eliminar un estudiante con materias inscritas");
            }

            _context.Estudiantes.Remove(estudiante);
            _context.SaveChanges();
            return true;
        }
       
        public EstudianteViewModel ActualizarEstudiante(int id, EstudianteViewModel estudianteViewModel)
        {
            var estudiante = _context.Estudiantes.FirstOrDefault(e => e.Id == id);
            if (estudiante == null) return null;

            // Actualizar solo los campos proporcionados
            estudiante.Nombre = estudianteViewModel.Nombre ?? estudiante.Nombre;
            estudiante.Carrera = estudianteViewModel.Carrera ?? estudiante.Carrera;

            _context.SaveChanges();

            return EstudianteMapping.ToViewModel(estudiante);
        }
        
        public async Task<List<VisibilidadViewModel>> ObtenerVisibilidadEstudiantes(int estudianteId)
        {
            // Obtener las materias del estudiante
            var materiasEstudiante = await _context.Registros
                .Where(r => r.EstudianteId == estudianteId)
                .Select(r => r.MateriaId)
                .ToListAsync();

            var registros = await _context.Registros
                .Where(r => r.EstudianteId != estudianteId) // Excluir al estudiante actual
                .Include(r => r.Estudiante)
                .Include(r => r.Materia)
                    .ThenInclude(m => m.Profesor)
                .ToListAsync(); // Cargar los datos en memoria

            // Mapear los registros a ViewModel usando la clase de mapeo
            var resultados = registros
                .Select(r => VisibilidadMapping.ToViewModel(r, materiasEstudiante))
                .OrderBy(r => r.Estudiante)
                .ThenBy(r => r.Materia)
                .ToList();

            return resultados;
        }

        public async Task<List<CompaneroViewModel>> ObtenerCompanerosPorEstudianteId(int estudianteId)
        {
            // Verificar si el estudiante existe
            var estudianteExiste = await _context.Estudiantes.AnyAsync(e => e.Id == estudianteId);
            if (!estudianteExiste)
            {
                throw new KeyNotFoundException("No se encontró el ID del estudiante.");
            }

            // Obtener las materias en las que está inscrito el estudiante
            var materiasDelEstudiante = await _context.Registros
                .Where(r => r.EstudianteId == estudianteId)
                .Select(r => r.MateriaId)
                .ToListAsync();

            if (!materiasDelEstudiante.Any())
            {
                throw new InvalidOperationException("No tiene materias asignadas");
            }

            // Obtener los compañeros de clase con las materias en común
            var registrosCompaneros = await _context.Registros
                .Where(r => materiasDelEstudiante.Contains(r.MateriaId) && r.EstudianteId != estudianteId)
                .Include(r => r.Estudiante)
                .Include(r => r.Materia)
                .ToListAsync();

            // Agrupar por compañero y listar las materias compartidas
            var companerosAgrupados = registrosCompaneros
                .GroupBy(r => r.Estudiante)
                .Select(grupo => new CompaneroViewModel
                {
                    Nombre = grupo.Key.Nombre,
                    Materias = grupo.Select(r => r.Materia.Nombre).Distinct().ToList()
                })
                .ToList();

            return companerosAgrupados;
        }
    }
}