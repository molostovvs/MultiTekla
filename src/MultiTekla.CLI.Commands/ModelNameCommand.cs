using System;
using System.Composition;
using System.Threading.Tasks;
using CliFx.Attributes;
using CliFx.Infrastructure;
using MultiTekla.Contracts;
using MultiTekla.Plugins;

namespace MultiTekla.CLI.Commands;

[Command("model name", Description = "get model name")]
public class ModelNameCommand : ICommandFor<ModelNamePlugin>
{
    [CommandOption("headless", 'h', Description = "run plugin headlessly")]
    public bool HeadlessOption { get; set; } = false;

    public ValueTask ExecuteAsync(IConsole console)
    {
        var plugin = Plugin.Value;
        plugin.Run();
        return default;
    }

    public Lazy<ModelNamePlugin> Plugin { get; set; }

    [Import]
    public Lazy<HeadlessTeklaPlugin> HeadlessPlugin { get; set; }
}