namespace MultiTekla.Plugins.ModelCreatePlugin;

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
        var plugin = Plugin.Value;
        plugin.ModelName = ModelName;
        plugin.ConfigName = ConfigName;

        plugin.Run();

        return default;
    }

    public Lazy<ModelCreatePlugin> Plugin { get; set; } = null!;
}