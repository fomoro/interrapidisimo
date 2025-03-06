using API.Models;
using API.ViewModels;
using AutoMapper;

public class MateriaProfile : Profile
{
    public MateriaProfile()
    {
        // De Modelo a ViewModel para Materias
        CreateMap<Materia, MateriaViewModel>()
            .ForMember(dest => dest.Profesor, opt => opt.MapFrom(src => src.Profesor != null ? src.Profesor.Nombre : null));
    }
}
