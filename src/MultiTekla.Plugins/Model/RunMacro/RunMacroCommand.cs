namespace MultiTekla.Plugins.Model.RunMacro;

[Command("model run macro", Description = "Run the macro")]
public class RunMacroCommand : CommandBase<RunMacroPlugin>
{
    [CommandOption("macro-name", 'r', Description = "Macro name")]
    public string? MacroName { get; init; }

    protected override ValueTask Execute(IConsole console, RunMacroPlugin plugin)
    {
        if (string.IsNullOrEmpty(MacroName))
            throw new ArgumentException("You must provide the macro name");

        plugin.MacroName = MacroName;
        plugin.RunPlugin();

        return default;
    }
}
