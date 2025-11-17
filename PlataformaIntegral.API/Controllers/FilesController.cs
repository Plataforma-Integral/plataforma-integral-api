using Microsoft.AspNetCore.Mvc;
using PlataformaIntegral.API.Services;

namespace PlataformaIntegral.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        // SUBIR ARCHIVO A UN BUCKET
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file, string bucket, int MinutesExpiry)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Debe seleccionar un archivo.");

            try
            {
                using var stream = file.OpenReadStream();
                await _minioService.UploadFileAsync(stream, file.FileName, bucket);

                // Devuelve la URL temporal para comprobarlo
                TimeSpan expiry = TimeSpan.FromMinutes(MinutesExpiry);
                var url = await _minioService.GetFileUrlAsync(bucket, file.FileName, expiry);
                return Ok(new
                {
                    message = "Archivo subido correctamente",
                    fileName = file.FileName,
                    bucket,
                    url
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error subiendo archivo a MinIO.");
                return StatusCode(500, "Ocurrió un error al subir el archivo.");
            }
        }

        // DESCARGAR ARCHIVO (STREAM)
        [HttpGet("download")]
        public async Task<IActionResult> DownloadFile(string bucket, string fileName)
        {
            try
            {
                var stream = await _minioService.GetFileAsync(bucket, fileName);
                return File(stream, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error descargando archivo desde MinIO.");
                return NotFound("No se pudo encontrar o descargar el archivo.");
            }
        }

        // OBTENER URL TEMPORAL
        [HttpGet("url")]
        public async Task<IActionResult> GetFileUrl(string bucket, string fileName, int minutesExpiry)
        {
            try
            {
                TimeSpan expiry = TimeSpan.FromMinutes(minutesExpiry);
                var url = await _minioService.GetFileUrlAsync(bucket, fileName, expiry);
                return Ok(new { bucket, fileName, url });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generando URL firmada de MinIO.");
                return StatusCode(500, "No se pudo generar la URL del archivo.");
            }
        }
    }
}
