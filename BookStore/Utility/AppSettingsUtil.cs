using System;
using System.IO;
using BookStore.Domain.DAL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace BookStore.Utility
{
    public  class AppSettingsUtil
    {
        private static AppSettings _appSettings;

        public AppSettingsUtil(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public static AppSettings GetAppSettings()
        {
            return _appSettings;
        }
    }
}