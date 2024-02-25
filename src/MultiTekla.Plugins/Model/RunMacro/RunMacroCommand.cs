using MultiTekla.Plugins.Headless.Config;

namespace MultiTekla.Plugins.Model.RunMacro;

[Command("model run macro", Description = "Run the macro")]
public class RunMacroCommand : ICommandFor<RunMacroPlugin>
{
    [CommandOption("headless", 's', Description = "Run plugin with headless tekla")]
    public bool HeadlessOption { get; init; } = true;

    [CommandOption("config", 'c', Description = "Config to use for headless run")]
    public string ConfigName { get; init; } = "default";

    [CommandOption("model-name", 'm', Description = "Model name")]
    public string? ModelName { get; init; }

    [CommandOption("macro-name", 'r', Description = "Macro name")]
    public string? MacroName { get; init; }

    public ValueTask ExecuteAsync(IConsole console)
    {
        var configPlugin = ConfigPlugin.Value;
        var config = configPlugin.GetConfigWithName(ConfigName);
        if (!string.IsNullOrEmpty(ModelName))
            config.ModelName = ModelName;

        if (string.IsNullOrEmpty(config.ModelName))
            throw new ArgumentException(
                "You must provide the model name either in the config or in the command"
            );

        var plugin = Plugin.Value;
        plugin.Headless = HeadlessOption;
        plugin.ModelName = config.ModelName;
        plugin.Config = config;

        if (MacroName is null or "")
            throw new ArgumentException("You must provide the macro name");

        plugin.MacroName = MacroName;
        plugin.RunPlugin();

        return default;
    }

    public Lazy<HeadlessConfigPlugin> ConfigPlugin { get; set; } = null!;
    public Lazy<RunMacroPlugin> Plugin { get; set; } = null!;
}