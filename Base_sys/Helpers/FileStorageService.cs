using ERP.Services.Lists.interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ERP.Base_sys.Helpers
{
    public class FileStorageService : IFileStorageService
    {
        private readonly string _storageRootPath;
        private readonly ILogger<FileStorageService> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FileStorageService(
            ILogger<FileStorageService> logger,
            IWebHostEnvironment environment,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;

            // Set thư mục lưu file: wwwroot/resource/files
            _storageRootPath = Path.Combine(_environment.WebRootPath, "resource", "files");
        }

        public async Task<string> SaveFileAsync(IFormFile file, string containerName)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            var now = DateTime.Now;
            var year = now.Year.ToString();
            var month = now.Month.ToString("D2");

            var folderPath = Path.Combine(_storageRootPath, containerName, year, month);
            Directory.CreateDirectory(folderPath);

            var originalName = Path.GetFileNameWithoutExtension(file.FileName);
            var extension = Path.GetExtension(file.FileName);
            var timestamp = now.ToString("yyyyMMddHHmmss");
            var uniqueFileName = $"{originalName}-{timestamp}{extension}";

            var fullPath = Path.Combine(folderPath, uniqueFileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            _logger.LogInformation($"File saved: {fullPath}");

            // Trả về URL công khai
            var request = _httpContextAccessor.HttpContext?.Request;
            var baseUrl = $"{request?.Scheme}://{request?.Host}";
            var publicUrl = $"{baseUrl}/resource/files/{containerName}/{year}/{month}/{uniqueFileName}";

            return publicUrl;
        }

        public async Task<byte[]> GetFileAsync(string fileUrl)
        {
            if (string.IsNullOrWhiteSpace(fileUrl))
                return null;

            try
            {
                var uri = new Uri(fileUrl);
                var relativePath = uri.LocalPath.Replace("/resource/files", "").TrimStart('/');
                var physicalPath = Path.Combine(_storageRootPath, relativePath);

                if (!File.Exists(physicalPath))
                    return null;

                return await File.ReadAllBytesAsync(physicalPath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving file");
                return null;
            }
        }

        public Task<bool> DeleteFileAsync(string fileUrl)
        {
            if (string.IsNullOrWhiteSpace(fileUrl))
                return Task.FromResult(false);

            try
            {
                var uri = new Uri(fileUrl);
                var relativePath = uri.LocalPath.Replace("/resource/files", "").TrimStart('/');
                var physicalPath = Path.Combine(_storageRootPath, relativePath);

                if (File.Exists(physicalPath))
                {
                    File.Delete(physicalPath);
                    _logger.LogInformation($"File deleted: {physicalPath}");
                    return Task.FromResult(true);
                }

                return Task.FromResult(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting file");
                return Task.FromResult(false);
            }
        }
    }
}
