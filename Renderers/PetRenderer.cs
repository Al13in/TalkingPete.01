using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace TalkingPete
{
    // Частичный класс MainForm: рендеринг питомца
    public partial class MainForm
    {
        void DrawPet(Graphics g)
        {
            bool sleeping = pet.State == PetState.Sleeping;
            bool happy    = pet.State == PetState.Happy || pet.State == PetState.Eating;
            bool tired    = pet.Energy <= 0 || pet.State == PetState.Sad;

            float cx = 520;
            float cy = (sleeping || tired) ? 368 : 368 + pet.CharBob;

            var savedTransform = g.Transform;
            g.TranslateTransform(cx, cy);

            DrawTail(g, happy, tired, sleeping);
            DrawBody(g);
            DrawHead(g);
            DrawEars(g);
            DrawEyes(g, sleeping, tired, happy);
            DrawNose(g);
            DrawMouth(g, happy, sleeping, tired);
            DrawWhiskers(g);
            DrawPaws(g, happy);
            DrawLegs(g);
            DrawSleepingZs(g, sleeping);
            DrawWashingBubbles(g);

            g.Transform = savedTransform;
        }

        void DrawTail(Graphics g, bool happy, bool tired, bool sleeping)
        {
            using var tPath = new GraphicsPath();
            tPath.AddBezier(-22, 58, -68, 35, -88, -5, -62, -22);

            float tailWag = (happy && !tired && !sleeping) ? (float)(6 * Math.Sin(animFrame * 1.2)) : 0;
            var tMat = new Matrix();
            tMat.RotateAt(tailWag, new PointF(-22, 58));
            tPath.Transform(tMat);

            using var tPen = new Pen(Color.FromArgb(255, 200, 150), 20)
            { EndCap = LineCap.Round, StartCap = LineCap.Round };
            g.DrawPath(tPen, tPath);
        }

        void DrawBody(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.FromArgb(255, 200, 150)), -48, 8, 96, 84);
            g.FillEllipse(new SolidBrush(Color.FromArgb(255, 228, 195)), -28, 22, 56, 58); // живот
        }

        void DrawHead(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.FromArgb(255, 200, 150)), -52, -78, 104, 94);
        }

        void DrawEars(Graphics g)
        {
            var fur  = new SolidBrush(Color.FromArgb(255, 200, 150));
            var pink = new SolidBrush(Color.FromArgb(255, 180, 185));
            g.FillPolygon(fur,  new[] { new PointF(-48, -52), new PointF(-68, -84), new PointF(-26, -72) });
            g.FillPolygon(fur,  new[] { new PointF( 48, -52), new PointF( 68, -84), new PointF( 26, -72) });
            g.FillPolygon(pink, new[] { new PointF(-44, -54), new PointF(-62, -78), new PointF(-32, -70) });
            g.FillPolygon(pink, new[] { new PointF( 44, -54), new PointF( 62, -78), new PointF( 32, -70) });
        }

        void DrawEyes(Graphics g, bool sleeping, bool tired, bool happy)
        {
            if (sleeping)
            {
                using var ep = new Pen(Color.FromArgb(65, 42, 20), 3);
                g.DrawArc(ep, -22, -50, 15, 10, 0, 180);
                g.DrawArc(ep, 7, -50, 15, 10, 0, 180);
            }
            else if (tired)
            {
                using var ep = new Pen(Color.FromArgb(62, 40, 20), 3.5f);
                g.DrawLine(ep, -26, -45, -10, -45);
                g.DrawLine(ep, 14, -45, 30, -45);
                g.FillEllipse(Brushes.LightBlue, -45, -60, 6, 10);
            }
            else
            {
                int px = happy ? 2 : 0;
                var dark = new SolidBrush(Color.FromArgb(62, 40, 20));
                g.FillEllipse(dark, -26 + px, -53, 16, 18);
                g.FillEllipse(dark, 14 + px, -53, 16, 18);
                g.FillEllipse(Brushes.White, -22 + px, -51, 5, 5);
                g.FillEllipse(Brushes.White, 18 + px, -51, 5, 5);

                if (pet.State == PetState.Washing)
                {
                    var water = new SolidBrush(Color.FromArgb(140, 100, 200, 255));
                    g.FillEllipse(water, -40, -62, 18, 18);
                    g.FillEllipse(water, 22, -62, 18, 18);
                }
            }
        }

        void DrawNose(Graphics g)
        {
            g.FillPolygon(new SolidBrush(Color.FromArgb(255, 160, 175)),
                new[] { new PointF(-5, -33), new PointF(5, -33), new PointF(0, -25) });
        }

        void DrawMouth(Graphics g, bool happy, bool sleeping, bool tired)
        {
            using var mp = new Pen(Color.FromArgb(155, 80, 60), 2);
            if (happy)
                g.DrawArc(mp, -15, -30, 30, 15, 0, 180);
            else if (sleeping)
                g.DrawLine(mp, -10, -23, 10, -23);
            else if (tired)
                g.DrawArc(mp, -10, -20, 20, 10, 0, 180);
            else
                g.DrawArc(mp, -10, -25, 20, 10, 0, 180);
        }

        void DrawWhiskers(Graphics g)
        {
            using var wp = new Pen(Color.FromArgb(175, 148, 115), 1f);
            g.DrawLine(wp, -5, -28, -52, -22);
            g.DrawLine(wp, -5, -25, -50, -18);
            g.DrawLine(wp,  5, -28,  52, -22);
            g.DrawLine(wp,  5, -25,  50, -18);
        }

        void DrawPaws(Graphics g, bool happy)
        {
            var fur = new SolidBrush(Color.FromArgb(255, 200, 150));
            if (happy)
            {
                g.FillEllipse(fur, -75, 0, 32, 52);
                g.FillEllipse(fur,  43, 0, 32, 52);
            }
            else
            {
                g.FillEllipse(fur, -68, 18, 30, 48);
                g.FillEllipse(fur,  38, 18, 30, 48);
            }
        }

        void DrawLegs(Graphics g)
        {
            var fur  = new SolidBrush(Color.FromArgb(255, 200, 150));
            var pads = new SolidBrush(Color.FromArgb(225, 165, 110));
            g.FillRR(fur, -38, 76, 32, 38, 8);
            g.FillRR(fur,   6, 76, 32, 38, 8);
            g.FillEllipse(pads, -43, 102, 38, 19);
            g.FillEllipse(pads,   3, 102, 38, 19);
        }

        void DrawSleepingZs(Graphics g, bool sleeping)
        {
            if (!sleeping) return;
            float za = (float)(0.5 + 0.5 * Math.Sin(animFrame * 0.6));
            g.DrawString("z", new Font("Arial", 13, FontStyle.Bold),
                new SolidBrush(Color.FromArgb((int)(za * 210), Color.LightBlue)), 54, -68);
            g.DrawString("Z", new Font("Arial", 18, FontStyle.Bold),
                new SolidBrush(Color.FromArgb((int)(za * 180), Color.LightBlue)), 70, -90);
            g.DrawString("Z", new Font("Arial", 22, FontStyle.Bold),
                new SolidBrush(Color.FromArgb((int)(za * 150), Color.LightBlue)), 90, -118);
        }

        void DrawWashingBubbles(Graphics g)
        {
            if (pet.State != PetState.Washing) return;
            var rb2 = new Random(animFrame * 7);
            for (int bb = 0; bb < 6; bb++)
            {
                float bx = (float)(rb2.NextDouble() * 90 - 45);
                float by = (float)(-45 - rb2.NextDouble() * 50);
                g.FillEllipse(new SolidBrush(Color.FromArgb(75, 130, 200, 255)), bx, by, 16, 16);
                g.DrawEllipse(new Pen(Color.FromArgb(140, 170, 220), 1), bx, by, 16, 16);
            }
        }
    }
}
