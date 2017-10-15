using System.IO;
using Microsoft.Extensions.Options;

namespace BookStore.Utility
{
    public class UserUtil
    {
        private readonly AppSettings _appSettings;

        public UserUtil(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }


        public string GetAvatar(string avatar)
        {
            var avatarImage = "/images/default_avatar.png";
            if (!string.IsNullOrEmpty(avatar))
            {
                avatarImage = "/" + _appSettings.UploadAvatarDir + "/" + avatar;
            }
            return avatarImage;
        }
    }
}