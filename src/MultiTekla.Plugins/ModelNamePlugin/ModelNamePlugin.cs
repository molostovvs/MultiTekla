using MultiTekla.Plugins.Config;

namespace MultiTekla.Plugins.ModelNamePlugin;

public class ModelNamePlugin : IPlugin<object>
{
    public string? ModelName { get; set; }
    public string? ConfigName { get; set; }
    private HeadlessConfig? HeadlessConfig { get; set; }

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