using MultiTekla.Plugins.Headless.Config;

namespace MultiTekla.Plugins.Model.Create;

[Command("model create", Description = "Creates a model")]
public class ModelCreateCommand : ICommandFor<ModelCreatePlugin>
{
    [CommandOption("headless", 's', Description = "Run plugin with headless tekla")]
    public bool HeadlessOption { get; init; } = false;

    [CommandOption("config", 'c', Description = "Config to use for headless run")]
    public string ConfigName { get; init; } = "default";

    [CommandOption("model-name", 'm', Description = "Model name")]
    public string? ModelName { get; init; }

    public ValueTask ExecuteAsync(IConsole console)
    {
        var configPlugin = ConfigPlugin.Value;
        var config = configPlugin.GetConfigWithName(ConfigName);
        config.ModelName = ModelName;

        var plugin = Plugin.Value;
        plugin.Headless = HeadlessOption;
        plugin.ModelName = ModelName;
        plugin.Config = config;

        plugin.RunPlugin();

        return default;
    }

    public Lazy<ModelCreatePlugin> Plugin { get; set; } = null!;
    public Lazy<HeadlessConfigPlugin> ConfigPlugin { get; set; } = null!;
}