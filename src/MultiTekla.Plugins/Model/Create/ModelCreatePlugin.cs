using System.IO;
using Tekla.Structures.Model;

namespace MultiTekla.Plugins.Model.Create;

public class ModelCreatePlugin : PluginBase
{
    public string? Template { get; set; }
    public string? Server { get; set; }

    protected override void Run()
    {
        var handler = new ModelHandler();

        if (Config is null && IsHeadlessMode is not false)
            throw new ArgumentException(nameof(Config), $"{nameof(Config)} is not specified");

        if (string.IsNullOrEmpty(Config?.ModelName) && IsHeadlessMode)
            throw new ArgumentException("Model name is not specified for headless run");

        //TODO: implement plugin for non-headless run
        if (IsHeadlessMode)
        {
            var singleModelCreateSuccess = handler.CreateNewSingleUserModel(
                Config!.ModelName,
                Config.ModelsPath,
                Template ?? ""
            );

            if (!string.IsNullOrEmpty(Server) && singleModelCreateSuccess)
            {
                if (Server is null or "")
                    throw new ArgumentException(
                        nameof(Server),
                        $"{nameof(Server)} is not specified"
                    );

                var multiuserConvertResult =
                    Tekla.Structures.ModelInternal.Operation.dotConvertAndOpenAsMultiUserModel(
                        Path.Combine(Config.ModelsPath, Config!.ModelName),
                        Server
                    );
            }
        }

        handler.Close();
    }
}
