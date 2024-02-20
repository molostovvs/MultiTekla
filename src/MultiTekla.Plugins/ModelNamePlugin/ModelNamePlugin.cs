namespace MultiTekla.Plugins.ModelNamePlugin;

public class ModelNamePlugin : PluginBase<Object>
{
    protected override object Run()
    {
        var model = new Tekla.Structures.Model.Model();
        var modelInfo = model.GetInfo();

        Console.WriteLine(
            "Model name: {0} \nModel path: {1} \nModel is SingleUser: {2}",
            modelInfo.ModelName,
            modelInfo.ModelPath,
            modelInfo.SingleUserModel
        );

        return new object();
    }
}