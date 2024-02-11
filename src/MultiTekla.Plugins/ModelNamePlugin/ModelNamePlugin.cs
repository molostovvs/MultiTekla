using MultiTekla.Plugins.Config;

namespace MultiTekla.Plugins.ModelNamePlugin;

public class ModelNamePlugin : IPlugin<object>
{
    public object Run()
    {
        var model = new Tekla.Structures.Model.Model();
        var modelInfo = model.GetInfo();

        Console.WriteLine(
            "Model name: {0} \nModel path: {1} \nModel is SingleUser: {2}",
            modelInfo.ModelName,
            modelInfo.ModelPath,
            modelInfo.SingleUserModel
        );

        return new object();
    }

    public TimeSpan StartHeadless(string? configName, string? modelPath)
    {
        var configPlugin = HeadlessConfigPlugin.Value;
        var headlessConfig = configPlugin.GetConfigWithName(configName ?? "default");
        headlessConfig.ModelPath = modelPath;

        var headlessPlugin = HeadlessTeklaPlugin.Value;
        headlessPlugin.Config = headlessConfig;
        return headlessPlugin.Run();
    }

    public Lazy<HeadlessTeklaPlugin> HeadlessTeklaPlugin { get; set; } = null!;
    public Lazy<HeadlessConfigPlugin> HeadlessConfigPlugin { get; set; } = null!;
}