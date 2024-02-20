using System.IO;
using MultiTekla.Plugins.Config;

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
        if (ModelName is null or "")
            throw new ArgumentException(
                "The model name must be defined",
                nameof(ModelName)
            );

        var configPlugin = ConfigPlugin.Value;
        var config = configPlugin.GetConfigWithName(ConfigName);
        config.ModelName = ModelName;

        var plugin = Plugin.Value;
        plugin.ModelName = ModelName;
        plugin.Config = config;

        plugin.RunPlugin();

        return default;
    }

    public Lazy<ModelNamePlugin> Plugin { get; set; } = null!;
    public Lazy<HeadlessConfigPlugin> ConfigPlugin { get; set; } = null!;
}