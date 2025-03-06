// API/Mapping/CompaneroMapping.cs
using API.Models;
using API.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace API.Mapping
{
    public static class CompaneroMapping
    {
        public static CompaneroViewModel ToViewModel(Estudiante estudiante, List<string> materias)
        {
            return new CompaneroViewModel
            {
                Nombre = estudiante.Nombre,
                Materias = materias
            };
        }
    }
}