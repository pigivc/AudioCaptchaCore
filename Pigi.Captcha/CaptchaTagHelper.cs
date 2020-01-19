using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Pigi.Captcha
{
    [HtmlTargetElement("captcha")]
    public class CaptchaTagHelper : TagHelper
    {

        [HtmlAttributeName("for-id")]
        public string Id { get; set; }

        [HtmlAttributeName("for-textLength")]
        public byte? TextLength { get; set; }

        [HtmlAttributeName("for-picWidth")]
        public short? PicWidth { get; set; }

        [HtmlAttributeName("for-picHeight")]
        public short? PicHeight { get; set; }

        [HtmlAttributeName("for-textStyle")]
        public TextStyle? TextStyle { get; set; }

        [HtmlAttributeName("for-captchaStyle")]
        public CaptchaStyle? CaptchaStyle { get; set; }

        [HtmlAttributeName("for-showInput")]
        public bool? ShowInput { get; set; }

        [HtmlAttributeName("for-enableAudio")]
        public bool? EnableAudio { get; set; }

        public readonly IActionContextAccessor _accessor;

        public CaptchaTagHelper(IActionContextAccessor accessor)
        {
            _accessor = accessor;
        }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            output.TagName = "Captcha";
            output.TagMode = TagMode.StartTagAndEndTag;

            var cs = context.Items.Any(p => (string)p.Key == "captchaScript");
            if (!cs)
            {
                context.Items.Add("captchaScript", true);
                RegisterBinPath();
            }

            var settings = new CaptchaSettings();

            if (!string.IsNullOrEmpty(Id))
            {
                settings.Id = Id;
            }
            else
                throw new Exception("Id is not set");

            if (TextLength.HasValue)
            {
                settings.TextLength = TextLength.Value;
            }

            if (PicWidth.HasValue)
            {
                settings.PicWidth = PicWidth.Value;
            }

            if (PicHeight.HasValue)
            {
                settings.PicHeight = PicHeight.Value;
            }

            if (TextStyle.HasValue)
            {
                settings.TextStyle = TextStyle.Value;
            }

            if (CaptchaStyle.HasValue)
            {
                settings.CaptchaStyle = CaptchaStyle.Value;
            }

            if (ShowInput.HasValue)
            {
                settings.ShowInput = ShowInput.Value;
            }

            if (EnableAudio.HasValue)
            {
                settings.EnableAudio = EnableAudio.Value;
            }

            AppContext.Current.Session.Set(Extensions._captchaPrefix + settings.Id, settings);

            var urlHelperFactory = (IUrlHelperFactory)_accessor.ActionContext.HttpContext.RequestServices.GetService(typeof(IUrlHelperFactory));
            var urlHelper = urlHelperFactory.GetUrlHelper(_accessor.ActionContext);

            StringBuilder sb = new StringBuilder();
            if (!cs)
                sb.Append(@"<style>.lol { text-decoration: none!important;cursor: pointer;} " +
                    "#capTbl tbody tr td a {color: black;}</style>");
            if (settings.ShowInput)
                sb.Append("<input id=\"" + settings.Id + "\"  name=\"" + settings.Id + "\" type=\"text\" class=\"form-control\" style=\"float:left;width:" + settings.PicWidth + "px\"/>");
            sb.Append("<div  style=\"display: flex; float:left; align-items:center\">");
            sb.Append("<img id=\"img" + settings.Id + "\" src='" + urlHelper.Content("~/captcha.ashx?id=" + settings.Id + "") + "' />");
            sb.Append("<table id=\"capTbl\" style=\"color: black; margin-left:5px\">");
            if (settings.EnableAudio)
                sb.Append("<tr><td><a class=\"lol glyphicon glyphicon-volume-up\" title=\"Speak!\" style=\"background-image:url('" + urlHelper.Content("~/static.ashx?id=audio") + "');display:block;height:16px;width:16px\" " +
                    " onclick=\"play('" + settings.Id + "')\"></a></td></tr>");

            sb.Append("<tr><td><a class=\"lol glyphicon glyphicon-refresh\" title=\"Refresh\" style=\"background-image:url('" + urlHelper.Content("~/static.ashx?id=refresh") + "');display:block;height:16px;width:16px\" " +
                "onclick=\"refresh('" + settings.Id + "')\"></a></td></tr>");
            sb.Append("</table></div>");

            if (!cs)
            {
                sb.Append("<script> var audioDic = {};function refresh(id) {var aud = audioDic[id];if (aud) { aud.pause(); aud.currentTime = 0;}" +
            "audioDic[id] = undefined;" +
        "$(\"#img\"+id).attr('src', '" + urlHelper.Content("~/captcha.ashx") + "?id='+id+'&'+Math.random()) }");

                sb.Append("var play = function(id) {var aud = audioDic[id];if (aud == undefined)" +
                       "aud = new Audio('" + urlHelper.Content("~/sayit.ashx") + "'+'?id=' + id);" +
                   "aud.pause();aud.currentTime = 0; aud.play(); audioDic[id] = aud;}");
                sb.Append("</script>");
            }

            output.PreContent.SetHtmlContent(sb.ToString());
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
