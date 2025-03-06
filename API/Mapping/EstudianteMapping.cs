using API.Models;
using API.ViewModels;
using System.Collections.Generic;

namespace API.Mapping
{
    public static class EstudianteMapping
    {
        public static Estudiante ToModel(EstudianteViewModel estudianteViewModel)
        {
            return new Estudiante
            {
                Nombre = estudianteViewModel.Nombre,
                Carrera = estudianteViewModel.Carrera
            };
        }

        public static EstudianteViewModel ToViewModel(Estudiante estudiante)
        {
            return new EstudianteViewModel
            {
                Id = estudiante.Id,
                Nombre = estudiante.Nombre,
                Carrera = estudiante.Carrera
            };
        }

        public static EstudianteDetalleViewModel ToDetalleViewModel(Estudiante estudiante, List<string> materias)
        {
            return new EstudianteDetalleViewModel
            {
                Id = estudiante.Id,
                Nombre = estudiante.Nombre,
                Carrera = estudiante.Carrera,
                Materias = materias
            };
        }
    }
}