using System;
using System.Web;
using MVC_Movie.Helpers;
using MVC_Movie.Models;

namespace MVC_Movie.Services
{
    public class MovieService
    {
        private readonly AppDbContext _db;

        public MovieService(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Thêm movie mới vào DB, kèm xử lý upload ảnh nếu có.
        /// </summary>
        public void AddMovie(Movie movie, HttpPostedFileBase imageFile, string serverImageFolder)
        {
            movie.DateAdded = DateTime.Now;
            movie.NumberAvailable = (byte)movie.NumberInStock;

            var imagePath = FileUploadHelper.SaveImage(
                imageFile,
                serverImageFolder,
                "/Content/images/movies"
            );

            if (imagePath != null)
                movie.ImagePath = imagePath;

            _db.Movies.Add(movie);
            _db.SaveChanges();
        }

        /// <summary>
        /// Cập nhật movie đã có trong DB, kèm xử lý upload ảnh nếu có.
        /// </summary>
        public bool UpdateMovie(Movie movie, HttpPostedFileBase imageFile, string serverImageFolder)
        {
            var movieInDb = _db.Movies.Find(movie.Id);
            if (movieInDb == null)
                return false;

            movieInDb.Name = movie.Name;
            movieInDb.GenreId = movie.GenreId;
            movieInDb.NumberInStock = movie.NumberInStock;
            movieInDb.ReleaseDate = movie.ReleaseDate;
            movieInDb.Description = movie.Description;
            movieInDb.RentalPrice = movie.RentalPrice;

            var imagePath = FileUploadHelper.SaveImage(
                imageFile,
                serverImageFolder,
                "/Content/images/movies"
            );

            if (imagePath != null)
                movieInDb.ImagePath = imagePath;

            _db.SaveChanges();
            return true;
        }
    }
}