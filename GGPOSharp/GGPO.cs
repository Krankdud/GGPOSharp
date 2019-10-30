using System;
using System.Runtime.InteropServices;

namespace GGPOSharp
{
    public class GGPO
    {
        public const int MaxPlayers = 4;
        public const int MaxPredictionFrames = 8;
        public const int MaxSpectators = 32;
        public const int SpectatorInputInterval = 4;

        public const int InvalidHandle = -1;

        public enum PlayerType
        {
            GGPO_PLAYERTYPE_LOCAL,
            GGPO_PLAYERTYPE_REMOTE,
            GGPO_PLAYERTYPE_SPECTATOR
        }

        public enum ErrorCode
        {
            GGPO_OK = 0,
            GGPO_ERRORCODE_SUCCESS = 0,
            GGPO_ERRORCODE_GENERAL_FAILURE = -1,
            GGPO_ERRORCODE_INVALID_SESSION = 1,
            GGPO_ERRORCODE_INVALID_PLAYER_HANDLE = 2,
            GGPO_ERRORCODE_PLAYER_OUT_OF_RANGE = 3,
            GGPO_ERRORCODE_PREDICTION_THRESHOLD = 4,
            GGPO_ERRORCODE_UNSUPPORTED = 5,
            GGPO_ERRORCODE_NOT_SYNCHRONIZED = 6,
            GGPO_ERRORCODE_IN_ROLLBACK = 7,
            GGPO_ERRORCODE_INPUT_DROPPED = 8,
            GGPO_ERRORCODE_PLAYER_DISCONNECTED = 9,
            GGPO_ERRORCODE_TOO_MANY_SPECTATORS = 10,
            GGPO_ERRORCODE_INVALID_REQUEST = 11,
        }

        public enum EventCode
        {
            GGPO_EVENTCODE_CONNECTED_TO_PEER = 1000,
            GGPO_EVENTCODE_SYNCHRONIZING_WITH_PEER = 1001,
            GGPO_EVENTCODE_SYNCHRONIZED_WITH_PEER = 1002,
            GGPO_EVENTCODE_RUNNING = 1003,
            GGPO_EVENTCODE_DISCONNECTED_FROM_PEER = 1004,
            GGPO_EVENTCODE_TIMESYNC = 1005,
            GGPO_EVENTCODE_CONNECTION_INTERRUPTED = 1006,
            GGPO_EVENTCODE_CONNECTION_RESUMED = 1007,
        }

        private unsafe struct GGPOSession { }
        private unsafe GGPOSession* session;
        private GGPOSessionCallbacks callbacks;

        /// <summary>
        /// Create a new GGPO.net session.
        /// </summary>
        /// <param name="callbacks">A GGPOSessionCallbacks structure which contains the callbacks you implement
        /// to help GGPO.net synchronize the two games. You must implement all functions in the callbacks, even
        /// if they do nothing but 'return true'</param>
        /// <param name="game">The name of the game. This is used internally for GGPO for logging purposes only.</param>
        /// <param name="numPlayers">The number of players which will be in this game. The number of players per
        /// session is fixed. If you need to change the number of players or any player disconnects, you must
        /// start a new session.</param>
        /// <param name="inputSize">The size of the game inputs which will be passed to AddLocalInput.</param>
        /// <param name="localPort">The port GGPO should bind to for UDP traffic.</param>
        /// <returns></returns>
        public ErrorCode StartSession(GGPOSessionCallbacks callbacks, string game, int numPlayers, int inputSize, int localPort)
        {
            this.callbacks = callbacks;
            unsafe
            {
                fixed (GGPOSession** s = &session)
                {
                    fixed (GGPOSessionCallbacks* cb = &this.callbacks)
                    {
                        return ggpo_start_session(s, cb, game, numPlayers, inputSize, localPort);
                    }
                }
            }
        }

        /// <summary>
        /// Must be called for each player in the session (e.g. in a 3 player session, must be called 3 times).
        /// </summary>
        /// <param name="player">A GGPOPlayer struct used to describe the player.</param>
        /// <param name="handle">An out parameter to a handle used to identify this player in the future
        /// (e.g. in the on_event callbacks).</param>
        /// <returns></returns>
        public ErrorCode AddPlayer(ref GGPOPlayer player, out int handle)
        {
            unsafe
            {
                fixed (int* h = &handle)
                {
                    return ggpo_add_player(session, ref player, h);
                }
            }
        }

        /// <summary>
        /// Used to being a new GGPO.net sync test session.  During a sync test, every
        /// frame of execution is run twice: once in prediction mode and once again to
        /// verify the result of the prediction.If the checksums of your save states
        /// do not match, the test is aborted.
        /// </summary>
        /// <param name="callbacks">A GGPOSessionCallbacks structure which contains the callbacks you implement
        /// to help GGPO.net synchronize the two games. You must implement all functions in the callbacks, even
        /// if they do nothing but 'return true'</param>
        /// <param name="game">The name of the game. This is used internally for GGPO for logging purposes only.</param>
        /// <param name="numPlayers">The number of players which will be in this game. The number of players per
        /// session is fixed. If you need to change the number of players or any player disconnects, you must
        /// start a new session.</param>
        /// <param name="inputSize">The size of the game inputs which will be passed to ggpo_add_local_input.</param>
        /// <param name="frames">The number of frames to run before verifying the prediction. The recommended
        /// value is 1.</param>
        /// <returns></returns>
        public ErrorCode StartSyncTest(GGPOSessionCallbacks callbacks, string game, int numPlayers, int inputSize, int frames)
        {
            this.callbacks = callbacks;
            unsafe
            {
                fixed (GGPOSession** s = &session)
                {
                    fixed (GGPOSessionCallbacks* cb = &this.callbacks)
                    {
                        return ggpo_start_synctest(s, cb, game, numPlayers, inputSize, frames);
                    }
                }
            }
        }

        /// <summary>
        /// Start a spectator session.
        /// </summary>
        /// <param name="callbacks">A GGPOSessionCallbacks structure which contains the callbacks you implement
        /// to help GGPO.net synchronize the two games. You must implement all functions in the callbacks, even
        /// if they do nothing but 'return true'</param>
        /// <param name="game">The name of the game. This is used internally for GGPO for logging purposes only.</param>
        /// <param name="numPlayers">The number of players which will be in this game. The number of players per
        /// session is fixed. If you need to change the number of players or any player disconnects, you must
        /// start a new session.</param>
        /// <param name="inputSize">The size of the game inputs which will be passed to ggpo_add_local_input.</param>
        /// <param name="localPort">The port GGPO should bind to for UDP traffic.</param>
        /// <param name="hostIp">The IP address of the host who will serve you the inputs of the game. Any
        /// player participating in the session can serve as a host.</param>
        /// <param name="hostPort">The port of the session on the host.</param>
        /// <returns></returns>
        public ErrorCode StartSpectating(GGPOSessionCallbacks callbacks, string game, int numPlayers, int inputSize, int localPort, string hostIp, int hostPort)
        {
            this.callbacks = callbacks;
            unsafe
            {
                fixed (GGPOSession** s = &session)
                {
                    fixed (GGPOSessionCallbacks* cb = &this.callbacks)
                    {
                        return ggpo_start_spectating(s, cb, game, numPlayers, inputSize, localPort, hostIp, hostPort);
                    }
                }
            }
        }

        /// <summary>
        /// Used to close a session. You must call CloseSession to free the resources allocated
        /// in StartSession.
        /// </summary>
        /// <returns></returns>
        public ErrorCode CloseSession()
        {
            unsafe
            {
                return ggpo_close_session(session);
            }
        }

        /// <summary>
        /// Change the amount of frames ggpo will delay local input. Must be called before the
        /// first call to SynchronizeInput.
        /// </summary>
        /// <param name="playerHandle"></param>
        /// <param name="frameDelay"></param>
        /// <returns></returns>
        public ErrorCode SetFrameDelay(int playerHandle, int frameDelay)
        {
            unsafe
            {
                return ggpo_set_frame_delay(session, playerHandle, frameDelay);
            }
        }

        /// <summary>
        /// Should be called periodically by your application to give GGPO.net a chance to do some
        /// work. Most packet transmissions and rollbacks occur in Idle.
        /// </summary>
        /// <param name="timeout">The amount of time GGPO.net is allowed to spend in this function,
        /// in milliseconds.</param>
        /// <returns></returns>
        public ErrorCode Idle(int timeout)
        {
            unsafe
            {
                return ggpo_idle(session, timeout);
            }
        }

        /// <summary>
        /// Used to notify GGPO.net of inputs that should be transmitted to remote players.
        /// AddLocalInput must be called once every frame for all players of type GGPO_PLAYERTYPE_LOCAL.
        /// </summary>
        /// <param name="playerHandle">The player handle returned for this player when you called
        /// AddPlayer</param>
        /// <param name="values">The controller inputs for this player</param>
        /// <param name="size">The size of the controller inputs. This must be exactly equal to the
        /// size passed into StartSession</param>
        /// <returns></returns>
        public ErrorCode AddLocalInput(int playerHandle, IntPtr values, int size)
        {
            unsafe
            {
                return ggpo_add_local_input(session, playerHandle, (void*) values, size);
            }
        }

        /// <summary>
        /// Used to notify GGPO.net of inputs that should be transmitted to remote players.
        /// AddLocalInput must be called once every frame for all players of type GGPO_PLAYERTYPE_LOCAL.
        /// </summary>
        /// <param name="playerHandle">The player handle returned for this player when you called
        /// AddPlayer</param>
        /// <param name="values">The controller inputs for this player. The size of the array must be
        /// exactly equal to the size passed into StartSession</param>
        /// <returns></returns>
        public ErrorCode AddLocalInput(int playerHandle, byte[] values)
        {
            unsafe
            {
                fixed (byte* p = values)
                {
                    return AddLocalInput(playerHandle, (IntPtr)p, values.Length * sizeof(byte));
                }
            }
        }

        /// <summary>
        /// You should call SynchronizeInput before every frame of execution, including those
        /// frames which happen during rollback.
        /// </summary>
        /// <param name="values">When the function returns, the values parameter will contain
        /// inputs for this frame for all players. The values array must be at least
        /// (size * players) large.</param>
        /// <param name="size">The size of the values array.</param>
        /// <param name="disconnectFlags">Indicated whether the input in slot (1 << flag) is valid.
        /// If a player has disconnected, the input in the values array for that player will be
        /// zeroed and the i-th flag will be set. For example, if only player 3 has disconnected,
        /// disconnect flags will be 8 (i.e. 1 << 3)</param>
        /// <returns></returns>
        public ErrorCode SynchronizeInput(IntPtr values, int size, out int disconnectFlags)
        {
            unsafe
            {
                fixed (int* flags = &disconnectFlags)
                {
                    return ggpo_synchronize_input(session, (void*)values, size, flags);
                }
            }
        }
        
        /// <summary>
        /// You should call SynchronizeInput before every frame of execution, including those
        /// frames which happen during rollback.
        /// </summary>
        /// <param name="values">When the function returns, the values parameter will contain
        /// inputs for this frame for all players. The values array must be at least
        /// (size * players) large.</param>
        /// <param name="disconnectFlags">Indicated whether the input in slot (1 << flag) is valid.
        /// If a player has disconnected, the input in the values array for that player will be
        /// zeroed and the i-th flag will be set. For example, if only player 3 has disconnected,
        /// disconnect flags will be 8 (i.e. 1 << 3)</param>
        /// <returns></returns>
        public ErrorCode SynchronizeInput(byte[] values, out int disconnectFlags)
        {
            unsafe
            {
                fixed (byte* p = values)
                {
                    return SynchronizeInput((IntPtr)p, values.Length * sizeof(byte), out disconnectFlags);
                }
            }
        }

        /// <summary>
        /// Disconnects a remote player from a game. Will return GGPO_ERRORCODE_PLAYER_DISCONNECTED
        /// if you try to disconnect a player who has already been disconnected.
        /// </summary>
        /// <param name="playerHandle"></param>
        /// <returns></returns>
        public ErrorCode DisconnectPlayer(int playerHandle)
        {
            unsafe
            {
                return ggpo_disconnect_player(session, playerHandle);
            }
        }

        /// <summary>
        /// You should call AdvanceFrame to notify GGPO.net that you have
        /// advanced your gamestate by a single frame.You should call this everytime
        /// you advance the gamestate by a frame, even during rollbacks.GGPO.net
        /// may call your save_state callback before this function returns.
        /// </summary>
        /// <returns></returns>
        public ErrorCode AdvanceFrame()
        {
            unsafe
            {
                return ggpo_advance_frame(session);
            }
        }

        /// <summary>
        /// Used to fetch some statistics about the quality of the network connection.
        /// </summary>
        /// <param name="playerHandle">The player handle returned from the AddPlayer function
        /// you used to add the remote player.</param>
        /// <param name="stats">Out parameter to the network statistics.</param>
        /// <returns></returns>
        public ErrorCode GetNetworkStats(int playerHandle, out GGPONetworkStats stats)
        {
            unsafe
            {
                return ggpo_get_network_stats(session, playerHandle, out stats);
            }
        }

        /// <summary>
        /// Sets the disconnect timeout.  The session will automatically disconnect
        /// from a remote peer if it has not received a packet in the timeout window.
        /// You will be notified of the disconnect via a GGPO_EVENTCODE_DISCONNECTED_FROM_PEER
        /// event.
        /// 
        /// Setting a timeout value of 0 will disable automatic disconnects.
        /// </summary>
        /// <param name="timeout">The time in milliseconds to wait before disconnecting a peer.</param>
        /// <returns></returns>
        public ErrorCode SetDisconnectTimeout(int timeout)
        {
            unsafe
            {
                return ggpo_set_disconnect_timeout(session, timeout);
            }
        }

        /// <summary>
        /// The time to wait before the first GGPO_EVENTCODE_NETWORK_INTERRUPTED timeout
        /// will be sent.
        /// </summary>
        /// <param name="timeout">The amount of time which needs to elapse without receiving
        /// a packet before the GGPO_EVENTCODE_NETWORK_INTERRUPTED event is sent.</param>
        /// <returns></returns>
        public ErrorCode SetDisconnectNotifyStart(int timeout)
        {
            unsafe
            {
                return ggpo_set_disconnect_notify_start(session, timeout);
            }
        }

        /// <summary>
        /// Utility function used to check if a method succeeded.
        /// </summary>
        /// <param name="result">ErrorCode returned from the called method.</param>
        /// <returns></returns>
        public static bool Succeeded(ErrorCode result)
        {
            return result == ErrorCode.GGPO_OK;
        }

        /// <summary>
        /// Utility function for creating a local player. Sets the type and size of the GGPOPlayer
        /// struct automatically.
        /// </summary>
        /// <param name="playerNum">The player number. Should be between 1 and the number of players in
        /// the game.</param>
        /// <returns></returns>
        public static GGPOPlayer CreateLocalPlayer(int playerNum)
        {
            GGPOPlayer player = new GGPOPlayer()
            {
                type = PlayerType.GGPO_PLAYERTYPE_LOCAL,
                playerNum = playerNum,
            };
            player.size = Marshal.SizeOf(player);
            return player;
        }

        /// <summary>
        /// Utility function for creating a remote player. Sets the type and size of the GGPOPlayer
        /// struct automatically.
        /// </summary>
        /// <param name="playerNum">The player number. Should be between 1 and the number of players in
        /// the game.</param>
        /// <param name="ipAddress">IP address of the remote player.</param>
        /// <param name="port">The port where UDP packets should be sent to reach this player.</param>
        /// <returns></returns>
        public static GGPOPlayer CreateRemotePlayer(int playerNum, string ipAddress, short port)
        {
            GGPOPlayer player = new GGPOPlayer()
            {
                type = PlayerType.GGPO_PLAYERTYPE_REMOTE,
                playerNum = playerNum,
                ipAddress = ipAddress,
                port = port,
            };
            player.size = Marshal.SizeOf(player);
            return player;
        }

        /// <summary>
        /// Utility function to create a GGPOSessionCallbacks struct with function pointers to an
        /// object that implements the IGGPOSession interface.
        /// </summary>
        /// <param name="callbacks">Object implementing the games callbacks.</param>
        /// <returns></returns>
        public static GGPOSessionCallbacks CreateSessionCallbacks(IGGPOSession callbacks)
        {
            return new GGPOSessionCallbacks()
            {
                AdvanceFrame = Marshal.GetFunctionPointerForDelegate(new AdvanceFrame(callbacks.AdvanceFrame)),
                BeginGame = Marshal.GetFunctionPointerForDelegate(new BeginGame(callbacks.BeginGame)),
                FreeBuffer = Marshal.GetFunctionPointerForDelegate(new FreeBuffer(callbacks.FreeBuffer)),
                LoadGameState = Marshal.GetFunctionPointerForDelegate(new LoadGameState(callbacks.LoadGameState)),
                LogGameState = Marshal.GetFunctionPointerForDelegate(new LogGameState(callbacks.LogGameState)),
                OnEvent = Marshal.GetFunctionPointerForDelegate(new OnEvent(callbacks.OnEvent)),
                SaveGameState = Marshal.GetFunctionPointerForDelegate(new SaveGameState(callbacks.SaveGameState)),
            };
        }

        [DllImport("GGPO", CallingConvention = CallingConvention.Cdecl)]
        private static unsafe extern ErrorCode ggpo_start_session(GGPOSession** session,
                                                                  GGPOSessionCallbacks* cb,
                                                                  [MarshalAs(UnmanagedType.LPStr)] string game,
                                                                  int numPlayers,
                                                                  int inputSize,
                                                                  int localPort);

        [DllImport("GGPO", CallingConvention = CallingConvention.Cdecl)]
        private static unsafe extern ErrorCode ggpo_add_player(GGPOSession* session,
                                                               ref GGPOPlayer player,
                                                               int* handle);

        [DllImport("GGPO", CallingConvention = CallingConvention.Cdecl)]
        private static unsafe extern ErrorCode ggpo_start_synctest(GGPOSession** session,
                                                                   GGPOSessionCallbacks* cb,
                                                                   [MarshalAs(UnmanagedType.LPStr)] string game,
                                                                   int numPlayers,
                                                                   int inputSize,
                                                                   int frames);

        [DllImport("GGPO", CallingConvention = CallingConvention.Cdecl)]
        private static unsafe extern ErrorCode ggpo_start_spectating(GGPOSession** session,
                                                                     GGPOSessionCallbacks* cb,
                                                                     [MarshalAs(UnmanagedType.LPStr)] string game,
                                                                     int numPlayers,
                                                                     int inputSize,
                                                                     int localPort,
                                                                     [MarshalAs(UnmanagedType.LPStr)] string hostIp,
                                                                     int hostPort);

        [DllImport("GGPO", CallingConvention = CallingConvention.Cdecl)]
        private static unsafe extern ErrorCode ggpo_close_session(GGPOSession* session);

        [DllImport("GGPO", CallingConvention = CallingConvention.Cdecl)]
        private static unsafe extern ErrorCode ggpo_set_frame_delay(GGPOSession* session,
                                                                    int player,
                                                                    int frameDelay);

        [DllImport("GGPO", CallingConvention = CallingConvention.Cdecl)]
        private static unsafe extern ErrorCode ggpo_idle(GGPOSession* session,
                                                         int timeout);

        [DllImport("GGPO", CallingConvention = CallingConvention.Cdecl)]
        private static unsafe extern ErrorCode ggpo_add_local_input(GGPOSession* session,
                                                                    int player,
                                                                    void* values,
                                                                    int size);

        [DllImport("GGPO", CallingConvention = CallingConvention.Cdecl)]
        private static unsafe extern ErrorCode ggpo_synchronize_input(GGPOSession* session,
                                                                      void* values,
                                                                      int size,
                                                                      int* disconnectFlags);

        [DllImport("GGPO", CallingConvention = CallingConvention.Cdecl)]
        private static unsafe extern ErrorCode ggpo_disconnect_player(GGPOSession* session,
                                                                      int player);

        [DllImport("GGPO", CallingConvention = CallingConvention.Cdecl)]
        private static unsafe extern ErrorCode ggpo_advance_frame(GGPOSession* session);

        [DllImport("GGPO", CallingConvention = CallingConvention.Cdecl)]
        private static unsafe extern ErrorCode ggpo_get_network_stats(GGPOSession* session,
                                                                      int player,
                                                                      out GGPONetworkStats stats);

        [DllImport("GGPO", CallingConvention = CallingConvention.Cdecl)]
        private static unsafe extern ErrorCode ggpo_set_disconnect_timeout(GGPOSession* session,
                                                                           int timeout);

        [DllImport("GGPO", CallingConvention = CallingConvention.Cdecl)]
        private static unsafe extern ErrorCode ggpo_set_disconnect_notify_start(GGPOSession* session,
                                                                                int timeout);
    }
}
