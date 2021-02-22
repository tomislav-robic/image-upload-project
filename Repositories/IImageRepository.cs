using System;
using System.Collections.Generic;
using Image_upload_project.Models;
using Image_upload_project.Models.Image;

namespace Image_upload_project.Repositories
{
    public interface IImageRepository
    {
        void CreateNewImage(ImageModel image);
        List<ImageViewModel> GetImages(string tags = null, DateTime? dateFrom = null, DateTime? dateTo = null);

        ImageDetails GetImage(int imageId);
    }
}