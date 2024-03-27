using System.IO;

namespace MultiTekla.Plugins.Headless.Config;

/// <summary>
/// Remove config with specified name
/// </summary>
public class RemoveHeadlessConfigPlugin : PluginBase
{
    public (bool Success, string ConfigFileName) Result { get; private set; }

    protected override void Run()
    {
        if (Config is null)
            throw new ArgumentNullException(nameof(Config), "You must provide config");

        if (string.IsNullOrEmpty(Config.Name))
            throw new ArgumentException(
                "You must provide name of the config to remove",
                nameof(Config.Name)
            );

        var configNameToRemove = Config.Name + ".toml";

        var listConfigPlugin = new ListHeadlessConfigPlugin();
        listConfigPlugin.RunPlugin();
        var before = listConfigPlugin.Result.Count;

        File.Delete(Path.Combine("plugins", configNameToRemove));

        listConfigPlugin.RunPlugin();
        var after = listConfigPlugin.Result.Count;

        Result = (before - after == 1, configNameToRemove);
    }
}