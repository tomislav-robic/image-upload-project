using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Image_upload_project.Models;
using Image_upload_project.Models.Image;
using Image_upload_project.Repositories.ImageFilters;
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


        public int CreateNewImage(ImageModel image)
        {
            image.Tags = SanitizeHashtags(image.Tags);

            using (var connection = new SqlConnection(_dbSettings.ConnectionString))
            {
                connection.Open();
                return connection.QuerySingle<int>(
                    @"INSERT INTO dbo.Image (UserId, FileName, LocalFilePath, Tags, Description, Timestamp, ImageSize)
                    OUTPUT INSERTED.Id
                    VALUES(@UserId, @FileName, @LocalFilePath, @Tags, @Description, @Timestamp, @ImageSize)",
                    new {image.UserId, image.FileName, image.LocalFilePath, image.Tags, image.Description, image.Timestamp, image.ImageSize});
            }
        }

        private string SanitizeHashtags(string hashtags)
        {
            //sanitize hastags - remove spaces, ensure leading hastag and add a trailing hashtag for filtering
            hashtags = hashtags?.Replace(" ", "");
            if (!string.IsNullOrEmpty(hashtags))
            {
                if (hashtags[0] != '#')
                {
                    hashtags = $"#{hashtags}";
                }

                if (hashtags[hashtags.Length - 1] != '#')
                {
                    hashtags += "#";
                }
            }

            return hashtags;
        }

        public List<ImageViewModel> GetImages(string tags = null, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            IImageFilter filter = null;
            if (!string.IsNullOrEmpty(tags))
            {
                filter = new TagFilterDecorator(filter, tags);
            }

            if (dateFrom != null || dateTo != null)
            {
                filter = new TimestampFilterDecorator(filter, dateFrom, dateTo);
            }

            var query =
                "SELECT Id, FileName, '/userImages/'+UserId+'/'+FileName AS RelativePath, Timestamp FROM dbo.Image ";
            var parameters = new Dictionary<string, object>();
            if (filter != null)
            {
                query = filter.FilterQuery(query);
                filter.AddParameters(parameters);
            }

            query += " ORDER BY FileName";
            using (var connection = new SqlConnection(_dbSettings.ConnectionString))
            {
                connection.Open();
                return connection.Query<ImageViewModel>(query, parameters).ToList();
            }
        }

        public ImageDetails GetImage(int imageId)
        {
            using (var connection = new SqlConnection(_dbSettings.ConnectionString))
            {
                connection.Open();
                return connection.QuerySingle<ImageDetails>(
                    @"SELECT i.Id, i.UserId, i.FileName, i.Tags, i.Description, i.Timestamp, i.ImageSize, u.UserName
                        FROM dbo.Image i
                        JOIN dbo.AspNetUsers u ON i.UserId = u.Id
                        WHERE i.Id = @imageId", new {imageId});
            }
        }

        public void EditImageProperties(int imageId, ImageEditModel model)
        {
            var tags = SanitizeHashtags(model.Tags);
            using (var connection = new SqlConnection(_dbSettings.ConnectionString))
            {
                connection.Open();
                connection.Execute(
                    @"UPDATE dbo.Image 
                        SET Tags = @tags, Description = @Description
                        WHERE Id = @imageId", new {imageId, tags, model.Description});
            }
        }
    }
}