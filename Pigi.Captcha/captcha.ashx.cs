using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System.Web;


namespace Pigi.Captcha
{
    /// <summary>
    /// Summary description for captcha
    /// </summary>
    internal class captcha
    {

        public async Task ProcessRequestAsync(HttpContext context)
        {
            var key = Extensions._captchaPrefix + context.Request.Query["id"];

            Stream originalBody = context.Response.Body;

            try
            {
                using (var image = await CaptchaManager.GenerateCaptchaImage(key))
                using (var memStream = new MemoryStream())
                {
                    image.Save(memStream, System.Drawing.Imaging.ImageFormat.Png);
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}