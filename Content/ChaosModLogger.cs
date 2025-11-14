using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using log4net.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrariaChaosMod;

/// <summary>
/// A logger wrapper for the Terraria Chaos Mod that adds a custom log tag
/// and prints log messages in-game with appropriate colors.
/// </summary>
public class ChaosModLogger
{
    public const string LogTag = "[ChaosMod] ";

    private readonly Mod _mod;

    public ChaosModLogger(Mod mod)
    {
        _mod = mod;
    }

    // --- Color helpers for chat messages ---
    private static Color _debugColor => Color.DimGray;
    private static Color _infoColor  => Color.DeepSkyBlue;
    private static Color _warnColor  => Color.Orange;
    private static Color _errorColor => Color.LightCoral;
    private static Color _fatalColor => new Color(255, 0, 0);

    private static Dictionary<Level, Color> _levelColors = new()
    {
        { Level.Debug,  _debugColor },
        { Level.Info,   _infoColor  },
        { Level.Warn,   _warnColor  },
        { Level.Error,  _errorColor },
        { Level.Fatal,  _fatalColor },
    };

    private static void PrintInGame(string msg, Color? color)
    {
        try
        {
            if (Main.netMode != NetmodeID.Server)
            {
                Main.NewText(msg, color);
            }
        }
        catch
        {
            // ignore if called before Main initialized
        }
    }

    public static string GetLogText(Level level, object message)
    {
        StringBuilder sb = new();

        if (level != null)
        {
            string color = _levelColors.ContainsKey(level)
                ? _levelColors[level].Hex3()
                : Color.White.Hex3();
            sb.Append("[c/");
            sb.Append(color);
            sb.Append(":[ChaosMod - ");
            sb.Append(level.Name.ToUpper());
            sb.Append("][c/");
            sb.Append(color);
            sb.Append(":]] ");
        }

        sb.Append(message?.ToString());

        return sb.ToString();
    }

    /// <summary>
    /// Logs a debug message.
    /// </summary>
    public void Debug(object message, Color? color = null)
    {
        Display(GetLogText(Level.Debug, message), color);
    }

    /// <summary>
    /// Logs an informational message.
    /// </summary>
    public void Info(object message, Color? color = null)
    {
        Display(GetLogText(Level.Info, message), color);
    }

    /// <summary>
    /// Logs a warning message.
    /// </summary>
    public void Warn(object message, Color? color = null)
    {
        Display(GetLogText(Level.Warn, message), color);
    }

    /// <summary>
    /// Displays a message in-game. Used for general notifications not
    /// necessarily tied to internal information logs such as effect
    /// activations.
    /// </summary>
    public void Display(object message, Color? color = null)
    {
        _mod?.Logger.Info(message);
        PrintInGame(message.ToString(), color);
    }
}
