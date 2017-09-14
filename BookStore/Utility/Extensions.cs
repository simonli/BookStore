using BookStore.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
            tempData.Remove(key);
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
            string classValue = "";
            if (helper.ViewContext.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                string currentController = controllerActionDescriptor.ControllerName;
                string currentAction = controllerActionDescriptor.ActionName;

                if (currentController == controller && currentAction == action)
                {
                    classValue = "active";
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
    }
}