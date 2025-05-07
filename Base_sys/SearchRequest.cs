namespace ERP.Base_sys
{
    public class SearchRequest<T>
    {
        public string? globalSearch { get; set; }
        public Dictionary<string, string>? columnFilters { get; set; }
        public Dictionary<string, DateRange>? dateFilters { get; set; }
        public string sortBy { get; set; } = "Id";
        public string sortOrder { get; set; } = "asc";
        public int page { get; set; } = 1;
        public int pageSize { get; set; } = 50;
    }

    public class DateRange
    {
        public DateTime? from { get; set; }
        public DateTime? to { get; set; }
    }
}
