using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using NAudio.Lame;
using NAudio.Wave;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pigi.Captcha
{
    public static class Extensions
    {
        internal static string _captchaPrefix = "captchaPrefix";

        internal static async Task<byte[]> CreateAudio(this string textToSpeech, bool doCompress = true)
        {
            //var logger = (ILogger<sayit>)AppContext.Current.RequestServices.GetService(typeof(ILogger<sayit>));
            var data = string.Empty;
            
            try
            {
                var rootDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.
GetExecutingAssembly().CodeBase).Remove(0, @"file:\".Length);
                await Task.Run(() =>
                {
                    var process = new System.Diagnostics.Process();

                    process.StartInfo = new System.Diagnostics.ProcessStartInfo()
                    {
                        FileName = rootDir + @"\ttsExec.exe",  // CHUPA MICROSOFT 02-10-2019 23:45                    
                        LoadUserProfile = false,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        Arguments = textToSpeech,
                        RedirectStandardOutput = true,
                        WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
                    };
                    process.Start();
                    while (!process.StandardOutput.EndOfStream)
                    {
                        data = process.StandardOutput.ReadLine();
                    }

                    process.WaitForExit();
                });
            }
            catch (Exception exc)
            {
                //logger.LogError(exc.Message, exc);
                throw;
            }

            var bytes = Convert.FromBase64String(data);

            if (doCompress)
                return ConvertToMp3(bytes);
            return bytes;
        }

        private static byte[] ConvertToMp3(byte[] wav)
        {
            using (var rdr = new WaveFileReader(new MemoryStream(wav)))
            using(var outMs = new MemoryStream())
            using (var wtr = new LameMP3FileWriter(outMs, rdr.WaveFormat, 24))
            {
                rdr.CopyTo(wtr);
                return outMs.ToArray();
            }
        }

        private static Task<byte[]> CompressWav(this byte[] wav)
        {
            var task = Task.Run(() =>
            {
                try
                {
                    using (var ms = new MemoryStream(wav))
                    {
                        using (var reader = new WaveFileReader(ms))
                        {
                            var newFormat = new WaveFormat(8000, 16, 1);

                            using (var conversionStream = new WaveFormatConversionStream(newFormat, reader))
                            {
                                using (var outms = new MemoryStream())
                                {
                                    using (var writer = new LameMP3FileWriter(outms, conversionStream.WaveFormat, 24, null))
                                    {
                                        conversionStream.CopyTo(writer);
                                        
                                        return outms.ToArray();
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }

            });
            return task;
        }

        public static HtmlString Captcha(this IHtmlHelper htmlHelper,CaptchaSettings settings)
        {
            var cs = htmlHelper.ViewData["captchaScript"];
            if (cs == null)
                htmlHelper.ViewData.Add("captchaScript", true);
            if(cs == null)
            {
                RegisterBinPath();
            }

            AppContext.Current.Session.Set(_captchaPrefix + settings.Id, settings);

            var urlHelperFactory = (IUrlHelperFactory)htmlHelper.ViewContext.HttpContext.RequestServices.GetService(typeof(IUrlHelperFactory));
            var urlHelper = urlHelperFactory.GetUrlHelper(htmlHelper.ViewContext);

            StringBuilder sb = new StringBuilder();
            if(cs == null)
            sb.Append(@"<style>.lol { text-decoration: none!important;cursor: pointer;} " +
                "#capTbl tbody tr td a {color: black;}</style>");
            if(settings.ShowInput)
            sb.Append("<input id=\""+settings.Id+"\"  name=\""+settings.Id+"\" type=\"text\" class=\"form-control\" style=\"float:left;width:"+settings.PicWidth+"px\"/>");
            sb.Append("<div  style=\"display: flex; float:left; align-items:center\">");
            sb.Append("<img id=\"img"+settings.Id+ "\" src='"+urlHelper.Content("~/captcha.ashx?id=" + settings.Id+"")+"' />");
            sb.Append("<table id=\"capTbl\" style=\"color: black; margin-left:5px\">");
            if (settings.EnableAudio)
                sb.Append("<tr><td><a class=\"lol glyphicon glyphicon-volume-up\" title=\"Speak!\" style=\"background-image:url('"+ urlHelper.Content("~/static.ashx?id=audio") + "');display:block;height:16px;width:16px\" "+
                    " onclick=\"play('" + settings.Id+"')\"></a></td></tr>");
            
            sb.Append("<tr><td><a class=\"lol glyphicon glyphicon-refresh\" title=\"Refresh\" style=\"background-image:url('" + urlHelper.Content("~/static.ashx?id=refresh") + "');display:block;height:16px;width:16px\" " +
                "onclick=\"refresh('" + settings.Id+"')\"></a></td></tr>");
            sb.Append("</table></div>");

            if (cs == null)
            {
                sb.Append("<script> var audioDic = {};function refresh(id) {var aud = audioDic[id];if (aud) { aud.pause(); aud.currentTime = 0;}" +
            "audioDic[id] = undefined;" +
        "$(\"#img\"+id).attr('src', '"+urlHelper.Content("~/captcha.ashx")+"?id='+id+'&'+Math.random()) }");

                sb.Append("var play = function(id) {var aud = audioDic[id];if (aud == undefined)" +
                       "aud = new Audio('"+ urlHelper.Content("~/sayit.ashx") + "'+'?id=' + id);" +
                   "aud.pause();aud.currentTime = 0; aud.play(); audioDic[id] = aud;}");
                sb.Append("</script>");
            }
            return new HtmlString(sb.ToString());
        }

        static void RegisterBinPath()
        {
            var binPath = Path.Combine(new string[] { AppDomain.CurrentDomain.BaseDirectory, "" });
            // get current search path from environment
            var path = Environment.GetEnvironmentVariable("PATH") ?? "";

            // add 'bin' folder to search path if not already present
            if (!path.Split(Path.PathSeparator).Contains(binPath, StringComparer.CurrentCultureIgnoreCase))
            {
                path = string.Join(Path.PathSeparator.ToString(), new string[] { path, binPath });
                Environment.SetEnvironmentVariable("PATH", path);
            }
        }
    }
}
