using System;
using System.Drawing;

namespace TalkingPete
{
    public struct HotSpot
    {
        public Rectangle Bounds;
        public string    Label;
        public string    Icon;
        public int       Cost;   // Затраты энергии
        public int       Gain;   // Восстановление энергии
        public Action    OnClick;
    }
}
