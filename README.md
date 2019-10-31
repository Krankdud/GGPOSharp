# GGPOSharp

C# wrapper for the [GGPO](https://github.com/pond3r/ggpo) networking library. This is very WIP and is **not** guaranteed to be working currently.

### What is GGPO?

As described from the GGPO repository:

> Traditional techniques account for network transmission time by adding delay to a players input, resulting in a sluggish, laggy game-feel. Rollback networking uses input prediction and speculative execution to send player inputs to the game immediately, providing the illusion of a zero-latency network. Using rollback, the same timings, reactions visual and audio queues, and muscle memory your players build up playing offline translate directly online. The GGPO networking SDK is designed to make incorporating rollback networking into new and existing games as easy as possible.

I recommend taking a look at the [documentation](https://github.com/pond3r/ggpo/tree/master/doc) on the GGPO repo.

## Todo
- [ ] Write some example code (in the meantime, check the [VectorWar](https://github.com/pond3r/ggpo/tree/master/src/apps/vectorwar) example at the GGPO repository)
- [ ] Finish writing documentation
- [ ] NuGet package
- [ ] Fix the unit tests

## API
- [GGPO](doc/GGPO.md) - The main class that interfaces between your game and GGPO.
- [IGGPOSession](doc/IGGPOSession.md) - The interface for callback functions that your 
game needs to implement