using API.Models;
using API.ViewModels;
using API.Data;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public interface IMateriaService
    {
        List<MateriaViewModel> ObtenerMaterias();
        MateriaViewModel ObtenerMateriaPorId(int id);
    }

    public class MateriaService : IMateriaService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public MateriaService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<MateriaViewModel> ObtenerMaterias()
        {
            // Usar AutoMapper para mapear la lista
            var materias = _context.Materias
                .Include(m => m.Profesor) // Incluir la relación Profesor
                .ToList();

            return _mapper.Map<List<MateriaViewModel>>(materias);
        }

        public MateriaViewModel ObtenerMateriaPorId(int id)
        {
            // Buscar una materia con el profesor
            var materia = _context.Materias
                .Include(m => m.Profesor) // Incluir la relación Profesor
                .FirstOrDefault(m => m.Id == id);

            return _mapper.Map<MateriaViewModel>(materia);
        }
    }
}
