using Image_upload_project.Models;
using Image_upload_project.Models.Image;

namespace Image_upload_project.Repositories
{
    public interface IImageRepository
    {
        void CreateNewImage(ImageModel image);
    }
}