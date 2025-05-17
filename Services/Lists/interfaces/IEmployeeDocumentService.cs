using ERP.Base_sys.Services;
using ERP.Base_sys;
using ERP.Entities.Vouchers.Employee;
using ERP.DTO.Lists;

namespace ERP.Services.Lists.interfaces
{
    public interface IEmployeeDocumentService
    {
        // Folder operations
        Task<ApiRespone_basic> GetFoldersByEmployeeIdAsync(int employeeId);
        Task<ApiRespone_basic> GetFolderByIdAsync(int folderId);
        Task<ApiRespone_basic> CreateFolderAsync(EmployeeDocumentFolderDTO folderDto);
        Task<ApiRespone_basic> UpdateFolderAsync(EmployeeDocumentFolderDTO folderDto);
        Task<ApiRespone_basic> DeleteFolderAsync(int folderId);

        // Document operations
        Task<ApiRespone_basic> GetDocumentsByFolderIdAsync(int folderId);
        Task<ApiRespone_basic> GetDocumentsByEmployeeIdAsync(int employeeId);
        Task<ApiRespone_basic> GetDocumentByIdAsync(int documentId);
        Task<ApiRespone_basic> CreateDocumentAsync(EmployeeDocumentDTO documentDto);
        Task<ApiRespone_basic> UpdateDocumentAsync(EmployeeDocumentDTO documentDto);
        Task<ApiRespone_basic> DeleteDocumentAsync(int documentId);

        // Combined operations
        Task<ApiRespone_basic> CreateFolderWithDocumentsAsync(EmployeeDocFolderWithDocs model);

        // Tag operations
        Task<ApiRespone_basic> GetAllTagsAsync();
        Task<ApiRespone_basic> ParseTagsAsync(string tagsString);
    }
}