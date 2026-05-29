using System;
using System.Linq;
using MVC_Movie.Models;

namespace MVC_Movie.Extensions
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// Phân trang cho bất kỳ IQueryable nào.
        /// pageIndex bắt đầu từ 1.
        /// </summary>
        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, int pageIndex, int pageSize)
        {
            if (pageSize <= 0) pageSize = 10;
            if (pageIndex < 1) pageIndex = 1;

            return query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        /// <summary>
        /// Sắp xếp danh sách Movie theo tên cột truyền vào.
        /// Mặc định sort theo Name nếu sortBy không hợp lệ.
        /// </summary>
        public static IQueryable<Movie> ApplySorting(this IQueryable<Movie> query, string sortBy)
        {
            switch (sortBy?.ToLower())
            {
                case "name":
                    return query.OrderBy(m => m.Name);
                case "name_desc":
                    return query.OrderByDescending(m => m.Name);
                case "releasedate":
                    return query.OrderBy(m => m.ReleaseDate);
                case "releasedate_desc":
                    return query.OrderByDescending(m => m.ReleaseDate);
                case "rentalprice":
                    return query.OrderBy(m => m.RentalPrice);
                case "rentalprice_desc":
                    return query.OrderByDescending(m => m.RentalPrice);
                default:
                    return query.OrderBy(m => m.Name);
            }
        }

        /// <summary>
        /// Sắp xếp danh sách Customer theo tên cột truyền vào.
        /// </summary>
        public static IQueryable<Customer> ApplySorting(this IQueryable<Customer> query, string sortBy)
        {
            switch (sortBy?.ToLower())
            {
                case "name":
                    return query.OrderBy(c => c.Name);
                case "name_desc":
                    return query.OrderByDescending(c => c.Name);
                case "birthdate":
                    return query.OrderBy(c => c.Birthdate);
                case "birthdate_desc":
                    return query.OrderByDescending(c => c.Birthdate);
                default:
                    return query.OrderBy(c => c.Name);
            }
        }
    }
}