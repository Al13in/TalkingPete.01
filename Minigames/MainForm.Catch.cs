using System.Drawing.Drawing2D;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace TalkingPete
{
    // Частичный класс MainForm: мини-игра «Поймай уточку»
    public partial class MainForm
    {
        // ── Состояние ─────────────────────────────────────────────────
        List<FallItem> catchItems     = new();
        Rectangle      catchBasket;
        int            catchScore     = 0;
        int            catchLives     = 3;
        int            catchSpawnTmr  = 0;
        bool           catchLeft      = false;
        bool           catchRight     = false;

        // ── Логика ────────────────────────────────────────────────────
        void InitCatch()
        {
            catchScore    = 0;
            catchLives    = 3;
            catchSpawnTmr = 0;
            catchItems.Clear();
            catchBasket = new Rectangle(Width / 2 - 65, 530, 130, 42);
        }

        void UpdateCatch()
        {
            MoveBasket();
            SpawnItems();
            UpdateItems();
            CheckGameOver();
        }

        void MoveBasket()
        {
            if (catchLeft  && catchBasket.X > 10)
                catchBasket = new Rectangle(catchBasket.X - 9, catchBasket.Y, catchBasket.Width, catchBasket.Height);
            if (catchRight && catchBasket.Right < Width - 10)
                catchBasket = new Rectangle(catchBasket.X + 9, catchBasket.Y, catchBasket.Width, catchBasket.Height);
        }

        void SpawnItems()
        {
            catchSpawnTmr++;
            int rate = Math.Max(22, 85 - catchScore * 2);
            if (catchSpawnTmr < rate) return;

            catchSpawnTmr = 0;
            catchItems.Add(new FallItem
            {
                X   = rnd.Next(50, Width - 80),
                Y   = 100,
                Bad = rnd.Next(5) == 0,
            });
        }

        void UpdateItems()
        {
            float speed = 2.8f + catchScore * 0.12f;
            for (int i = catchItems.Count - 1; i >= 0; i--)
            {
                var it = catchItems[i];
                it.Y += speed;
                catchItems[i] = it;

                var r = new Rectangle((int)it.X, (int)it.Y, 38, 38);
                if (r.IntersectsWith(catchBasket))
                {
                    if (it.Bad)
                    {
                        catchLives--;
                        Burst(catchBasket.X + 65, catchBasket.Y, Color.Red, 8);
                    }
                    else
                    {
                        catchScore++;
                        GainXP(5);
                        Burst(catchBasket.X + 65, catchBasket.Y, Color.Gold, 6);
                    }
                    catchItems.RemoveAt(i);
                }
                else if (it.Y > 580)
                {
                    if (!it.Bad) catchLives--;
                    catchItems.RemoveAt(i);
                }
            }
        }

        void CheckGameOver()
        {
            if (catchLives > 0) return;
            GainXP(catchScore * 3 + 10);
            Drain(15);
            lastAction  = $"🛁 Конец игры! Счёт: {catchScore}";
            actionTimer = 200;
            actionY     = 230f;
            ExitGame();
        }

        void ClickCatch(System.Drawing.Point p) { } // Управление только клавиатурой

        // ── Отрисовка ─────────────────────────────────────────────────
        void DrawCatch(Graphics g)
        {
            DrawCatchBackground(g);
            DrawCatchHUD(g);
            DrawCatchItems(g);
            DrawCatchBasket(g);
            DrawCatchHints(g);
        }

        void DrawCatchBackground(Graphics g)
        {
            using var sky = new LinearGradientBrush(
                new Rectangle(0, 95, 900, 505),
                Color.FromArgb(22, 65, 128), Color.FromArgb(55, 95, 155), 90f);
            g.FillRectangle(sky, 0, 95, 900, 505);

            // Облака
            for (int c = 0; c < 3; c++)
            {
                int cx = 120 + c * 280 + (animFrame * (c + 1)) % 30;
                g.FillEllipse(new SolidBrush(Color.FromArgb(60, 255, 255, 255)), cx, 130 + c * 20, 90, 35);
                g.FillEllipse(new SolidBrush(Color.FromArgb(60, 255, 255, 255)), cx + 30, 118 + c * 20, 70, 40);
            }

            // Вода
            g.FillRectangle(new SolidBrush(Color.FromArgb(60, 100, 175)), 0, 508, 900, 90);
            using var wpen = new Pen(Color.FromArgb(100, 150, 210, 255), 2);
            for (int wx = 0; wx < 900; wx += 40)
                g.DrawArc(wpen, wx + (animFrame * 3 % 40), 510, 40, 14, 0, -180);
        }

        void DrawCatchHUD(Graphics g)
        {
            using var tf = new Font("Segoe UI", 18, FontStyle.Bold);
            DrawCenteredStr(g, "🛁 Поймай уточку!", tf, Brushes.Gold, 108);

            using var sf = new Font("Segoe UI", 12, FontStyle.Bold);
            g.DrawString($"Счёт: {catchScore}", sf, Brushes.White, 30, 138);

            string hearts = "";
            for (int h = 0; h < 3; h++) hearts += h < catchLives ? "❤️ " : "🖤 ";
            g.DrawString(hearts, new Font("Segoe UI Emoji", 14), Brushes.White, 600, 133);
        }

        void DrawCatchItems(Graphics g)
        {
            using var itemFont = new Font("Segoe UI Emoji", 22);
            foreach (var it in catchItems)
                g.DrawString(it.Bad ? "💩" : "🦆", itemFont, Brushes.White, it.X, it.Y);
        }

        void DrawCatchBasket(Graphics g)
        {
            g.FillRR(new SolidBrush(Color.FromArgb(148, 96, 46)),
                catchBasket.X, catchBasket.Y, catchBasket.Width, catchBasket.Height, 8);
            g.DrawRR(new Pen(Color.FromArgb(200, 135, 75), 3),
                catchBasket.X, catchBasket.Y, catchBasket.Width, catchBasket.Height, 8);
            g.DrawString("🧺", new Font("Segoe UI Emoji", 22), Brushes.White,
                catchBasket.X + catchBasket.Width / 2 - 15, catchBasket.Y + 4);
        }

        void DrawCatchHints(Graphics g)
        {
            using var hf = new Font("Segoe UI", 9);
            DrawCenteredStr(g, "← → для движения корзинки",
                hf, new SolidBrush(Color.FromArgb(160, 200, 220, 255)), 568);
            DrawCenteredStr(g, "Лови 🦆 уточек, избегай 💩",
                hf, new SolidBrush(Color.FromArgb(140, 255, 210, 100)), 585);
        }
    }
}
