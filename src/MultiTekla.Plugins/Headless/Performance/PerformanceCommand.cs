using System.Diagnostics;

namespace MultiTekla.Plugins.Headless.Performance;

[Command(
    "headless perf",
    Description =
        "Measure headless performance. Returns the time taken to open the model in headless mode "
)]
public sealed class PerformanceCommand : CommandBase<PerformancePlugin>
{
    [CommandParameter(0, Name = "MODEL NAME", Description = "The name of the model to open", IsRequired = false)]
    public override string? ModelName { get; init; }

    [CommandOption("config", 'c', Description = "Config to use for headless run")]
    public override string ConfigName { get; init; } = "default";

    public override bool IsHeadlessMode { get; init; } = true;

    protected override ValueTask Execute(IConsole console, PerformancePlugin plugin)
    {
        var sw = new Stopwatch();
        sw.Start();

        plugin.RunPlugin();
        console.Clear();

        sw.Stop();

        console.Output.WriteLine(
            $"It took {sw.Elapsed.Seconds} seconds to open the model \"{plugin.Config.ModelName}\" using headless tekla."
        );

        return default;
    }
}
