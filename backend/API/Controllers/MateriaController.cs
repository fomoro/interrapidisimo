// API/Controllers/MateriaController.cs
using Microsoft.AspNetCore.Mvc;
using API.Services;
using API.ViewModels;
using System.Collections.Generic;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MateriaController : ControllerBase
    {
        private readonly IMateriaService _materiaService;

        public MateriaController(IMateriaService materiaService)
        {
            _materiaService = materiaService;
        }

        [HttpGet]
        public IActionResult ObtenerMaterias()
        {
            try
            {
                var materias = _materiaService.ObtenerMaterias();
                return Ok(materias);
            }
            catch
            {
                return StatusCode(500, new { error = "Error al obtener las materias" });
            }
        }

        [HttpGet("{id}")]
        public IActionResult ObtenerMateriaPorId(int id)
        {
            try
            {
                var materia = _materiaService.ObtenerMateriaPorId(id);
                if (materia == null)
                {
                    return NotFound(new { error = "Materia no encontrada" });
                }
                return Ok(materia);
            }
            catch
            {
                return StatusCode(500, new { error = "Error al obtener la materia" });
            }
        }
    }
}