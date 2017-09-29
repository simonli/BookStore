using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Domain.Models;

namespace BookStore.Utility
{
    public class BookUtil
    {
        public static string GetBookLogo(string logo)
        {
            var bookLogo = logo;
//            if (logo.IndexOf("http", StringComparison.Ordinal) < 0 || logo.IndexOf("https", StringComparison.Ordinal) < 0)
//            {
//                bookLogo = Env.WebRootPath + "/" + AppSettings.Value.UploadAvatarDir + "/" + book.Logo;
//            }
            return bookLogo;

        }
    }
}
