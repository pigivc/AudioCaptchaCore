using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pigi.Captcha
{
    public class RequestHandler
    {
        private RequestDelegate _next;

        public RequestHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.Value.EndsWith("sayit.ashx"))
            {
                await new sayit().ProcessRequestAsync(context);
            }
            else if (context.Request.Path.Value.EndsWith("captcha.ashx"))
            {
                await new captcha().ProcessRequestAsync(context);

            }
            else if (context.Request.Path.Value.EndsWith("static.ashx"))
            {
                await new StaticeResources().ProcessRequestAsync(context);

            }
            await context.Response.WriteAsync
            ("<h2>This handler is written for processing files with .report extension<h2>");

            //more logic follows...
        }
    }
}
