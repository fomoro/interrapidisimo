using API.Models;
using API.ViewModels;
using AutoMapper;

public class ProfesorProfile : Profile
{
    public ProfesorProfile()
    {
        // De Modelo a ViewModel y viceversa
        CreateMap<Profesor, ProfesorViewModel>().ReverseMap();
        CreateMap<Profesor, ProfesorConMateriasViewModel>()
            .ForMember(dest => dest.Materias, opt => opt.MapFrom(src => src.Materias.Select(m => m.Nombre).ToList()));
    }
}
