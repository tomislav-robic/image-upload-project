using System;

namespace Image_upload_project.Models.Image
{
    public class ImageViewModel
    {
        public string RelativePath { get; set; }
        
        public string FileName { get; set; }
        
        public DateTime Timestamp { get; set; }
        
        public int Id { get; set; }
    }
}