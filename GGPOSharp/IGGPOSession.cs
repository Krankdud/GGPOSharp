using System;

namespace GGPOSharp
{
    /// <summary>
    /// Interface that contains all of the methods that need to be implemented as callback
    /// functions for GGPO.
    /// </summary>
    public interface IGGPOSession
    {
        /// <summary>
        /// This callback has been deprecated. You must implement it, but should ignore the
        /// game parameter.
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        bool BeginGame(string game);
        /// <summary>
        /// The client should allocate a buffer, copy the entire contents of the current game
        /// state into it, and copy the length into the len parameter. Optionally, the client
        /// can compute a checksum of the data and store it in the checksum argument.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="len"></param>
        /// <param name="checksum"></param>
        /// <param name="frame"></param>
        /// <returns></returns>
        bool SaveGameState(ref byte[] buffer, ref int len, ref int checksum, int frame);
        /// <summary>
        /// GGPO.net will call this function at the beginning
        /// of a rollback.The buffer and len parameters contain a previously
        /// saved state returned from the save_game_state function.The client
        /// should make the current game state match the state contained in the
        /// buffer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        bool LoadGameState(byte[] buffer, int len);
        /// <summary>
        /// Used in diagnostic testing.  The client should use
        /// the ggpo_log function to write the contents of the specified save
        /// state in a human readible form.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="buffer"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        bool LogGameState(string filename, byte[] buffer, int len);
        /// <summary>
        /// Frees a game state allocated in SaveGameState.  You
        /// should deallocate the memory contained in the buffer.
        /// </summary>
        /// <param name="buffer"></param>
        void FreeBuffer(IntPtr buffer);
        /// <summary>
        /// Called during a rollback.  You should advance your game
        /// state by exactly one frame. Before each frame, call SynchronizeInput
        /// to retrieve the inputs you should use for that frame. After each frame,
        /// you should call AdvanceFrame to notify GGPO.net that you're
        /// finished.
        /// 
        /// The flags parameter is reserved.It can safely be ignored at this time.
        /// </summary>
        /// <param name="flags"></param>
        /// <returns></returns>
        bool AdvanceFrame(int flags);
        /// <summary>
        /// Notification that something has happened.  See the GGPOEventCode
        /// structure above for more information.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        bool OnEvent(ref GGPOEvent info);
    }
}
