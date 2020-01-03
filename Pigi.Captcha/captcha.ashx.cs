using Microsoft.AspNetCore.Http;
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
            var bmCaptcha = await CaptchaManager.GenerateCaptchaImage(key);
            bmCaptcha.Save(context.Response.Body, System.Drawing.Imaging.ImageFormat.Png);
            context.Response.ContentType = "image/png";
            //context.Response.Write("Hello World");
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