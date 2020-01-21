using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pigi.Captcha
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomHanlderMiddleware
                                      (this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestHandler>();
            //return builder.UseMvcWithDefaultRoute();
        }

    }
}
