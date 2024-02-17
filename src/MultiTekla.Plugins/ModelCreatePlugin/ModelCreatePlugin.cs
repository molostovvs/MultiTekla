using System.IO;
using MultiTekla.Plugins.Config;
using Tekla.Structures.Model;

namespace MultiTekla.Plugins.ModelCreatePlugin;

public class ModelCreatePlugin : IPlugin<bool>
{
    public bool MultiUser { get; set; }
    public string? ModelName { get; set; }
    public string? Template { get; set; }
    public string? ServerName { get; set; }
    public string? ConfigName { get; set; }
    private HeadlessConfig? HeadlessConfig { get; set; }

    public bool Run()
    {
        var time = StartHeadless();

        var handler = new ModelHandler();

        if (ModelName is null or "")
            throw new ArgumentException(nameof(ModelName), $"{nameof(ModelName)} is not specified");

        var singleModelCreateSuccess = handler.CreateNewSingleUserModel(
            ModelName,
            HeadlessConfig.ModelsPath,
            Template ?? ""
        );

        if (MultiUser && singleModelCreateSuccess)
        {
            if (ServerName is null or "")
                throw new ArgumentException(
                    nameof(ServerName),
                    $"{nameof(ServerName)} is not specified"
                );

            var multiuserConvertResult =
                Tekla.Structures.ModelInternal.Operation.dotConvertAndOpenAsMultiUserModel(
                    Path.Combine(HeadlessConfig.ModelsPath, HeadlessConfig.ModelName),
                    ServerName
                );

            return multiuserConvertResult;
        }

        return singleModelCreateSuccess;
    }

    private TimeSpan StartHeadless()
    {
        var configPlugin = HeadlessConfigPlugin.Value;
        HeadlessConfig = configPlugin.GetConfigWithName(ConfigName ?? "default");
        HeadlessConfig.ModelName = ModelName;

        var headlessPlugin = HeadlessTeklaPlugin.Value;
        headlessPlugin.Config = HeadlessConfig;
        return headlessPlugin.Run();
    }

    public Lazy<HeadlessTeklaPlugin> HeadlessTeklaPlugin { get; set; } = null!;
    public Lazy<HeadlessConfigPlugin> HeadlessConfigPlugin { get; set; } = null!;
}