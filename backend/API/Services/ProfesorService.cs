using API.Models;
using API.ViewModels;
using API.Data;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public interface IProfesorService
    {
        List<ProfesorViewModel> ObtenerProfesores();
        ProfesorConMateriasViewModel ObtenerProfesorConMaterias(int id);
    }

    public class ProfesorService : IProfesorService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProfesorService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<ProfesorViewModel> ObtenerProfesores()
        {
            // Usar AutoMapper para mapear la lista de profesores
            var profesores = _context.Profesores.ToList();
            return _mapper.Map<List<ProfesorViewModel>>(profesores);
        }

        public ProfesorConMateriasViewModel ObtenerProfesorConMaterias(int id)
        {
            // Buscar el profesor con sus materias
            var profesor = _context.Profesores
                .Include(p => p.Materias) // Incluye la relación de Materias
                .FirstOrDefault(p => p.Id == id);

            // Retornar el ViewModel mapeado con AutoMapper
            return _mapper.Map<ProfesorConMateriasViewModel>(profesor);
        }
    }
}
