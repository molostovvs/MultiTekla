namespace MultiTekla.Plugins.Headless.Config;

[Command("headless config create", Description = "Create config file for headless tekla plugin")]
public sealed class CreateHeadlessConfigCommand : CommandBase<CreateHeadlessConfigPlugin>
{
    [CommandParameter(0, Name = "CONFIG NAME", Description = "Config name to create")]
    public override required string ConfigName { get; init; }

    [CommandOption("bin-path", 'b', Description = "Path to Tekla /bin/ directory")]
    public string? TeklaBinPath { get; init; }

    [CommandOption("env-ini-path", 'e', Description = "Path to Environment.ini")]
    public string? EnvironmentIniPath { get; init; }

    [CommandOption("role-ini-path", 'r', Description = "Path to Role.ini")]
    public string? RoleIniPath { get; init; }

    [CommandOption("models-path", 'p', Description = "Path to folder with models")]
    public string? ModelsPath { get; init; }

    [CommandOption("from-config", 'f', Description = "Reuse values from existing config file")]
    public string? FromConfigName { get; set; }

    [CommandOption("model-name", 'm', Description = "Model name for default initialization")]
    public override string? ModelName { get; init; }

    public override bool IsHeadlessMode { get; init; } = false;

    protected override ValueTask Execute(IConsole console, CreateHeadlessConfigPlugin plugin)
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
            var headlessConfigPluginGetter = new GetHeadlessConfigPlugin
            {
                Config = new HeadlessConfig { Name = FromConfigName, }
            };

            headlessConfigPluginGetter.RunPlugin();

            var existedConfig = headlessConfigPluginGetter.Result;

            if (existedConfig is null)
                throw new ArgumentException(
                    $"There is no config with name {FromConfigName}",
                    nameof(FromConfigName)
                );

            config = existedConfig.CreateFrom(config);
        }

        plugin.Config = config;
        plugin.RunPlugin();

        return default;
    }
}
