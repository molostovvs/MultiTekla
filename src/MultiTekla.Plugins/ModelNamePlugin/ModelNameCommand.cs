using System.Composition;
using System.IO;
using CliFx;
using MultiTekla.Plugins.Config;

namespace MultiTekla.Plugins.ModelNamePlugin;

[Command("model name", Description = "Get model name")]
public class ModelNameCommand : ICommandFor<ModelNamePlugin>
{
    [CommandOption("headless", 's', Description = "Run plugin headlessly")]
    public bool HeadlessOption { get; set; } = false;

    [CommandOption("model-path", 'm', Description = "Path to model folder")]
    public string? ModelPath { get; set; }

    [CommandOption(
        "config",
        'c',
        Description = "Config to use for headless run, if not specified, \"default.toml\" used"
    )]
    public string? ConfigName { get; set; }

    public ValueTask ExecuteAsync(IConsole console)
    {
        if (ModelPath is null or "" || !Directory.Exists(ModelPath))
            throw new ArgumentNullException(
                nameof(ModelPath),
                "Model path is not specified or doesn't exist"
            );

        var configPlugin = HeadlessConfigPlugin.Value;
        var headlessConfig = configPlugin.GetConfigWithName(ConfigName ?? "default");
        headlessConfig.ModelPath = ModelPath;

        var headlessPlugin = HeadlessPlugin.Value;
        headlessPlugin.Config = headlessConfig;
        var time = headlessPlugin.Run();

        console.Clear();
        console.Output.WriteLine($"Headless initialization took {time.Seconds}s");

        var modelPlugin = Plugin.Value;
        modelPlugin.Run();

        return default;
    }

    public Lazy<ModelNamePlugin> Plugin { get; set; }

    [Import]
    public Lazy<HeadlessTeklaPlugin> HeadlessPlugin { get; set; }

    [Import]
    public Lazy<HeadlessConfigPlugin> HeadlessConfigPlugin { get; set; }
}

[Command("model", Description = "Model related commands")]
public class ModelCommand : ICommand
{
    public ValueTask ExecuteAsync(IConsole console)
        => default;
}