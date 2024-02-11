using System.IO;

namespace MultiTekla.Plugins.ModelNamePlugin;

[Command("model name", Description = "Get model name")]
public class ModelNameCommand : ICommandFor<ModelNamePlugin>
{
    [CommandOption("headless", 's', Description = "Run plugin with headless Tekla")]
    public bool HeadlessOption { get; init; } = false;

    [CommandOption("model-path", 'm', Description = "Path to model folder")]
    public string? ModelPath { get; init; }

    [CommandOption(
        "config",
        'c',
        Description = "Config to use for headless run, if not specified, \"default.toml\" used"
    )]
    public string ConfigName { get; init; } = "default";

    public ValueTask ExecuteAsync(IConsole console)
    {
        if (ModelPath is null or "" || !Directory.Exists(ModelPath))
            throw new ArgumentNullException(
                nameof(ModelPath),
                "Model path is not specified or doesn't exist"
            );

        var modelPlugin = Plugin.Value;

        var time = modelPlugin.StartHeadless(ConfigName, ModelPath);
        console.Clear();
        console.Output.WriteLine($"Headless initialization took {time.Seconds}s");

        modelPlugin.Run();

        return default;
    }

    public Lazy<ModelNamePlugin> Plugin { get; set; } = null!;
}