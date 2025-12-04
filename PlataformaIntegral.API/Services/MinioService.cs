using Minio;
using Minio.DataModel.Args;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;

namespace PlataformaIntegral.API.Services
{
    public class MinioService
    {
        private readonly IMinioClient _minioClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<MinioService> _logger;

        public MinioService(IMinioClient minioClient, IConfiguration configuration, ILogger<MinioService> logger)
        {
            _minioClient = minioClient;
            _configuration = configuration;
            _logger = logger;
        }

        private static string GetContentTypeFromExtension(string extension)
        {
            return extension.ToLower() switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".tiff" => "image/tiff",
                ".ico" => "image/x-icon",
                ".mp4" => "video/mp4",
                ".mov" => "video/quicktime",
                ".avi" => "video/x-msvideo",
                ".wmv" => "video/x-ms-wmv",
                ".mkv" => "video/x-matroska",
                _ => "application/octet-stream"
            };
        }

        // ================================
        // Asegurar bucket
        // ================================
        private async Task EnsureBucketExistsAsync(string bucketName)
        {
            try
            {
                var found = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName));
                if (!found)
                {
                    await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName));
                    _logger.LogInformation("Bucket {BucketName} creado en MinIO.", bucketName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error asegurando bucket {BucketName}", bucketName);
                throw;
            }
        }

        // ================================
        // SUBIR ARCHIVO (IFormFile)
        // ================================
        public async Task<string> UploadFileAsync(IFormFile file, string bucketName, string? prefix = null)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            var extension = Path.GetExtension(file.FileName);
            var objectKey = $"{prefix ?? ""}{Guid.NewGuid()}{extension}";

            using var stream = file.OpenReadStream();
            await EnsureBucketExistsAsync(bucketName);

            var contentType = string.IsNullOrWhiteSpace(file.ContentType)
                ? GetContentTypeFromExtension(extension)
                : file.ContentType;

            var args = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectKey)
                .WithStreamData(stream)
                .WithObjectSize(file.Length)
                .WithContentType(contentType);

            await _minioClient.PutObjectAsync(args);
            return objectKey;
        }

        // ================================
        // SUBIR ARCHIVO (Stream)
        // ================================
        public async Task<string> UploadFileAsync(Stream fileStream, string originalFileName, string bucketName, string? prefix = null)
        {
            var extension = Path.GetExtension(originalFileName);
            var objectKey = $"{prefix ?? ""}{Guid.NewGuid()}{extension}";

            await EnsureBucketExistsAsync(bucketName);

            var contentType = GetContentTypeFromExtension(extension);

            var args = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectKey)
                .WithStreamData(fileStream)
                .WithObjectSize(fileStream.CanSeek ? fileStream.Length : -1)
                .WithContentType(contentType);

            await _minioClient.PutObjectAsync(args);
            return objectKey;
        }

        // ================================
        // DESCARGAR ARCHIVO
        // ================================
        public async Task<Stream> GetFileAsync(string bucketName, string objectKey)
        {
            var memStream = new MemoryStream();

            var args = new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectKey)
                .WithCallbackStream(stream => stream.CopyTo(memStream));

            await _minioClient.GetObjectAsync(args);
            memStream.Position = 0;
            return memStream;
        }

        // ================================
        // URL PRESIGNADA
        // ================================
        public async Task<string> GetFileUrlAsync(string bucketName, string objectKey, TimeSpan expiry)
        {
            var seconds = (int)expiry.TotalSeconds;

            var args = new PresignedGetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectKey)
                .WithExpiry(seconds);

            return await _minioClient.PresignedGetObjectAsync(args);
        }

        // ================================
        // ELIMINAR ARCHIVO
        // ================================
        public async Task DeleteFileAsync(string bucketName, string objectKey)
        {
            try
            {
                var args = new RemoveObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectKey);

                await _minioClient.RemoveObjectAsync(args);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando objeto {ObjectKey} en bucket {BucketName}", objectKey, bucketName);
                throw;
            }
        }


        // ================================
        // MÉTODOS ESPECÍFICOS PARA VIDEOS
        // ================================
        public async Task<string> UploadVideoAsync(IFormFile file)
        {
            string bucket = _configuration["Minio:Buckets:Videos"] ?? "videos";
            return await UploadFileAsync(file, bucket);
        }

        public async Task<string> GetVideoUrlAsync(string objectKey, TimeSpan expiry)
        {
            string bucket = _configuration["Minio:Buckets:Videos"] ?? "videos";
            return await GetFileUrlAsync(bucket, objectKey, expiry);
        }

        public async Task DeleteVideoAsync(string objectKey)
        {
            string bucket = _configuration["Minio:Buckets:Videos"] ?? "videos";
            await DeleteFileAsync(bucket, objectKey);
        }

        // ===================================
        // METODOS ESPECIFICOS PARA MINIATURAS
        // ===================================
        public async Task<string> UploadMiniaturaAsync(IFormFile file)
        {
            string bucket = _configuration["Minio:Buckets:Miniaturas"] ?? "miniaturas";
            return await UploadFileAsync(file, bucket);
        }

        public async Task<string> GetMiniaturaUrlAsync(string objectKey, TimeSpan expiry)
        {
            string bucket = _configuration["Minio:Buckets:Miniaturas"] ?? "miniaturas";
            return await GetFileUrlAsync(bucket, objectKey, expiry);
        }
        public async Task DeleteMiniaturaAsync(string objectKey)
        {
            string bucket = _configuration["Minio:Buckets:Miniaturas"] ?? "miniaturas";
            await DeleteFileAsync(bucket, objectKey);
        }

        // ================================
        // MÉTODOS ESPECÍFICOS PARA IMÁGENES
        // ================================
        public async Task<string> UploadImageAsync(IFormFile file)
        {
            string bucket = _configuration["Minio:Buckets:Resources"] ?? "otros-recursos";
            return await UploadFileAsync(file, bucket);
        }

        public async Task<string> GetImageUrlAsync(string objectKey, TimeSpan expiry)
        {
            string bucket = _configuration["Minio:Buckets:Resources"] ?? "otros-recursos";
            return await GetFileUrlAsync(bucket, objectKey, expiry);
        }

        public async Task DeleteImageAsync(string objectKey)
        {
            string bucket = _configuration["Minio:Buckets:Resources"] ?? "otros-recursos";
            await DeleteFileAsync(bucket, objectKey);
        }

        // =================================
        // MÉTODOS ESPECÍFICOS PARA USUARIOS
        // =================================
        public async Task<string> UploadUsuarioImageAsync(IFormFile file)
        {
            string bucket = _configuration["Minio:Buckets:Users"] ?? "usuarios";
            return await UploadFileAsync(file, bucket);
        }

        public async Task<string> GetUsuarioImageUrlAsync(string objectKey, TimeSpan expiry)
        {
            string bucket = _configuration["Minio:Buckets:Users"] ?? "usuarios";
            return await GetFileUrlAsync(bucket, objectKey, expiry);
        }

        public async Task DeleteUsuarioImageAsync(string objectKey)
        {
            string bucket = _configuration["Minio:Buckets:Users"] ?? "usuarios";
            await DeleteFileAsync(bucket, objectKey);
        }
    }
}
