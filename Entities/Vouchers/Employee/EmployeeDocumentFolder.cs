namespace ERP.Entities.Vouchers.Employee
{
    public class EmployeeDocumentFolder: BaseEntity
    {
        public int employeeId { get; set; }
        public string? folderName { get; set; }
        public string? description { get; set; }
        public ICollection<EmployeeDocument>? documents { get; set; }
    }
}
