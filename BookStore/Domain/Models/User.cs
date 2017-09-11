using BookStore.Domain.Enum;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Domain.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "姓名")]
        public string Username { get; set; }

        [StringLength(2000)]
        public string Password { get; set; }

        [StringLength(500)]
        public string Email { get; set; }

        [StringLength(2000)]
        public string Avatar { get; set; }

        [StringLength(100)]
        public string LoginIp { get; set; }
        public DateTime LoginTime { get; set; }
        public int LoginCount { get; set; }
        public DateTime CreateTime { get; set; }
        public int IsDelete { get; set; }

        /// <summary>
        /// 总积分
        /// </summary>
        public int PointCount { get; set; }

        public UserTypeEnum UserType { get; set; }
    }

    
}
