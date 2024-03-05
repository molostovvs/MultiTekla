using System.IO;
using Tomlyn;

namespace MultiTekla.Plugins.Headless.Config;

public class CreateHeadlessConfigPlugin : PluginBase
{
    protected override void Run()
    {
        var toml = Toml.FromModel(Config ?? new HeadlessConfig());

        File.WriteAllText(
            Config?.Name is null
                ? Path.Combine("plugins", "default.toml")
                : Path.Combine("plugins", Config.Name + ".toml"),
            toml
        );
    }
}