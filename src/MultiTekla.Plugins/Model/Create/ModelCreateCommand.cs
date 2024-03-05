namespace MultiTekla.Plugins.Model.Create;

[Command("model create", Description = "Creates a model")]
public class ModelCreateCommand : CommandBase<ModelCreatePlugin>
{
    protected override ValueTask Execute(IConsole console, ModelCreatePlugin plugin)
    {
        plugin.RunPlugin();
        return default;
    }
}
