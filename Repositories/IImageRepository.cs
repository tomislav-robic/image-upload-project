using Image_upload_project.Models;

namespace Image_upload_project.Repositories
{
    public interface IImageRepository
    {
        void CreateNewImage(ImageUploadModel image, string userId);
    }
}