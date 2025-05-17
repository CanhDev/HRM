using ERP.DTO.Lists;
using ERP.Entities.Vouchers.Employee;
using ERP.Entities;
using ERP.Services.Lists.interfaces;
using ERP.Base_sys;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ERP.Services.Lists
{
    public class EmployeeDocumentService : IEmployeeDocumentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileStorageService _fileService;
        private readonly ILogger<EmployeeDocumentService> _logger;

        public EmployeeDocumentService(
            ApplicationDbContext context,
            IFileStorageService fileService,
            ILogger<EmployeeDocumentService> logger)
        {
            _context = context;
            _fileService = fileService;
            _logger = logger;
        }

        // Folder operations
        public async Task<ApiRespone_basic> GetFoldersByEmployeeIdAsync(int employeeId)
        {
            var res =  await _context.employeeDocumentFolders
                .Where(f => f.employeeId == employeeId)
                .ToListAsync();
            return new ApiRespone_basic
            {
                Success = true,
                Data = res
            };
        }

        public async Task<ApiRespone_basic> GetFolderByIdAsync(int folderId)
        {
            var res = await _context.employeeDocumentFolders
                .Include(f => f.documents)
                .FirstOrDefaultAsync(f => f.id == folderId);
            return new ApiRespone_basic
            {
                Success = true,
                Data = res
            };
        }

        public async Task<ApiRespone_basic> CreateFolderAsync(EmployeeDocumentFolderDTO folderDto)
        {
            var folder = new EmployeeDocumentFolder
            {
                employeeId = folderDto.employeeId,
                folderName = folderDto.folderName,
                description = folderDto.description,
            };

            _context.employeeDocumentFolders.Add(folder);
            await _context.SaveChangesAsync();

            folderDto.id = folder.id;
            return new ApiRespone_basic
            {
                Success = true,
                Data = folderDto
            };
        }

        public async Task<ApiRespone_basic> UpdateFolderAsync(EmployeeDocumentFolderDTO folderDto)
        {
            var folder = await _context.employeeDocumentFolders.FindAsync(folderDto.id);
            if (folder == null)
                throw new KeyNotFoundException($"Folder with ID {folderDto.id} not found");

            folder.folderName = folderDto.folderName;
            folder.description = folderDto.description;

            await _context.SaveChangesAsync();
            return new ApiRespone_basic
            {
                Success = true,
                Data = folderDto
            };
        }

        public async Task<ApiRespone_basic> DeleteFolderAsync(int folderId)
        {
            var folder = await _context.employeeDocumentFolders
                .Include(f => f.documents)
                .FirstOrDefaultAsync(f => f.id == folderId);

            if (folder == null)
                return new ApiRespone_basic { Success = false};

            // Delete all documents in the folder
            if (folder.documents != null && folder.documents.Any())
            {
                foreach (var document in folder.documents)
                {
                    if (!string.IsNullOrEmpty(document.filePath))
                    {
                        await _fileService.DeleteFileAsync(document.filePath);
                    }
                }
                _context.EmployeeDocuments.RemoveRange(folder.documents);
            }

            _context.employeeDocumentFolders.Remove(folder);
            await _context.SaveChangesAsync();
            return new ApiRespone_basic { Success = true};
        }

        // Document operations
        public async Task<ApiRespone_basic> GetDocumentsByFolderIdAsync(int folderId)
        {
            var res = await _context.EmployeeDocuments
                .Where(d => d.folderId == folderId)
                .ToListAsync();
            return new ApiRespone_basic
            {
                Success = true,
                Data = res
            };
        }

        public async Task<ApiRespone_basic> GetDocumentsByEmployeeIdAsync(int employeeId)
        {
            var res =  await _context.EmployeeDocuments
                .Where(d => d.employeeId == employeeId)
                .ToListAsync();
            return new ApiRespone_basic
            {
                Success = true,
                Data = res
            };
        }

        public async Task<ApiRespone_basic> GetDocumentByIdAsync(int documentId)
        {
            var res =  await _context.EmployeeDocuments.FindAsync(documentId);
            return new ApiRespone_basic
            {
                Success = true,
                Data = res
            };
        }

        public async Task<ApiRespone_basic> CreateDocumentAsync(EmployeeDocumentDTO documentDto)
        {
            var document = new EmployeeDocument
            {
                employeeId = documentDto.employeeId,
                folderId = documentDto.folderId,
                documentName = documentDto.documentName,
                documentType = documentDto.documentType,
                filePath = documentDto.filePath,
                notes = documentDto.notes,
                tags = documentDto.tags,
                expiryDate = documentDto.expiryDate,
                isCompany = documentDto.isCompany ?? false,
                status = documentDto.status,
                documentUrl = documentDto.documentUrl,
                createdAt = DateTime.Now,
            };

            _context.EmployeeDocuments.Add(document);
            await _context.SaveChangesAsync();

            documentDto.id = document.id;
            return new ApiRespone_basic
            {
                Success = true,
                Data = documentDto
            };

        }

        public async Task<ApiRespone_basic> UpdateDocumentAsync(EmployeeDocumentDTO documentDto)
        {
            var document = await _context.EmployeeDocuments.FindAsync(documentDto.id);
            if (document == null)
                throw new KeyNotFoundException($"Document with ID {documentDto.id} not found");

            document.documentName = documentDto.documentName;
            document.documentType = documentDto.documentType;
            document.notes = documentDto.notes;
            document.tags = documentDto.tags;
            document.expiryDate = documentDto.expiryDate;
            document.isCompany = documentDto.isCompany ?? document.isCompany;
            document.status = documentDto.status;
            document.updateAt = DateTime.Now;

            // Update file path if new file was uploaded
            if (!string.IsNullOrEmpty(documentDto.filePath))
            {
                document.filePath = documentDto.filePath;
                document.documentUrl = documentDto.documentUrl;
            }

            await _context.SaveChangesAsync();
            return new ApiRespone_basic
            {
                Success = true,
                Data = documentDto
            };
        }

        public async Task<ApiRespone_basic> DeleteDocumentAsync(int documentId)
        {
            var document = await _context.EmployeeDocuments.FindAsync(documentId);
            if (document == null)
                return new ApiRespone_basic { Success = false};

            _context.EmployeeDocuments.Remove(document);
            await _context.SaveChangesAsync();
            return new ApiRespone_basic { Success = true };
        }

        // Combined operations
        public async Task<ApiRespone_basic> CreateFolderWithDocumentsAsync(EmployeeDocFolderWithDocs model)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Create folder first
                var folderResult = await CreateFolderAsync(model.folder);
                var folder = folderResult.Data as EmployeeDocumentFolderDTO;
                // Create documents with the new folder ID
                if (model.documents != null && model.documents.Any())
                {
                    foreach (var doc in model.documents)
                    {
                        doc.folderId = folder.id;
                        await CreateDocumentAsync(doc);
                    }
                }

                await transaction.CommitAsync();
                return new ApiRespone_basic
                {
                    Success = true,
                    Data = model
                };
            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync();
                return new ApiRespone_basic
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        // Tag operations
        public async Task<ApiRespone_basic> GetAllTagsAsync()
        {
            // Get all unique tags from documents
            var allTags = await _context.EmployeeDocuments
                .Where(d => !string.IsNullOrEmpty(d.tags))
                .Select(d => d.tags)
                .Distinct()
                .ToListAsync();

            var tagsList = new List<DocumentTag>();
            foreach (var tagString in allTags)
            {
                var res = await ParseTagsAsync(tagString);
                var parsedTags = res.Data as List<DocumentTag>;
                tagsList.AddRange(parsedTags);
            }

            // Return distinct tags
            return new ApiRespone_basic
            {
                Success = true,
                Data = tagsList
                .GroupBy(t => t.name)
                .Select(g => g.First())
                .ToList()
            };
        }

        public async Task<ApiRespone_basic> ParseTagsAsync(string tagsString)
        {
            if (string.IsNullOrEmpty(tagsString))
                return new ApiRespone_basic
                {
                    Success = true,
                    Data = new List<DocumentTag>()
                };

            // Split by comma and trim each tag
            var tags = tagsString.Split(',')
                .Select(t => t.Trim())
                .Where(t => !string.IsNullOrEmpty(t))
                .Select(tag => new DocumentTag
                {
                    name = tag,
                    color = GetColorForTag(tag) // Helper method to assign a color based on tag name
                })
                .ToList();

            var res = await Task.FromResult(tags);
            return new ApiRespone_basic
            {
                Success = true,
                Data = res
            };
        }

        // Helper method to generate consistent colors for tags
        private string GetColorForTag(string tag)
        {
            // Generate a color based on the hash of the tag name
            // This ensures the same tag always gets the same color
            var hash = tag.GetHashCode();
            var colors = new[] { "#4CAF50", "#2196F3", "#FFC107", "#E91E63", "#9C27B0", "#FF5722", "#607D8B" };
            var index = Math.Abs(hash) % colors.Length;
            return colors[index];
        }
    }
}
