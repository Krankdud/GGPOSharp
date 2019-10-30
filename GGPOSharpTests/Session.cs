using GGPOSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace GGPOSharpTests
{
    class Session : IGGPOSession
    {
        public GGPOSessionCallbacks CreateCallbacks()
        {
            return new GGPOSessionCallbacks()
            {
                AdvanceFrame = Marshal.GetFunctionPointerForDelegate(new AdvanceFrame(AdvanceFrame)),
                BeginGame = Marshal.GetFunctionPointerForDelegate(new BeginGame(BeginGame)),
                FreeBuffer = Marshal.GetFunctionPointerForDelegate(new FreeBuffer(FreeBuffer)),
                LoadGameState = Marshal.GetFunctionPointerForDelegate(new LoadGameState(LoadGameState)),
                LogGameState = Marshal.GetFunctionPointerForDelegate(new LogGameState(LogGameState)),
                OnEvent = Marshal.GetFunctionPointerForDelegate(new OnEvent(OnEvent)),
                SaveGameState = Marshal.GetFunctionPointerForDelegate(new SaveGameState(SaveGameState)),
            };
        }

        public bool AdvanceFrame(int flags)
        {
            return true;
        }

        public bool BeginGame(string game)
        {
            return true;
        }

        public void FreeBuffer(IntPtr buffer)
        {
            return;
        }

        public bool LoadGameState(byte[] buffer, int len)
        {
            return true;
        }

        public bool LogGameState(string filename, byte[] buffer, int len)
        {
            return true;
        }

        public bool OnEvent(ref GGPOEvent info)
        {
            return true;
        }

        public bool SaveGameState(ref byte[] buffer, ref int len, ref int checksum, int frame)
        {
            return true;
        }
    }
}
