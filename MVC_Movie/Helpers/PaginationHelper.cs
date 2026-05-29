namespace MVC_Movie.Helpers
{
    public static class PaginationHelper
    {
        /// <summary>
        /// Chuẩn hóa pageIndex về giá trị hợp lệ (>= 1).
        /// </summary>
        public static int NormalizePage(int? pageIndex)
        {
            return (!pageIndex.HasValue || pageIndex < 1) ? 1 : pageIndex.Value;
        }

        /// <summary>
        /// Tính số trang cần thiết dựa trên tổng số item và page size.
        /// </summary>
        public static int TotalPages(int totalItems, int pageSize)
        {
            if (pageSize <= 0) return 1;
            return (totalItems + pageSize - 1) / pageSize;
        }

        /// <summary>
        /// Tính số item cần skip khi query LINQ/EF.
        /// </summary>
        public static int SkipCount(int pageIndex, int pageSize)
        {
            var page = pageIndex < 1 ? 1 : pageIndex;
            return (page - 1) * pageSize;
        }
    }
}