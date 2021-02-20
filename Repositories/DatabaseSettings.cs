namespace Image_upload_project.Repositories
{
    public class DatabaseSettings
    {
        public DatabaseSettings(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; }
    }
}