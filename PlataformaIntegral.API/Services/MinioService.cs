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

        public MinioService(IMinioClient minioClient, IConfiguration configuration)
        {
            _minioClient = minioClient;
            _configuration = configuration;
        }

        // ================================
        //  MÉTODO PRINCIPAL: SUBIR ARCHIVO
        // ================================
        public async Task<string> UploadFileAsync(IFormFile file, string bucketName)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));

            // Crear nombre único
            var extension = Path.GetExtension(file.FileName);
            var objectKey = $"{Guid.NewGuid()}{extension}";

            using var stream = file.OpenReadStream();

            // Asegurar bucket
            await EnsureBucketExistsAsync(bucketName);

            var args = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectKey)
                .WithStreamData(stream)
                .WithObjectSize(file.Length)
                .WithContentType(file.ContentType ?? "application/octet-stream");

            await _minioClient.PutObjectAsync(args);

            return objectKey; // LO QUE GUARDAS EN BD
        }

        // =====================================
        // OPCIONAL: subir archivo desde Stream
        // =====================================
        public async Task<string> UploadFileAsync(Stream fileStream, string originalFileName, string bucketName)
        {
            var extension = Path.GetExtension(originalFileName);
            var objectKey = $"{Guid.NewGuid()}{extension}";

            await EnsureBucketExistsAsync(bucketName);

            var args = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectKey)
                .WithStreamData(fileStream)
                .WithObjectSize(-1) // tamaño desconocido
                .WithContentType("application/octet-stream");

            await _minioClient.PutObjectAsync(args);

            return objectKey;
        }

        // ================================
        // DESCARGAR ARCHIVO
        // ================================
        public async Task<Stream> GetFileAsync(string bucketName, string objectKey)
        {
            MemoryStream memStream = new();

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
            var args = new RemoveObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectKey);

            await _minioClient.RemoveObjectAsync(args);
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


        // ================================
        // HELPERS
        // ================================
        private async Task EnsureBucketExistsAsync(string bucketName)
        {
            bool exists = await _minioClient.BucketExistsAsync(
                new BucketExistsArgs().WithBucket(bucketName)
            );

            if (!exists)
            {
                await _minioClient.MakeBucketAsync(
                    new MakeBucketArgs().WithBucket(bucketName)
                );
            }
        }
    }
}
