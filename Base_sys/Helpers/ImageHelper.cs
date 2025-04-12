using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using System.Text.RegularExpressions;


namespace ERP.Base_sys.Helpers
{
    public class ImageHelper
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        private readonly long _maxFileSize = 10 * 1024 * 1024; // 10MB mặc định

        public ImageHelper(
            IWebHostEnvironment environment,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;

            // Đọc cấu hình giới hạn kích thước từ appsettings.json nếu có
            if (long.TryParse(_configuration["ImageSettings:MaxFileSizeBytes"], out long configFileSize))
            {
                _maxFileSize = configFileSize;
            }
        }

        /// <summary>
        /// Lưu ảnh với nhiều tùy chọn tối ưu hóa
        /// </summary>
        /// <param name="image">File ảnh</param>
        /// <param name="imageType">Loại ảnh (thư mục lưu trữ)</param>
        /// <param name="maxWidth">Chiều rộng tối đa (null = không thay đổi)</param>
        /// <param name="maxHeight">Chiều cao tối đa (null = không thay đổi)</param>
        /// <param name="quality">Chất lượng nén (1-100)</param>
        /// <param name="preserveOriginalName">Giữ tên file gốc hay không</param>
        /// <returns>URL của ảnh hoặc null nếu thất bại</returns>
        public async Task<string?> SaveImageAsync(
            IFormFile? image,
            string imageType,
            int? maxWidth = null,
            int? maxHeight = null,
            int quality = 85,
            bool preserveOriginalName = false)
        {
            if (image == null || image.Length == 0)
            {
                return null;
            }

            // Kiểm tra kích thước file
            if (image.Length > _maxFileSize)
            {
                throw new ArgumentException($"Kích thước file vượt quá giới hạn cho phép ({_maxFileSize / 1024 / 1024}MB)");
            }

            // Kiểm tra định dạng file
            var fileExtension = Path.GetExtension(image.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(fileExtension))
            {
                throw new ArgumentException($"Định dạng file không được hỗ trợ. Các định dạng cho phép: {string.Join(", ", _allowedExtensions)}");
            }

            // Tạo tên file
            string fileName;
            if (preserveOriginalName)
            {
                // Tạo tên file an toàn từ tên gốc
                string safeFileName = Path.GetFileNameWithoutExtension(image.FileName)
                    .Replace(" ", "-")
                    .Replace(".", "")
                    .Replace("_", "-");

                safeFileName = Regex.Replace(safeFileName, "[^a-zA-Z0-9-]", "");
                fileName = $"{safeFileName}-{Guid.NewGuid().ToString().Substring(0, 8)}{fileExtension}";
            }
            else
            {
                fileName = $"{Guid.NewGuid():N}{fileExtension}";
            }

            // Tạo đường dẫn thư mục phân cấp theo năm/tháng
            string dateFolder = DateTime.Now.ToString("yyyy/MM");
            string relativePath = Path.Combine("resource", "images", imageType, dateFolder);
            string directoryPath = Path.Combine(_environment.WebRootPath, relativePath);

            // Tạo thư mục nếu chưa tồn tại
            Directory.CreateDirectory(directoryPath);

            string filePath = Path.Combine(directoryPath, fileName);
            string fileUrl = $"/{relativePath.Replace('\\', '/')}/{fileName}";

            // Xử lý và tối ưu hóa ảnh
            try
            {
                using (var imageStream = image.OpenReadStream())
                {
                    using (var outputImage = await Image.LoadAsync(imageStream))
                    {
                        // Thay đổi kích thước nếu cần
                        if (maxWidth.HasValue || maxHeight.HasValue)
                        {
                            int targetWidth = maxWidth ?? outputImage.Width;
                            int targetHeight = maxHeight ?? outputImage.Height;

                            // Tính toán tỷ lệ để duy trì aspect ratio
                            if (maxWidth.HasValue && maxHeight.HasValue)
                            {
                                double widthRatio = (double)maxWidth.Value / outputImage.Width;
                                double heightRatio = (double)maxHeight.Value / outputImage.Height;
                                double ratio = Math.Min(widthRatio, heightRatio);

                                targetWidth = (int)(outputImage.Width * ratio);
                                targetHeight = (int)(outputImage.Height * ratio);
                            }
                            else if (maxWidth.HasValue)
                            {
                                double ratio = (double)maxWidth.Value / outputImage.Width;
                                targetHeight = (int)(outputImage.Height * ratio);
                            }
                            else if (maxHeight.HasValue)
                            {
                                double ratio = (double)maxHeight.Value / outputImage.Height;
                                targetWidth = (int)(outputImage.Width * ratio);
                            }

                            outputImage.Mutate(x => x.Resize(targetWidth, targetHeight));
                        }

                        // Lưu với các tùy chọn nén
                        IImageEncoder encoder;
                        if (fileExtension == ".jpg" || fileExtension == ".jpeg")
                        {
                            encoder = new JpegEncoder { Quality = quality };
                        }
                        else if (fileExtension == ".png")
                        {
                            encoder = new PngEncoder { CompressionLevel = PngCompressionLevel.BestCompression };
                        }
                        else
                        {
                            // Mặc định cho các định dạng khác
                            encoder = new JpegEncoder { Quality = quality };
                        }

                        await outputImage.SaveAsync(filePath, encoder);
                    }
                }

                // Tạo URL đầy đủ
                var request = _httpContextAccessor.HttpContext?.Request;
                if (request != null)
                {
                    var baseUrl = $"{request.Scheme}://{request.Host}";
                    return $"{baseUrl}{fileUrl}";
                }

                return fileUrl;
            }
            catch (Exception ex)
            {
                // Ghi log lỗi nếu cần
                System.Diagnostics.Debug.WriteLine($"Lỗi xử lý ảnh: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Xóa ảnh từ máy chủ
        /// </summary>
        /// <param name="imageUrl">URL của ảnh cần xóa</param>
        /// <returns>True nếu xóa thành công, ngược lại là False</returns>
        public bool DeleteImage(string imageUrl)
        {
            try
            {
                // Lấy đường dẫn tương đối từ URL
                var request = _httpContextAccessor.HttpContext?.Request;
                string baseUrl = string.Empty;

                if (request != null)
                {
                    baseUrl = $"{request.Scheme}://{request.Host}";
                }

                string relativePath = imageUrl;
                if (imageUrl.StartsWith(baseUrl))
                {
                    relativePath = imageUrl.Substring(baseUrl.Length);
                }

                // Chuyển đường dẫn URL thành đường dẫn file vật lý
                string filePath = Path.Combine(_environment.WebRootPath, relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Tải nhiều ảnh cùng lúc
        /// </summary>
        /// <param name="images">Danh sách ảnh</param>
        /// <param name="imageType">Loại ảnh</param>
        /// <returns>Danh sách URL ảnh đã lưu</returns>
        public async Task<List<string>> SaveMultipleImagesAsync(List<IFormFile> images, string imageType)
        {
            List<string> imageUrls = new List<string>();

            foreach (var image in images)
            {
                var imageUrl = await SaveImageAsync(image, imageType);
                if (imageUrl != null)
                {
                    imageUrls.Add(imageUrl);
                }
            }

            return imageUrls;
        }

        /// <summary>
        /// Kiểm tra tính hợp lệ của file ảnh
        /// </summary>
        /// <param name="file">File cần kiểm tra</param>
        /// <returns>True nếu hợp lệ, ngược lại là False</returns>
        public bool IsValidImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            // Kiểm tra kích thước
            if (file.Length > _maxFileSize)
                return false;

            // Kiểm tra định dạng
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(extension))
                return false;

            // Kiểm tra nội dung file
            try
            {
                using (var stream = file.OpenReadStream())
                {
                    // Thử đọc file như một ảnh
                    using (var image = Image.Load(stream))
                    {
                        // Nếu không có lỗi, file là ảnh hợp lệ
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
