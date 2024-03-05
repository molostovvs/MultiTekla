namespace MultiTekla.Plugins.Headless.Config.Commands;

[Command("headless config create", Description = "Create config file for headless tekla plugin")]
public class CreateHeadlessConfigCommand : CommandBase<HeadlessConfigPlugin>
{
    [CommandOption("bin-path", 'b', Description = "Path to Tekla /bin/ directory")]
    public string? TeklaBinPath { get; init; }

    [CommandOption("env-ini-path", 'e', Description = "Path to environment.ini")]
    public string? EnvironmentIniPath { get; init; }

    [CommandOption("role-ini-path", 'r', Description = "Path to role.ini")]
    public string? RoleIniPath { get; init; }

    [CommandOption("models-path", 'p', Description = "Path to folder with models")]
    public string? ModelsPath { get; init; }

    [CommandOption("from-config", 'f', Description = "Reuse values from existing config file")]
    public string? FromConfigName { get; init; }

    protected override ValueTask Execute(IConsole console, HeadlessConfigPlugin plugin)
    {
        var config = new HeadlessConfig
        {
            Name = ConfigName,
            TeklaBinPath = TeklaBinPath,
            EnvironmentIniPath = EnvironmentIniPath,
            RoleIniPath = RoleIniPath,
            ModelsPath = ModelsPath,
            ModelName = ModelName,
        };

        if (FromConfigName is not null)
        {
            var existedConfig = plugin.GetConfigWithName(FromConfigName);
            config = existedConfig.CreateFrom(config);
        }

        plugin.Config = config;
        plugin.RunPlugin();

        return default;
    }
}