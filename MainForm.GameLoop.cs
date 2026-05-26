using System;

namespace TalkingPete
{
    public partial class MainForm
    {
        // ──────────────────────────────────────────────────────────────
        //  ИГРОВОЙ ЦИКЛ — UPDATE
        // ──────────────────────────────────────────────────────────────
        void Update_()
        {
            UpdateParticles();
            UpdateActionText();
            UpdatePetState();

            switch (curGame)
            {
                case GameState.Cooking: UpdateCooking(); break;
                case GameState.Memory:  UpdateMemory();  break;
                case GameState.Catch:   UpdateCatch();   break;
            }
        }

        void UpdateParticles()
        {
            for (int i = particles.Count - 1; i >= 0; i--)
            {
                var p = particles[i];
                p.X  += p.VX;
                p.Y  += p.VY;
                p.VY += 0.12f;
                p.Life--;
                particles[i] = p;
                if (p.Life <= 0) particles.RemoveAt(i);
            }
        }

        void UpdateActionText()
        {
            if (actionTimer > 0)
            {
                actionTimer--;
                actionY -= 0.4f;
            }
        }

        void UpdatePetState()
        {
            if (pet.Energy <= 0)
            {
                pet.CharBob = 0;
                if (pet.State == PetState.Idle)
                    pet.State = PetState.Sad;
            }
            else
            {
                pet.BobTime += 0.1f;
                pet.CharBob  = (int)(Math.Sin(pet.BobTime) * 6);
                if (pet.State == PetState.Sad)
                    pet.State = PetState.Idle;
            }
        }
    }
}
