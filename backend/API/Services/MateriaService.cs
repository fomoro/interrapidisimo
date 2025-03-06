// API/Services/MateriaService.cs
using API.Models;
using API.ViewModels;
using API.Data;
using System.Collections.Generic;
using System.Linq;

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

        public MateriaService(AppDbContext context)
        {
            _context = context;
        }

        public List<MateriaViewModel> ObtenerMaterias()
        {
            return _context.Materias
                .Select(m => new MateriaViewModel
                {
                    Id = m.Id,
                    Nombre = m.Nombre,
                    Creditos = m.Creditos,
                    Profesor = m.Profesor.Nombre // Asume que Profesor está cargado
                })
                .ToList();
        }

        public MateriaViewModel ObtenerMateriaPorId(int id)
        {
            return _context.Materias
                .Where(m => m.Id == id)
                .Select(m => new MateriaViewModel
                {
                    Id = m.Id,
                    Nombre = m.Nombre,
                    Creditos = m.Creditos,
                    Profesor = m.Profesor.Nombre // Asume que Profesor está cargado
                })
                .FirstOrDefault();
        }
    }
}