using System.IO;

namespace Image_upload_project.Models.Image
{
    public class ImageSharpImageBuilder : IImageBuilder
    {
        public void SetBaseImageInfo(string filename, string localFilePath, Stream inputStream)
        {
            throw new System.NotImplementedException();
        }

        public void AddTags(string tags)
        {
            throw new System.NotImplementedException();
        }

        public void AddDescription(string description)
        {
            throw new System.NotImplementedException();
        }

        public void AssignUser(string userId)
        {
            throw new System.NotImplementedException();
        }

        public void Resize(float percentage)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveExifData()
        {
            throw new System.NotImplementedException();
        }

        public ImageModel Build()
        {
            throw new System.NotImplementedException();
        }
    }
}