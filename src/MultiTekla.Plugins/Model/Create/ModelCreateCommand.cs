namespace MultiTekla.Plugins.Model.Create;

[Command("model create", Description = "Creates a model")]
public sealed class ModelCreateCommand : CommandBase<ModelCreatePlugin>
{
    [CommandParameter(0, Name = "MODEL NAME", Description = "The name of the model being created", IsRequired = false)]
    public override string? ModelName { get; init; }

    [CommandOption("headless", 'l', Description = "Run plugin with headless Tekla")]
    public override bool IsHeadlessMode { get; init; } = true;

    [CommandOption("config", 'c', Description = "Config to use for headless run")]
    public override string ConfigName { get; init; } = "default";

    [CommandOption("server", 's', Description = "Multi-user server")]
    public string? Server { get; set; }

    [CommandOption("template", 't', Description = "Model template name")]
    public string? Template { get; set; }

    protected override ValueTask Execute(IConsole console, ModelCreatePlugin plugin)
    {
        plugin.Template = Template;
        plugin.Server = Server;
        plugin.RunPlugin();
        return default;
    }
}
