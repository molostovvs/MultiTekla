namespace MultiTekla.Plugins.Headless.Config.Commands;

[Command("headless config", Description = "Manage config file for headless tekla")]
public class RootHeadlessConfigCommand : CommandBase<HeadlessConfigPlugin>
{
    protected override ValueTask Execute(IConsole console, HeadlessConfigPlugin plugin)
        => default;
}