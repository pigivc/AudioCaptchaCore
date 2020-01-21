﻿using Microsoft.AspNetCore.Http;
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

            var rootDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.
GetExecutingAssembly().CodeBase).Remove(0,@"file:\".Length);
            Image bm = null;
            await Task.Run(() =>
            {
                if (key == "refresh") {
                    bm = new Bitmap(rootDir + @"\refresh.png");
                }
                else if (key == "audio")
                bm = new Bitmap(rootDir + @"\audio.png");


            });

            //bm.Save(context.Response.Body, System.Drawing.Imaging.ImageFormat.Png);
            //context.Response.ContentType = "image/png";
            //context.Response.Write("Hello World");

            //using (MemoryStream ms = new MemoryStream())
            //{
            //    bm.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            //    context.Response.Body = ms;

            //    var bytes = ms.ToArray();
            //    await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
            //    //context.Response.Body.Seek(0, SeekOrigin.Begin);
            //    context.Response.ContentType = "image/png";

            //}

            Stream originalBody = context.Response.Body;

            try
            {
                using (var memStream = new MemoryStream())
                {
                    bm.Save(memStream, System.Drawing.Imaging.ImageFormat.Png);
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
