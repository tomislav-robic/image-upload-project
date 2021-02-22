using System;

namespace Image_upload_project.Models.Image
{
    public class ImageSearchModel
    {
        public string Tags { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}