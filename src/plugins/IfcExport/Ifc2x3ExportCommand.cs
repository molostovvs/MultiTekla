namespace IfcExport;

[Command("export ifc2x3", Description = "Export model to IFC2x3")]
public sealed class Ifc2x3ExportCommand : CommandBase<Ifc2x3ExportPlugin>
{
    [CommandParameter(0, Name = "MODEL NAME", Description = "The name of the model to be exported to IFC", IsRequired = false)]
    public override string? ModelName { get; init; }

    [CommandOption("output", 'o', Description = "Path to file for output")]
    public string? OutputFile { get; init; }

    [CommandOption("config", 'c', Description = "Config to use for headless run")]
    public override string ConfigName { get; init; } = "default";

    [CommandOption("headless", 'l', Description = "Run plugin with headless Tekla")]
    public override bool IsHeadlessMode { get; init; } = true;

    protected override ValueTask Execute(IConsole console, Ifc2x3ExportPlugin plugin)
    {
        plugin.OutputFile = OutputFile;
        plugin.RunPlugin();
        return default;
    }
}
