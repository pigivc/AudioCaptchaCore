using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pigi.Captcha
{
    public static class Initializer
    {
        public static void AddPigiCaptcha(this IServiceCollection services)
        {
            services.AddSession(options =>
            {

                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddHttpContextAccessor();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        }

        public static void ConfigPigiCaptcha(this IApplicationBuilder app)
        {
            app.UseSession();
            Pigi.Captcha.AppContext.Configure(app.ApplicationServices
                      .GetRequiredService<IHttpContextAccessor>());

            app.MapWhen(context => context.Request.Path.ToString().EndsWith("sayit.ashx") ||
            context.Request.Path.ToString().EndsWith("captcha.ashx") ||
            context.Request.Path.ToString().EndsWith("static.ashx")
             ,
                appBuilder =>
                {
                    appBuilder.UseCustomHanlderMiddleware();
                });
        }
    }
}
