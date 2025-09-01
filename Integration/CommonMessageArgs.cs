using System;

namespace TerrariaChaosMod.Integration;

public class CommonMessageArgs : EventArgs
{
    public string Username { get; set; }

    public string Message { get; set; }

    public bool IsPrivileged { get; set; } = false;
}
