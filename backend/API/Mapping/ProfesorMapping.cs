// API/Mapping/ProfesorMapping.cs
using API.Models;
using API.ViewModels;

namespace API.Mapping
{
    public static class ProfesorMapping
    {
        public static ProfesorViewModel ToViewModel(Profesor profesor)
        {
            return new ProfesorViewModel
            {
                Id = profesor.Id,
                Nombre = profesor.Nombre
            };
        }

        public static ProfesorConMateriasViewModel ToViewModelConMaterias(Profesor profesor)
        {
            return new ProfesorConMateriasViewModel
            {
                Id = profesor.Id,
                Nombre = profesor.Nombre,
                Materias = profesor.Materias.Select(m => m.Nombre).ToList()
            };
        }
    }
}