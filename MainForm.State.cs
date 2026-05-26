using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TalkingPete
{
    public partial class MainForm
    {
        // ── питомец ─────────────────────────────────────────────────────
        readonly PetModel pet = new();

        // ── сцена ───────────────────────────────────────────────────────
        Room      curRoom  = Room.Living;
        GameState curGame  = GameState.Normal;
        int       animFrame = 0;

        // ── всплывающий текст действия ──────────────────────────────────
        string lastAction  = "Привет!";
        int    actionTimer = 120;
        float  actionY     = 240f;

        // ── частицы ─────────────────────────────────────────────────────
        readonly List<Particle> particles = new();

        // ── горячие зоны ────────────────────────────────────────────────
        readonly List<HotSpot> hotSpots = new();

        // ── UI-контролы ─────────────────────────────────────────────────
        Button[] navBtns = Array.Empty<Button>();
        Button   exitBtn = null!;
        Bitmap   backBuf = null!;

        // ── таймеры ─────────────────────────────────────────────────────
        Timer loopTmr  = null!;
        Timer animTmr  = null!;
        Timer drainTmr = null!;

        readonly Random rnd = new();
    }
}
