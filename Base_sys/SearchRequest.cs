namespace ERP.Base_sys
{
    public class SearchRequest<T>
    {
        public string? GlobalSearch { get; set; }
        public Dictionary<string, string>? ColumnFilters { get; set; }
        public Dictionary<string, DateRange>? DateFilters { get; set; }
        public string SortBy { get; set; } = "Id";
        public string SortOrder { get; set; } = "asc";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }
    public class DateRange
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
