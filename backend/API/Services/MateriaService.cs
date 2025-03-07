// API/Services/MateriaService.cs
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
    public interface IMateriaService
    {
        List<MateriaViewModel> ObtenerMaterias();
        MateriaViewModel ObtenerMateriaPorId(int id);
    }

    public class MateriaService : IMateriaService
    {
        private readonly IMateriaRepository _materiaRepository;
        private readonly IMapper _mapper;

        public MateriaService(IMateriaRepository materiaRepository, IMapper mapper)
        {
            _materiaRepository = materiaRepository;
            _mapper = mapper;
        }

        public List<MateriaViewModel> ObtenerMaterias()
        {
            var materias = _materiaRepository.ObtenerMaterias();
            return _mapper.Map<List<MateriaViewModel>>(materias);
        }

        public MateriaViewModel ObtenerMateriaPorId(int id)
        {
            var materia = _materiaRepository.ObtenerMateriaPorId(id);
            return _mapper.Map<MateriaViewModel>(materia);
        }
    }
}