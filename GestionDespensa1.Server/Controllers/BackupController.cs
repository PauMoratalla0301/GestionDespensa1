using Microsoft.AspNetCore.Mvc;
using GestionDespensa1.Server.Servicios;

namespace GestionDespensa1.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BackupController : ControllerBase
    {
        private readonly BackupService _backupService;

        public BackupController(BackupService backupService)
        {
            _backupService = backupService;
        }

        [HttpPost("crear")]
        public async Task<ActionResult<BackupInfoDTO>> CrearBackup()
        {
            try
            {
                var resultado = await _backupService.CrearBackup();
                if (resultado.Success)
                {
                    return Ok(resultado);
                }
                return BadRequest(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("listar")]
        public async Task<ActionResult<List<BackupInfoDTO>>> ListarBackups()
        {
            try
            {
                var backups = await _backupService.ObtenerBackups();
                return Ok(backups);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("descargar/{fileName}")]
        public async Task<IActionResult> DescargarBackup(string fileName)
        {
            try
            {
                var fileBytes = await _backupService.DescargarBackup(fileName);
                return File(fileBytes, "application/octet-stream", fileName);
            }
            catch (FileNotFoundException)
            {
                return NotFound("Archivo no encontrado");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost("restaurar/{fileName}")]
        public async Task<IActionResult> RestaurarBackup(string fileName)
        {
            try
            {
                var resultado = await _backupService.RestaurarBackup(fileName);
                if (resultado)
                {
                    return Ok("Backup restaurado correctamente");
                }
                return BadRequest("No se pudo restaurar el backup");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}