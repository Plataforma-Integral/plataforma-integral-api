using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaIntegral.API.Services;

namespace PlataformaIntegral.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrador")] // Solo administradores pueden manipular archivos
    public class FilesController : ControllerBase
    {
        private readonly MinioService _minioService;
        private readonly IConfiguration _config;
        private readonly ILogger<FilesController> _logger;

        public FilesController(MinioService minioService, IConfiguration config, ILogger<FilesController> logger)
        {
            _minioService = minioService;
            _config = config;
            _logger = logger;
        }

        // =========================================
        // SUBIR ARCHIVO A UN BUCKET
        // =========================================
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file, [FromQuery] string bucket, [FromQuery] int minutesExpiry = 30)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Debe seleccionar un archivo válido.");

            if (string.IsNullOrWhiteSpace(bucket))
                return BadRequest("Debe especificar un bucket.");

            try
            {
                var objectKey = await _minioService.UploadFileAsync(file, bucket);

                var expiry = TimeSpan.FromMinutes(minutesExpiry > 0 ? minutesExpiry : 30);
                var url = await _minioService.GetFileUrlAsync(bucket, objectKey, expiry);

                return CreatedAtAction(nameof(GetFileUrl), new { bucket, fileName = objectKey, minutesExpiry }, new
                {
                    message = "Archivo subido correctamente",
                    bucket,
                    objectKey,
                    url
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error subiendo archivo a MinIO.");
                return StatusCode(500, "Ocurrió un error al subir el archivo.");
            }
        }

        // =========================================
        // DESCARGAR ARCHIVO (STREAM)
        // =========================================
        [HttpGet("download")]
        public async Task<IActionResult> DownloadFile([FromQuery] string bucket, [FromQuery] string fileName)
        {
            if (string.IsNullOrWhiteSpace(bucket) || string.IsNullOrWhiteSpace(fileName))
                return BadRequest("Debe especificar bucket y nombre de archivo.");

            try
            {
                var stream = await _minioService.GetFileAsync(bucket, fileName);
                return File(stream, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error descargando archivo desde MinIO.");
                return NotFound($"No se pudo encontrar o descargar el archivo {fileName} en el bucket {bucket}.");
            }
        }

        // =========================================
        // OBTENER URL TEMPORAL
        // =========================================
        [HttpGet("url")]
        public async Task<IActionResult> GetFileUrl([FromQuery] string bucket, [FromQuery] string fileName, [FromQuery] int minutesExpiry = 30)
        {
            if (string.IsNullOrWhiteSpace(bucket) || string.IsNullOrWhiteSpace(fileName))
                return BadRequest("Debe especificar bucket y nombre de archivo.");

            try
            {
                var expiry = TimeSpan.FromMinutes(minutesExpiry > 0 ? minutesExpiry : 30);
                var url = await _minioService.GetFileUrlAsync(bucket, fileName, expiry);
                return Ok(new { bucket, fileName, url });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generando URL firmada de MinIO.");
                return StatusCode(500, "No se pudo generar la URL del archivo.");
            }
        }

        // =========================================
        // ELIMINAR ARCHIVO
        // =========================================
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteFile([FromQuery] string bucket, [FromQuery] string fileName)
        {
            if (string.IsNullOrWhiteSpace(bucket) || string.IsNullOrWhiteSpace(fileName))
                return BadRequest("Debe especificar bucket y nombre de archivo.");

            try
            {
                await _minioService.DeleteFileAsync(bucket, fileName);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando archivo en MinIO.");
                return NotFound($"No se pudo eliminar el archivo {fileName} en el bucket {bucket}.");
            }
        }
    }
}
