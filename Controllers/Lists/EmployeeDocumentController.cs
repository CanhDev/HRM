using ERP.Base_sys;
using ERP.DTO.Lists;
using ERP.Entities.Vouchers.Employee;
using ERP.Services.Lists.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Controllers.Lists
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeDocumentController : ControllerBase
    {
        private readonly IEmployeeDocumentService _documentService;
        private readonly IFileStorageService _fileService;
        private readonly ILogger<EmployeeDocumentController> _logger;

        public EmployeeDocumentController(
            IEmployeeDocumentService documentService,
            IFileStorageService fileService,
            ILogger<EmployeeDocumentController> logger)
        {
            _documentService = documentService;
            _fileService = fileService;
            _logger = logger;
        }

        [HttpGet("folders/{employeeId}")]
        public async Task<ActionResult> GetFoldersByEmployeeId(int employeeId)
        {
            try
            {
                var folders = await _documentService.GetFoldersByEmployeeIdAsync(employeeId);
                return Ok(folders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting employee document folders");
                return StatusCode(500, "An error occurred while retrieving the employee document folders");
            }
        }

        [HttpGet("folder/{folderId}")]
        public async Task<ActionResult> GetFolderById(int folderId)
        {
            try
            {
                var folder = await _documentService.GetFolderByIdAsync(folderId);
                if (folder == null)
                    return NotFound();

                return Ok(folder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting employee document folder");
                return StatusCode(500, "An error occurred while retrieving the employee document folder");
            }
        }

        [HttpPost("folder")]
        public async Task<ActionResult> CreateFolder([FromBody] EmployeeDocumentFolderDTO folderDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _documentService.CreateFolderAsync(folderDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating employee document folder");
                return StatusCode(500, "An error occurred while creating the employee document folder");
            }
        }

        [HttpPut("folder/{id}")]
        public async Task<ActionResult> UpdateFolder(int id, [FromBody] EmployeeDocumentFolderDTO folderDto)
        {
            try
            {
                if (id != folderDto.id)
                    return BadRequest("Folder ID mismatch");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _documentService.UpdateFolderAsync(folderDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee document folder");
                return StatusCode(500, "An error occurred while updating the employee document folder");
            }
        }

        [HttpDelete("folder/{id}")]
        public async Task<ActionResult> DeleteFolder(int id)
        {
            try
            {
                var res = await _documentService.DeleteFolderAsync(id);
                if (!res.Success)
                    return Ok(new ApiRespone_basic { Success = false, Message = res.Message });

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting employee document folder");
                return StatusCode(500, "An error occurred while deleting the employee document folder");
            }
        }

        [HttpGet("documents/{folderId}")]
        public async Task<ActionResult> GetDocumentsByFolderId(int folderId)
        {
            try
            {
                var documents = await _documentService.GetDocumentsByFolderIdAsync(folderId);
                return Ok(documents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting employee documents");
                return StatusCode(500, "An error occurred while retrieving the employee documents");
            }
        }

        [HttpGet("document/{id}")]
        public async Task<ActionResult> GetDocumentById(int id)
        {
            try
            {
                var document = await _documentService.GetDocumentByIdAsync(id);
                if (document == null)
                    return NotFound();

                return Ok(document);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting employee document");
                return StatusCode(500, "An error occurred while retrieving the employee document");
            }
        }

        [HttpPost("document")]
        public async Task<ActionResult> CreateDocument([FromForm] EmployeeDocumentDTO documentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (documentDto.docfile != null)
                {
                    // Handle file upload
                    var filePath = await _fileService.SaveFileAsync(documentDto.docfile, "employee-documents");
                    documentDto.filePath = $"/api/document/download/{Path.GetFileName(filePath)}";

                    // Generate document URL
                    documentDto.documentUrl = filePath;
                }

                var result = await _documentService.CreateDocumentAsync(documentDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating employee document");
                return StatusCode(500, "An error occurred while creating the employee document");
            }
        }

        [HttpPut("document/{id}")]
        public async Task<ActionResult> UpdateDocument(int id, [FromForm] EmployeeDocumentDTO documentDto)
        {
            try
            {
                if (id != documentDto.id)
                    return BadRequest("Document ID mismatch");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (documentDto.docfile != null)
                {
                    // Handle file update
                    var res = await _documentService.GetDocumentByIdAsync(id);
                    var existingDocument = res.Data as EmployeeDocument;
                    if (existingDocument != null && !string.IsNullOrEmpty(existingDocument.filePath))
                    {
                        await _fileService.DeleteFileAsync(existingDocument.filePath);
                    }

                    var filePath = await _fileService.SaveFileAsync(documentDto.docfile, "employee-documents");
                    documentDto.filePath = $"/api/document/download/{Path.GetFileName(filePath)}";
                    documentDto.documentUrl = filePath;
                }

                var result = await _documentService.UpdateDocumentAsync(documentDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee document");
                return StatusCode(500, "An error occurred while updating the employee document");
            }
        }

        [HttpDelete("document/{id}")]
        public async Task<ActionResult> DeleteDocument(int id)
        {
            try
            {
                var res = await _documentService.GetDocumentByIdAsync(id);
                if (res.Data == null)
                    return NotFound();
                var document = res.Data as EmployeeDocument;
                // Delete file from storage if exists
                if (!string.IsNullOrEmpty(document.filePath))
                {
                    await _fileService.DeleteFileAsync(document.filePath);
                }

                var response = await _documentService.DeleteDocumentAsync(id);
                if (!response.Success)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting employee document");
                return StatusCode(500, "An error occurred while deleting the employee document");
            }
        }

        [HttpPost("folder-with-documents")]
        public async Task<ActionResult> CreateFolderWithDocuments([FromForm] EmployeeDocFolderWithDocs model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _documentService.CreateFolderWithDocumentsAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating folder with documents");
                return StatusCode(500, "An error occurred while creating the folder with documents");
            }
        }

        [HttpGet("download/{id}")]
        public async Task<ActionResult> DownloadDocument(int id)
        {
            try
            {
                var res = await _documentService.GetDocumentByIdAsync(id);
                var document = res.Data as EmployeeDocument;
                if (document == null || string.IsNullOrEmpty(document.documentUrl))
                    return NotFound();

                var fileBytes = await _fileService.GetFileAsync(document.documentUrl);
                if (fileBytes == null)
                    return NotFound();

                return File(fileBytes, "application/octet-stream", document.documentName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading document");
                return StatusCode(500, "An error occurred while downloading the document");
            }
        }

        [HttpGet("tags")]
        public async Task<ActionResult> GetAllTags()
        {
            try
            {
                var tags = await _documentService.GetAllTagsAsync();
                return Ok(tags);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting document tags");
                return StatusCode(500, "An error occurred while retrieving document tags");
            }
        }
    }
}
