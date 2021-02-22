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


        public void CreateNewImage(ImageModel image)
        {
            //sanitize hastags - remove spaces and add a trailing hashtag for filtering
            image.Tags = image.Tags.Replace(" ", "");
            if (image.Tags[image.Tags.Length - 1] != '#')
                image.Tags += "#";
            
            using (var connection = new SqlConnection(_dbSettings.ConnectionString))
            {
                connection.Open();
                connection.Execute(
                    @"INSERT INTO dbo.Image (UserId, FileName, LocalFilePath, Tags, Description, Timestamp, ImageSize)
                    VALUES(@UserId, @FileName, @LocalFilePath, @Tags, @Description, @Timestamp, @ImageSize)",
                    new {image.UserId, image.FileName, image.LocalFilePath, image.Tags, image.Description, image.Timestamp, image.ImageSize});
            }
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
    }
}