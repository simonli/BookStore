using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class RegisterViewModel
    {
        
        [Remote("ValidateUsername", "Account", HttpMethod = "Post", ErrorMessage = "{0}已经存在,请换一个", AdditionalFields = "initialUsername")]
        [MaxLength(100)]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "{0}长度为{2}-{1}个字符")]
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "用户名")]
        public string Username { get; set; }

        
        [Required(ErrorMessage = "{0}不能为空")]
        [DataType(DataType.Password)]
        [StringLength(1000, ErrorMessage = "{0}长度少于{1}个字符")]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Required(ErrorMessage = "{0}不能为空")]
        [Compare("Password", ErrorMessage = "两次输入密码不一致")]
        [DataType(DataType.Password)]
        [Display(Name = "重复密码")]
        public string ConfirmPassword { get; set; }

        
        [Remote("ValidateEmail", "Account", HttpMethod = "Post", ErrorMessage = "{0}已经存在", AdditionalFields = "InitialEmail")]
        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "{0}长度为{2}-{1}个字符")]
        [EmailAddress(ErrorMessage = "请输入正确的{0}地址")]
        [Display(Name = "邮箱")]
        public string Email { get; set; }
    }
}
