using CliFx;

namespace MultiTekla.Plugins.Headless;

[Command("headless", Description = "Headless related commands")]
public class HeadlessCommand : ICommand
{
    public ValueTask ExecuteAsync(IConsole console)
        => default;
}
