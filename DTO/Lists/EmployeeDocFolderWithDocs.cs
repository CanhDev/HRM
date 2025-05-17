namespace ERP.DTO.Lists
{
    public class EmployeeDocFolderWithDocs
    {
        public required EmployeeDocumentFolderDTO folder;
        public List<EmployeeDocumentDTO>? documents;
    }
}
