using System.IO;

namespace MultiTekla.Plugins.ModelNamePlugin;

[Command("model name", Description = "Get model name")]
public class ModelNameCommand : ICommandFor<ModelNamePlugin>
{
    [CommandOption("headless", 's', Description = "Run plugin with headless Tekla")]
    public bool HeadlessOption { get; init; } = false;

    [CommandOption("model-name", 'm', Description = "Model Name")]
    public string? ModelName { get; init; }

    [CommandOption("config", 'c', Description = "Config to use for headless run")]
    public string ConfigName { get; init; } = "default";

    public ValueTask ExecuteAsync(IConsole console)
    {
        if (ModelName is null or "" || !Directory.Exists(ModelName))
            throw new ArgumentNullException(
                nameof(ModelName),
                "Model path is not specified or doesn't exist"
            );

        var plugin = Plugin.Value;
        plugin.ModelName = ModelName;
        plugin.ConfigName = ConfigName;

        plugin.Run();

        return default;
    }

    public Lazy<ModelNamePlugin> Plugin { get; set; } = null!;
}