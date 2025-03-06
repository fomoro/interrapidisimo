// API/Mapping/RegistroMapping.cs
using API.Models;
using API.ViewModels;

namespace API.Mapping
{
    public static class RegistroMapping
    {
        public static RegistroViewModel ToViewModel(Registro registro)
        {
            return new RegistroViewModel
            {
                Id = registro.Id,
                EstudianteId = registro.EstudianteId,
                MateriaId = registro.MateriaId
            };
        }
    }
}