using System.Runtime.InteropServices;

namespace GGPOSharp
{
    [StructLayout(LayoutKind.Explicit)]
    public struct GGPOEvent
    {
        [FieldOffset(0)] public GGPO.EventCode code;
        [FieldOffset(4)] public Connected connected;
        [FieldOffset(4)] public Synchronizing synchronizing;
        [FieldOffset(4)] public Disconnected disconnected;
        [FieldOffset(4)] public TimeSync timeSync;
        [FieldOffset(4)] public ConnectionInterrupted connectionInterrupted;
        [FieldOffset(4)] public ConnectionResumed connectionResumed;

        [StructLayout(LayoutKind.Sequential)]
        public struct Connected
        {
            public int player;
        }
    
        [StructLayout(LayoutKind.Sequential)]
        public struct Synchronizing
        {
            public int player;
            public int count;
            public int total;
        }
    
        [StructLayout(LayoutKind.Sequential)]
        public struct Disconnected
        {
            public int player;
        }
    
        [StructLayout(LayoutKind.Sequential)]
        public struct TimeSync
        {
            public int framesAhead;
        }
    
        [StructLayout(LayoutKind.Sequential)]
        public struct ConnectionInterrupted
        {
            public int player;
            public int disconnectTimeout;
        }
    
        [StructLayout(LayoutKind.Sequential)]
        public struct ConnectionResumed
        {
            public int player;
        }
    }

}
