using API.Models;
using API.ViewModels;
using API.Mapping;
using API.Data;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using API.Repositories;

namespace API.Services
{
    public interface IEstudianteService
    {
        EstudianteViewModel CrearEstudiante(EstudianteViewModel estudianteViewModel);
        List<EstudianteViewModel> ObtenerEstudiantes();
        EstudianteDetalleViewModel ObtenerEstudiantePorId(int id);
        Task<bool> EliminarEstudianteAsync(int id);
        EstudianteViewModel ActualizarEstudiante(int id, EstudianteViewModel estudianteViewModel);
        Task<List<CompaneroViewModel>> ObtenerCompanerosPorEstudianteId(int estudianteId);
        Task<List<VisibilidadViewModel>> ObtenerVisibilidadEstudiantes(int estudianteId);

    }

    public class EstudianteService : IEstudianteService
    {
        private readonly IEstudianteRepository _estudianteRepository;
        private readonly IMapper _mapper;

        public EstudianteService(IEstudianteRepository estudianteRepository, IMapper mapper)
        {
            _estudianteRepository = estudianteRepository;
            _mapper = mapper;
        }

        public EstudianteViewModel CrearEstudiante(EstudianteViewModel estudianteViewModel)
        {
            var estudiante = _mapper.Map<Estudiante>(estudianteViewModel);
            estudiante = _estudianteRepository.CrearEstudianteAsync(estudiante).Result;
            return _mapper.Map<EstudianteViewModel>(estudiante);
        }

        public List<EstudianteViewModel> ObtenerEstudiantes()
        {
            var estudiantes = _estudianteRepository.ObtenerEstudiantesAsync().Result;
            return _mapper.Map<List<EstudianteViewModel>>(estudiantes);
        }

        public EstudianteDetalleViewModel ObtenerEstudiantePorId(int id)
        {
            var estudiante = _estudianteRepository.ObtenerEstudiantePorIdAsync(id).Result;
            return _mapper.Map<EstudianteDetalleViewModel>(estudiante);
        }        

        public async Task<bool> EliminarEstudianteAsync(int id)
        {
            var estudiante = await _estudianteRepository.ObtenerEstudiantePorIdAsync(id);
            if (estudiante == null) return false;

            var tieneMaterias = await _estudianteRepository.TieneMateriasInscritas(id);
            if (tieneMaterias)
            {
                throw new InvalidOperationException("No se puede eliminar un estudiante con materias inscritas");
            }

            return await _estudianteRepository.EliminarEstudianteAsync(id);
        }

        public EstudianteViewModel ActualizarEstudiante(int id, EstudianteViewModel estudianteViewModel)
        {
            var estudiante = _estudianteRepository.ObtenerEstudiantePorIdAsync(id).Result;
            if (estudiante == null) return null;

            _mapper.Map(estudianteViewModel, estudiante);
            estudiante = _estudianteRepository.ActualizarEstudianteAsync(estudiante).Result;

            return _mapper.Map<EstudianteViewModel>(estudiante);
        }

        public async Task<List<CompaneroViewModel>> ObtenerCompanerosPorEstudianteId(int estudianteId)
        {
            var registros = await _estudianteRepository.ObtenerRegistrosPorEstudianteIdAsync(estudianteId);
            var materiasIds = registros.Select(r => r.MateriaId).ToList();

            var companerosRegistros = await _estudianteRepository.ObtenerRegistrosPorMateriasAsync(materiasIds);

            var companerosAgrupados = companerosRegistros
                .Where(r => r.EstudianteId != estudianteId)
                .GroupBy(r => r.Estudiante)
                .Select(grupo => new CompaneroViewModel
                {
                    Nombre = grupo.Key.Nombre,
                    Materias = grupo.Select(r => r.Materia.Nombre).Distinct().ToList()
                })
                .ToList();

            return _mapper.Map<List<CompaneroViewModel>>(companerosAgrupados);
        }

        public async Task<List<VisibilidadViewModel>> ObtenerVisibilidadEstudiantes(int estudianteId)
        {
            var materiasEstudiante = await _estudianteRepository.ObtenerMateriasEstudianteAsync(estudianteId);
            var registros = await _estudianteRepository.ObtenerRegistrosConEstudianteMateriaProfesorAsync();

            var resultados = registros
                .Select(r =>
                {
                    var viewModel = _mapper.Map<VisibilidadViewModel>(r);
                    viewModel.Estudiante = materiasEstudiante.Contains(r.MateriaId) && r.EstudianteId != estudianteId
                        ? r.Estudiante.Nombre
                        : "";
                    return viewModel;
                })
                .OrderBy(r => r.Estudiante)
                .ThenBy(r => r.Materia)
                .ToList();

            return resultados;
        }
    }
}