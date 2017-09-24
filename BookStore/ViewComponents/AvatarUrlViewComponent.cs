using System.Threading.Tasks;
using BookStore.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BookStore.ViewComponents
{
    public class AvatarUrlViewComponent : ViewComponent
    {
        private readonly AppSettings _appSettings;

        public AvatarUrlViewComponent(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public async Task<string> InvokeAsync(User user)
        {
            var avatar = user.Avatar != null
                ? "/" + _appSettings.UploadAvatarDir + "/" + user.Avatar
                : "/images/default_avatar.png";
            return avatar;
        }
    }
}