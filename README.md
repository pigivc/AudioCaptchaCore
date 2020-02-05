# AudioCaptchaCore
Audio Captcha for AspNet Core MVC


Enable Audio feature only if you are gonna host your asp.net core app in windows platform. or disable audio or implement your own tts audio relative to your selected hosting platform!

Sample project included! 
>>>>>>>>and be sure to copy 'ttsExec.exe' and 'libmp3lame.64.dll' 'libmp3lame.32.dll' (Included in nuget package) in bin directory of main project<<<<<<<<<
<h1>Instructions</h1>

1. Add a <pre>Pigi.Captcha.dll</pre> reference to your project.
or Install via Nuget Package Manager:
<pre>Install-Package Pigi.Captcha -Version 1.0.7</pre>

2. In startup.cs file or your asp.net core project add <pre>@using Pigi.Captcha</pre> at the top.

3. Add <pre>services.AddPigiCaptcha();</pre> to the <pre>ConfigureServices</pre> method.

4. Add <pre>app.ConfigPigiCaptcha();</pre> to the <pre>Configure</pre> method.

5. When hosting make sure <pre>Load User Profile</pre> is set to <pre>true</pre> in your IIS application pool > advanced settings (this is not required if you disable audio feature)

6. Sample use in a view.cshtm:

<pre>@Html.Captcha(new CaptchaSettings { Id = "c1", TextLength = 5 })</pre>
or

<pre>&lt;captcha for-id="c2" for-textLength="9" for-showInput="false" for-textStyle="TextStyle.Numeric"&gt;&lt;/captcha&gt;</pre>
7. You can configure other settings.

8. Then validate captcha in code like this:

      <pre>
[HttpPost]&nbsp;
        public IActionResult Index(string c1)&nbsp;
        {
            var isCaptcha1Valid = CaptchaManager.ValidateCurrentCaptcha("c1", c1);

            ViewBag.c1 = isCaptcha1Valid;
            return View();
        }
</pre>

<h1>Live Demo</h1>

<a href="http://captcha.pigivc.ir" target="_blank" >Demo</a>
