namespace MultiTekla.Plugins.Headless.Config.Commands;

[Command("headless config", Description = "Manage config file for headless tekla")]
public class RootHeadlessConfigCommand : ICommandFor<HeadlessConfigPlugin>
{
    public ValueTask ExecuteAsync(IConsole console)
        => default;

    public Lazy<HeadlessConfigPlugin> Plugin { get; set; } = null!;
}