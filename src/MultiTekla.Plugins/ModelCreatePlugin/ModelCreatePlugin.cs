using System.IO;
using MultiTekla.Plugins.Config;

namespace MultiTekla.Plugins.ModelCreatePlugin;

public class ModelCreatePlugin : IPlugin<bool>
{
    public bool MultiUser { get; set; } = false;
    public string? ModelName { get; set; }
    public string? Template { get; set; }
    public string? ServerName { get; set; }
    private HeadlessConfig? HeadlessConfig { get; set; }

    public bool Run()
    {
        Console.WriteLine($"~~~~~~~~~Model path is {HeadlessConfig.ModelPath}");
        var handler = new Tekla.Structures.Model.ModelHandler();

        if (ModelName is null or "")
            throw new ArgumentException(nameof(ModelName), $"{nameof(ModelName)} is not specified");

        if (!Directory.Exists(HeadlessConfig.ModelPath))
            Directory.CreateDirectory(HeadlessConfig.ModelPath);

        var singleModelCreateSuccess = handler.CreateNewSingleUserModel(
            ModelName,
            HeadlessConfig?.ModelPath,
            Template ?? ""
        );

        if (MultiUser && singleModelCreateSuccess)
        {
            if (ServerName is null or "")
                throw new ArgumentException(
                    nameof(ServerName),
                    $"{nameof(ServerName)} is not specified"
                );

            var pathToModel = Path.Combine(
                HeadlessConfig?.ModelPath ?? @"C:\TeklaStructuresModels\",
                ModelName
            );

            var multiuserConvertResult =
                Tekla.Structures.ModelInternal.Operation.dotConvertAndOpenAsMultiUserModel(
                    pathToModel,
                    ServerName
                );

            return multiuserConvertResult;
        }

        return singleModelCreateSuccess;
    }

    public TimeSpan StartHeadless(string? configName, string? modelName)
    {
        var configPlugin = HeadlessConfigPlugin.Value;
        var headlessConfig = configPlugin.GetConfigWithName(configName ?? "default");
        headlessConfig.ModelPath = Path.Combine(headlessConfig.ModelPath);
        HeadlessConfig = headlessConfig;

        var headlessPlugin = HeadlessTeklaPlugin.Value;
        headlessPlugin.Config = headlessConfig;
        return headlessPlugin.Run();
    }

    public Lazy<HeadlessTeklaPlugin> HeadlessTeklaPlugin { get; set; } = null!;
    public Lazy<HeadlessConfigPlugin> HeadlessConfigPlugin { get; set; } = null!;
}

[Command("model create", Description = "Creates a model")]
public class ModelCreateCommand : ICommandFor<ModelCreatePlugin>
{
    [CommandOption("headless", 's', Description = "Run plugin with headless tekla")]
    public bool HeadlessOption { get; init; } = false;

    [CommandOption(
        "config",
        'c',
        Description = "Config to use for headless run, if not specified, \"default.toml\" used"
    )]
    public string ConfigName { get; init; } = "default";

    [CommandOption("model-name", 'm', Description = "Model name")]
    public string? ModelName { get; init; }

    public ValueTask ExecuteAsync(IConsole console)
    {
        var plugin = Plugin.Value;

        plugin.StartHeadless(ConfigName, ModelName);

        plugin.ModelName = ModelName;
        plugin.Run();

        return default;
    }

    public Lazy<ModelCreatePlugin> Plugin { get; set; }
}