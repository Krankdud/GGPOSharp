# IGGPOSession

Interface that contains all of the methods that need to be implemented as callback
functions for GGPO.

```C#
bool BeginGame(string game)
```

```C#
bool SaveGameState(ref byte[] buffer, ref int len, ref int checksum, int frame)
```

```C#
bool LoadGameState(byte[] buffer, int len)
```

```C#
bool LogGameState(string filename, byte[] buffer, int len)
```

```C#
void FreeBuffer(IntPtr buffer)
```

```C#
bool AdvanceFrame(int flags)
```

```C#
bool OnEvent(ref GGPOEvent info)
```