namespace MultiTekla.Plugins.Config.Commands;

[Command("headless config create", Description = "Create config file for headless tekla plugin")]
public class CreateHeadlessConfigCommand : ICommandFor<HeadlessConfigPlugin>
{
    [CommandOption("name", 'n', Description = "Name of the config file")]
    public string? ConfigName { get; init; }

    [CommandOption("bin-path", 'b', Description = "Path to Tekla /bin/ directory")]
    public string? TeklaBinPath { get; init; }

    [CommandOption("env-ini-path", 'e', Description = "Path to environment.ini")]
    public string? EnvironmentIniPath { get; init; }

    [CommandOption("role-ini-path", 'r', Description = "Path to role.ini")]
    public string? RoleIniPath { get; init; }

    [CommandOption("model-path", 'm', Description = "Path to model")]
    public string? ModelPath { get; init; }

    [CommandOption("from-config", 'f', Description = "Reuse values from existing config file")]
    public string? FromConfigName { get; init; }

    public ValueTask ExecuteAsync(IConsole console)
    {
        var config = new HeadlessConfig
        {
            Name = ConfigName,
            TeklaBinPath = TeklaBinPath,
            EnvironmentIniPath = EnvironmentIniPath,
            RoleIniPath = RoleIniPath,
            ModelPath = ModelPath,
        };

        var pluginValue = Plugin.Value;

        if (FromConfigName is not null)
        {
            var existedConfig = pluginValue.GetConfigWithName(FromConfigName);
            config = existedConfig.CreateFrom(config);
        }

        pluginValue.Config = config;
        pluginValue.Run();

        return default;
    }

    public Lazy<HeadlessConfigPlugin> Plugin { get; set; }
}