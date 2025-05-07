namespace ERP.APIs.Contracts.Model
{
    public class ContractChangeItem
    {
        public string? fieldCode { get; set; }
        public string fieldName { get; set; }
        public string oldValue { get; set; }
        public string newValue { get; set; }
    }
}
