using System;
using System.Drawing;
using System.Windows.Forms;

namespace TalkingPete
{
    /// <summary>
    /// Главная форма — тонкий оркестратор.
    /// Логика разбита по partial-файлам:
    ///   MainForm.State.cs       — поля / состояние
    ///   MainForm.Setup.cs       — построение UI и горячих зон
    ///   MainForm.GameActions.cs — действия питомца, XP, частицы
    ///   MainForm.GameLoop.cs    — Update_ (тик логики)
    ///   MainForm.Render.cs      — HUD, горячие зоны, частицы, текст
    ///   MainForm.Input.cs       — мышь, клавиатура, закрытие формы
    ///   Renderers/RoomRenderer  — отрисовка комнат
    ///   Renderers/PetRenderer   — отрисовка питомца
    ///   Minigames/Cooking       — кулинарная мини-игра
    ///   Minigames/Memory        — мини-игра на память
    ///   Minigames/Catch         — мини-игра «поймай уточку»
    /// </summary>
    public partial class MainForm : Form
    {
        public MainForm()
        {
            Text             = "Talking Pete 🐱";
            Size             = new Size(1280, 720);
            WindowState      = FormWindowState.Maximized;
            StartPosition    = FormStartPosition.CenterScreen;
            DoubleBuffered   = true;
            BackColor        = Color.FromArgb(20, 20, 40);
            FormBorderStyle  = FormBorderStyle.None;
            MaximizeBox      = false;
            KeyPreview       = true;

            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint
                    | ControlStyles.UserPaint
                    | ControlStyles.OptimizedDoubleBuffer,
                true);
            this.UpdateStyles();

            backBuf = new Bitmap(ClientSize.Width, ClientSize.Height);

            BuildNavButtons();
            BuildExitButton();
            BuildHotSpots();

            Resize += (_, _) => UpdateLayout();
            UpdateLayout();

            SetupTimers();

            Paint      += (_, e) => e.Graphics.DrawImage(backBuf, 0, 0);
            MouseClick += OnClick;
            KeyDown    += OnKeyDown;
            KeyUp      += OnKeyUp;
        }

        void SetupTimers()
        {
            loopTmr = new System.Windows.Forms.Timer { Interval = 16 };
            loopTmr.Tick += (_, _) =>
            {
                Update_();
                Render();
                Invalidate();
            };

            animTmr = new System.Windows.Forms.Timer { Interval = 180 };
            animTmr.Tick += (_, _) => animFrame = (animFrame + 1) % 8;

            drainTmr = new System.Windows.Forms.Timer { Interval = 4000 };
            drainTmr.Tick += (_, _) =>
            {
                if (curGame == GameState.Normal)
                {
                    Drain(2);
                    pet.Mood = Math.Max(0, pet.Mood - 1);
                }
            };

            loopTmr.Start();
            animTmr.Start();
            drainTmr.Start();
        }
    }
}
