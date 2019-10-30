using System.Runtime.InteropServices;

namespace GGPOSharp
{
    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
    public struct GGPOPlayer
    {
        public int size;
        public GGPO.PlayerType type;
        public int playerNum;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=32)] public string ipAddress;
        public short port;
    }

}
