namespace MultiTekla.Plugins.Headless.Config.Commands;

[Command("headless config print", Description = "Prints headless config")]
public class PrintHeadlessConfigCommand : CommandBase<GetHeadlessConfigPlugin>
{
    protected override ValueTask Execute(IConsole console, GetHeadlessConfigPlugin plugin)
    {
        plugin.Config = new HeadlessConfig { Name = ConfigName, };
        console.Output.WriteLine(plugin.Result);

        return default;
    }
}
