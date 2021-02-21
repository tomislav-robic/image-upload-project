using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Image_upload_project.Models.Image
{
    public class ImageModel
    {
        public string UserId { get; private set; }
        
        public string Tags { get; private set; }
        public string Description { get; private set; }
        
        public string FileName { get; private set; }
        public string LocalFilePath { get; private set; }
        
        public DateTime Timestamp { get; private set; }

        public long ImageSize { get; private set; }

        public Stream ImageStream { get; private set; }

        public void WriteToLocalFilePath()
        {
            using (var fileStream = new FileStream(LocalFilePath, FileMode.CreateNew))
            {
                ImageStream.CopyTo(fileStream);
            }
        }
    }


}