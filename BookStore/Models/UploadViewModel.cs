using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using BookStore.Utility;

namespace BookStore.Models
{
    public class UploadViewModel
    {
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "图书文件")]
        [FileExt(".epub,.mobi,.pdf,.txt", ErrorMessage = "仅支持mobi, epub, pdf, txt格式的图书")]
        [FileSize(50*1024*1024,ErrorMessage ="文件大小不能超过50MB")]
        public IFormFile BooKFile { get; set; }


        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "豆瓣链接")]
        public string DoubanUrl { get; set; }

        [Display(Name = "版本简介")]
        [DataType(DataType.MultilineText)]
        public string BookEditionCommnet { get; set; }



    }
}
