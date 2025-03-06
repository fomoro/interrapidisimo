// API/Controllers/ProfesorController.cs
using Microsoft.AspNetCore.Mvc;
using API.Services;
using API.ViewModels;
using System.Collections.Generic;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfesorController : ControllerBase
    {
        private readonly IProfesorService _profesorService;

        public ProfesorController(IProfesorService profesorService)
        {
            _profesorService = profesorService;
        }

        [HttpGet]
        public IActionResult ObtenerProfesores()
        {
            try
            {
                var profesores = _profesorService.ObtenerProfesores();
                return Ok(profesores);
            }
            catch
            {
                return StatusCode(500, new { error = "Error al obtener los profesores" });
            }
        }

        [HttpGet("{id}")]
        public IActionResult ObtenerProfesorPorId(int id)
        {
            try
            {
                var profesor = _profesorService.ObtenerProfesorConMaterias(id);
                if (profesor == null)
                {
                    return NotFound(new { error = "Profesor no encontrado" });
                }
                return Ok(profesor);
            }
            catch
            {
                return StatusCode(500, new { error = "Error al obtener el profesor" });
            }
        }
    }
}