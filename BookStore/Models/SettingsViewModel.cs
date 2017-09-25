using BookStore.Domain.Models;
using BookStore.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class SettingsViewModel
    {
        public User User { get; set; }
    }

    public class SettingsUsernameViewModel : SettingsViewModel
    {
        [Remote("ValidateUsername", "Account", HttpMethod = "Post", ErrorMessage = "{0}已经存在,请换一个", AdditionalFields = "initialUsername")]
        [MaxLength(100)]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "{0}长度为{2}-{1}个字符")]
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "用户名")]
        public string Username { get; set; }
        public string PushmainDomain { get; set; }
    }

    public class SettingsEmailViewModel : SettingsViewModel
    {
        public string CurrentEmail { get; set; }

        [Remote("ValidateEmail", "Account", HttpMethod = "Post", ErrorMessage = "{0}已经存在",
            AdditionalFields = "InitialEmail")]
        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "{0}长度为{2}-{1}个字符")]
        [EmailAddress(ErrorMessage = "请输入正确的{0}地址")]
        [Display(Name = "邮箱")]
        public string Email { get; set; }
    }

    public class SettingsPasswordViewModel : SettingsViewModel
    {
        [Remote("CheckPassword", "Account", HttpMethod = "Post", ErrorMessage = "{0}不正确")]
        [Required(ErrorMessage = "{0}不能为空")]
        [DataType(DataType.Password)]
        [StringLength(1000, ErrorMessage = "{0}长度少于{1}个字符")]
        [Display(Name = "旧密码")]
        public string Password { get; set; }

        [Required(ErrorMessage = "{0}不能为空")]
        [DataType(DataType.Password)]
        [StringLength(1000, ErrorMessage = "{0}长度少于{1}个字符")]
        [Display(Name = "新密码")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "{0}不能为空")]
        [Compare(nameof(NewPassword), ErrorMessage = "两次输入密码不一致")]
        [DataType(DataType.Password)]
        [Display(Name = "重复新密码")]
        public string ConfirmNewPassword { get; set; }
    }

    public class SettingsPushViewModel : SettingsViewModel
    {
        [Remote("ValidatePushEmail", "Account", HttpMethod = "Post", ErrorMessage = "{0}已经存在, 切勿重复添加")]
        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "{0}长度为{2}-{1}个字符")]
        [EmailAddress(ErrorMessage = "请输入正确的{0}地址")]
        [Display(Name = "推送邮箱")]
        public string PushEmail { get; set; }

        public List<PushSetting> PushSettings { get; set; }
    }

    public class SettingsAvatarViewModel : SettingsViewModel
    {
        public string CurrentAvatar { get; set; }

        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "头像")]
        [FileExt(".jpg,.png,.gif,.jpeg", ErrorMessage = "仅支持jpg, jpeg, png, gif 格式的头像")]
        [FileSize(10 * 1024 * 1024, ErrorMessage = "头像大小不能超过10MB")]
        public IFormFile AvatarFile { get; set; }
    }
    
    public class SettingsComponentViewModel
    {
        public User User { get; set; }
        public int TodayDownloadCount { get; set; }
        public int TodayPushCount { get; set; }
    }
}