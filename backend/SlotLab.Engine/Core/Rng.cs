using System;
using System.Runtime.InteropServices;

namespace SlotLab.Engine.Core
{
    public sealed class Rng : IDisposable
    {
        private const string LIB_NAME = "libpcg_rng.so";

        // --- Native API (instantiable C++ layer) ---
        [DllImport(LIB_NAME, EntryPoint = "pcg_create", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr pcg_create(ulong seed, ulong seq);

        [DllImport(LIB_NAME, EntryPoint = "pcg_destroy", CallingConvention = CallingConvention.Cdecl)]
        private static extern void pcg_destroy(IntPtr rng);

        [DllImport(LIB_NAME, EntryPoint = "pcg32", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint pcg32(IntPtr rng);

        [DllImport(LIB_NAME, EntryPoint = "pcg_normalized", CallingConvention = CallingConvention.Cdecl)]
        private static extern double pcg_normalized(IntPtr rng);

        [DllImport(LIB_NAME, EntryPoint = "pcg_between", CallingConvention = CallingConvention.Cdecl)]
        private static extern int pcg_between(IntPtr rng, int min, int max, bool incMin, bool incMax);

        [DllImport(LIB_NAME, EntryPoint = "pcg_between_u32", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint pcg_between_u32(IntPtr rng, uint min, uint max, bool incMin, bool incMax);

        [DllImport(LIB_NAME, EntryPoint = "pcg_between_float", CallingConvention = CallingConvention.Cdecl)]
        private static extern double pcg_between_float(IntPtr rng, double min, double max);

        [DllImport(LIB_NAME, EntryPoint = "pcg_bool", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool pcg_bool(IntPtr rng);

        // --- Instance state ---
        private IntPtr _handle;
        private bool _disposed;

        public Rng(ulong? seed = null, ulong sequence = 1)
        {
            ulong actualSeed = seed ?? (ulong)DateTime.UtcNow.Ticks;
            _handle = pcg_create(actualSeed, sequence);

            if (_handle == IntPtr.Zero)
                throw new InvalidOperationException("Failed to create RNG instance.");

            Console.WriteLine($"🎲 RNG instance created (seed = {actualSeed}, seq = {sequence})");
        }

        // --- Public API ---
        public uint NextUInt32() => pcg32(_handle);
        public double NextNormalized() => pcg_normalized(_handle);

        public int NextIntBetween(int min, int max, bool inclusiveMin = true, bool inclusiveMax = true)
            => pcg_between(_handle, min, max, inclusiveMin, inclusiveMax);

        public uint NextUIntBetween(uint min, uint max, bool inclusiveMin = true, bool inclusiveMax = true)
            => pcg_between_u32(_handle, min, max, inclusiveMin, inclusiveMax);

        public double NextFloatBetween(double min, double max)
            => pcg_between_float(_handle, min, max);

        public bool NextBool()
            => pcg_bool(_handle);

        // --- Cleanup ---
        public void Dispose()
        {
            if (!_disposed)
            {
                pcg_destroy(_handle);
                _handle = IntPtr.Zero;
                _disposed = true;
                GC.SuppressFinalize(this);
            }
        }

        ~Rng() => Dispose();
    }
}
