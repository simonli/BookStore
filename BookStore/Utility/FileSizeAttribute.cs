using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Utility
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FileSizeAttribute : ValidationAttribute
    {
        private int FileSize { get; set; }

        public FileSizeAttribute(int fileSize= 5 * 1024 * 1024)
        {
            FileSize = fileSize;
        }

        public override bool IsValid(object value)
        {
            if (value is IFormFile file)
            {
                var size = file.Length;
                return size <= FileSize;
            }
            return true;
        }
    }
}
