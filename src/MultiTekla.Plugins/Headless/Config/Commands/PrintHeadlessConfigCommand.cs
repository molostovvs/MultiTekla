namespace MultiTekla.Plugins.Headless.Config.Commands;

[Command("headless config print", Description = "Prints headless config")]
public class PrintHeadlessConfigCommand : CommandBase<HeadlessConfigPlugin>
{
    protected override ValueTask Execute(IConsole console, HeadlessConfigPlugin plugin)
    {
        console.Output.WriteLine(plugin.GetConfigWithName(ConfigName));

        return default;
    }
}