using Dapper;
using Image_upload_project.Models;
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


        public void CreateNewImage(ImageUploadModel image, string userId)
        {
            using (var connection = new SqlConnection(_dbSettings.ConnectionString))
            {
                connection.Open();
                connection.Execute(
                    @"INSERT INTO dbo.Image (UserId, FileName, LocalFilePath, Tags, Description, Timestamp, ImageSize)
                    VALUES(@userId, @FileName, @LocalFilePath, @Tags, @Description, @Timestamp, @ImageSize)",
                    new {userId, image.FileName, image.LocalFilePath, image.Tags, image.Description, image.Timestamp, image.ImageSize});
            }
        }
    }
}