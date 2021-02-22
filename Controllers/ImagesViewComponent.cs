using System;
using System.Threading.Tasks;
using Image_upload_project.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Image_upload_project.Controllers
{
    public class ImagesViewComponent : ViewComponent
    {
        private readonly IImageRepository _imageRepository;

        public ImagesViewComponent(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }
        
        public async Task<IViewComponentResult> InvokeAsync([FromQuery] string tags = null, [FromQuery] DateTime? dateFrom = null, [FromQuery] DateTime? dateTo = null)
        {
            var images = _imageRepository.GetImages(tags, dateFrom, dateTo);
            return View(images);
        }
    }
}