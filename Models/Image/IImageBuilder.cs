using System.IO;

namespace Image_upload_project.Models.Image
{
    public interface IImageBuilder
    {
        void SetBaseImageInfo(string filename, string localFilePath, Stream inputStream);

        void AddTags(string tags);

        void AddDescription(string description);

        void AssignUser(string userId);

        void Resize(float percentage);

        void RemoveExifData();

        ImageModel Build();
    }
}