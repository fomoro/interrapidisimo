// API/Mapping/MateriaMapping.cs
using API.Models;
using API.ViewModels;

namespace API.Mapping
{
    public static class MateriaMapping
    {
        public static MateriaViewModel ToViewModel(Materia materia)
        {
            return new MateriaViewModel
            {
                Id = materia.Id,
                Nombre = materia.Nombre,
                Creditos = materia.Creditos,
                Profesor = materia.Profesor?.Nombre // Asume que Profesor está cargado
            };
        }
    }
}