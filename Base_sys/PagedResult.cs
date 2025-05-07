namespace ERP.Base_sys
{
    public class PagedResult<T>
    {
        public List<T> items { get; set; } = new();
        public int totalCount { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
    }
}
