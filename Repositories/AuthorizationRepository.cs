using Dapper;
using Microsoft.Data.SqlClient;

namespace Image_upload_project.Repositories
{
    public class AuthorizationRepository : IAuthorizationRepository
    {
        private readonly DatabaseSettings _dbSettings;

        public AuthorizationRepository(DatabaseSettings dbSettings)
        {
            _dbSettings = dbSettings;
        }

        public long UserDataConsumption(string userId)
        {
            using (var connection = new SqlConnection(_dbSettings.ConnectionString))
            {
                connection.Open();
                var totalSizeStored = connection.QuerySingleOrDefault<long?>(
                    "SELECT SUM(ImageSize) FROM dbo.Image WHERE UserId=@userId", new {userId});
                if (totalSizeStored == null)
                    return 0;
                return totalSizeStored.Value;
            }
        }

        public bool UserIsImageOwner(string userId, int imageId)
        {
            using (var connection = new SqlConnection(_dbSettings.ConnectionString))
            {
                connection.Open();
                var count = connection.QuerySingle<int>(
                    "SELECT COUNT(*) FROM dbo.Image WHERE Id = @imageId AND UserId = @userId", new {imageId, userId});
                return count == 1;
            }
        }
    }
}