using CliFx;

namespace MultiTekla.Plugins.Model;

[Command("model", Description = "Model related commands")]
public class ModelCommand : ICommand
{
    public ValueTask ExecuteAsync(IConsole console)
        => default;
}