using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;



namespace ERP.Base_sys.Helpers
{
    public class FileHelper
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        // Định nghĩa các loại file được phép
        private readonly Dictionary<string, string[]> _allowedFileTypes = new Dictionary<string, string[]>
        {
            { "document", new[] { ".doc", ".docx", ".pdf", ".rtf", ".txt" } },
            { "spreadsheet", new[] { ".xls", ".xlsx", ".csv" } },
            { "presentation", new[] { ".ppt", ".pptx" } },
            { "archive", new[] { ".zip", ".rar", ".7z" } },
            { "pdf", new[] { ".pdf" } }
        };

        // Giới hạn kích thước file mặc định là 20MB
        private readonly long _maxFileSize = 20 * 1024 * 1024;

        public FileHelper(
            IWebHostEnvironment environment,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;

            // Đọc cấu hình giới hạn kích thước từ appsettings.json nếu có
            if (long.TryParse(_configuration["FileSettings:MaxFileSizeBytes"], out long configFileSize))
            {
                _maxFileSize = configFileSize;
            }
        }

        /// <summary>
        /// Lưu file với các tùy chọn
        /// </summary>
        /// <param name="file">File để lưu</param>
        /// <param name="fileCategory">Danh mục file (document, spreadsheet, presentation, pdf, archive)</param>
        /// <param name="subFolder">Thư mục con tùy chọn (ví dụ: contracts, reports)</param>
        /// <param name="preserveOriginalName">Giữ tên file gốc hay không</param>
        /// <returns>URL của file hoặc null nếu thất bại</returns>
        public async Task<FileUploadResult> SaveFileAsync(
            IFormFile file,
            string fileCategory,
            string subFolder = "",
            bool preserveOriginalName = true)
        {
            if (file == null || file.Length == 0)
            {
                return new FileUploadResult
                {
                    Success = false,
                    ErrorMessage = "File rỗng hoặc không tồn tại"
                };
            }

            // Kiểm tra kích thước file
            if (file.Length > _maxFileSize)
            {
                return new FileUploadResult
                {
                    Success = false,
                    ErrorMessage = $"Kích thước file vượt quá giới hạn cho phép ({_maxFileSize / 1024 / 1024}MB)"
                };
            }

            // Kiểm tra định dạng file
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            bool isValidExtension = false;

            // Kiểm tra extension hợp lệ
            if (_allowedFileTypes.ContainsKey(fileCategory))
            {
                isValidExtension = _allowedFileTypes[fileCategory].Contains(fileExtension);
            }
            else
            {
                // Nếu fileCategory không được định nghĩa, kiểm tra tất cả các loại file cho phép
                isValidExtension = _allowedFileTypes.Values.Any(extensions => extensions.Contains(fileExtension));
            }

            if (!isValidExtension)
            {
                string allowedExtensions = string.Join(", ", _allowedFileTypes[fileCategory]);
                return new FileUploadResult
                {
                    Success = false,
                    ErrorMessage = $"Định dạng file không được hỗ trợ. Các định dạng cho phép: {allowedExtensions}"
                };
            }

            try
            {
                // Tạo tên file
                string fileName;
                string originalFileName = Path.GetFileNameWithoutExtension(file.FileName);

                if (preserveOriginalName)
                {
                    // Tạo tên file an toàn từ tên gốc
                    string safeFileName = originalFileName
                        .Replace(" ", "-")
                        .Replace(".", "");

                    safeFileName = Regex.Replace(safeFileName, "[^a-zA-Z0-9-_]", "");

                    // Thêm timestamp để tránh trùng lặp
                    fileName = $"{safeFileName}-{DateTime.Now:yyyyMMddHHmmss}{fileExtension}";
                }
                else
                {
                    fileName = $"{Guid.NewGuid():N}{fileExtension}";
                }

                // Tạo đường dẫn thư mục phân cấp theo năm/tháng
                string dateFolder = DateTime.Now.ToString("yyyy/MM");
                string folderPath = string.IsNullOrEmpty(subFolder)
                    ? Path.Combine("resource", "files", fileCategory, dateFolder)
                    : Path.Combine("resource", "files", fileCategory, subFolder, dateFolder);

                string directoryPath = Path.Combine(_environment.WebRootPath, folderPath);

                // Tạo thư mục nếu chưa tồn tại
                Directory.CreateDirectory(directoryPath);

                string filePath = Path.Combine(directoryPath, fileName);
                string relativePath = folderPath.Replace('\\', '/');
                string fileUrl = $"/{relativePath}/{fileName}";

                // Lưu file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Tạo URL đầy đủ
                var request = _httpContextAccessor.HttpContext?.Request;
                string fullUrl = fileUrl;

                if (request != null)
                {
                    var baseUrl = $"{request.Scheme}://{request.Host}";
                    fullUrl = $"{baseUrl}{fileUrl}";
                }

                return new FileUploadResult
                {
                    Success = true,
                    FileUrl = fullUrl,
                    FileName = fileName,
                    OriginalFileName = file.FileName,
                    FileSize = file.Length,
                    FileType = fileExtension.TrimStart('.').ToUpper(),
                    FilePath = filePath
                };
            }
            catch (Exception ex)
            {
                return new FileUploadResult
                {
                    Success = false,
                    ErrorMessage = $"Lỗi khi lưu file: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Lưu nhiều file cùng lúc
        /// </summary>
        /// <param name="files">Danh sách file</param>
        /// <param name="fileCategory">Danh mục file</param>
        /// <param name="subFolder">Thư mục con tùy chọn</param>
        /// <returns>Danh sách kết quả tải lên</returns>
        public async Task<List<FileUploadResult>> SaveMultipleFilesAsync(
            List<IFormFile> files,
            string fileCategory,
            string subFolder = "")
        {
            List<FileUploadResult> results = new List<FileUploadResult>();

            foreach (var file in files)
            {
                var result = await SaveFileAsync(file, fileCategory, subFolder);
                results.Add(result);
            }

            return results;
        }

        /// <summary>
        /// Xóa file từ máy chủ
        /// </summary>
        /// <param name="fileUrl">URL của file cần xóa</param>
        /// <returns>True nếu xóa thành công, ngược lại là False</returns>
        public bool DeleteFile(string fileUrl)
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

                string relativePath = fileUrl;
                if (fileUrl.StartsWith(baseUrl))
                {
                    relativePath = fileUrl.Substring(baseUrl.Length);
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
        /// Lấy thông tin file
        /// </summary>
        /// <param name="fileUrl">URL của file</param>
        /// <returns>Thông tin file hoặc null nếu không tìm thấy</returns>
        public FileInfo GetFileInfo(string fileUrl)
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

                string relativePath = fileUrl;
                if (fileUrl.StartsWith(baseUrl))
                {
                    relativePath = fileUrl.Substring(baseUrl.Length);
                }

                // Chuyển đường dẫn URL thành đường dẫn file vật lý
                string filePath = Path.Combine(_environment.WebRootPath, relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

                if (File.Exists(filePath))
                {
                    var fileInfo = new FileInfo(filePath);
                    return fileInfo;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Kiểm tra tính hợp lệ của file
        /// </summary>
        /// <param name="file">File cần kiểm tra</param>
        /// <param name="fileCategory">Danh mục file</param>
        /// <returns>Kết quả kiểm tra</returns>
        public FileValidationResult ValidateFile(IFormFile file, string fileCategory)
        {
            if (file == null || file.Length == 0)
            {
                return new FileValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "File rỗng hoặc không tồn tại"
                };
            }

            // Kiểm tra kích thước file
            if (file.Length > _maxFileSize)
            {
                return new FileValidationResult
                {
                    IsValid = false,
                    ErrorMessage = $"Kích thước file vượt quá giới hạn cho phép ({_maxFileSize / 1024 / 1024}MB)"
                };
            }

            // Kiểm tra định dạng file
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            bool isValidExtension = false;

            if (_allowedFileTypes.ContainsKey(fileCategory))
            {
                isValidExtension = _allowedFileTypes[fileCategory].Contains(fileExtension);
            }
            else
            {
                // Nếu fileCategory không được định nghĩa, kiểm tra tất cả các loại file cho phép
                isValidExtension = _allowedFileTypes.Values.Any(extensions => extensions.Contains(fileExtension));
            }

            if (!isValidExtension)
            {
                string allowedExtensions = string.Join(", ", _allowedFileTypes[fileCategory]);
                return new FileValidationResult
                {
                    IsValid = false,
                    ErrorMessage = $"Định dạng file không được hỗ trợ. Các định dạng cho phép: {allowedExtensions}"
                };
            }

            // Kiểm tra nội dung file (chỉ kiểm tra header/signature của file)
            try
            {
                using (var stream = file.OpenReadStream())
                {
                    byte[] headerBytes = new byte[8];
                    stream.Read(headerBytes, 0, headerBytes.Length);

                    // Kiểm tra signature của file
                    bool isValidContent = IsValidFileSignature(headerBytes, fileExtension);

                    if (!isValidContent)
                    {
                        return new FileValidationResult
                        {
                            IsValid = false,
                            ErrorMessage = "Nội dung file không hợp lệ hoặc không khớp với định dạng khai báo"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new FileValidationResult
                {
                    IsValid = false,
                    ErrorMessage = $"Lỗi khi kiểm tra file: {ex.Message}"
                };
            }

            return new FileValidationResult { IsValid = true };
        }

        /// <summary>
        /// Kiểm tra chữ ký (signature) của file
        /// </summary>
        private bool IsValidFileSignature(byte[] headerBytes, string extension)
        {
            // Định nghĩa các signature phổ biến cho từng loại file
            Dictionary<string, List<byte[]>> fileSignatures = new Dictionary<string, List<byte[]>>
            {
                { ".pdf", new List<byte[]> { new byte[] { 0x25, 0x50, 0x44, 0x46 } } }, // %PDF
                { ".doc", new List<byte[]> { new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 } } },
                { ".docx", new List<byte[]> { new byte[] { 0x50, 0x4B, 0x03, 0x04 } } }, // PK.. (ZIP format)
                { ".xls", new List<byte[]> { new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 } } },
                { ".xlsx", new List<byte[]> { new byte[] { 0x50, 0x4B, 0x03, 0x04 } } }, // PK.. (ZIP format)
                { ".ppt", new List<byte[]> { new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 } } },
                { ".pptx", new List<byte[]> { new byte[] { 0x50, 0x4B, 0x03, 0x04 } } }, // PK.. (ZIP format)
                { ".zip", new List<byte[]> { new byte[] { 0x50, 0x4B, 0x03, 0x04 } } },
                { ".rar", new List<byte[]> { new byte[] { 0x52, 0x61, 0x72, 0x21, 0x1A, 0x07 } } }, // Rar!..
                { ".7z", new List<byte[]> { new byte[] { 0x37, 0x7A, 0xBC, 0xAF, 0x27, 0x1C } } } // 7z¼¯'..
            };

            // Nếu không có signature đã định nghĩa cho extension này, coi như hợp lệ
            if (!fileSignatures.ContainsKey(extension))
                return true;

            var signatures = fileSignatures[extension];

            // Kiểm tra từng signature có thể có
            return signatures.Any(signature =>
                headerBytes.Take(signature.Length).SequenceEqual(signature));
        }
    }

    /// <summary>
    /// Kết quả tải file lên
    /// </summary>
    public class FileUploadResult
    {
        public bool Success { get; set; }
        public string FileUrl { get; set; }
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        public long FileSize { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
        public string ErrorMessage { get; set; }
    }

    /// <summary>
    /// Kết quả kiểm tra tính hợp lệ của file
    /// </summary>
    public class FileValidationResult
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
    }
}
