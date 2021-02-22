using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace Image_upload_project.Authorization
{
    public class AuthorizationUtilities
    {
        private static AuthorizationUtilities _instance = null;

        public static AuthorizationUtilities Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AuthorizationUtilities();
                }

                return _instance;
            }
        }

        private readonly AuthorizationSettings _settings;
        private AuthorizationUtilities()
        {
            _settings = Startup.Configuration.GetSection("AuthorizationSettings").Get<AuthorizationSettings>();
        }

        public static class Roles
        {
            public const string Admin = "Admin";
            public const string Gold = "Gold";
            public const string Pro = "Pro";
            public const string Free = "Free";
        }

        public long UserByteLimit(ClaimsPrincipal user)
        {
            if (UserIsInRole(user, Roles.Admin))
                return long.MaxValue;
            if (UserIsInRole(user,Roles.Gold))
                return _settings.GoldUserStorageLimit;
            if (UserIsInRole(user,Roles.Pro))
                return _settings.ProUserStorageLimit;

            return _settings.FreeUserStorageLimit;
        }

        public string GetUserId(ClaimsPrincipal user)
        {
            return user?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public bool UserIsInRole(ClaimsPrincipal user, string role)
        {
            //return user.IsInRole(role);
            return user.Identity.Name.StartsWith(role.ToLower());
        }
    }
}