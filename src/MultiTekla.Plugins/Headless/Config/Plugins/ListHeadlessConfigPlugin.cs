using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MultiTekla.Plugins.Headless.Config;

/// <summary>
/// List all configs
/// </summary>
public class ListHeadlessConfigPlugin : PluginBase
{
    public List<string> Result { get; private set; } = new();

    protected override void Run()
        => Result = Directory.GetFiles("plugins", "*.toml")
           .Select(f => f.Replace("plugins/", ""))
           .ToList();
}