// API/Services/RegistroService.cs
using API.Models;
using API.ViewModels;
using API.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using API.Mapping;
using AutoMapper;
using API.Repositories;
using System;
using System.Collections.Generic;

namespace API.Services
{
    public interface IRegistroService
    {
        Task<RegistroViewModel> CrearRegistro(RegistroViewModel registroViewModel);
    }

    public class RegistroService : IRegistroService
    {
        private readonly IRegistroRepository _registroRepository;
        private readonly IEstudianteRepository _estudianteRepository;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public RegistroService(IRegistroRepository registroRepository, IEstudianteRepository estudianteRepository, IMapper mapper, AppDbContext context)
        {
            _registroRepository = registroRepository;
            _estudianteRepository = estudianteRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<RegistroViewModel> CrearRegistro(RegistroViewModel registroViewModel)
        {
            var estudiante = await _estudianteRepository.ObtenerEstudiantePorIdAsync(registroViewModel.EstudianteId);
            if (estudiante == null)
            {
                throw new InvalidOperationException("Datos inválidos: estudiante no encontrado");
            }

            var materia = await _context.Materias
                .Include(m => m.Profesor)
                .FirstOrDefaultAsync(m => m.Id == registroViewModel.MateriaId);
            if (materia == null)
            {
                throw new InvalidOperationException("Datos inválidos: materia no encontrada");
            }

            var existeRegistro = await _context.Registros
                .AnyAsync(r => r.EstudianteId == registroViewModel.EstudianteId && r.MateriaId == registroViewModel.MateriaId);
            if (existeRegistro)
            {
                throw new InvalidOperationException("El estudiante ya está inscrito en esta materia");
            }

            var registrosEstudiante = await _estudianteRepository.ObtenerRegistrosPorEstudianteIdAsync(registroViewModel.EstudianteId);
            if (registrosEstudiante.Count >= 3)
            {
                throw new InvalidOperationException("Máximo 3 materias por estudiante");
            }

            foreach (var reg in registrosEstudiante)
            {
                var mat = await _context.Materias
                    .Include(m => m.Profesor)
                    .FirstOrDefaultAsync(m => m.Id == reg.MateriaId);
                if (mat != null && mat.Profesor != null && materia.Profesor != null && mat.Profesor.Id == materia.Profesor.Id)
                {
                    throw new InvalidOperationException("No puedes tener dos materias con el mismo profesor");
                }
            }

            var totalEstudiantes = await _context.Registros
                .CountAsync(r => r.MateriaId == registroViewModel.MateriaId);
            if (totalEstudiantes >= 30)
            {
                throw new InvalidOperationException("La materia ha alcanzado el límite de estudiantes");
            }

            var registro = _mapper.Map<Registro>(registroViewModel);
            _registroRepository.AgregarRegistro(registro);
            return _mapper.Map<RegistroViewModel>(registro);
        }
    }
}