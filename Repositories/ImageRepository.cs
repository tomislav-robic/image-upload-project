using Dapper;
using Image_upload_project.Models;
using Image_upload_project.Models.Image;
using Microsoft.Data.SqlClient;

namespace Image_upload_project.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly DatabaseSettings _dbSettings;

        public ImageRepository(DatabaseSettings dbSettings)
        {
            _dbSettings = dbSettings;
        }


        public void CreateNewImage(ImageModel image)
        {
            using (var connection = new SqlConnection(_dbSettings.ConnectionString))
            {
                connection.Open();
                connection.Execute(
                    @"INSERT INTO dbo.Image (UserId, FileName, LocalFilePath, Tags, Description, Timestamp, ImageSize)
                    VALUES(@UserId, @FileName, @LocalFilePath, @Tags, @Description, @Timestamp, @ImageSize)",
                    new {image.UserId, image.FileName, image.LocalFilePath, image.Tags, image.Description, image.Timestamp, image.ImageSize});
            }
        }
    }
}