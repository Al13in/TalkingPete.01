using System.Windows.Forms;

namespace TalkingPete
{
    public partial class MainForm
    {
        // ──────────────────────────────────────────────────────────────
        //  ВВОД
        // ──────────────────────────────────────────────────────────────
        void OnClick(object? sender, MouseEventArgs e)
        {
            if (curGame == GameState.Normal)
            {
                HandleHotSpotClick(e);
                HandlePetClick(e);
            }
            else if (curGame == GameState.Cooking) ClickCooking(e.Location);
            else if (curGame == GameState.Memory)  ClickMemory(e.Location);
            else if (curGame == GameState.Catch)   ClickCatch(e.Location);
        }

        void HandleHotSpotClick(MouseEventArgs e)
        {
            foreach (var hs in hotSpots)
            {
                if (!hs.Bounds.Contains(e.Location)) continue;

                if (pet.Energy < hs.Cost)
                {
                    lastAction  = "Нет энергии! Нужен отдых";
                    actionTimer = 120;
                    actionY     = 240f;
                    return;
                }
                Drain(hs.Cost);
                hs.OnClick?.Invoke();
                return;
            }
        }

        void HandlePetClick(MouseEventArgs e)
        {
            if (System.Math.Abs(e.X - 450) >= 65 || System.Math.Abs(e.Y - 368) >= 90)
                return;

            if (pet.Energy <= 0)
            {
                pet.State = PetState.Sad;
                var phrases = new[] { "Я устал... 😴", "Нет сил...", "Слишком мало энергии..." };
                lastAction  = phrases[rnd.Next(phrases.Length)];
                actionTimer = 140;
                actionY     = 240f;
                Burst(450, 300, System.Drawing.Color.FromArgb(120, 120, 150), 6);
            }
            else
            {
                pet.State = PetState.Happy;
                var phrases = new[] { "Мур-мур!", "Почеши за ушком!", "Играть!" };
                lastAction  = phrases[rnd.Next(phrases.Length)];
                actionTimer = 100;
                actionY     = 240f;
                Burst(450, 300, System.Drawing.Color.Pink, 10);
                GainXP(2);
            }
        }

        void OnKeyDown(object? sender, KeyEventArgs e)
        {
            if (curGame == GameState.Catch)
            {
                if (e.KeyCode == Keys.Left  || e.KeyCode == Keys.A) catchLeft  = true;
                if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D) catchRight = true;
            }
            if (e.KeyCode == Keys.Escape) ExitGame();
        }

        void OnKeyUp(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left  || e.KeyCode == Keys.A) catchLeft  = false;
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D) catchRight = false;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            loopTmr?.Stop();
            animTmr?.Stop();
            drainTmr?.Stop();
            backBuf?.Dispose();
            base.OnFormClosed(e);
        }
    }
}
