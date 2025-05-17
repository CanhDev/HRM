using System.ComponentModel.DataAnnotations;

namespace ERP.DTO.Lists
{
    public class EmployeeDoc_dataset
    {
        public List<EmployeeDocumentDTO> documents { get; set; }
    }
    public class EmployeeDocumentDTO : IActionDto
    {
        public int id { get; set; }
        public int employeeId { get; set; }
        public int folderId { get; set; }

        [Required]
        [StringLength(100)]
        public string documentName { get; set; }

        [StringLength(50)]
        public string? documentType { get; set; }

        [StringLength(250)]
        public string? filePath { get; set; }

        [StringLength(250)]
        public string? notes { get; set; }
        public string? tags { get; set; }
        public DateTime? expiryDate { get; set; }

        public bool? isCompany { get; set; }

        public string? documentUrl { get; set; }
        public int status { get; set; } = 1;
        public IFormFile? docfile { get; set; }
        public string? actionType { get; set; }
    }
}
