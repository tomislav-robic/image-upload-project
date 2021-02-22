using System;

namespace Image_upload_project.Models.Image
{
    public class ImageDetails
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        
        public string UserName { get; set; }
        
        public string Tags { get; set; }
        public string Description { get; set; }
        
        public string FileName { get; set; }

        public DateTime Timestamp { get; set; }

        public long ImageSize { get; set; }

        public string RelativePath => $"/userImages/{UserId}/{FileName}";
    }
}