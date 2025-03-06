using API.Models;
using API.ViewModels;
using API.Mapping;
using API.Data;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

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
        private readonly IMapper _mapper;

        public EstudianteService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public EstudianteViewModel CrearEstudiante(EstudianteViewModel estudianteViewModel)
        {
            var estudiante = _mapper.Map<Estudiante>(estudianteViewModel);
            _context.Estudiantes.Add(estudiante);
            _context.SaveChanges();

            return _mapper.Map<EstudianteViewModel>(estudiante);
        }

        public List<EstudianteViewModel> ObtenerEstudiantes()
        {
            var estudiantes = _context.Estudiantes.ToList();
            return _mapper.Map<List<EstudianteViewModel>>(estudiantes);
        }

        public EstudianteDetalleViewModel ObtenerEstudiantePorId(int id)
        {
            var estudiante = _context.Estudiantes
                .Include(e => e.Registros)
                .ThenInclude(r => r.Materia)
                .FirstOrDefault(e => e.Id == id);

            return _mapper.Map<EstudianteDetalleViewModel>(estudiante);
        }

        public bool EliminarEstudiante(int id)
        {
            var estudiante = _context.Estudiantes.FirstOrDefault(e => e.Id == id);
            if (estudiante == null) return false;

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

            // Mapear solo los campos que se pueden modificar
            _mapper.Map(estudianteViewModel, estudiante);

            // Asegurarse de que el Id no se modifique
            estudiante.Id = id;

            _context.SaveChanges();

            return _mapper.Map<EstudianteViewModel>(estudiante);
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
                throw new InvalidOperationException("No tiene materias asignadas.");
            }

            // Obtener los registros de los compañeros de clase con materias en común
            var companeros = await _context.Registros
                .Include(r => r.Estudiante)
                .Include(r => r.Materia)
                .Where(r => materiasDelEstudiante.Contains(r.MateriaId) && r.EstudianteId != estudianteId)
                .ToListAsync();

            // Usar AutoMapper para transformar los datos
            var companerosAgrupados = companeros
                .GroupBy(r => r.Estudiante)
                .Select(grupo => new CompaneroViewModel
                {
                    Nombre = grupo.Key.Nombre,
                    Materias = grupo.Select(r => r.Materia.Nombre).Distinct().ToList()
                })
                .ToList();

            // Mapeo con AutoMapper
            return _mapper.Map<List<CompaneroViewModel>>(companerosAgrupados);
        }

        public async Task<List<VisibilidadViewModel>> ObtenerVisibilidadEstudiantes(int estudianteId)
        {
            // Obtener las materias en las que está inscrito el estudiante
            var materiasEstudiante = await _context.Registros
                .Where(r => r.EstudianteId == estudianteId)
                .Select(r => r.MateriaId)
                .ToListAsync();

            // Obtener los registros de estudiantes en materias comunes o no
            var registros = await _context.Registros
                .Include(r => r.Estudiante)
                .Include(r => r.Materia)
                    .ThenInclude(m => m.Profesor)
                .ToListAsync();

            // Mapear los registros con AutoMapper y aplicar la lógica requerida
            var resultados = registros
                .Select(r =>
                {
                    var viewModel = _mapper.Map<VisibilidadViewModel>(r);
                    viewModel.Estudiante = materiasEstudiante.Contains(r.MateriaId) && r.EstudianteId != estudianteId
                        ? r.Estudiante.Nombre
                        : ""; // Se asegura de asignar "" cuando no hay coincidencia
                    return viewModel;
                })
                .OrderBy(r => r.Estudiante)
                .ThenBy(r => r.Materia)
                .ToList();

            return resultados;
        }        

    }
}