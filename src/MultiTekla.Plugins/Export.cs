using CliFx;

namespace MultiTekla.Plugins;

[Command("export", Description = "Export related commands")]
public class ExportCommand : ICommand
{
    public ValueTask ExecuteAsync(IConsole console)
        => default;
}
