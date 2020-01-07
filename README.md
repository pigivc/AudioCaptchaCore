# AudioCaptchaCore
Audio Captcha for AspNet Core MVC


Enable Audio feature only if you are gonna host your asp.net core app in windows platform. or disable audio or implement your own tts audio relative to your selected hosting platform!

Sample project included! 
>>>>>>>>and be sure to copy 'ttsExec.exe' and 'libmp3lame.64.dll' 'libmp3lame.32.dll' in bin directory of main project<<<<<<<<<
<h1>Instructions</h1>

1. Add a <pre>Pigi.Captcha.dll</pre> reference to your project.

2. In startup.cs file or your asp.net core project add <pre>@using Pigi.Captcha</pre> at the top.

3. Add <pre>services.AddPigiCaptcha();</pre> to the <pre>ConfigureServices</pre> method.

4. Add <pre>app.ConfigPigiCaptcha();</pre> to the <pre>Configure</pre> method.

5. When hosting make sure <pre>Load User Profile</pre> is set to <pre>true</pre> in your IIS application pool > advanced settings (this is not required if you disable audio feature)

6. Sample use in a view.cshtm:

<pre>@Html.Captcha(new CaptchaSettings { Id = "c1", TextLength = 5 })</pre>

7. You can configure other settings.

<h1>Live Demo</h1>

<a href="http://captcha.pigivc.ir" target="_blank" >Demo</a>
