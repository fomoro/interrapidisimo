// API/Services/ProfesorService.cs
using API.Models;
using API.ViewModels;
using API.Data;
using System.Collections.Generic;
using System.Linq;

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

        public ProfesorService(AppDbContext context)
        {
            _context = context;
        }

        public List<ProfesorViewModel> ObtenerProfesores()
        {
            return _context.Profesores
                .Select(p => new ProfesorViewModel
                {
                    Id = p.Id,
                    Nombre = p.Nombre
                })
                .ToList();
        }

        public ProfesorConMateriasViewModel ObtenerProfesorConMaterias(int id)
        {
            var profesor = _context.Profesores
                .Where(p => p.Id == id)
                .Select(p => new ProfesorConMateriasViewModel
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Materias = p.Materias.Select(m => m.Nombre).ToList()
                })
                .FirstOrDefault();

            return profesor;
        }
    }
}