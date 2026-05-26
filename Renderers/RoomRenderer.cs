using System.Drawing;
using System.Drawing.Drawing2D;

namespace TalkingPete
{
    // Частичный класс MainForm: рендеринг комнат
    public partial class MainForm
    {
        void DrawRoom(Graphics g)
        {
            switch (curRoom)
            {
                case Room.Living:   DrawLiving(g);  break;
                case Room.Kitchen:  DrawKitchen(g); break;
                case Room.Bedroom:  DrawBedroom(g); break;
                case Room.Bathroom: DrawBath(g);    break;
            }
        }

        void DrawFloor(Graphics g, Color wall, Color floor)
        {
            g.FillRectangle(new SolidBrush(wall),  0,   0, 900, 590);
            g.FillRectangle(new SolidBrush(floor),  0, 430, 900, 160);
        }

        // ──────────────────────────────────────────────────────────────
        void DrawLiving(Graphics g)
        {
            DrawFloor(g, Color.FromArgb(225, 190, 150), Color.FromArgb(170, 130, 95));

            // Паркет
            using var pk = new Pen(Color.FromArgb(140, 110, 75), 1.5f);
            for (int x = 0; x < 900; x += 70) g.DrawLine(pk, x, 430, x, 590);
            for (int y = 430; y < 590; y += 28) g.DrawLine(pk, 0, y, 900, y);

            // Обои
            using var wp = new Pen(Color.FromArgb(30, 140, 100, 60), 1);
            for (int xx = 20; xx < 900; xx += 55)
            for (int yy = 20; yy < 430; yy += 55)
            {
                g.DrawEllipse(wp, xx, yy, 8, 8);
                g.DrawEllipse(wp, xx + 4, yy + 12, 5, 5);
            }

            // Плинтус
            g.FillRectangle(new SolidBrush(Color.FromArgb(210, 175, 130)), 0, 425, 900, 10);

            // Окно
            g.FillRectangle(new SolidBrush(Color.FromArgb(170, 210, 240, 210)), 540, 80, 180, 130);
            using var wp2 = new Pen(Color.FromArgb(160, 125, 80), 5);
            g.DrawRectangle(wp2, 540, 80, 180, 130);
            g.DrawLine(wp2, 630, 80, 630, 210);
            g.DrawLine(wp2, 540, 145, 720, 145);

            // Шторы
            using var cb = new SolidBrush(Color.FromArgb(200, 180, 80, 80));
            g.FillPolygon(cb, new[] { new Point(535, 70), new Point(560, 70), new Point(548, 215), new Point(530, 215) });
            g.FillPolygon(cb, new[] { new Point(720, 70), new Point(730, 70), new Point(730, 215), new Point(708, 215) });

            // Диван
            using var sb1 = new SolidBrush(Color.FromArgb(100, 60, 130));
            using var sb2 = new SolidBrush(Color.FromArgb(130, 80, 160));
            g.FillRR(sb1, 30, 360, 210, 95, 14);
            g.FillRR(sb2, 30, 338, 210, 35, 10);
            g.FillRR(sb2, 18, 358, 32, 95, 8);
            g.FillRR(sb2, 220, 358, 32, 95, 8);
            g.FillRR(new SolidBrush(Color.FromArgb(200, 155, 225)), 52, 344, 55, 52, 10);
            g.FillRR(new SolidBrush(Color.FromArgb(200, 155, 225)), 165, 344, 55, 52, 10);

            // Телевизор
            g.FillRR(new SolidBrush(Color.FromArgb(50, 50, 50)), 245, 255, 210, 135, 10);
            g.FillRectangle(new SolidBrush(Color.FromArgb(20, 40, 100)), 258, 265, 184, 115);
            using var glow = new LinearGradientBrush(new Rectangle(258, 265, 184, 115),
                Color.FromArgb(60, 30, 80, 180), Color.FromArgb(180, 10, 20, 60), 45f);
            g.FillRectangle(glow, 258, 265, 184, 115);
            g.DrawString("📺", new Font("Segoe UI Emoji", 28), Brushes.White, 316, 285);
            g.FillRectangle(new SolidBrush(Color.FromArgb(60, 60, 60)), 347, 390, 16, 32);
            g.FillRR(new SolidBrush(Color.FromArgb(80, 80, 80)), 315, 420, 80, 10, 5);

            // Цветок
            g.FillRR(new SolidBrush(Color.FromArgb(170, 125, 75)), 695, 400, 65, 50, 5);
            g.FillEllipse(new SolidBrush(Color.FromArgb(45, 145, 55)), 665, 285, 110, 130);
            g.FillEllipse(new SolidBrush(Color.FromArgb(55, 170, 65)), 682, 265, 80, 110);
            g.FillEllipse(new SolidBrush(Color.FromArgb(210, 80, 80)), 714, 272, 26, 26);

            // Ковёр
            g.FillEllipse(new SolidBrush(Color.FromArgb(80, 200, 120, 80)), 170, 415, 360, 28);
        }

        // ──────────────────────────────────────────────────────────────
        void DrawKitchen(Graphics g)
        {
            DrawFloor(g, Color.FromArgb(245, 235, 205), Color.FromArgb(185, 165, 125));

            // Плитка стены
            using var tb = new SolidBrush(Color.FromArgb(252, 245, 225));
            using var tp = new Pen(Color.FromArgb(210, 200, 180), 1f);
            for (int x = 0; x < 900; x += 55)
            for (int y = 0; y < 430; y += 55)
            {
                g.FillRectangle(tb, x + 1, y + 1, 53, 53);
                g.DrawRectangle(tp, x, y, 55, 55);
            }
            // Плитка пола
            for (int x = 0; x < 900; x += 55)
            for (int y = 430; y < 600; y += 55)
                g.FillRectangle(new SolidBrush(Color.FromArgb(195, 180, 140)), x + 1, y + 1, 53, 53);

            // Столешница
            g.FillRectangle(new SolidBrush(Color.FromArgb(100, 80, 60)),   0, 395, 900, 55);
            g.FillRectangle(new SolidBrush(Color.FromArgb(215, 200, 175)), 0, 385, 900, 16);
            g.FillRectangle(new SolidBrush(Color.FromArgb(80, 60, 40)),    0, 450, 900, 20);

            // Холодильник
            g.FillRR(new SolidBrush(Color.FromArgb(232, 232, 244)), 35, 190, 145, 210, 10);
            g.FillRR(new SolidBrush(Color.FromArgb(215, 215, 228)), 35, 190, 145, 95, 10);
            g.FillRR(new SolidBrush(Color.FromArgb(215, 215, 228)), 35, 290, 145, 110, 10);
            g.DrawLine(new Pen(Color.FromArgb(185, 185, 200), 1), 35, 288, 180, 288);
            g.FillRR(new SolidBrush(Color.Silver), 150, 230, 16, 45, 5);
            g.FillRR(new SolidBrush(Color.Silver), 150, 315, 16, 32, 5);
            g.DrawString("🧊", new Font("Segoe UI Emoji", 22), Brushes.LightBlue, 70, 235);

            // Плита
            g.FillRR(new SolidBrush(Color.FromArgb(72, 72, 82)), 265, 365, 205, 38, 5);
            g.FillRR(new SolidBrush(Color.FromArgb(55, 55, 65)), 272, 280, 190, 85, 8);
            for (int bx = 0; bx < 2; bx++)
            for (int by = 0; by < 2; by++)
                g.FillEllipse(new SolidBrush(Color.FromArgb(38, 38, 45)), 300 + bx * 80, 295 + by * 36, 40, 30);
            g.FillEllipse(new SolidBrush(Color.FromArgb(28, 28, 28)), 308, 248, 85, 28);
            g.FillRectangle(new SolidBrush(Color.FromArgb(28, 28, 28)), 390, 253, 32, 9);
            g.DrawString("🔥", new Font("Segoe UI Emoji", 14), Brushes.OrangeRed, 335, 290);

            // Навесные шкафы
            g.FillRR(new SolidBrush(Color.FromArgb(160, 120, 80)), 0, 80, 280, 140, 5);
            g.FillRR(new SolidBrush(Color.FromArgb(175, 135, 90)), 10, 90, 120, 120, 4);
            g.FillRR(new SolidBrush(Color.FromArgb(175, 135, 90)), 140, 90, 130, 120, 4);
            g.FillRR(new SolidBrush(Color.Silver), 125, 148, 8, 10, 3);

            // Кофемашина
            g.FillRR(new SolidBrush(Color.FromArgb(55, 38, 28)), 605, 275, 135, 115, 10);
            g.FillRR(new SolidBrush(Color.FromArgb(170, 75, 35)), 618, 292, 62, 65, 8);
            g.FillEllipse(new SolidBrush(Color.FromArgb(35, 18, 8)), 645, 355, 32, 15);
            g.DrawString("☕", new Font("Segoe UI Emoji", 18), Brushes.White, 630, 305);
        }

        // ──────────────────────────────────────────────────────────────
        void DrawBedroom(Graphics g)
        {
            DrawFloor(g, Color.FromArgb(195, 205, 225), Color.FromArgb(155, 135, 115));

            // Полосатые обои
            for (int x = 0; x < 900; x += 45)
                g.FillRectangle(new SolidBrush(Color.FromArgb(18, 0, 0, 110)), x, 0, 22, 430);

            // Звёзды
            using var sf2 = new Font("Segoe UI Emoji", 8);
            for (int x = 22; x < 900; x += 90)
            for (int y = 30; y < 420; y += 70)
                g.DrawString("⭐", sf2, new SolidBrush(Color.FromArgb(50, 255, 230, 100)), x, y);

            // Кровать
            g.FillRR(new SolidBrush(Color.FromArgb(115, 78, 48)),  55, 315, 275, 85, 10);
            g.FillRR(new SolidBrush(Color.FromArgb(240, 242, 255)), 65, 298, 255, 92, 8);
            g.FillRR(new SolidBrush(Color.FromArgb(95, 58, 35)),   55, 308, 275, 52, 8);
            g.FillRR(new SolidBrush(Color.White), 75, 300, 105, 58, 10);
            g.FillRR(new SolidBrush(Color.White), 195, 300, 105, 58, 10);
            using var dp = new Pen(Color.FromArgb(40, 120, 120, 200), 1.5f);
            for (int xx = 75; xx < 315; xx += 28) g.DrawLine(dp, xx, 300, xx, 390);

            // Тумбочка и лампа
            g.FillRR(new SolidBrush(Color.FromArgb(115, 85, 50)),  338, 360, 70, 70, 5);
            g.FillRectangle(new SolidBrush(Color.FromArgb(80, 55, 35)), 367, 325, 12, 38);
            g.FillEllipse(new SolidBrush(Color.FromArgb(240, 220, 155)), 342, 290, 62, 38);
            g.FillEllipse(new SolidBrush(Color.FromArgb(60, 255, 245, 180)), 320, 270, 105, 90);

            // Зеркало
            g.FillRR(new SolidBrush(Color.FromArgb(175, 135, 95)),     355, 210, 140, 180, 10);
            g.FillRR(new SolidBrush(Color.FromArgb(195, 215, 235, 200)), 365, 220, 120, 162, 8);
            g.DrawString("🪞", new Font("Segoe UI Emoji", 30),
                new SolidBrush(Color.FromArgb(180, 255, 255, 255)), 390, 275);

            // Книжная полка
            g.FillRR(new SolidBrush(Color.FromArgb(112, 75, 48)), 552, 228, 182, 205, 5);
            Color[] bc = { Color.Crimson, Color.RoyalBlue, Color.SeaGreen, Color.DarkOrange, Color.Purple, Color.Teal };
            for (int i = 0; i < 6; i++) g.FillRR(new SolidBrush(bc[i]), 562 + i * 27, 242, 22, 65, 3);
            for (int i = 0; i < 6; i++) g.FillRR(new SolidBrush(bc[(i + 2) % 6]), 562 + i * 27, 328, 22, 75, 3);
            using var shp = new Pen(Color.FromArgb(85, 55, 28), 3);
            g.DrawLine(shp, 552, 315, 734, 315);
            g.DrawLine(shp, 552, 410, 734, 410);
        }

        // ──────────────────────────────────────────────────────────────
        void DrawBath(Graphics g)
        {
            Color tc1 = Color.FromArgb(218, 228, 238), tc2 = Color.FromArgb(178, 192, 205);
            using var tpen = new Pen(Color.FromArgb(175, 190, 208), 1f);
            for (int x = 0; x < 900; x += 62)
            for (int y = 0; y < 590; y += 62)
            {
                bool floor = y >= 430;
                g.FillRectangle(new SolidBrush(floor ? tc2 : tc1), x + 1, y + 1, 60, 60);
                g.DrawRectangle(tpen, x, y, 62, 62);
            }

            // Душевая кабина
            g.FillRR(new SolidBrush(Color.FromArgb(110, 195, 208, 228)), 25, 242, 175, 192, 5);
            g.DrawRR(new Pen(Color.FromArgb(145, 165, 195), 3), 25, 242, 175, 192, 5);
            g.FillEllipse(new SolidBrush(Color.Silver), 52, 238, 48, 18);
            g.FillRectangle(new SolidBrush(Color.Silver), 70, 215, 10, 28);
            int dt = animFrame % 4;
            using var drb = new SolidBrush(Color.FromArgb(120, 100, 175, 240));
            for (int d = 0; d < 6; d++) g.FillEllipse(drb, 68 + d * 16, 310 + (d + dt) * 8 % 60, 6, 10);
            g.DrawString("🚿", new Font("Segoe UI Emoji", 26), Brushes.LightCyan, 78, 292);

            // Зеркало над раковиной
            g.FillRR(new SolidBrush(Color.FromArgb(195, 215, 232)), 305, 182, 210, 118, 8);
            g.DrawRR(new Pen(Color.Silver, 3), 305, 182, 210, 118, 8);
            g.DrawString("😊", new Font("Segoe UI Emoji", 30), Brushes.White, 380, 205);

            // Раковина
            g.FillRR(new SolidBrush(Color.FromArgb(235, 238, 248)), 318, 312, 195, 112, 14);
            g.FillEllipse(new SolidBrush(Color.FromArgb(195, 205, 222)), 348, 332, 115, 68);
            g.FillEllipse(new SolidBrush(Color.FromArgb(170, 182, 202)), 395, 360, 32, 22);
            g.FillRectangle(new SolidBrush(Color.Silver), 403, 298, 10, 22);
            g.FillRR(new SolidBrush(Color.Silver), 382, 292, 54, 14, 5);
            g.DrawString("🪥", new Font("Segoe UI Emoji", 22), Brushes.White, 382, 285);

            // Ванна
            g.FillRR(new SolidBrush(Color.FromArgb(238, 238, 255)), 590, 292, 248, 138, 20);
            g.FillRR(new SolidBrush(Color.FromArgb(130, 175, 225, 140)), 602, 312, 225, 100, 14);
            for (int b = 0; b < 8; b++)
            {
                int bx = 615 + b * 26, by = 315 + (b * 13 + animFrame * 3) % 55;
                g.FillEllipse(new SolidBrush(Color.FromArgb(90, 200, 220, 255)), bx, by, 18, 18);
            }
            g.DrawString("🦆", new Font("Segoe UI Emoji", 30), Brushes.Yellow, 688, 328);
            g.FillRR(new SolidBrush(Color.Silver), 590, 282, 62, 16, 5);
            g.FillRectangle(new SolidBrush(Color.Silver), 610, 262, 10, 25);
        }
    }
}
