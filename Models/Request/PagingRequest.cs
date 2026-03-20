namespace ManageCars.Models.Request
{
    public class PagingRequest
    {
        public int _pageNumber { get; set; }
        public int _pageSize { get; set; }


        // FILTER

        public int? categoryId { get; set; }

        public string? SortBy { get; set; }
        public string? Status { get; set; }
        public string? SortDate { get; set; }

    }
}
