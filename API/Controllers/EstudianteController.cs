﻿using Microsoft.AspNetCore.Mvc;
using API.Services;
using API.ViewModels;
using System.Collections.Generic;
using System;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstudianteController : ControllerBase
    {
        private readonly IEstudianteService _estudianteService;

        public EstudianteController(IEstudianteService estudianteService)
        {
            _estudianteService = estudianteService;
        }

        [HttpPost]
        public IActionResult CrearEstudiante([FromBody] EstudianteViewModel estudianteViewModel)
        {
            if (string.IsNullOrWhiteSpace(estudianteViewModel.Nombre) || estudianteViewModel.Nombre.Trim().Length < 3)
            {
                return BadRequest(new { error = "El nombre debe tener al menos 3 caracteres" });
            }

            try
            {
                var estudiante = _estudianteService.CrearEstudiante(estudianteViewModel);
                return CreatedAtAction(nameof(ObtenerEstudiantePorId), new { id = estudiante.Id }, estudiante);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult ObtenerEstudiantes()
        {
            try
            {
                var estudiantes = _estudianteService.ObtenerEstudiantes();
                return Ok(estudiantes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public IActionResult ObtenerEstudiantePorId(int id)
        {
            try
            {
                var estudiante = _estudianteService.ObtenerEstudiantePorId(id);
                if (estudiante == null)
                {
                    return NotFound(new { error = "Estudiante no encontrado" });
                }
                return Ok(estudiante);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult ActualizarEstudiante(int id, [FromBody] EstudianteViewModel estudianteViewModel)
        {
            if (estudianteViewModel.Nombre != null && estudianteViewModel.Nombre.Trim().Length < 3)
            {
                return BadRequest(new { error = "El nombre debe tener al menos 3 caracteres" });
            }

            try
            {
                var estudiante = _estudianteService.ActualizarEstudiante(id, estudianteViewModel);
                if (estudiante == null)
                {
                    return NotFound(new { error = "Estudiante no encontrado" });
                }
                return Ok(estudiante);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult EliminarEstudiante(int id)
        {
            try
            {
                var resultado = _estudianteService.EliminarEstudiante(id);
                if (!resultado)
                {
                    return NotFound(new { error = "Estudiante no encontrado" });
                }
                return Ok(new { mensaje = "Eliminado correctamente" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("{id}/companeros")]
        public async Task<IActionResult> ObtenerCompanerosPorEstudianteId(int id)
        {
            try
            {
                var companeros = await _estudianteService.ObtenerCompanerosPorEstudianteId(id);
                if (companeros == null || companeros.Count == 0)
                {
                    return NotFound(new { error = "No se encontraron compañeros para el estudiante especificado" });
                }
                return Ok(companeros);
            }
            catch
            {
                return StatusCode(500, new { error = "Error al obtener los compañeros del estudiante" });
            }
        }

        [HttpGet("{id}/visibilidad")]
        public async Task<IActionResult> ObtenerVisibilidadEstudiantes(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { error = "ID de estudiante no válido" });
                }

                var resultados = await _estudianteService.ObtenerVisibilidadEstudiantes(id);
                if (resultados == null || resultados.Count == 0)
                {
                    return NotFound(new { error = "No se encontraron resultados para el estudiante especificado" });
                }
                return Ok(resultados);
            }
            catch
            {
                return StatusCode(500, new { error = "Error al obtener la visibilidad de los estudiantes" });
            }
        }

    }
}