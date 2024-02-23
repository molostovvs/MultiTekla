using System.Diagnostics;
using MultiTekla.Plugins.Headless.Config;

namespace MultiTekla.Plugins.Headless.Performance;

[Command(
    "headless perf",
    Description =
        "Measure headless performance. Returns the time taken to open the model in headless mode "
)]
public class PerformanceCommand : ICommandFor<PerformancePlugin>
{
    [CommandOption("headless", 's', Description = "Run plugin with headless Tekla")]
    public bool HeadlessOption { get; init; } = true;

    [CommandOption("model-name", 'm', Description = "Model Name")]
    public string? ModelName { get; init; }

    [CommandOption("config", 'c', Description = "Config to use for headless run")]
    public string ConfigName { get; init; } = "default";

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

        //TODO: remove ModelName property from plugin base and always use config file?
        plugin.ModelName = config.ModelName;
        plugin.Config = config;

        var sw = new Stopwatch();
        sw.Start();

        plugin.RunPlugin();
        console.Clear();

        sw.Stop();
        console.Output.WriteLine(
            $"It took {sw.Elapsed.Seconds} seconds to open the model \"{config.ModelName
            }\" using headless tekla."
        );

        return default;
    }

    public Lazy<PerformancePlugin> Plugin { get; set; } = null!;
    public Lazy<HeadlessConfigPlugin> ConfigPlugin { get; set; } = null!;
}