using System;

namespace TerrariaChaosMod.Integration;

public class CommonReadyArgs : EventArgs
{
    public string ChannelName { get; set; }
}
