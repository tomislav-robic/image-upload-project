﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Image_upload_project.Models;
using Image_upload_project.Models.Image;
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
            var model = new ImageSearchModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Search(ImageSearchModel model)
        {
            return View("Index", model);
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

    }
}
