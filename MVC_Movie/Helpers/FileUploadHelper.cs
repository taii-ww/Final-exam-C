using System;
using System.IO;
using System.Web;

namespace MVC_Movie.Helpers
{
    public static class FileUploadHelper
    {
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
        private const int MaxFileSizeBytes = 2 * 1024 * 1024; // 2MB

        /// <summary>
        /// Kiểm tra file có hợp lệ để upload không (extension + size).
        /// </summary>
        public static bool IsValidImage(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength == 0)
                return false;

            if (file.ContentLength > MaxFileSizeBytes)
                return false;

            var ext = Path.GetExtension(file.FileName)?.ToLower();
            return Array.Exists(AllowedExtensions, e => e == ext);
        }

        /// <summary>
        /// Lưu file vào thư mục chỉ định, trả về relative path để lưu vào DB.
        /// Trả về null nếu file không hợp lệ.
        /// </summary>
        /// <param name="file">File được upload</param>
        /// <param name="serverSavePath">Đường dẫn vật lý trên server (dùng Server.MapPath)</param>
        /// <param name="relativeFolder">Relative URL folder, ví dụ: "/Content/images/movies"</param>
        public static string SaveImage(HttpPostedFileBase file, string serverSavePath, string relativeFolder)
        {
            if (!IsValidImage(file))
                return null;

            Directory.CreateDirectory(serverSavePath);

            var fileName = Path.GetFileName(file.FileName);
            var fullPath = Path.Combine(serverSavePath, fileName);

            file.SaveAs(fullPath);

            return relativeFolder.TrimEnd('/') + "/" + fileName;
        }
    }
}