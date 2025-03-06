// API/Mapping/VisibilidadMapping.cs
using API.Models;
using API.ViewModels;
using System.Collections.Generic;

namespace API.Mapping
{
    public static class VisibilidadMapping
    {
        public static VisibilidadViewModel ToViewModel(Registro registro, List<int> materiasEstudiante)
        {
            if (registro == null || registro.Materia == null)
                return new VisibilidadViewModel(); // Retorna un objeto vacío si los datos son inválidos.

            bool estudianteVisible = materiasEstudiante?.Contains(registro.MateriaId) ?? false;

            return new VisibilidadViewModel
            {
                Estudiante = estudianteVisible && registro.Estudiante != null ? registro.Estudiante.Nombre : string.Empty,
                Materia = registro.Materia.Nombre ?? "Materia desconocida",
                Profesor = registro.Materia.Profesor?.Nombre ?? "Sin profesor"
            };
        }
    }
}
