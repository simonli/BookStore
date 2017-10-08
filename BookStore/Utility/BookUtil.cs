using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace BookStore.Utility
{
    public class BookUtil
    {
        private readonly AppSettings _appSettings;

        public BookUtil(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public string GetBookLogo(string logo)
        {
            var bookLogo = logo;

            if (logo.IndexOf("http", StringComparison.Ordinal) < 0 ||
                logo.IndexOf("https", StringComparison.Ordinal) < 0)
            {
                bookLogo = "/" + _appSettings.UploadAvatarDir + "/" + logo;
            }
            else
            {

                bookLogo = logo.Replace("/lpic/", "/mpic/");
            }
            return bookLogo;
        }
    }
}