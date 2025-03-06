using AutoMapper;
using API.Models;
using API.ViewModels;
using System.Linq;

namespace API.Mapping
{
    public class EstudianteProfile : Profile
    {
        public EstudianteProfile()
        {
            // Mapeo de Estudiante a EstudianteViewModel
            CreateMap<Estudiante, EstudianteViewModel>();

            // Mapeo inverso de EstudianteViewModel a Estudiante
            CreateMap<EstudianteViewModel, Estudiante>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()); // Ignorar el Id al mapear

            // Mapeo de Estudiante a EstudianteDetalleViewModel
            CreateMap<Estudiante, EstudianteDetalleViewModel>()
                .ForMember(dest => dest.Materias, opt => opt.MapFrom(src => src.Registros.Select(r => r.Materia.Nombre).ToList()));
            
        }
    }
}