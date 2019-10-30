using System;
using System.Runtime.InteropServices;

namespace GGPOSharp
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool BeginGame(string game);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool SaveGameState(ref byte[] buffer, ref int len, ref int checksum, int frame);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool LoadGameState(byte[] buffer, int len);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool LogGameState(string filename, byte[] buffer, int len);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FreeBuffer(IntPtr buffer);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool AdvanceFrame(int flags);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool OnEvent(ref GGPOEvent info);

    [StructLayout(LayoutKind.Sequential)]
    public struct GGPOSessionCallbacks
    {
        public IntPtr BeginGame;
        public IntPtr SaveGameState;
        public IntPtr LoadGameState;
        public IntPtr LogGameState;
        public IntPtr FreeBuffer;
        public IntPtr AdvanceFrame;
        public IntPtr OnEvent;
    }
}
