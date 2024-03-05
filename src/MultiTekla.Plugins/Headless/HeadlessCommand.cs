using CliFx;

namespace MultiTekla.Plugins.Headless;

[Command("headless", Description = "Root command for headless related commands")]
public class HeadlessCommand : ICommand
{
    public ValueTask ExecuteAsync(IConsole console)
        => default;
}