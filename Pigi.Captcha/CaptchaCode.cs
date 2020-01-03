using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pigi.Captcha
{
    public enum TextStyle
    {
        Alphanumeric,
        CharachterOnly,
        Numeric
    }

    public enum CaptchaStyle
    {
        TextOnly,
        TextWithLine,
        TextWithLineAndCircles
    }
    internal class CaptchaCode
    {
        static Random r = new Random();

        public static string GenerateCode(int lenght,TextStyle cStyle)
        {
            var charSet1 = "abcdefghigklmnopqrstuvwxyz0123456789";
            var charSet2 = "abcdefghigklmnopqrstuvwxyz";
            var charSet3 = "0123456789";

            var charSet = string.Empty;
            switch (cStyle)
            {
                case TextStyle.Alphanumeric:
                    charSet = charSet1.ToUpper();
                    break;
                case TextStyle.CharachterOnly:
                    charSet = charSet2.ToUpper();
                    break;
                case TextStyle.Numeric:
                    charSet = charSet3.ToUpper();
                    break;
                default:
                    break;
            }

            var outStr = string.Empty;

            for (int i = 0; i < lenght; i++)
            {
                outStr += charSet.Substring(r.Next(0, charSet.Length), 1);
            }
            return outStr;
        }

    }
}