﻿using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Shop2.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                               new DefaultContractResolver { IgnoreSerializableAttribute = true }; // cấu hình để sử dụng  Serializable loại bỏ những thuộc tính mặc định

            // nhớ cài Microsoft.AspNet.WebApi.Owin
            // giúp lọc cơ chế đăng nhập cho client(cookie authen) và admin(based token)
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType)); // riêng api sẽ sử dụng OAuthDefaults

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
