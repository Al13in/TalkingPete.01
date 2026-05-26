using System;
using System.Drawing;
using System.Windows.Forms;

namespace TalkingPete
{
    public partial class MainForm
    {
        // ──────────────────────────────────────────────────────────────
        //  ИНИЦИАЛИЗАЦИЯ UI
        // ──────────────────────────────────────────────────────────────
        void BuildNavButtons()
        {
            string[] labels = { "Гостиная", "Кухня", "Спальня", "Ванная" };
            navBtns = new Button[4];
            for (int i = 0; i < 4; i++)
            {
                int idx = i;
                navBtns[i] = new Button
                {
                    Text      = labels[i],
                    Size      = new Size(196, 40),
                    Location  = new Point(10 + i * 218, 608),
                    Font      = new Font("Segoe UI", 10, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(45, 65, 110),
                    ForeColor = Color.White,
                    Cursor    = Cursors.Hand,
                };
                navBtns[i].FlatAppearance.BorderColor = Color.FromArgb(80, 120, 200);
                navBtns[i].Click += (_, _) =>
                {
                    if (curGame != GameState.Normal) return;
                    curRoom = (Room)idx;
                    BuildHotSpots();
                };
                Controls.Add(navBtns[i]);
            }
        }

        void BuildExitButton()
        {
            exitBtn = new Button
            {
                Text      = "✕ Выйти",
                Size      = new Size(110, 34),
                Location  = new Point(770, 8),
                Font      = new Font("Segoe UI", 9, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(160, 45, 45),
                ForeColor = Color.White,
                Visible   = false,
                Cursor    = Cursors.Hand,
            };
            exitBtn.Click += (_, _) => ExitGame();
            Controls.Add(exitBtn);
        }

        void UpdateLayout()
        {
            if (ClientSize.Width <= 0 || ClientSize.Height <= 0) return;

            backBuf?.Dispose();
            backBuf = new Bitmap(ClientSize.Width, ClientSize.Height);

            for (int i = 0; i < navBtns.Length; i++)
                navBtns[i].Location = new Point(10 + i * 218, ClientSize.Height - 52);

            exitBtn.Location = new Point(ClientSize.Width - exitBtn.Width - 10, 8);
        }

        // ──────────────────────────────────────────────────────────────
        //  ГОРЯЧИЕ ЗОНЫ
        // ──────────────────────────────────────────────────────────────
        void AddHS(System.Drawing.Rectangle b, string lbl, string icon, int cost, int gain, Action cb) =>
            hotSpots.Add(new HotSpot
            {
                Bounds  = b,
                Label   = lbl,
                Icon    = icon,
                Cost    = cost,
                Gain    = gain,
                OnClick = cb,
            });

        void BuildHotSpots()
        {
            hotSpots.Clear();
            switch (curRoom)
            {
                case Room.Living:
                    AddHS(new Rectangle(40, 340, 180, 110),  "Диван",        "", 0,  12, () => DoAction("Отдыхает на диване",    PetState.Idle,    0,  restoreE: 12));
                    AddHS(new Rectangle(260, 285, 170, 100), "Телевизор",    "", 8,  0,  () => DoAction("Смотрит ТВ",            PetState.Happy,   20));
                    AddHS(new Rectangle(670, 270, 120, 150), "Цветок",       "", 5,  0,  () => DoAction("Поливает цветок",       PetState.Happy,   15));
                    break;
                case Room.Kitchen:
                    AddHS(new Rectangle(40, 280, 140, 150),  "Холодильник",  "", 0,  18, () => DoAction("Ест из холодильника",   PetState.Eating,  10, restoreE: 18));
                    AddHS(new Rectangle(280, 290, 180, 110), "Плита",        "", 15, 0,  () => StartMG(GameState.Cooking));
                    AddHS(new Rectangle(610, 275, 140, 120), "Кофемашина",   "", 0,  22, () => DoAction("Пьёт кофе ",            PetState.Happy,   12, restoreE: 22));
                    break;
                case Room.Bedroom:
                    AddHS(new Rectangle(60, 305, 260, 140),  "Кровать",      "", 0,  40, () => DoAction("Сладко спит...",        PetState.Sleeping, 0, restoreE: 40));
                    AddHS(new Rectangle(560, 255, 160, 190), "Книжная полка","", 10, 0,  () => StartMG(GameState.Memory));
                    AddHS(new Rectangle(360, 295, 120, 130), "Зеркало",      "", 5,  0,  () => DoAction("Любуется собой",        PetState.Happy,   10));
                    break;
                case Room.Bathroom:
                    AddHS(new Rectangle(30, 245, 170, 185),  "Душ",          "", 12, 0,  () => DoAction("Принимает душ",         PetState.Washing, 15));
                    AddHS(new Rectangle(340, 305, 150, 105), "Раковина",     "", 5,  0,  () => DoAction("Чистит зубы",           PetState.Happy,   10));
                    AddHS(new Rectangle(600, 285, 240, 145), "Ванна",        "", 15, 0,  () => StartMG(GameState.Catch));
                    break;
            }
        }
    }
}
