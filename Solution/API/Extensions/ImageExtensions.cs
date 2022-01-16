using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace API.Extensions
{
    public static class ImageExtensions
    {
        public static string GetImageFormat(this Image img)
        {
            var bmp = Encoding.ASCII.GetBytes("BM");     // BMP
            var gif = Encoding.ASCII.GetBytes("GIF");    // GIF
            var png = new byte[] { 137, 80, 78, 71 };    // PNG
            var tiff = new byte[] { 73, 73, 42 };         // TIFF
            var tiff2 = new byte[] { 77, 77, 42 };         // TIFF
            var jpeg = new byte[] { 255, 216, 255, 224 }; // jpeg
            var jpeg2 = new byte[] { 255, 216, 255, 225 }; // jpeg canon

            using (MemoryStream stream = new MemoryStream(img.Bytes))
            {
                var buffer = new byte[4];
                stream.Read(buffer, 0, buffer.Length);

                if (bmp.SequenceEqual(buffer.Take(bmp.Length)))
                    return "image/bmp";

                if (gif.SequenceEqual(buffer.Take(gif.Length)))
                    return "image/gif";

                if (png.SequenceEqual(buffer.Take(png.Length)))
                    return "image/png";

                if (tiff.SequenceEqual(buffer.Take(tiff.Length)))
                    return "image/tiff";

                if (tiff2.SequenceEqual(buffer.Take(tiff2.Length)))
                    return "image/tiff";

                if (jpeg.SequenceEqual(buffer.Take(jpeg.Length)))
                    return "image/jpeg";

                if (jpeg2.SequenceEqual(buffer.Take(jpeg2.Length)))
                    return "image/jpeg";

                return "image/png";
            }
        }

        public static byte[] GetBytes(this string str)
        {
            var parts = str.Split(";base64,");

            return Convert.FromBase64String(parts[1]);
        }

        public static bool IsImage(this IFormFile file)
        {
            const int ImageMinimumBytes = 512;

            if (file.ContentType.ToLower() != "image/jpg" &&
                file.ContentType.ToLower() != "image/jpeg" &&
                file.ContentType.ToLower() != "image/pjpeg" &&
                file.ContentType.ToLower() != "image/gif" &&
                file.ContentType.ToLower() != "image/x-png" &&
                file.ContentType.ToLower() != "image/png")
            {
                return false;
            }

            if (Path.GetExtension(file.FileName).ToLower() != ".jpg"
                && Path.GetExtension(file.FileName).ToLower() != ".png"
                && Path.GetExtension(file.FileName).ToLower() != ".gif"
                && Path.GetExtension(file.FileName).ToLower() != ".jpeg")
            {
                return false;
            }

            try
            {
                if (!file.OpenReadStream().CanRead)
                {
                    return false;
                }

                if (file.Length < ImageMinimumBytes)
                {
                    return false;
                }

                byte[] buffer = new byte[ImageMinimumBytes];
                file.OpenReadStream().Read(buffer, 0, ImageMinimumBytes);
                string content = System.Text.Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private static byte[] Combine(byte[] first, byte[] second)
        {
            byte[] bytes = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, bytes, 0, first.Length);
            Buffer.BlockCopy(second, 0, bytes, first.Length, second.Length);
            return bytes;
        }

    }
}
