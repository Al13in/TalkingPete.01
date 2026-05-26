using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace TalkingPete
{
    public partial class MainForm
    {
        // ──────────────────────────────────────────────────────────────
        //  ГЛАВНЫЙ РЕНДЕР
        // ──────────────────────────────────────────────────────────────
        void Render()
        {
            using var g = Graphics.FromImage(backBuf);
            g.SmoothingMode      = SmoothingMode.AntiAlias;
            g.TextRenderingHint  = TextRenderingHint.ClearTypeGridFit;
            g.InterpolationMode  = InterpolationMode.HighQualityBicubic;

            if (curGame == GameState.Normal)
            {
                DrawRoom(g);
                DrawHotSpots(g);
                DrawPet(g);
            }
            else
            {
                DrawMiniGame(g);
            }

            DrawUI(g);
            DrawParticles(g);
            if (actionTimer > 0) DrawActionText(g);
        }

        // ──────────────────────────────────────────────────────────────
        //  HUD / UI
        // ──────────────────────────────────────────────────────────────
        void DrawUI(Graphics g)
        {
            using var panBr = new LinearGradientBrush(
                new Rectangle(0, 0, 900, 95),
                Color.FromArgb(215, 18, 18, 38),
                Color.FromArgb(160, 28, 28, 58),
                90f);
            g.FillRectangle(panBr, 0, 0, 900, 95);
            g.DrawLine(new Pen(Color.FromArgb(60, 255, 255, 255), 1), 0, 95, 900, 95);

            DrawBar(g, 10, 14, 210, 22, pet.Energy / pet.MaxEnergy,
                Color.FromArgb(200, 80, 30), Color.FromArgb(255, 165, 40), "⚡ Энергия");
            DrawBar(g, 10, 50, 210, 22, (float)pet.XP / pet.XpNext,
                Color.FromArgb(40, 80, 220), Color.FromArgb(80, 160, 255), "✨ Опыт");

            // Бейдж уровня
            g.FillRR(new SolidBrush(Color.FromArgb(200, 45, 45, 100)), 228, 8, 100, 72, 12);
            using var lf2 = new Font("Segoe UI", 10, FontStyle.Bold);
            using var lf3 = new Font("Segoe UI", 26, FontStyle.Bold);
            g.DrawString("УРОВЕНЬ", lf2, Brushes.LightGray, 238, 12);
            g.DrawString(pet.Level.ToString(), lf3, Brushes.Gold, 248, 28);

            // Настроение
            string moodIco = pet.Mood > 70 ? "😊" : pet.Mood > 40 ? "😐" : "😢";
            DrawBar(g, 345, 14, 165, 22, pet.Mood / 100f,
                Color.FromArgb(160, 40, 160), Color.FromArgb(220, 100, 220), $"{moodIco} Настроение");

            using var stf = new Font("Segoe UI", 16);
            g.DrawString($"⚡ {(int)pet.Energy}/{(int)pet.MaxEnergy}   XP {pet.XP}/{pet.XpNext}",
                stf, Brushes.Orange, 345, 50);

            // Название комнаты
            string[] rnames = { "Гостиная", "Кухня", "Спальня", "Ванная" };
            using var rnf = new Font("Segoe UI", 18, FontStyle.Bold);
            g.DrawString(rnames[(int)curRoom], rnf, Brushes.White, 535, 18);

            // Название мини-игры
            if (curGame != GameState.Normal)
            {
                string mgn = curGame switch
                {
                    GameState.Cooking => "Кулинария",
                    GameState.Memory  => "Память",
                    _                 => "Поймай уточку!",
                };
                using var mgf = new Font("Segoe UI", 13, FontStyle.Bold);
                g.DrawString(mgn, mgf, Brushes.Yellow, 530, 55);
            }
        }

        void DrawBar(Graphics g, float x, float y, float w, float h,
                     float frac, Color c1, Color c2, string label)
        {
            frac = Math.Max(0, Math.Min(1, frac));
            g.FillRR(new SolidBrush(Color.FromArgb(90, 0, 0, 0)), x, y, w, h, h / 2);
            if (frac > 0.01f)
            {
                using var gr2 = new LinearGradientBrush(
                    new RectangleF(x, y, w * frac + 1, h), c1, c2, 0f);
                g.FillRR(gr2, x, y, w * frac, h, h / 2);
            }
            using var lf4 = new Font("Segoe UI", 10, FontStyle.Bold);
            g.DrawString(label, lf4, Brushes.White, x + 6, y + (h - 12) / 2);
            g.DrawRR(new Pen(Color.FromArgb(90, 255, 255, 255), 1f), x, y, w, h, h / 2);
        }

        // ──────────────────────────────────────────────────────────────
        //  ГОРЯЧИЕ ЗОНЫ
        // ──────────────────────────────────────────────────────────────
        void DrawHotSpots(Graphics g)
        {
            float pulse = (float)(0.5 + 0.5 * Math.Sin(animFrame * 0.5));
            foreach (var hs in hotSpots)
            {
                int ga = (int)(30 + 20 * pulse);
                g.FillRR(new SolidBrush(Color.FromArgb(ga, 255, 240, 80)),
                    hs.Bounds.X - 4, hs.Bounds.Y - 4, hs.Bounds.Width + 8, hs.Bounds.Height + 8, 14);

                g.DrawRR(new Pen(Color.FromArgb(180 + (int)(60 * pulse), 255, 220, 70), 2f), hs.Bounds, 11);

                using var ef = new Font("Segoe UI Emoji", 24);
                float ew = g.MeasureString(hs.Icon, ef).Width;
                g.DrawString(hs.Icon, ef, Brushes.White,
                    hs.Bounds.X + (hs.Bounds.Width - ew) / 2,
                    hs.Bounds.Y + hs.Bounds.Height / 2 - 30);

                using var lf = new Font("Segoe UI", 8, FontStyle.Bold);
                float lw = g.MeasureString(hs.Label, lf).Width;
                g.DrawString(hs.Label, lf, Brushes.White,
                    hs.Bounds.X + (hs.Bounds.Width - lw) / 2f, hs.Bounds.Bottom - 20);

                string energyText = hs.Cost > 0 ? $"-{hs.Cost}⚡" : hs.Gain > 0 ? $"+{hs.Gain}⚡" : "0";
                Color  textColor  = hs.Cost > 0 ? Color.Orange : hs.Gain > 0 ? Color.LimeGreen : Color.LightGray;
                using var cf = new Font("Segoe UI", 15, FontStyle.Bold);
                float cw = g.MeasureString(energyText, cf).Width;
                g.DrawString(energyText, cf, new SolidBrush(textColor),
                    hs.Bounds.X + (hs.Bounds.Width - cw) / 2f, hs.Bounds.Y + 4);
            }
        }

        // ──────────────────────────────────────────────────────────────
        //  ДИСПЕТЧЕР МИНИ-ИГР
        // ──────────────────────────────────────────────────────────────
        void DrawMiniGame(Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color.FromArgb(235, 14, 14, 32)), 0, 0, 900, 595);
            switch (curGame)
            {
                case GameState.Cooking: DrawCooking(g); break;
                case GameState.Memory:  DrawMemory(g);  break;
                case GameState.Catch:   DrawCatch(g);   break;
            }
        }

        // ──────────────────────────────────────────────────────────────
        //  ЧАСТИЦЫ
        // ──────────────────────────────────────────────────────────────
        void DrawParticles(Graphics g)
        {
            foreach (var p in particles)
            {
                int   a  = Math.Min(255, p.Life * 4);
                float sz = Math.Max(2, p.Life / 10f);
                g.FillEllipse(new SolidBrush(Color.FromArgb(a, p.Color)),
                    p.X - sz / 2, p.Y - sz / 2, sz, sz);
            }
        }

        // ──────────────────────────────────────────────────────────────
        //  ТЕКСТ ДЕЙСТВИЯ
        // ──────────────────────────────────────────────────────────────
        void DrawActionText(Graphics g)
        {
            float a     = Math.Min(1f, actionTimer / 40f);
            int   alpha = (int)(a * 255);
            using var tf = new Font("Segoe UI", 17, FontStyle.Bold);
            float tw = g.MeasureString(lastAction, tf).Width;
            float tx = (Width - tw) / 2;
            g.DrawString(lastAction, tf, new SolidBrush(Color.FromArgb(alpha / 2, 0, 0, 0)), tx + 2, actionY + 2);
            g.DrawString(lastAction, tf, new SolidBrush(Color.FromArgb(alpha, Color.Gold)), tx, actionY);
        }

        // ──────────────────────────────────────────────────────────────
        //  ВСПОМОГАТЕЛЬНЫЕ
        // ──────────────────────────────────────────────────────────────
        void DrawCenteredStr(Graphics g, string s, Font f, Brush b, float y)
        {
            float w = g.MeasureString(s, f).Width;
            g.DrawString(s, f, b, (Width - w) / 2, y);
        }
    }
}
