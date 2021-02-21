using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Image_upload_project.Models.Image
{
    public class ImageSharpImageBuilder : IImageBuilder
    {
        private SixLabors.ImageSharp.Image _image = null;
        private ImageModel _model;
        

        public ImageSharpImageBuilder()
        {
            _model = new ImageModel();
        }
        
        public void SetBaseImageInfo(string fileName, string localFilePath, Stream inputStream)
        {
            _model.FileName = fileName;
            _model.LocalFilePath = localFilePath;
            _image = SixLabors.ImageSharp.Image.Load(inputStream);
        }

        public void AddTags(string tags)
        {
            _model.Tags = tags;
        }

        public void AddDescription(string description)
        {
            _model.Description = description;
        }

        public void AssignUser(string userId)
        {
            _model.UserId = userId;
        }

        public void Resize(float percentage)
        {
            if (percentage > 1)
            {
                throw new ArgumentException("Enlarging the image is not supported.", nameof(percentage));
            }

            if (percentage == 1)
            {
                return;
            }
            _image.Mutate(x =>
            {
                var currentSize = x.GetCurrentSize();
                x.Resize((int)(currentSize.Width*percentage), (int)(currentSize.Height*percentage) );
            });
        }

        public void RemoveExifData()
        {
            foreach (var exifValue in _image.Metadata.ExifProfile.Values)
            {
                _image.Metadata.ExifProfile.RemoveValue(exifValue.Tag);
            }
        }

        public ImageModel Build()
        {
            if (_image == null)
            {
                throw new Exception("Base image info must be set");
            }

            _model.ImageStream = new MemoryStream();
            _image.Save(_model.ImageStream, _image.DetectEncoder(_model.LocalFilePath));
            _model.Timestamp = DateTime.Now;
            _model.ImageSize = _model.ImageStream.Length;

            return _model;
        }
    }
}