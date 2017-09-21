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


        [Required(ErrorMessage = "{0}不能为空, 请从右侧列表确认图书")]
        [Display(Name = "豆瓣链接")]
        public string DoubanUrl { get; set; }

        [Display(Name = "版本简介")]
        [DataType(DataType.MultilineText)]
        public string BookEditionCommnet { get; set; }

    }

    public class UploadExtViewModel
    {
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "图书名称")]
        public string Title { get; set; }
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "作者")]
        public string Author { get; set; }
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "内容简介")]
        public string Introduction { get; set; }
        [Display(Name = "链接")]
        public string Url { get; set; }
        [Display(Name = "译者")]
        public string Translator { get; set; }
        [Display(Name = "出版社")]
        public string Publisher { get; set; }
        [Display(Name = "ISBN")]
        public string Isbn { get; set; }
        [Display(Name = "图书目录")]
        public string BookCatelog { get; set; }
        [Display(Name = "作者简介")]
        public string AuthorIntroduction { get; set; }
        [Display(Name = "标签")]
        public string Tags { get; set; }
        
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "图书文件")]
        [FileExt(".epub,.mobi,.pdf,.txt", ErrorMessage = "仅支持mobi, epub, pdf, txt格式的图书")]
        [FileSize(50*1024*1024,ErrorMessage ="文件大小不能超过50MB")]
        public IFormFile BooKFile { get; set; }
        
        [Display(Name = "图书封面")]
        [FileExt(".jpg,.png,.jpeg,.gif", ErrorMessage = "jpg, png, jpeg, gif格式的图片")]
        [FileSize(5*1024*1024,ErrorMessage ="文件大小不能超过5MB")]
        public IFormFile Logo { get; set; }
        
        [Display(Name = "版本简介")]
        [DataType(DataType.MultilineText)]
        public string BookEditionCommnet { get; set; }
        
        
    }
}
