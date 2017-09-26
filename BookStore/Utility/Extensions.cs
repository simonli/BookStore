using BookStore.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStore.Utility
{
    public static class Extensions
    {
        public static ITempDataDictionary Flash(this ITempDataDictionary tempData, string key, object value)
        {
            tempData.Clear();
            tempData.Add(key, value);
            return tempData;
        }

        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public static string GetUserIp(this HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }

        public static string ActivePage(this IHtmlHelper helper, string action, string controller)
        {
            var classValue = "btn-default";
            if (helper.ViewContext.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                var currentController = controllerActionDescriptor.ControllerName;
                var currentAction = controllerActionDescriptor.ActionName;

                if (currentController == controller && currentAction == action)
                {
                    classValue = "btn-success";
                }
            }
            return classValue;
        }

        public static bool IsActivePage(this IHtmlHelper helper, string action, string controller)
        {
            bool isActive = false;
            if (helper.ViewContext.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                string currentController = controllerActionDescriptor.ControllerName;
                string currentAction = controllerActionDescriptor.ActionName;

                if (currentController == controller && currentAction == action)
                {
                    isActive = true;
                }
            }
            return isActive;
        }

        public static string NoHtml(this string htmlstring)
        {
            string result;
            if (string.IsNullOrEmpty(htmlstring))
            {
                result = string.Empty;
            }
            else
            {
                htmlstring = Regex.Replace(htmlstring, "<script[^>]*?>.*?</script>", " ", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "<iframe[\\s]*[^>]*?>.*?</iframe[\\s]*>", " ", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "<frameset[\\s]*[^>]*?>.*?</frameset[\\s]*>", " ", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "<(.[^>]*)>", " ", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "-->", " ", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "<!--.*", " ", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "&(quot|#34);", "\"", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "&(amp|#38);", "&", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "&(lt|#60);", "<", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "&(gt|#62);", ">", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "&(iexcl|#161);", "¡", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "&(cent|#162);", "¢", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "&(pound|#163);", "£", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "&(copy|#169);", "©", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "&#(\\d+);", " ", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "(\\s+)", " ", RegexOptions.IgnoreCase);
                htmlstring = htmlstring.Replace("<", " ");
                htmlstring = htmlstring.Replace(">", " ");
                htmlstring = htmlstring.Replace("(", "");
                htmlstring = htmlstring.Replace(")", "");
                htmlstring = htmlstring.Replace("）", "");
                htmlstring = htmlstring.Replace("（", "");
                htmlstring = htmlstring.Replace(" ", "");
                result = htmlstring;
            }
            return result;
        }
    }
}