namespace ContactManagment.DTOs.GlobaResponse
{
    public class PaginatedResponse<T> : ApiResponse<IEnumerable<T>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }

        public PaginatedResponse(IEnumerable<T> data, int page, int sizeOfPage, int totalRecords, string message) 
        {
            Success = true;
            PageNumber = page;
            PageSize = sizeOfPage;
            TotalRecords = totalRecords;
            Data = data;
            TotalPages = (int)Math.Ceiling(totalRecords / (double)PageSize);
        }
    }
}
