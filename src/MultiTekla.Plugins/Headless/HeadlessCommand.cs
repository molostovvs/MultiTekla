using CliFx;

namespace MultiTekla.Plugins;

[Command("headless", Description = "Headless related commands")]
public class HeadlessCommand : ICommand
{
    public ValueTask ExecuteAsync(IConsole console)
        => default;
}