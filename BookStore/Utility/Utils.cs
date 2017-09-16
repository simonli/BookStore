using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;

namespace BookStore.Utility
{
    public static class Utils
    {
        public static string GeneratePassword(string password)
        {
            string cl = password;
            //string pwd = "";
            MD5 md5 = MD5.Create(); //实例化一个md5对像
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            return Convert.ToBase64String(s);
        }


        public static string GetDigitalRandomNum(int NumCount)
        {
            string allChar = "0,1,2,3,4,5,6,7,8,9";
            string[] allCharArray = allChar.Split(','); //拆分成数组
            string randomNum = "";
            int temp = -1; //记录上次随机数的数值，尽量避免产生几个相同的随机数
            var rand = new Random();
            for (int i = 0; i < NumCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * ((int) DateTime.Now.Ticks));
                }
                int t = rand.Next(9);
                if (temp == t)
                {
                    return GetDigitalRandomNum(NumCount);
                }
                temp = t;
                randomNum += allCharArray[t];
            }
            return "";
        }

        public static string GetCheckSum(string filepath)
        {
            StringBuilder sb = new StringBuilder();
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filepath))
                {
                    byte[] hashBytes = md5.ComputeHash(stream);
                    foreach (byte bt in hashBytes)
                    {
                        sb.Append(bt.ToString("x2"));
                    }
                }
            }
            return sb.ToString();
        }

        public static string GetCheckSum(Stream stream)
        {
            StringBuilder sb = new StringBuilder();
            using (var md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(stream);
                foreach (byte bt in hashBytes)
                {
                    sb.Append(bt.ToString("x2"));
                }
            }
            return sb.ToString();
        }


        public static string GetRandomFileName()
        {
            //20位数字文件名
            return DateTime.Now.ToString("yyMMddHHmmss") + GetDigitalRandomNum(8);
        }

        public static string GetGuid()
        {
            return Guid.NewGuid().ToString().Replace("-", "").ToLower();
        }

        public static string GetEnumDescription(Enum enumValue)
        {
            string str = enumValue.ToString();
            System.Reflection.FieldInfo field = enumValue.GetType().GetField(str);
            object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (objs == null || objs.Length == 0) return str;
            DescriptionAttribute da = (DescriptionAttribute) objs[0];
            return da.Description;
        }

        public static string FormatDateTime(DateTime d)
        {
            return d.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string FormatFileSize(long fileSize)
        {
            if (fileSize < 0)
            {
                return "";
            }
            if (fileSize >= 1024 * 1024 * 1024)
            {
                return $"{(double) fileSize / (1024 * 1024 * 1024):########0.00} GB";
            }
            if (fileSize >= 1024 * 1024)
            {
                return $"{(double) fileSize / (1024 * 1024):####0.00} MB";
            }
            return fileSize >= 1024 ? $"{(double) fileSize / 1024:####0.00} KB" : $"{fileSize} bytes";
        }

        public static string GetFileExt(string filename)
        {
            return !string.IsNullOrEmpty(filename) ? Path.GetExtension(filename).Split('.')[1] : "";
        }
        
    }
}