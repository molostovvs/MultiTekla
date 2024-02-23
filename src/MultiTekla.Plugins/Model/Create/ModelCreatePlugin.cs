using System.IO;
using Tekla.Structures.Model;

namespace MultiTekla.Plugins.Model.Create;

public class ModelCreatePlugin : PluginBase<bool>
{
    public bool MultiUser { get; set; }
    public string? Template { get; set; }
    public string? ServerName { get; set; }

    protected override bool Run()
    {
        var handler = new ModelHandler();

        if (ModelName is null or "" && Headless)
            throw new ArgumentException(nameof(ModelName), $"{nameof(ModelName)} is not specified");

        if (Config is null && Headless)
            throw new ArgumentException(nameof(Config), $"{nameof(Config)} is not specified");

        //TODO: implement plugin for non-headless run
        if (Headless)
        {
            var singleModelCreateSuccess = handler.CreateNewSingleUserModel(
                ModelName,
                Config.ModelsPath,
                Template ?? ""
            );

            if (MultiUser && singleModelCreateSuccess)
            {
                if (ServerName is null or "")
                    throw new ArgumentException(
                        nameof(ServerName),
                        $"{nameof(ServerName)} is not specified"
                    );

                var multiuserConvertResult =
                    Tekla.Structures.ModelInternal.Operation.dotConvertAndOpenAsMultiUserModel(
                        Path.Combine(Config.ModelsPath, Config.ModelName),
                        ServerName
                    );

                return multiuserConvertResult;
            }

            return singleModelCreateSuccess;
        }

        return false;
    }
}