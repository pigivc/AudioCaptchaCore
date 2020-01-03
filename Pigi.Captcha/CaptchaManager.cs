using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace Pigi.Captcha
{
    public class CaptchaManager
    {
        public async static Task<Bitmap> GenerateCaptchaImage(string key)
        {
            return await Task.Run(() =>
            {
                var cSettings = AppContext.Current.Session.Get<CaptchaSettings>(key);
                var text = CaptchaCode.GenerateCode(cSettings.TextLength, cSettings.TextStyle);
                cSettings.CaptchaText = text;
                AppContext.Current.Session.Set<CaptchaSettings>(key, cSettings);
                return new CaptchaImage().MakeCaptchaImge(text, cSettings.PicHeight - 10, cSettings.PicHeight - 1, cSettings.PicWidth, cSettings.PicHeight, cSettings.CaptchaStyle);
            });
            
        }

        public static async Task<byte[]> GenerateCurrentCaptachAudio(string key)
        {
            var text = AppContext.Current.Session.Get<CaptchaSettings>(key).CaptchaText;
            if (string.IsNullOrEmpty(text))
                return await Task.Run(() => new byte[0]);

            return await text.CreateAudio();
        }

        public static bool ValidateCurrentCaptcha(string captchaId, string userCaptchaText)
        {
            var text = AppContext.Current.Session.Get<CaptchaSettings>(Extensions._captchaPrefix + captchaId).CaptchaText;
            return text == userCaptchaText.ToUpper();
        }
    }
}