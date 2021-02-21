using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Image_upload_project.Models.Image
{
    public class ImageModel : IDisposable
    {
        public string UserId { get; internal set; }
        
        public string Tags { get; internal set; }
        public string Description { get; internal set; }
        
        public string FileName { get; internal set; }
        public string LocalFilePath { get; internal set; }
        
        public DateTime Timestamp { get; internal set; }

        public long ImageSize { get; internal set; }

        public Stream ImageStream { get; internal set; }

        public void WriteToLocalFilePath()
        {
            using (var fileStream = new FileStream(LocalFilePath, FileMode.CreateNew))
            {
                ImageStream.CopyTo(fileStream);
            }
        }

        public void Dispose()
        {
            ImageStream?.Dispose();
        }
    }


}