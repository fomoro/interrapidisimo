using API.Models;
using API.ViewModels;
using AutoMapper;

public class RegistroProfile : Profile
{
    public RegistroProfile()
    {
        // De Modelo a ViewModel
        CreateMap<Registro, RegistroViewModel>().ReverseMap();

        CreateMap<IGrouping<Estudiante, Registro>, CompaneroViewModel>()
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Key.Nombre))
                .ForMember(dest => dest.Materias, opt => opt.MapFrom(src => src.Select(r => r.Materia.Nombre).Distinct().ToList()));


        CreateMap<Registro, VisibilidadViewModel>()
            .ForMember(dest => dest.Estudiante, opt => opt.Ignore()) // Se maneja en el método del servicio
            .ForMember(dest => dest.Materia, opt => opt.MapFrom(src => src.Materia.Nombre))
            .ForMember(dest => dest.Profesor, opt => opt.MapFrom(src => src.Materia.Profesor.Nombre ?? "Sin profesor"));
    }
}
