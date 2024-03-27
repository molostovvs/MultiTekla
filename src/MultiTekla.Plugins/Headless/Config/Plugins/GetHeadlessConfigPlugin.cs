using System.IO;
using Tomlyn;

namespace MultiTekla.Plugins.Headless.Config;

/// <summary>
/// Get headless config from config file
/// </summary>
public class GetHeadlessConfigPlugin : PluginBase
{
    public HeadlessConfig? Result { get; private set; }

    protected override void Run()
    {
        var configFileName =
            Config?.Name ?? throw new ArgumentNullException(
                nameof(Config.Name),
                "You must provide config name"
            );

        configFileName = configFileName.Replace(".toml", "");

        var configPath = Path.Combine("plugins", configFileName + ".toml");

        var existingConfig = File.ReadAllText(configPath);
        Result = Toml.ToModel<HeadlessConfig>(existingConfig);
        Config = Result;
    }
}
