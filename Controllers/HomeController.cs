using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Image_upload_project.Models;
using Image_upload_project.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Image_upload_project.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly ILogger<HomeController> _logger;
        private readonly IImageRepository _imageRepository;

        public HomeController(ILogger<HomeController> logger, IImageRepository imageRepository)
        {
            _logger = logger;
            _imageRepository = imageRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private const string FileRepositoryLocation = "C:/Temp/Images";
        
        [Authorize]
        public IActionResult ImageUpload()
        {
            return View(new ImageUploadModel());
        }
        
        [Authorize]
        [HttpPost]
        public IActionResult ImageUploadPost(ImageUploadModel model, IFormFile file)
        {
            var fileName = Path.GetFileName(file.FileName);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var localFolderPath = Path.Combine(FileRepositoryLocation, userId);
            if (!Directory.Exists(localFolderPath))
            {
                Directory.CreateDirectory(localFolderPath);
            }

            var localImagePath = Path.Combine(localFolderPath, fileName);

            if (System.IO.File.Exists(localImagePath))
            {
                ViewBag.Error = "An image with the same name has already been uploaded.";
                return View("ImageUpload", model);
            }

            using (var fileStream = new FileStream(localImagePath, FileMode.CreateNew))
            {
                file.CopyTo(fileStream);
            }

            model.FileName = fileName;
            model.LocalFilePath = localImagePath;
            model.Timestamp = DateTime.Now;
            model.ImageSize = file.Length;

            _imageRepository.CreateNewImage(model, userId);
            
            return RedirectToAction("Index");
        }
    }
}
