using System;
using System.Drawing;

namespace TalkingPete
{
    public partial class MainForm
    {
        // ──────────────────────────────────────────────────────────────
        //  ДЕЙСТВИЯ ПИТОМЦА
        // ──────────────────────────────────────────────────────────────
        void DoAction(string msg, PetState ps, int xpGain, float restoreE = 0)
        {
            pet.State  = ps;
            lastAction = msg;
            actionTimer = 160;
            actionY = 235f;

            if (xpGain > 0)
                GainXP(xpGain);

            if (restoreE > 0)
            {
                pet.Energy = Math.Min(pet.MaxEnergy, pet.Energy + restoreE);
                pet.Mood   = Math.Min(100, pet.Mood + 5);
            }
            Burst(Width / 2, 350, Color.Gold, 10);
        }

        void Drain(float amt) =>
            pet.Energy = Math.Max(0, pet.Energy - amt);

        void GainXP(int amt)
        {
            pet.XP += amt;
            Burst(780, 55, Color.LimeGreen, 8);

            while (pet.XP >= pet.XpNext)
            {
                pet.XP     -= pet.XpNext;
                pet.Level++;
                pet.XpNext  = (int)(pet.XpNext * 1.4f);
                pet.MaxEnergy += 5;
                pet.Energy    = pet.MaxEnergy;
                lastAction  = $"🎉 Уровень {pet.Level}!";
                actionTimer = 220;
                actionY     = 240f;
                Burst(Width / 2, Height / 2, Color.Gold, 25);
            }
        }

        void Burst(float x, float y, Color c, int n)
        {
            for (int i = 0; i < n; i++)
            {
                double a = rnd.NextDouble() * Math.PI * 2;
                float  s = (float)(rnd.NextDouble() * 3.5 + 0.8);
                particles.Add(new Particle
                {
                    X = x, Y = y,
                    VX = (float)Math.Cos(a) * s,
                    VY = (float)Math.Sin(a) * s,
                    Color = c,
                    Life  = 55 + rnd.Next(25),
                });
            }
        }

        // ──────────────────────────────────────────────────────────────
        //  УПРАВЛЕНИЕ МИНИГРАМИ
        // ──────────────────────────────────────────────────────────────
        void StartMG(GameState mg)
        {
            if (pet.Energy <= 0)
            {
                lastAction  = "❌ Питомец слишком устал для игр!";
                actionTimer = 120;
                actionY     = 240f;
                return;
            }
            curGame = mg;
            exitBtn.Visible = true;
            switch (mg)
            {
                case GameState.Cooking: InitCooking(); break;
                case GameState.Memory:  InitMemory();  break;
                case GameState.Catch:   InitCatch();   break;
            }
        }

        void ExitGame()
        {
            curGame = GameState.Normal;
            exitBtn.Visible = false;
            pet.State = PetState.Idle;
            catchItems.Clear();
        }
    }
}
