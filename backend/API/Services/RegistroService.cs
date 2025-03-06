// API/Services/RegistroService.cs
using API.Models;
using API.ViewModels;
using API.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using API.Mapping;
using AutoMapper;

namespace API.Services
{
    public interface IRegistroService
    {
        Task<RegistroViewModel> CrearRegistro(RegistroViewModel registroViewModel);
    }

    public class RegistroService : IRegistroService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public RegistroService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<RegistroViewModel> CrearRegistro(RegistroViewModel registroViewModel)
        {
            // Validar que el estudiante existe
            var estudiante = await _context.Estudiantes.FindAsync(registroViewModel.EstudianteId);
            if (estudiante == null)
            {
                throw new InvalidOperationException("Datos inválidos: estudiante no encontrado");
            }

            // Validar que la materia existe y cargar el profesor
            var materia = await _context.Materias
                .Include(m => m.Profesor)
                .FirstOrDefaultAsync(m => m.Id == registroViewModel.MateriaId);
            if (materia == null)
            {
                throw new InvalidOperationException("Datos inválidos: materia no encontrada");
            }

            // Validar que el estudiante no esté ya inscrito en la materia
            var existeRegistro = await _context.Registros
                .AnyAsync(r => r.EstudianteId == registroViewModel.EstudianteId && r.MateriaId == registroViewModel.MateriaId);
            if (existeRegistro)
            {
                throw new InvalidOperationException("El estudiante ya está inscrito en esta materia");
            }

            // Validar que el estudiante no tenga más de 3 materias
            var registrosEstudiante = await _context.Registros
                .Where(r => r.EstudianteId == registroViewModel.EstudianteId)
                .ToListAsync();
            if (registrosEstudiante.Count >= 3)
            {
                throw new InvalidOperationException("Máximo 3 materias por estudiante");
            }

            // Validar que el estudiante no tenga materias con el mismo profesor
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

            // Validar que la materia no tenga más de 30 estudiantes
            var totalEstudiantes = await _context.Registros
                .CountAsync(r => r.MateriaId == registroViewModel.MateriaId);
            if (totalEstudiantes >= 30)
            {
                throw new InvalidOperationException("La materia ha alcanzado el límite de estudiantes");
            }

            // Crear el registro
            var registro = new Registro
            {
                EstudianteId = registroViewModel.EstudianteId,
                MateriaId = registroViewModel.MateriaId
            };

            _context.Registros.Add(registro);
            await _context.SaveChangesAsync();
            
            return _mapper.Map<RegistroViewModel>(registro);
        }
    }
}