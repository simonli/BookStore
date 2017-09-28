using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.ComponentModel;

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


        

        public static string GetCheckSum(string filepath)
        {
            var sb = new StringBuilder();
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
            var sb = new StringBuilder();
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

        /// <summary>
        /// 根据GUID获取19位的唯一数字序列
        /// </summary>
        public static long GetId()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }

        /// <summary>
        /// 根据文件名（不带扩展名）从Path环境变量中获取文件路径
        /// </summary>
        /// <param name="filenameWithoutExt"></param>
        /// <returns></returns>
        public static string GetFilePathFromPathEnvironmentVariable(string filenameWithoutExt)
        {
            var pathEnvs = Environment.GetEnvironmentVariable("path");
            var os = Environment.GetEnvironmentVariable("os");
            var filename = os.IndexOf("windows", StringComparison.OrdinalIgnoreCase) < 0
                ? filenameWithoutExt
                : $"{filenameWithoutExt}.exe";
            if (!string.IsNullOrEmpty(pathEnvs))
            {
                foreach (var pathEnv in pathEnvs.Split(Path.PathSeparator))
                {
                    string filepath = Path.Combine(pathEnv, filename);
                    if (File.Exists(filepath))
                    {
                        return filepath;
                    }
                }
            }
            return "";
        }


    }
}