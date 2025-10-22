using System;
using System.Runtime.InteropServices;

namespace SlotLab.Engine.Core
{
    public static class Rng
    {
        // 🔹 Nom exacte del teu fitxer natiu (ja tens build final)
        private const string LIB_NAME = "libpcg_rng.so";

        // --- Signatures natives (C) ---

        // Seeding / state
        [DllImport(LIB_NAME, EntryPoint = "set_seed", CallingConvention = CallingConvention.Cdecl)]
        private static extern void set_seed(ulong seed);

        [DllImport(LIB_NAME, EntryPoint = "reset_rng", CallingConvention = CallingConvention.Cdecl)]
        private static extern void reset_rng(ulong seed);

        [DllImport(LIB_NAME, EntryPoint = "get_state", CallingConvention = CallingConvention.Cdecl)]
        private static extern ulong get_state();

        [DllImport(LIB_NAME, EntryPoint = "set_state", CallingConvention = CallingConvention.Cdecl)]
        private static extern void set_state(ulong state);

        // Core
        [DllImport(LIB_NAME, EntryPoint = "pcg32", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint pcg32();

        [DllImport(LIB_NAME, EntryPoint = "pcg_normalized", CallingConvention = CallingConvention.Cdecl)]
        private static extern double pcg_normalized();

        // Helpers
        [DllImport(LIB_NAME, EntryPoint = "pcg_between", CallingConvention = CallingConvention.Cdecl)]
        private static extern int pcg_between(int min, int max, bool inc_min, bool inc_max);

        [DllImport(LIB_NAME, EntryPoint = "pcg_between_u32", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint pcg_between_u32(uint min, uint max, bool inc_min, bool inc_max);

        [DllImport(LIB_NAME, EntryPoint = "pcg_between_float", CallingConvention = CallingConvention.Cdecl)]
        private static extern double pcg_between_float(double min, double max);

        [DllImport(LIB_NAME, EntryPoint = "pcg_bool", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool pcg_bool();


        // --- Capa C# “friendly” ---

        private static bool _initialized = false;

        public static void Initialize(ulong? seed = null)
        {
            if (_initialized) return;

            ulong actualSeed = seed ?? (ulong)DateTime.UtcNow.Ticks;
            set_seed(actualSeed);
            _initialized = true;

            Console.WriteLine($"🎲 PCG RNG inicialitzat (seed = {actualSeed})");
        }

        public static void Reset(ulong seed)
        {
            reset_rng(seed);
        }

        public static ulong GetState() => get_state();
        public static void SetState(ulong state) => set_state(state);

        // Core values
        public static uint NextUInt32()
        {
            if (!_initialized) Initialize();
            return pcg32();
        }

        public static double NextNormalized()
        {
            if (!_initialized) Initialize();
            return pcg_normalized();
        }

        // Helpers
        public static int NextIntBetween(int min, int max, bool inclusiveMin = true, bool inclusiveMax = true)
        {
            if (!_initialized) Initialize();
            return pcg_between(min, max, inclusiveMin, inclusiveMax);
        }

        public static uint NextUIntBetween(uint min, uint max, bool inclusiveMin = true, bool inclusiveMax = true)
        {
            if (!_initialized) Initialize();
            return pcg_between_u32(min, max, inclusiveMin, inclusiveMax);
        }

        public static double NextFloatBetween(double min, double max)
        {
            if (!_initialized) Initialize();
            return pcg_between_float(min, max);
        }

        public static bool NextBool()
        {
            if (!_initialized) Initialize();
            return pcg_bool();
        }
    }
}
