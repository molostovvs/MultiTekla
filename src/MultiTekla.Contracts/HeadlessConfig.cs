using System;
using System.Text;

namespace MultiTekla.Contracts;

public class HeadlessConfig
{
    public string? Name { get; set; }
    public string? TeklaBinPath { get; set; }
    public string? EnvironmentIniPath { get; set; }
    public string? RoleIniPath { get; set; }
    public string? ModelsPath { get; set; }
    public string? ModelName { get; set; }

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine($"Config name = {Name}");
        sb.AppendLine($"{nameof(TeklaBinPath)} = {TeklaBinPath}");
        sb.AppendLine($"{nameof(EnvironmentIniPath)} = {EnvironmentIniPath}");
        sb.AppendLine($"{nameof(RoleIniPath)} = {RoleIniPath}");
        sb.AppendLine($"{nameof(ModelsPath)} = {ModelsPath}");
        sb.Append($"{nameof(ModelName)} = {ModelName}");

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
            ModelsPath = config.ModelsPath ?? ModelsPath,
            ModelName = config.ModelName ?? ModelName,
        };
    }
}