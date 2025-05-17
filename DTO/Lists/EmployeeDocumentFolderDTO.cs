namespace ERP.DTO.Lists
{
    public class EmployeeDocumentFolderDTO 
    {
        public int id { get; set; }
        public int employeeId { get; set; }
        public string? folderName { get; set; }  
        public string? description { get; set; }
        public int status { get; set; } = 1;
    }
}
