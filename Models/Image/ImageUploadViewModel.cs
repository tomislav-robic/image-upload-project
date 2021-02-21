namespace Image_upload_project.Models.Image
{
    public class ImageUploadViewModel
    {
        public string Tags { get; set; } = null;
        public string Description { get; set; } = null;

        public float ResizePercentage { get; set; } = 1;

        public bool ClearExifData { get; set; } = false;

    }
}