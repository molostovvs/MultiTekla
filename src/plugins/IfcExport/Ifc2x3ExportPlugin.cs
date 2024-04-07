namespace IfcExport;

public sealed class Ifc2x3ExportPlugin : PluginBase
{
    public string? OutputFile { get; set; }

    protected override void Run()
    {
        if (Config is null && IsHeadlessMode is not false)
            throw new ArgumentException(nameof(Config), $"{nameof(Config)} is not specified");

        if (string.IsNullOrEmpty(Config?.ModelName) && IsHeadlessMode)
            throw new ArgumentException("Model name is not specified for headless run");

        var componentInput = new TSM.ComponentInput();
        _ = componentInput.AddOneInputPosition(new TSG.Point(0, 0, 0));
        var component = new TSM.Component(componentInput)
        {
            Name = "ExportIFC",
            Number = TSM.BaseComponent.PLUGIN_OBJECT_NUMBER,
        };

        _ = component.LoadAttributesFromFile("my_test");

        if (!string.IsNullOrEmpty(OutputFile))
            component.SetAttribute("OutputFile", OutputFile);

        _ = component.Insert();

        new TSM.ModelHandler().Close();
    }
}
