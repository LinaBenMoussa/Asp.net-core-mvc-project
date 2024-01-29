namespace TripExpress.Models
{
    public class Pager
    {
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }

       
        public Pager(int totalItems, int page, int pageSize = 10)
        {
            TotalItems = totalItems;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
            CurrentPage = Math.Max(1, Math.Min(page, TotalPages));

            StartPage = Math.Max(1, CurrentPage - 5);
            EndPage = Math.Min(TotalPages, StartPage + 9);

            if (EndPage - StartPage < 9)
            {
                StartPage = Math.Max(1, EndPage - 9);
            }
        }
    }
}
