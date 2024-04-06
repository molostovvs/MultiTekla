namespace MultiTekla.Plugins.Model.RunMacro;

[Command("model run macro", Description = "Run the macro")]
public sealed class RunMacroCommand : CommandBase<RunMacroPlugin>
{
    [CommandOption("macro-name", 'r', Description = "The name of the macro to run")]
    public string? MacroName { get; init; }

    [CommandParameter(0, Name = "MODEL NAME", Description = "The name of the model to open", IsRequired = false)]
    public override string? ModelName { get; init; }

    [CommandOption("headless", 'l', Description = "Run plugin with headless Tekla")]
    public override bool IsHeadlessMode { get; init; } = true;

    [CommandOption("config", 'c', Description = "Config to use for headless run")]
    public override string ConfigName { get; init; } = "default";

    protected override ValueTask Execute(IConsole console, RunMacroPlugin plugin)
    {
        if (string.IsNullOrEmpty(MacroName))
            throw new ArgumentException("You must provide the macro name");

        plugin.MacroName = MacroName;
        plugin.RunPlugin();

        return default;
    }
}
