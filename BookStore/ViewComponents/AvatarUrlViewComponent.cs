﻿using System.Threading.Tasks;
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

        public string Invoke(User user)
        {
            var avatar = "/images/default_avatar.png";
            if (user != null)
            {
                avatar = user.Avatar != null
                    ? "/" + _appSettings.UploadAvatarDir + "/" + user.Avatar
                    : avatar;
            }
            return avatar;
        }
    }
}