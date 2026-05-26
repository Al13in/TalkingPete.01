using System;
using System.Collections.Generic;
using System.Drawing;

namespace TalkingPete
{
    // Частичный класс MainForm: мини-игра «Память»
    public partial class MainForm
    {
        // ── Состояние ─────────────────────────────────────────────────
        List<int> memSeq = new();
        List<int> memPly = new();
        int       memRound     = 1;
        int       memScore     = 0;
        bool      memShowing   = false;
        int       memShowIdx   = 0;
        int       memShowTmr   = 0;
        int       memHighlight = -1;
        Rectangle[] memBtns   = Array.Empty<Rectangle>();

        readonly Color[] memCol  = {
            Color.FromArgb(200, 50, 50),
            Color.FromArgb(50, 50, 210),
            Color.FromArgb(50, 170, 50),
            Color.FromArgb(200, 190, 40),
        };
        readonly string[] memIcon = { "🔴", "🔵", "🟢", "🟡" };

        // ── Логика ────────────────────────────────────────────────────
        void InitMemory()
        {
            memRound     = 1;
            memScore     = 0;
            memHighlight = -1;
            memSeq.Clear();
            memPly.Clear();

            int cx = Width / 2, cy = 310;
            memBtns = new[]
            {
                new Rectangle(cx - 155, cy - 135, 130, 130),
                new Rectangle(cx +  25, cy - 135, 130, 130),
                new Rectangle(cx - 155, cy +  15, 130, 130),
                new Rectangle(cx +  25, cy +  15, 130, 130),
            };
            AddMemStep();
        }

        void AddMemStep()
        {
            memSeq.Add(rnd.Next(4));
            memPly.Clear();
            memShowing   = true;
            memShowIdx   = 0;
            memShowTmr   = 0;
            memHighlight = -1;
        }

        void UpdateMemory()
        {
            if (!memShowing) return;
            memShowTmr++;
            if (memShowTmr <= 45) return;
            memShowTmr = 0;

            if (memHighlight >= 0)
            {
                memHighlight = -1;
                memShowIdx++;
            }
            else if (memShowIdx < memSeq.Count)
                memHighlight = memSeq[memShowIdx];
            else
            {
                memShowing   = false;
                memHighlight = -1;
            }
        }

        void ClickMemory(System.Drawing.Point p)
        {
            if (memShowing) return;
            for (int i = 0; i < 4; i++)
            {
                if (!memBtns[i].Contains(p)) continue;
                memPly.Add(i);
                int step = memPly.Count - 1;

                if (memSeq[step] == i)
                {
                    Burst(memBtns[i].X + 65, memBtns[i].Y + 65, memCol[i], 6);
                    if (memPly.Count >= memSeq.Count)
                    {
                        memScore += memRound * 20;
                        GainXP(memRound * 15);
                        Drain(5);
                        memRound++;
                        lastAction  = $"🧠 Правильно! Раунд {memRound}";
                        actionTimer = 120;
                        actionY     = 230f;
                        AddMemStep();
                    }
                }
                else
                {
                    Burst(memBtns[i].X + 65, memBtns[i].Y + 65, Color.Red, 10);
                    GainXP(memScore / 5 + 5);
                    Drain(10);
                    lastAction  = $"❌ Игра окончена! Счёт: {memScore}";
                    actionTimer = 160;
                    actionY     = 230f;
                    ExitGame();
                }
                return;
            }
        }

        // ── Отрисовка ─────────────────────────────────────────────────
        void DrawMemory(Graphics g)
        {
            using var tf = new Font("Segoe UI", 22, FontStyle.Bold);
            DrawCenteredStr(g, "Игра на Память", tf, Brushes.Gold, 110);

            using var inf = new Font("Segoe UI", 12);
            string st = memShowing ? "Смотри внимательно..." : "Твоя очередь! Повтори";
            DrawCenteredStr(g, st, inf, Brushes.White, 152);
            DrawCenteredStr(g, $"Раунд: {memRound}   Счёт: {memScore}", inf, Brushes.Yellow, 182);

            for (int i = 0; i < 4; i++)
            {
                if (memBtns == null || i >= memBtns.Length) break;

                bool  lit = memHighlight == i;
                Color bg  = lit ? Color.White : memCol[i];
                float sc2 = lit ? 1.05f : 1f;
                int mx = (int)(memBtns[i].X + (1 - sc2) * memBtns[i].Width  / 2);
                int my = (int)(memBtns[i].Y + (1 - sc2) * memBtns[i].Height / 2);
                int mw = (int)(memBtns[i].Width * sc2), mh = (int)(memBtns[i].Height * sc2);

                g.FillRR(new SolidBrush(bg), mx, my, mw, mh, 15);
                if (lit)
                    g.FillRR(new SolidBrush(Color.FromArgb(100, 255, 255, 200)), mx, my, mw, mh, 15);
                g.DrawRR(new Pen(Color.FromArgb(200, 255, 255, 255), 2), mx, my, mw, mh, 15);

                g.DrawString(memIcon[i], new Font("Segoe UI Emoji", 42),
                    new SolidBrush(lit ? Color.Black : Color.White),
                    mx + mw / 2 - 24, my + mh / 2 - 28);
            }

            if (!memShowing)
            {
                using var hint = new Font("Segoe UI", 9, FontStyle.Italic);
                DrawCenteredStr(g, "Нажми кнопки в том же порядке",
                    hint, new SolidBrush(Color.FromArgb(160, 200, 220, 255)), 498);
                DrawCenteredStr(g, $"Осталось нажать: {memSeq.Count - memPly.Count}",
                    hint, new SolidBrush(Color.FromArgb(160, 255, 220, 100)), 518);
            }
        }
    }
}
