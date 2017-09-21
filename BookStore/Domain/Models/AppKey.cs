using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BookStore.Domain.Models
{
    [Table("app_keys")]
    public class AppKey
    {
        [Key]
        public string Name { get; set; }

        public long MaxId { get; set; }
    }

    public enum AppkeyEnum
    {
        Users,
        Books,
        BookEditions,
        BookEditionComments,
        BookTags,
        Tags,
        ActionLogs,
        UserPointLogs,
        PushSettings
    }

    public class AppkeyUtil
    {
        public static long GetInitValue(string name)
        {
            var initiAppKeys = new Dictionary<string, long>
            {
                {AppkeyEnum.Users.ToString(), 1000},
                {AppkeyEnum.Books.ToString(), 60000000},
                {AppkeyEnum.BookEditions.ToString(), 1000},
                {AppkeyEnum.BookEditionComments.ToString(), 1000},
                {AppkeyEnum.BookTags.ToString(), 1},
                {AppkeyEnum.Tags.ToString(), 100},
                {AppkeyEnum.ActionLogs.ToString(), 1},
                {AppkeyEnum.UserPointLogs.ToString(), 1},
                {AppkeyEnum.PushSettings.ToString(), 1}
            };


            return initiAppKeys.GetValueOrDefault(name, 1);
        }
    }
}