using System;

namespace Image_upload_project.Models
{
    public class ImageUploadModel
    {
        public string Tags { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public string LocalFilePath { get; set; }
        
        public DateTime Timestamp { get; set; }
        
        public long ImageSize { get; set; }
    }
}