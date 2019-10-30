using System.Runtime.InteropServices;

namespace GGPOSharp
{
    [StructLayout(LayoutKind.Sequential)]
    public struct GGPONetworkStats
    {
        public Network network;
        public TimeSync timesync;

        [StructLayout(LayoutKind.Sequential)]
        public struct Network
        {
            public int sendQueueLen;
            public int recvQueueLen;
            public int ping;
            public int kbpsSent;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct TimeSync
        {
            public int localFramesBehind;
            public int remoteFramesBehind;
        }
    }
}
