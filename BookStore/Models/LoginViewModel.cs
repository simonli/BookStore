using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class LoginViewModel
    {
        [HiddenInput(DisplayValue = true)]
        public int Id { get; set; }

        
        [Remote("CheckUsername", "Account", HttpMethod = "Post", ErrorMessage = "{0}不存在或被锁定！")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "{0}长度为{2}-{1}个字符")]
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "用户名")]
        public string Username { get; set; }

        
        [Required(ErrorMessage = "{0}不能为空")]
        [DataType(DataType.Password)]
        [StringLength(1000, ErrorMessage = "{0}长度少于{1}个字符")]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Remote("CheckVerifyCode", "Account", HttpMethod = "Post", ErrorMessage = "{0}不正确!")]
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "验证码")]
        public string VerifyCode { get; set; }
    }
}
