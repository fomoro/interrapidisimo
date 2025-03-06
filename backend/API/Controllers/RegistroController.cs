// API/Controllers/RegistroController.cs
using Microsoft.AspNetCore.Mvc;
using API.Services;
using API.ViewModels;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistroController : ControllerBase
    {
        private readonly IRegistroService _registroService;

        public RegistroController(IRegistroService registroService)
        {
            _registroService = registroService;
        }

        [HttpPost]
        public async Task<IActionResult> CrearRegistro([FromBody] RegistroViewModel registroViewModel)
        {
            try
            {
                var registro = await _registroService.CrearRegistro(registroViewModel);
                return CreatedAtAction(nameof(CrearRegistro), new { id = registro.Id }, registro);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch
            {
                return StatusCode(500, new { error = "Error interno del servidor al inscribir estudiante" });
            }
        }
    }
}