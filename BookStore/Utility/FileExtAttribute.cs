using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Utility
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FileExtAttribute: ValidationAttribute
    {
        private List<string> AllowedExtensions { get; set; }

        public FileExtAttribute(string fileExtensions)
        {
            AllowedExtensions = fileExtensions.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public override bool IsValid(object value)
        {
            if (value is IFormFile file)
            {
                var ext = Path.GetExtension(file.FileName);
                return AllowedExtensions.Any(y => string.Equals(ext, y, StringComparison.CurrentCultureIgnoreCase));
            }

            return true;
        }
    }
}
