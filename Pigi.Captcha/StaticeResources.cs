using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Pigi.Captcha
{
    internal class StaticeResources
    {
        public async Task ProcessRequestAsync(HttpContext context)
        {
            var key = context.Request.Query["id"];
            byte[] imgBytes = null;

            await Task.Run(() =>
            {
                if (key == "refresh")
                {
                    imgBytes = ImgResources.GetRefreshImg;
                }
                else if (key == "audio")
                {
                    imgBytes = ImgResources.GetAudioImg;
                }


            });

            Stream originalBody = context.Response.Body;

            try
            {
                using (var memStream = new MemoryStream(imgBytes))
                {
                    context.Response.Body = memStream;
                    memStream.Position = 0;
                    await memStream.CopyToAsync(originalBody);
                }
            }
            finally
            {
                context.Response.Body = originalBody;
            }
        }
    }
}
