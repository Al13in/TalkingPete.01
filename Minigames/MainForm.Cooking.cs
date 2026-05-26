using System;
using System.Drawing;

namespace TalkingPete
{
    // Частичный класс MainForm: мини-игра «Кулинария»
    public partial class MainForm
    {
        // ── Состояние ─────────────────────────────────────────────────
        int[]    cookSeq      = Array.Empty<int>();
        int      cookStep     = 0;
        int      cookScore    = 0;
        bool     cookShowing  = false;
        int      cookShowIdx  = 0;
        int      cookShowTmr  = 0;
        Rectangle[] ingBounds = Array.Empty<Rectangle>();

        readonly string[] ingEmoji = { "🧄", "🧅", "🥕", "🥩", "🧂" };
        readonly string[] ingName  = { "Чеснок", "Лук", "Морковь", "Мясо", "Соль" };

        // ── Логика ────────────────────────────────────────────────────
        void InitCooking()
        {
            cookScore   = 0;
            cookStep    = 0;
            cookShowing = true;
            cookShowIdx = 0;
            cookShowTmr = 0;

            int len = Math.Min(3 + pet.Level / 3, 6);
            cookSeq = new int[len];
            for (int i = 0; i < len; i++) cookSeq[i] = rnd.Next(5);

            ingBounds = new Rectangle[5];
            for (int i = 0; i < 5; i++)
                ingBounds[i] = new Rectangle(120 + i * 136, 390, 110, 105);
        }

        void UpdateCooking()
        {
            if (!cookShowing) return;
            cookShowTmr++;
            if (cookShowTmr <= 35) return;
            cookShowTmr = 0;
            cookShowIdx++;
            if (cookShowIdx >= cookSeq.Length)
            {
                cookShowing  = false;
                cookShowIdx  = -1;
            }
        }

        void ClickCooking(System.Drawing.Point p)
        {
            if (cookShowing) return;
            for (int i = 0; i < 5; i++)
            {
                if (!ingBounds[i].Contains(p)) continue;

                if (cookSeq[cookStep] == i)
                {
                    cookScore += 10;
                    cookStep++;
                    Burst(ingBounds[i].X + 55, ingBounds[i].Y + 50, Color.Gold, 8);
                    GainXP(10);

                    if (cookStep >= cookSeq.Length)
                    {
                        GainXP(40);
                        Drain(15);
                        lastAction  = $"🍳 Блюдо готово! +{40 + cookScore} XP";
                        actionTimer = 160;
                        actionY     = 230f;
                        InitCooking();
                    }
                }
                else
                {
                    Burst(ingBounds[i].X + 55, ingBounds[i].Y + 50, Color.OrangeRed, 8);
                    Drain(8);
                    cookScore   = 0;
                    lastAction  = "❌ Не тот ингредиент!";
                    actionTimer = 100;
                    actionY     = 230f;
                    InitCooking();
                }
                return;
            }
        }

        // ── Отрисовка ─────────────────────────────────────────────────
        void DrawCooking(Graphics g)
        {
            using var tf = new Font("Segoe UI", 22, FontStyle.Bold);
            DrawCenteredStr(g, "Кулинарный Мастер", tf, Brushes.Gold, 110);

            using var inf = new Font("Segoe UI", 13);
            string info = cookShowing
                ? $"Запомни порядок! ({cookSeq.Length} ингредиентов)"
                : $"Повтори! Шаг {cookStep + 1}/{cookSeq.Length}";
            DrawCenteredStr(g, info, inf, Brushes.White, 158);

            using var sf = new Font("Segoe UI", 13, FontStyle.Bold);
            DrawCenteredStr(g, $"Счёт: {cookScore}", sf, Brushes.Yellow, 195);

            using var df = new Font("Segoe UI", 10);
            DrawCenteredStr(g, "Кликни на ингредиенты в правильном порядке",
                df, new SolidBrush(Color.FromArgb(180, 220, 220, 220)), 232);

            // Кнопки ингредиентов
            for (int i = 0; i < 5; i++)
            {
                if (ingBounds == null || i >= ingBounds.Length) break;

                bool showHi = cookShowing && cookShowIdx < cookSeq.Length && cookSeq[cookShowIdx] == i;
                Color bg = showHi ? Color.FromArgb(255, 225, 50) : Color.FromArgb(55, 75, 115);

                g.FillRR(new SolidBrush(bg), ingBounds[i].X, ingBounds[i].Y,
                    ingBounds[i].Width, ingBounds[i].Height, 14);

                if (!cookShowing)
                    g.DrawRR(new Pen(Color.FromArgb(140, 255, 255, 90), 2),
                        ingBounds[i].X, ingBounds[i].Y, ingBounds[i].Width, ingBounds[i].Height, 14);

                g.DrawString(ingEmoji[i], new Font("Segoe UI Emoji", 32), Brushes.White,
                    ingBounds[i].X + ingBounds[i].Width / 2 - 20, ingBounds[i].Y + 10);

                using var nf = new Font("Segoe UI", 8);
                float nw = g.MeasureString(ingName[i], nf).Width;
                g.DrawString(ingName[i], nf, Brushes.White,
                    ingBounds[i].X + (ingBounds[i].Width - nw) / 2, ingBounds[i].Bottom - 20);
            }

            // Прогресс последовательности
            using var pf = new Font("Segoe UI Emoji", 16);
            for (int i = 0; i < cookSeq.Length; i++)
            {
                bool done = !cookShowing && i < cookStep;
                g.DrawString(done ? "✅" : ingEmoji[cookSeq[i]], pf,
                    done ? Brushes.LightGreen : new SolidBrush(Color.FromArgb(100, 200, 200, 200)),
                    180 + i * 90, 500);
            }

            if (cookShowing)
            {
                using var tipf = new Font("Segoe UI", 9, FontStyle.Italic);
                DrawCenteredStr(g, "Внимание! Запоминай...",
                    tipf, new SolidBrush(Color.FromArgb(180, 255, 220, 100)), 548);
            }
        }
    }
}
