namespace TalkingPete
{
    public class PetModel
    {
        public float Energy    { get; set; } = 100f;
        public float MaxEnergy { get; set; } = 100f;
        public int   XP        { get; set; } = 0;
        public int   Level     { get; set; } = 1;
        public int   XpNext    { get; set; } = 100;
        public float Mood      { get; set; } = 80f;
        public PetState State  { get; set; } = PetState.Idle;
        public float BobTime   { get; set; } = 0f;
        public int   CharBob   { get; set; } = 0;
    }
}
