using Xunit;
using GGPOSharpTests;
using GGPOSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace GGPOsharp.Tests
{
    public class GGPOTests
    {
        Session session;
        GGPO ggpo;

        public GGPOTests()
        {
            session = new Session();
            ggpo = new GGPO();
        }

        [Fact()]
        public void StartSessionTest()
        {
            GGPO.ErrorCode result = ggpo.StartSession(session.CreateCallbacks(), "GGPOSharp", 2, 8, 10600);
            Assert.True(GGPO.Succeeded(result), String.Format("Session was not started successfully: {0}", result));
        }

        [Fact()]
        public void AddLocalPlayerTest()
        {
            ggpo.StartSession(session.CreateCallbacks(), "GGPOSharp", 2, 8, 10600);
            
            int handle;
            GGPOPlayer player = GGPO.CreateLocalPlayer(1);
            GGPO.ErrorCode result = ggpo.AddPlayer(ref player, out handle);
            Assert.True(GGPO.Succeeded(result), String.Format("Could not add local player: {0}", result));
        }

        [Fact()]
        public void AddRemotePlayerTest()
        {
            ggpo.StartSession(session.CreateCallbacks(), "GGPOSharp", 2, 8, 10600);
            
            int handle;
            GGPOPlayer player = GGPO.CreateRemotePlayer(1, "127.0.0.1", 10600);
            GGPO.ErrorCode result = ggpo.AddPlayer(ref player, out handle);
            Assert.True(GGPO.Succeeded(result), String.Format("Could not add remote player: {0}", result));
        }

        [Fact()]
        public void StartSyncTestTest()
        {
            GGPO.ErrorCode result = ggpo.StartSyncTest(session.CreateCallbacks(), "GGPOSharp", 2, 8, 1);
            Assert.True(GGPO.Succeeded(result), String.Format("Sync test was not started successfully: {0}", result));
        }

        [Fact()]
        public void StartSpectatingTest()
        {
            GGPO.ErrorCode result = ggpo.StartSpectating(session.CreateCallbacks(), "GGPOSharp", 2, 8, 10601, "127.0.0.1", 10600);
            Assert.True(GGPO.Succeeded(result), String.Format("Spectating was not started successfully: {0}", result));
        }

        [Fact()]
        public void CloseSessionTest()
        {
            ggpo.StartSession(session.CreateCallbacks(), "GGPOSharp", 2, 8, 10600);
            GGPO.ErrorCode result = ggpo.CloseSession();
            Assert.True(GGPO.Succeeded(result), String.Format("Session was not properly closed: {0}", result));
        }

        [Fact()]
        public void SetFrameDelayTest()
        {
            ggpo.StartSession(session.CreateCallbacks(), "GGPOSharp", 2, 8, 10600);

            int handle;
            GGPOPlayer player = GGPO.CreateLocalPlayer(1);
            ggpo.AddPlayer(ref player, out handle);

            GGPO.ErrorCode result = ggpo.SetFrameDelay(handle, 3);
            Assert.True(GGPO.Succeeded(result), String.Format("Could not set frame delay: {0}", result));
        }

        [Fact()]
        public void IdleTest()
        {
            ggpo.StartSession(session.CreateCallbacks(), "GGPOSharp", 2, 8, 10600);
            GGPO.ErrorCode result = ggpo.Idle(100);
            Assert.True(GGPO.Succeeded(result), String.Format("Could not idle: {0}", result));
        }

        [Fact()]
        public void AddLocalInputTest()
        {
            ggpo.StartSession(session.CreateCallbacks(), "GGPOSharp", 2, 8, 10600);

            int[] handles = new int[2];
            GGPOPlayer player1 = GGPO.CreateLocalPlayer(1);
            ggpo.AddPlayer(ref player1, out handles[0]);

            GGPOPlayer player2 = GGPO.CreateRemotePlayer(2, "127.0.0.1", 10600);
            ggpo.AddPlayer(ref player2, out handles[1]);

            ggpo.Idle(100);

            byte[] inputs = {8, 0, 0, 0, 0, 0, 0, 0};
            GGPO.ErrorCode result = ggpo.AddLocalInput(handles[0], inputs);
            Assert.True(GGPO.Succeeded(result), String.Format("Could not add local input: {0}", result));
        }

        [Fact()]
        public void SynchronizeInputTest()
        {
            ggpo.StartSession(session.CreateCallbacks(), "GGPOSharp", 2, 8, 10600);

            int[] handles = new int[2];
            GGPOPlayer player1 = GGPO.CreateLocalPlayer(1);
            ggpo.AddPlayer(ref player1, out handles[0]);

            GGPOPlayer player2 = GGPO.CreateRemotePlayer(2, "127.0.0.1", 10600);
            ggpo.AddPlayer(ref player2, out handles[1]);

            ggpo.Idle(500);

            byte[] inputs = {8, 0, 0, 0, 0, 0, 0, 0};
            ggpo.AddLocalInput(handles[0], inputs);

            byte[] syncrhonized = new byte[16];
            GGPO.ErrorCode result = ggpo.SynchronizeInput(syncrhonized, out _);
            Assert.True(GGPO.Succeeded(result), String.Format("Could not synchronize inputs: {0}", result));
        }

        [Fact()]
        public void DisconnectPlayerTest()
        {
            Assert.True(false, "TODO: Fix this test");

            ggpo.StartSession(session.CreateCallbacks(), "GGPOSharp", 2, 8, 10600);

            int[] handles = new int[2];
            GGPOPlayer player1 = GGPO.CreateLocalPlayer(1);
            ggpo.AddPlayer(ref player1, out handles[0]);

            GGPOPlayer player2 = GGPO.CreateRemotePlayer(2, "127.0.0.1", 10600);
            ggpo.AddPlayer(ref player2, out handles[1]);
            
            ggpo.Idle(500);
            ggpo.AdvanceFrame();

            GGPO.ErrorCode result = ggpo.DisconnectPlayer(handles[1]);
            Assert.True(GGPO.Succeeded(result), String.Format("Could not disconnect the player: {0}", result));
        }

        [Fact()]
        public void AdvanceFrameTest()
        {
            ggpo.StartSession(session.CreateCallbacks(), "GGPOSharp", 2, 8, 10600);

            int[] handles = new int[2];
            GGPOPlayer player1 = GGPO.CreateLocalPlayer(1);
            ggpo.AddPlayer(ref player1, out handles[0]);

            GGPOPlayer player2 = GGPO.CreateRemotePlayer(2, "127.0.0.1", 10600);
            ggpo.AddPlayer(ref player2, out handles[1]);
            
            ggpo.Idle(500);
            
            GGPO.ErrorCode result = ggpo.AdvanceFrame();
            Assert.True(GGPO.Succeeded(result), String.Format("Could not advance frame: {0}", result));
        }

        [Fact()]
        public void GetNetworkStatsTest()
        {
            ggpo.StartSession(session.CreateCallbacks(), "GGPOSharp", 2, 8, 10600);

            int[] handles = new int[2];
            GGPOPlayer player1 = GGPO.CreateLocalPlayer(1);
            ggpo.AddPlayer(ref player1, out handles[0]);

            GGPOPlayer player2 = GGPO.CreateRemotePlayer(2, "127.0.0.1", 10600);
            ggpo.AddPlayer(ref player2, out handles[1]);
            
            ggpo.Idle(500);

            GGPONetworkStats stats;
            GGPO.ErrorCode result = ggpo.GetNetworkStats(handles[1], out stats);
            Assert.True(GGPO.Succeeded(result), String.Format("Could not get network stats: {0}", result));
        }

        [Fact()]
        public void SetDisconnectTimeoutTest()
        {
            ggpo.StartSession(session.CreateCallbacks(), "GGPOSharp", 2, 8, 10600);
            GGPO.ErrorCode result = ggpo.SetDisconnectTimeout(5000);
            Assert.True(GGPO.Succeeded(result), String.Format("Could not set disconnect timeout: {0}", result));
        }

        [Fact()]
        public void SetDisconnectNotifyStartTest()
        {
            ggpo.StartSession(session.CreateCallbacks(), "GGPOSharp", 2, 8, 10600);
            GGPO.ErrorCode result = ggpo.SetDisconnectNotifyStart(5000);
            Assert.True(GGPO.Succeeded(result), String.Format("Could not set disconnect notify start: {0}", result));
        }

        [Fact()]
        public void SucceededTest()
        {
            Assert.True(GGPO.Succeeded(GGPO.ErrorCode.GGPO_ERRORCODE_SUCCESS));
            Assert.True(GGPO.Succeeded(GGPO.ErrorCode.GGPO_OK));
            Assert.False(GGPO.Succeeded(GGPO.ErrorCode.GGPO_ERRORCODE_GENERAL_FAILURE));
            Assert.False(GGPO.Succeeded(GGPO.ErrorCode.GGPO_ERRORCODE_INVALID_SESSION));
            Assert.False(GGPO.Succeeded(GGPO.ErrorCode.GGPO_ERRORCODE_INVALID_PLAYER_HANDLE));
            Assert.False(GGPO.Succeeded(GGPO.ErrorCode.GGPO_ERRORCODE_PLAYER_OUT_OF_RANGE));
            Assert.False(GGPO.Succeeded(GGPO.ErrorCode.GGPO_ERRORCODE_PREDICTION_THRESHOLD));
            Assert.False(GGPO.Succeeded(GGPO.ErrorCode.GGPO_ERRORCODE_UNSUPPORTED));
            Assert.False(GGPO.Succeeded(GGPO.ErrorCode.GGPO_ERRORCODE_NOT_SYNCHRONIZED));
            Assert.False(GGPO.Succeeded(GGPO.ErrorCode.GGPO_ERRORCODE_IN_ROLLBACK));
            Assert.False(GGPO.Succeeded(GGPO.ErrorCode.GGPO_ERRORCODE_INPUT_DROPPED));
            Assert.False(GGPO.Succeeded(GGPO.ErrorCode.GGPO_ERRORCODE_PLAYER_DISCONNECTED));
            Assert.False(GGPO.Succeeded(GGPO.ErrorCode.GGPO_ERRORCODE_TOO_MANY_SPECTATORS));
            Assert.False(GGPO.Succeeded(GGPO.ErrorCode.GGPO_ERRORCODE_INVALID_REQUEST));
        }

        [Fact()]
        public void CreateLocalPlayerTest()
        {
            GGPOPlayer player = GGPO.CreateLocalPlayer(1);
            Assert.True(player.type == GGPO.PlayerType.GGPO_PLAYERTYPE_LOCAL);
            Assert.True(player.playerNum == 1);
        }

        [Fact()]
        public void CreateRemotePlayerTest()
        {
            GGPOPlayer player = GGPO.CreateRemotePlayer(2, "127.0.0.1", 10600);
            Assert.True(player.type == GGPO.PlayerType.GGPO_PLAYERTYPE_REMOTE);
            Assert.True(player.playerNum == 2);
            Assert.Equal("127.0.0.1", player.ipAddress);
            Assert.True(player.port == 10600);
        }
    }
}