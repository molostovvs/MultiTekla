using CliFx;

namespace MultiTekla.Plugins;

[Command("model", Description = "Model related commands")]
public class ModelCommand : ICommand
{
    public ValueTask ExecuteAsync(IConsole console)
        => default;
}