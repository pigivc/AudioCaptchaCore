using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Pigi.Captcha
{
    /// <summary>
    /// Summary description for sayit
    /// </summary>
    internal class sayit 
    {

        public async Task ProcessRequestAsync(HttpContext context)
        {
            var key = Extensions._captchaPrefix + context.Request.Query["id"];
            var cSetting = AppContext.Current.Session.Get<CaptchaSettings>(key);
            byte[] bytes = new byte[0];
            if (cSetting.EnableAudio)
                bytes = await CaptchaManager.GenerateCurrentCaptachAudio(key);
            context.Response.Clear();
            context.Response.ContentType = "audio/mp3";
            
            await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);

        }


    }
}