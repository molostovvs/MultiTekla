using System.Text;

namespace MultiTekla.Plugins.Config;

public class HeadlessConfig
{
    public string? Name { get; init; }
    public string? TeklaBinPath { get; init; }
    public string? EnvironmentIniPath { get; init; }
    public string? RoleIniPath { get; init; }
    public string? ModelPath { get; set; }

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.Append($"Config name = {Name}\n");
        sb.Append($"{nameof(TeklaBinPath)} = {TeklaBinPath}\n");
        sb.Append($"{nameof(EnvironmentIniPath)} = {EnvironmentIniPath}\n");
        sb.Append($"{nameof(RoleIniPath)} = {RoleIniPath}\n");
        sb.Append($"{nameof(ModelPath)} = {ModelPath}");

        return sb.ToString();
    }

    public HeadlessConfig CreateFrom(HeadlessConfig? config)
    {
        if (config is null)
            throw new ArgumentNullException(nameof(config));

        return new HeadlessConfig
        {
            Name = config.Name ?? Name,
            TeklaBinPath = config.TeklaBinPath ?? TeklaBinPath,
            EnvironmentIniPath = config.EnvironmentIniPath ?? EnvironmentIniPath,
            RoleIniPath = config.RoleIniPath ?? RoleIniPath,
            ModelPath = config.ModelPath ?? ModelPath,
        };
    }
}