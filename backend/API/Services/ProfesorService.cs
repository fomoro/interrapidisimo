using API.Models;
using API.ViewModels;
using API.Data;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using API.Repositories;

namespace API.Services
{
    public interface IProfesorService
    {
        List<ProfesorViewModel> ObtenerProfesores();
        ProfesorConMateriasViewModel ObtenerProfesorConMaterias(int id);
    }

    public class ProfesorService : IProfesorService
    {
        private readonly IProfesorRepository _profesorRepository;
        private readonly IMapper _mapper;

        public ProfesorService(IProfesorRepository profesorRepository, IMapper mapper)
        {
            _profesorRepository = profesorRepository;
            _mapper = mapper;
        }

        public List<ProfesorViewModel> ObtenerProfesores()
        {
            var profesores = _profesorRepository.ObtenerProfesores();
            return _mapper.Map<List<ProfesorViewModel>>(profesores);
        }

        public ProfesorConMateriasViewModel ObtenerProfesorConMaterias(int id)
        {
            var profesor = _profesorRepository.ObtenerProfesorPorId(id);
            return _mapper.Map<ProfesorConMateriasViewModel>(profesor);
        }
    }
}
