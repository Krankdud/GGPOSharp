# GGPO

The GGPO class is the main interface between your code and GGPO.

---

```C#
public GGPO.ErrorCode StartSession(GGPOSessionCallbacks callbacks, string game, int numPlayers, int inputSize, int localPort)
```
Creates a new GGPO session. 

Parameters:
* `callbacks` - A `GGPOSessionCallbacks` structure which contains the callbacks that 
you implement to help GGPO synchronize the two games. You must implement **all** 
functions in the callbacks. You can use `GGPO.CreateSessionCallbacks` to create a  
`GGPOSessionCallbacks` to pass into this method.
* `game` - The name of the game. Used for logging purposes only.
* `numPlayers` - The number of players that will be in this game. The number of players per session is fixed.
* `inputSize` - The size of the game inputs which will be passed to `AddLocalInput`.
* `localPort` - The port GGPO should bind to for UDP traffic.

---

```C#
public GGPO.ErrorCode AddPlayer(ref GGPOPlayer player, out int handle)
```
Must be called for each player in the session (e.g. in a 3 player session, must be
called 3 times).

Parameters:
* `player` - A `GGPOPlayer` struct used to describe the player. You can use 
`GGPO.CreateLocalPlayer` and `GGPO.CreateRemotePlayer` to create local and remote
players respectively.
* `handle` - An out parameter to a handle used to identify the player in the future
(e.g. in the OnEvent callbacks).

---

```C#
public GGPO.ErrorCode StartSyncTest(GGPOSessionCallbacks callbacks, string game, int numPlayers, int inputSize, int frames)
```
Used to being a new GGPO sync test session.  During a sync test, every
frame of execution is run twice: once in prediction mode and once again to
verify the result of the prediction. If the checksums of your save states
do not match, the test is aborted.

Parameters:
* `callbacks` - A `GGPOSessionCallbacks` structure which contains the callbacks that 
you implement to help GGPO synchronize the two games. You must implement **all** 
functions in the callbacks. You can use `GGPO.CreateSessionCallbacks` to create a  
`GGPOSessionCallbacks` to pass into this method.
* `game` - The name of the game. Used for logging purposes only.
* `numPlayers` - The number of players that will be in this game. The number of players per session is fixed.
* `inputSize` - The size of the game inputs which will be passed to `AddLocalInput`.
* `frames` - The number of rames to run before verifying the prediction. The recommended value is 1.

---

```C#
public GGPO.ErrorCode StartSpectating(GGPOSessionCallbacks callbacks, string game, int numPlayers, int inputSize, int localPort, int hostIp, int hostPort)
```
Start a spectator session.

Parameters
* `callbacks` - A `GGPOSessionCallbacks` structure which contains the callbacks that 
you implement to help GGPO synchronize the two games. You must implement **all** 
functions in the callbacks. You can use `GGPO.CreateSessionCallbacks` to create a  
`GGPOSessionCallbacks` to pass into this method.
* `game` - The name of the game. Used for logging purposes only.
* `numPlayers` - The number of players that will be in this game. The number of players per session is fixed.
* `inputSize` - The size of the game inputs which will be passed to `AddLocalInput`.
* `localPort` - The port GGPO should bind to for UDP traffic.
* `hostIp` - The IP address of the host who will serve you the inputs of the game. Any 
player participating in the session can serve as a host.
* `hostPort` - The port of the session on the host.

---

```C#
public GGPO.ErrorCode CloseSession()
```
Used to close a session. You must call `CloseSession` to free the resources allocated
in `StartSession`.

---

```C#
public GGPO.ErrorCode SetFrameDelay(int playerHandle, int frameDelay)
```
Change the amount of frames ggpo will delay local input. Must be called before the
first call to `SynchronizeInput`.

Parameters
* `playerHandle` - Player handle of a local player obtained from `AddPlayer`.
* `frameDelay` - Number of frames to delay input by.

---

```C#
public GGPO.ErrorCode Idle(int timeout)
```
Should be called periodically by your application to give GGPO.net a chance to do some
work. Most packet transmissions and rollbacks occur in Idle.

Parameters
* `timeout` - The amount of time GGPO is allowed to spend in this function, in
milliseconds.

---

```C#
public GGPO.ErrorCode AddLocalInput(int playerHandle, IntPtr values, int size)
```
```C#
public GGPO.ErrorCode AddLocalInput(int playerHandle, byte[] values)
```
Used to notify GGPO of inputs that should be transmitted to remote players.
AddLocalInput must be called once every frame for all players of type 
GGPO_PLAYERTYPE_LOCAL.

Parameters
* `playerHandle` - The player handle returned for this player when you called 
`AddPlayer`.
* `values` - The controller inputs for this player. The size must be exactly equal
to the size passed into `StartSession`.
* `size` - If using the `IntPtr` version, this is the size of the controller inputs.

---

```C#
public GGPO.ErrorCode SynchronizeInput(IntPtr values, int size, out int disconnectFlags)
```
```C#
public GGPO.ErrorCode SynchronizeInput(byte[] values, out int disconnectFlags)
```
You should call SynchronizeInput before every frame of execution, including those
frames which happen during rollback.

Parameters
* `values` - When the function returns, the values parameter will contain
inputs for this frame for all players. The values array must be at least
(size * players) large.
* `size` - If using the `IntPtr` version, this is the size of values array.
* `disconnectFlags` - Indicated whether the input in slot (1 << flag) is valid.
If a player has disconnected, the input in the values array for that player will be
zeroed and the i-th flag will be set. For example, if only player 3 has disconnected,
disconnect flags will be 8 (i.e. 1 << 3)

---

```C#
public GGPO.ErrorCode DisconnectPlayer(int playerHandle)
```
Disconnects a remote player from a game. Will return 
`GGPO_ERRORCODE_PLAYER_DISCONNECTED` if you try to disconnect a player who has already 
been disconnected.

Parameters
* `playerHandle` - The handle of the remote player that was obtained from `AddPlayer`.

---

```C#
public GGPO.ErrorCode GetNetworkStats(int playerHandle, out GGPONetworkStats stats)
```
Used to fetch some statistics about the quality of the network connection.

Parameters
* `playerHandle` - The player handle returned from the `AddPlayer` function you used
to add the remote player.
* `stats` - Out parameter to the network statistics.

---

```C#
public GGPO.ErrorCode SetDisconnectTimeout(int timeout)
```
Sets the disconnect timeout.  The session will automatically disconnect
from a remote peer if it has not received a packet in the timeout window.
You will be notified of the disconnect via a `GGPO_EVENTCODE_DISCONNECTED_FROM_PEER`
event.

Parameters
* `timeout` - The time in milliseconds to wait before disconnecting a peer.

---

```C#
public GGPO.ErrorCode SetDisconnectNotifyStart(int timeout)
```
The time to wait before the first `GGPO_EVENTCODE_NETWORK_INTERRUPTED` timeout
will be sent.

Parameters
* `timeout` - The amount of time which needs to elapse without receiving
a packet before the `GGPO_EVENTCODE_NETWORK_INTERRUPTED` event is sent.

---

```C#
public static bool Succeeded(ErrorCode result)
```
Utility function used to check if a method succeeded.

Parameters
* `result` - ErrorCode returned from the called method.

---

```C#
public static GGPOPlayer CreateLocalPlayer(int playerNum)
```
Utility function for creating a local player. Sets the type and size of the
`GGPOPlayer` struct automatically.

Parameters
* `playerNum` - The player number. Should be between 1 and the number of players in
the game.

---

```C#
public static GGPOPlayer CreateRemotePlayer(int playerNum, string ipAddress, short port)
```
Utility function for creating a remote player. Sets the type and size of the 
`GGPOPlayer` struct automatically.

Parameters
* `playerNum` - The player number. Should be between 1 and the number of players in
the game.
* `ipAddress` - IP address of the remote player.
* `port` - The port where UDP packets should be sent to reach this player.

---

```C#
public static GGPOSessionCallbacks CreateSessionCallbacks(IGGPOSession callbacks)
```
Utility function to create a `GGPOSessionCallbacks` struct with function pointers to an
object that implements the `IGGPOSession` interface.

Parameters
* `callbacks` - Object implementing the games callbacks.