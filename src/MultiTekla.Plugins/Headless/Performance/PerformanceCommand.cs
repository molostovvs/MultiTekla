using System.Diagnostics;

namespace MultiTekla.Plugins.Headless.Performance;

[Command(
    "headless perf",
    Description =
        "Measure headless performance. Returns the time taken to open the model in headless mode "
)]
public class PerformanceCommand : CommandBase<PerformancePlugin>
{
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

    public new bool IsHeadlessMode { get; init; }
}
