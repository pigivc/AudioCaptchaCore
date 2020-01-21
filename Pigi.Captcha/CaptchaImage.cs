using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;

namespace Pigi.Captcha
{
    internal class CaptchaImage
    {
        private Random Rand = new Random();

        // Make a captcha image for the text.
        public Bitmap MakeCaptchaImge(string txt,
            int min_size, int max_size, int wid, int hgt, CaptchaStyle cStyle = CaptchaStyle.TextWithLineAndCircles)
        {
            // Make the bitmap and associated Graphics object.
            Bitmap bm = new Bitmap(wid, hgt);
            using (Graphics gr = Graphics.FromImage(bm))
            {
                gr.SmoothingMode = SmoothingMode.HighQuality;
                gr.Clear(Color.LightYellow);



                var r = new Random();

                if (cStyle == CaptchaStyle.TextWithLineAndCircles)
                    for (int i = 0; i < 10; i++)
                    {
                        var s = r.Next(10, 15);
                        gr.DrawEllipse(new Pen(Color.LightGreen, 2), r.Next(5, wid), r.Next(5, hgt), s, s);
                    }

                // See how much room is available for each character.
                int ch_wid = (int)(wid / txt.Length);

                // Draw each character.
                for (int i = 0; i < txt.Length; i++)
                {
                    float font_size = Rand.Next(min_size, max_size);
                    using (Font the_font = new Font("Times New Roman",
                        font_size, FontStyle.Bold))
                    {
                        DrawCharacter(txt.Substring(i, 1), gr,
                            the_font, i * ch_wid, ch_wid, wid, hgt);
                    }
                }




                if (cStyle == CaptchaStyle.TextWithLineAndCircles || cStyle == CaptchaStyle.TextWithLine)
                {
                    var points = new List<PointF>();

                    for (int i = 0; i < 7; i++)
                    {
                        var x = r.Next(0, wid);
                        var y = r.Next(0, hgt);
                        points.Add(new PointF(x, y));
                    }

                    gr.DrawCurve(new Pen(Color.LightBlue, 2), points.ToArray());
                }
            }

            return bm;

        }

        // Draw a deformed character at this position.
        private int PreviousAngle = 0;
        private void DrawCharacter(string txt, Graphics gr,
            Font the_font, int X, int ch_wid, int wid, int hgt)
        {
            // Center the text.
            using (StringFormat string_format = new StringFormat())
            {
                string_format.Alignment = StringAlignment.Center;
                string_format.LineAlignment = StringAlignment.Center;
                RectangleF rectf = new RectangleF(X, 0, ch_wid, hgt);

                // Convert the text into a path.
                using (GraphicsPath graphics_path = new GraphicsPath())
                {
                    graphics_path.AddString(txt,
                        the_font.FontFamily, (int)FontStyle.Italic,
                        the_font.Size, rectf, string_format);

                    // Make random warping parameters.
                    float x1 = (float)(X + Rand.Next(ch_wid) / 2);
                    float y1 = (float)(Rand.Next(hgt) / 2);
                    float x2 = (float)(X + ch_wid / 2 +
                        Rand.Next(ch_wid) / 2);
                    float y2 = (float)(hgt / 2 + Rand.Next(hgt) / 2);
                    PointF[] pts = {
            new PointF(
                (float)(X + Rand.Next(ch_wid) / 4),
                (float)(Rand.Next(hgt) / 4)),
            new PointF(
                (float)(X + ch_wid - Rand.Next(ch_wid) / 4),
                (float)(Rand.Next(hgt) / 4)),
            new PointF(
                (float)(X + Rand.Next(ch_wid) / 4),
                (float)(hgt - Rand.Next(hgt) / 4)),
            new PointF(
                (float)(X + ch_wid - Rand.Next(ch_wid) / 4),
                (float)(hgt - Rand.Next(hgt) / 4))
        };
                    Matrix mat = new Matrix();
                    graphics_path.Warp(pts, rectf, mat,
                        WarpMode.Perspective, 0);

                    // Rotate a bit randomly.
                    float dx = (float)(X + ch_wid / 2);
                    float dy = (float)(hgt / 2);
                    gr.TranslateTransform(-dx, -dy, MatrixOrder.Append);
                    int angle = PreviousAngle;
                    do
                    {
                        angle = Rand.Next(-30, 30);
                    } while (Math.Abs(angle - PreviousAngle) < 20);
                    PreviousAngle = angle;
                    gr.RotateTransform(angle, MatrixOrder.Append);
                    gr.TranslateTransform(dx, dy, MatrixOrder.Append);

                    // Draw the text.
                    gr.FillPath(Brushes.Blue, graphics_path);
                    gr.ResetTransform();
                }
            }
        }
    }
}