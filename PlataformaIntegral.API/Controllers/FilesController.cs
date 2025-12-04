using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaIntegral.API.DTOs;
using PlataformaIntegral.API.Services;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Administrador")]
public class FilesController : ControllerBase
{
    private readonly MinioService _minioService;
    private readonly ILogger<FilesController> _logger;

    public FilesController(MinioService minioService, ILogger<FilesController> logger)
    {
        _minioService = minioService;
        _logger = logger;
    }

    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadFile([FromForm] FileUploadDto dto, [FromQuery] string bucket, [FromQuery] int minutesExpiry = 30)
    {
        if (dto.File == null || dto.File.Length == 0)
            return BadRequest(new { success = false, message = "Debe seleccionar un archivo válido." });

        if (string.IsNullOrWhiteSpace(bucket))
            return BadRequest(new { success = false, message = "Debe especificar un bucket." });

        try
        {
            var objectKey = await _minioService.UploadFileAsync(dto.File, bucket);
            var expiry = TimeSpan.FromMinutes(minutesExpiry > 0 ? minutesExpiry : 30);
            var url = await _minioService.GetFileUrlAsync(bucket, objectKey, expiry);

            return CreatedAtAction(nameof(GetFileUrl), new { bucket, fileName = objectKey, minutesExpiry }, new
            {
                success = true,
                message = "Archivo subido correctamente",
                bucket,
                objectKey,
                url,
                expiresAt = DateTime.UtcNow.Add(expiry)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error subiendo archivo a MinIO.");
            return StatusCode(500, new { success = false, message = "Ocurrió un error al subir el archivo." });
        }
    }

    [HttpGet("download")]
    public async Task<IActionResult> DownloadFile([FromQuery] string bucket, [FromQuery] string fileName)
    {
        if (string.IsNullOrWhiteSpace(bucket) || string.IsNullOrWhiteSpace(fileName))
            return BadRequest(new { success = false, message = "Debe especificar bucket y nombre de archivo." });

        try
        {
            var stream = await _minioService.GetFileAsync(bucket, fileName);
            return File(stream, "application/octet-stream", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error descargando archivo desde MinIO.");
            return NotFound(new { success = false, message = $"No se pudo encontrar o descargar el archivo {fileName} en el bucket {bucket}." });
        }
    }

    [HttpGet("url")]
    public async Task<IActionResult> GetFileUrl([FromQuery] string bucket, [FromQuery] string fileName, [FromQuery] int minutesExpiry = 30)
    {
        if (string.IsNullOrWhiteSpace(bucket) || string.IsNullOrWhiteSpace(fileName))
            return BadRequest(new { success = false, message = "Debe especificar bucket y nombre de archivo." });

        try
        {
            var expiry = TimeSpan.FromMinutes(minutesExpiry > 0 ? minutesExpiry : 30);
            var url = await _minioService.GetFileUrlAsync(bucket, fileName, expiry);
            return Ok(new { success = true, bucket, fileName, url, expiresAt = DateTime.UtcNow.Add(expiry) });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generando URL firmada de MinIO.");
            return StatusCode(500, new { success = false, message = "No se pudo generar la URL del archivo." });
        }
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteFile([FromQuery] string bucket, [FromQuery] string fileName)
    {
        if (string.IsNullOrWhiteSpace(bucket) || string.IsNullOrWhiteSpace(fileName))
            return BadRequest(new { success = false, message = "Debe especificar bucket y nombre de archivo." });

        try
        {
            await _minioService.DeleteFileAsync(bucket, fileName);
            return Ok(new { success = true, message = $"Archivo {fileName} eliminado correctamente del bucket {bucket}." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error eliminando archivo en MinIO.");
            return NotFound(new { success = false, message = $"No se pudo eliminar el archivo {fileName} en el bucket {bucket}." });
        }
    }
}
