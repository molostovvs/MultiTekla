using MultiTekla.Contracts;
using Tomlyn;

namespace MultiTekla.Plugins;

public class HeadlessConfigPlugin : IPlugin<HeadlessConfig>
{
    public string? TeklaBinDirectory { get; set; }
    public string? EnvironmentIniPath { get; set; }
    public string? RoleIniPath { get; set; }
    public string? ModelPath { get; set; }

    public HeadlessConfig Run()
    {
        var toml = """
                   TsBinDirectory = C:\TeklaStructures\2022.0\bin\
                   EnvironmentIniPath = C:\TeklaStructures\2022.0\Environments\default\env_Default_environment.ini
                   RoleIniPath = C:\TeklaStructures\2022.0\Environments\default\role_Steel_Detailer.ini
                   ModelPath = C:\TeklaStructuresModels\test-model
                   """;

        var model = Toml.ToModel<HeadlessConfig>(toml);

        TeklaBinDirectory = model.TeklaBinDirectory;
        EnvironmentIniPath = model.EnvironmentIniPath;
        RoleIniPath = model.RoleIniPath;
        ModelPath = model.ModelPath;

        return model;
    }
}