using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tomlyn;

namespace MultiTekla.Plugins.Config;

/*public class HeadlessConfigPlugin : IPlugin<bool>
{
    public HeadlessConfig? Config { get; set; }

    public bool Run()
    {
        var toml = Toml.FromModel(Config ?? new HeadlessConfig());

        File.WriteAllText(
            Config?.Name is null
                ? Path.Combine("plugins", "default.toml")
                : Path.Combine("plugins", Config.Name + ".toml"),
            toml
        );

        return true;
    }

    public HeadlessConfig GetConfigWithName(string configFileName)
    {
        configFileName = configFileName.Replace(".toml", "");

        var configPath = Path.Combine("plugins", configFileName + ".toml");

        var existingConfig = File.ReadAllText(configPath);
        return Toml.ToModel<HeadlessConfig>(existingConfig);
    }

    public IReadOnlyList<string> GetAllConfigNames()
        => Directory.GetFiles("plugins", "*.toml")
           .Select(f => f.Replace("plugins/", ""))
           .ToList();

    public (bool success, string configFileName) Remove(string configNameToRemove)
    {
        if (!configNameToRemove.Contains(".toml"))
            configNameToRemove += ".toml";

        var before = GetAllConfigNames().Count;
        File.Delete(Path.Combine("plugins", configNameToRemove));
        var after = GetAllConfigNames().Count;
        return (before - after == 1, configNameToRemove);
    }
}*/

public class HeadlessConfigPlugin : PluginBase<bool>
{
    protected override bool Run()
    {
        var toml = Toml.FromModel(Config ?? new HeadlessConfig());

        File.WriteAllText(
            Config?.Name is null
                ? Path.Combine("plugins", "default.toml")
                : Path.Combine("plugins", Config.Name + ".toml"),
            toml
        );

        return true;
    }

    public HeadlessConfig GetConfigWithName(string configFileName)
    {
        configFileName = configFileName.Replace(".toml", "");

        var configPath = Path.Combine("plugins", configFileName + ".toml");

        var existingConfig = File.ReadAllText(configPath);
        return Toml.ToModel<HeadlessConfig>(existingConfig);
    }

    public IReadOnlyList<string> GetAllConfigNames()
        => Directory.GetFiles("plugins", "*.toml")
           .Select(f => f.Replace("plugins/", ""))
           .ToList();

    public (bool success, string configFileName) Remove(string configNameToRemove)
    {
        if (!configNameToRemove.Contains(".toml"))
            configNameToRemove += ".toml";

        var before = GetAllConfigNames().Count;
        File.Delete(Path.Combine("plugins", configNameToRemove));
        var after = GetAllConfigNames().Count;
        return (before - after == 1, configNameToRemove);
    }
}