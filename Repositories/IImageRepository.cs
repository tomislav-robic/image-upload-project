using System;
using System.Collections.Generic;
using Image_upload_project.Models;
using Image_upload_project.Models.Image;

namespace Image_upload_project.Repositories
{
    public interface IImageRepository
    {
        int CreateNewImage(ImageModel image);
        List<ImageViewModel> GetImages(string tags = null, DateTime? dateFrom = null, DateTime? dateTo = null);

        ImageDetails GetImage(int imageId);
        void EditImageProperties(int imageId, ImageEditModel model);
    }
}