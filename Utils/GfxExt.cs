using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace TalkingPete
{
    // Класс расширений для отрисовки скругленных прямоугольников
    static class GfxExt
    {
        private static GraphicsPath RR(float x, float y, float w, float h, float r)
        {
            r = Math.Min(r, Math.Min(w / 2f, h / 2f));
            var p = new GraphicsPath();
            p.AddArc(x, y, r * 2, r * 2, 180, 90);
            p.AddArc(x + w - r * 2, y, r * 2, r * 2, 270, 90);
            p.AddArc(x + w - r * 2, y + h - r * 2, r * 2, r * 2, 0, 90);
            p.AddArc(x, y + h - r * 2, r * 2, r * 2, 90, 90);
            p.CloseFigure();
            return p;
        }

        public static void FillRR(this Graphics g, Brush b, float x, float y, float w, float h, float r)
        { 
            using var p = RR(x, y, w, h, r); 
            g.FillPath(b, p); 
        }

        public static void DrawRR(this Graphics g, Pen pen, float x, float y, float w, float h, float r)
        { 
            using var p = RR(x, y, w, h, r); 
            g.DrawPath(pen, p); 
        }

        public static void DrawRR(this Graphics g, Pen pen, Rectangle rc, float r)
            => DrawRR(g, pen, rc.X, rc.Y, rc.Width, rc.Height, r);
    }
}