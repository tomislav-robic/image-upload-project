using System.Security.Claims;
using Image_upload_project.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Image_upload_project.Authorization
{
    public class AuthorizationService
    {
        private readonly IAuthorizationRepository _repository;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthorizationService(IAuthorizationRepository repository, SignInManager<IdentityUser> signInManager)
        {
            _repository = repository;
            _signInManager = signInManager;
        }

        public bool CanUserEditImage(ClaimsPrincipal user, int imageId)
        {
            if (AuthorizationUtilities.Instance.UserIsInRole(user, AuthorizationUtilities.Roles.Admin))
            {
                return true;
            }

            var userId = AuthorizationUtilities.Instance.GetUserId(user);
            

            if (_repository.UserIsImageOwner(userId, imageId))
            {
                return true;
            }

            return false;
        }

        public bool CanUserEditImage(ClaimsPrincipal user, string imageUserId)
        {
            var userId = AuthorizationUtilities.Instance.GetUserId(user);
            return userId == imageUserId || AuthorizationUtilities.Instance.UserIsInRole(user, AuthorizationUtilities.Roles.Admin);
        }

        public bool IsImageSizeOverUserLimit(long imageSize, ClaimsPrincipal user)
        {
            if (AuthorizationUtilities.Instance.UserIsInRole(user, AuthorizationUtilities.Roles.Admin))
            {
                return false;
            }
            
            var userId = AuthorizationUtilities.Instance.GetUserId(user);

            var totalBytesUsed = _repository.UserDataConsumption(userId);

            if (totalBytesUsed + imageSize > AuthorizationUtilities.Instance.UserByteLimit(user))
            {
                return false;
            }

            return true;
        }

        public bool IsUserSignedIn(ClaimsPrincipal User)
        {
            return _signInManager.IsSignedIn(User);
        }
    }
}