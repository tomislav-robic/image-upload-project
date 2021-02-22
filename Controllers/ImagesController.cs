using System.IO;
using System.Security.Claims;
using Image_upload_project.Authorization;
using Image_upload_project.Models;
using Image_upload_project.Models.Image;
using Image_upload_project.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Image_upload_project.Controllers
{
    public class ImagesController : Controller
    {
        private readonly IImageRepository _imageRepository;
        private readonly AuthorizationService _authService;
        private readonly ImageBuilderFactory _imageBuilderFactory;
        private readonly ImageStorageSettings _settings;

        public ImagesController(IImageRepository imageRepository, AuthorizationService authService, ImageBuilderFactory imageBuilderFactory, ImageStorageSettings settings)
        {
            _imageRepository = imageRepository;
            _authService = authService;
            _imageBuilderFactory = imageBuilderFactory;
            _settings = settings;
        }
        
        
        [Authorize]
        public IActionResult ImageUpload()
        {
            return View(new ImageUploadViewModel());
        }
        
        [Authorize]
        [HttpPost]
        public IActionResult ImageUploadPost(ImageUploadViewModel viewModel, IFormFile file)
        {
            var fileName = Path.GetFileName(file.FileName);
            var userId = AuthorizationUtilities.Instance.GetUserId(User);

            var localFolderPath = Path.Combine(_settings.ImageRepositoryPath, userId);
            if (!Directory.Exists(localFolderPath))
            {
                Directory.CreateDirectory(localFolderPath);
            }

            var localImagePath = Path.Combine(localFolderPath, fileName);

            if (System.IO.File.Exists(localImagePath))
            {
                ViewBag.Error = "An image with the same name has already been uploaded.";
                return View("ImageUpload", viewModel);
            }
            
            var imageBuilder = _imageBuilderFactory.CreateImageBuilder();
            imageBuilder.SetBaseImageInfo(fileName, localImagePath, file.OpenReadStream());
            imageBuilder.AssignUser(userId);
            if (!string.IsNullOrEmpty(viewModel.Tags))
            {
                imageBuilder.AddTags(viewModel.Tags);
            }
            if (!string.IsNullOrEmpty(viewModel.Description))
            {
                imageBuilder.AddDescription(viewModel.Description);
            }
            if (viewModel.ResizePercentage < 1)
            {
                imageBuilder.Resize(viewModel.ResizePercentage);
            }

            if (viewModel.ClearExifData)
            {
                imageBuilder.RemoveExifData();
            }

            using (var imageModel = imageBuilder.Build())
            {

                if (_authService.IsImageSizeOverUserLimit(imageModel.ImageSize, User))
                {
                    ViewBag.Error =
                        "There is not enough room left in your personal repository to fit an image of this size.";
                    return View("ImageUpload", viewModel);
                }
                

                imageModel.WriteToLocalFilePath();
                var imageId = _imageRepository.CreateNewImage(imageModel);
                return RedirectToAction("Details",new {id = imageId});
            }

            
        }

        public IActionResult Details([FromRoute] int id)
        {
            var imageDetails = _imageRepository.GetImage(id);
            ViewBag.CanEdit = false;
            
            if (_authService.IsUserSignedIn(User))
            {
                ViewBag.CanEdit = _authService.CanUserEditImage(User, imageDetails.UserId);
            }
            
            return View(imageDetails);
        }

        [Authorize]
        public IActionResult Edit([FromRoute] int id)
        {
            if (!_authService.CanUserEditImage(User, id))
            {
                ViewBag.Error = "Your account is not authorized to edit the requested image.";
                ViewBag.CanEdit = false;
                return View(new ImageEditModel());
            }

            ViewBag.CanEdit = true;
            var imageDetails = _imageRepository.GetImage(id);
            return View(new ImageEditModel()
            {
                Id = imageDetails.Id,
                Description = imageDetails.Description,
                Tags = imageDetails.Tags.Substring(0, imageDetails.Tags.Length-1).Replace("#"," #").Substring(1)
            });
        }
        
        [Authorize]
        [HttpPost]
        public IActionResult EditPost(ImageEditModel model, [FromRoute] int id)
        {
            if (!_authService.CanUserEditImage(User, id))
            {
                ViewBag.Error = "Your account is not authorized to edit the requested image.";
                ViewBag.CanEdit = false;
                return View("Edit", model);
            }

            _imageRepository.EditImageProperties(id, model);
            return RedirectToAction("Details", new {id});
        }
    }
}