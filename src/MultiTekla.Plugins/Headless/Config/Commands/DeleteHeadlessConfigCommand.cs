namespace MultiTekla.Plugins.Headless.Config.Commands;

[Command("headless config delete", Description = "Delete the config file")]
public class DeleteHeadlessConfigCommand : CommandBase<DeleteHeadlessConfigPlugin>
{
    [CommandParameter(0, Name = "CONFIG NAME", Description = "Config name to delete")]
    public new required string ConfigName { get; init; }

    protected override ValueTask Execute(IConsole console, DeleteHeadlessConfigPlugin plugin)
    {
        plugin.Config = new HeadlessConfig { Name = ConfigName, };
        plugin.RunPlugin();

        console.Output.WriteLine(
            plugin.Result.Success ? "Removed successfully"
                : "Fail to remove" + $"config with name {ConfigName}"
        );

        return default;
    }

    [Obsolete("This property is meaningless in this command", true)]
    public new string? ModelName { get; init; }

    [Obsolete("This property is meaningless in this command", true)]
    public new bool IsHeadlessMode { get; init; }
}
