using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pigi.Captcha
{
    public class CaptchaSettings
    {
        public string Id { get; set; }

        public int TextLength { get; set; } = 5;

        public int PicWidth { get; set; } = 130;

        public int PicHeight { get; set; } = 40;

        public TextStyle TextStyle { get; set; } = TextStyle.Alphanumeric;

        public CaptchaStyle CaptchaStyle { get; set; } = CaptchaStyle.TextWithLineAndCircles;

        public bool ShowInput { get; set; } = true;

        public bool EnableAudio { get; set; } = true;

        [JsonProperty]
        internal string CaptchaText { get; set; }
    }
}