namespace Image_upload_project.Repositories
{
    public interface IAuthorizationRepository
    {
        long UserDataConsumption(string userId);
        bool UserIsImageOwner(string userId, int imageId);
    }
}