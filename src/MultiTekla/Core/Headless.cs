using System.Text;
using MultiTekla.Plugins;
using TSS = Tekla.Structures.Service;
using TSM = Tekla.Structures.Model;

namespace MultiTekla.Core;

public sealed class Headless : IDisposable
{
    public TSS.TeklaStructuresService? TeklaService { get; private set; }
    public string TsBinDirectory { get; private set; }
    public string ModelsPath { get; private set; }
    public string ModelName { get; private set; }
    public string EnvironmentIniPath { get; private set; }
    public string RoleIniPath { get; private set; }
    public List<IPlugin> Plugins = new() { new ModelNamePlugin(), };

    public void RunPlugins()
    {
        foreach (var p in Plugins)
            p.Run();
    }

    public void Initialize(
        DirectoryInfo? modelName = null,
        string? license = null,
        string? trimbleIdentityIdToken = null,
        string? trimbleAccessToken = null,
        bool useExistingLogin = false,
        string? organizationId = null)
    {
        modelName ??= new DirectoryInfo(Path.Combine(ModelsPath, ModelName));

        TeklaService?.Initialize(
            modelName,
            license,
            trimbleIdentityIdToken,
            trimbleAccessToken,
            useExistingLogin,
            organizationId
        );
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Headless() {}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    private Headless(Headless headless)
    {
        TsBinDirectory = headless.TsBinDirectory;
        ModelsPath = headless.ModelsPath;
        ModelName = headless.ModelName;
        EnvironmentIniPath = headless.EnvironmentIniPath;
        RoleIniPath = headless.RoleIniPath;
        TeklaService = new TSS.TeklaStructuresService(
            new DirectoryInfo(TsBinDirectory),
            "EN",
            new FileInfo(EnvironmentIniPath),
            new FileInfo(RoleIniPath)
        );
    }

    #region Builder

    public class BuildHeadless : IEmptyHeadless, ICompletedHeadless, IHeadlessWithBinDirectory,
                                 IHeadlessWithModelsPath, IHeadlessWithModelName,
                                 IHeadlessWithEnvironmentPath
    {
        private readonly Headless _headless = new();

        private BuildHeadless() {}

        private BuildHeadless(Headless headless)
            => _headless = headless;

        public static IEmptyHeadless With()
            => new BuildHeadless();

        public static ICompletedHeadless Default()
        {
            var headless = new Headless()
            {
                TsBinDirectory = @"C:\TeklaStructures\2022.0\bin\",
                ModelsPath = @"C:\TeklaStructuresModels\",
                ModelName = "test-model",
                EnvironmentIniPath =
                    @"C:\TeklaStructures\2022.0\Environments\default\env_Default_environment.ini",
                RoleIniPath =
                    @"C:\TeklaStructures\2022.0\Environments\default\role_Steel_Detailer.ini",
            };

            return new BuildHeadless(headless);
        }

        public IHeadlessWithBinDirectory BinDirectory(string pathToBinDirectory)
        {
            _headless.TsBinDirectory = pathToBinDirectory
                ?? throw new ArgumentNullException(nameof(pathToBinDirectory));
            return this;
        }

        ICompletedHeadless ICompletedHeadless.ModelsPath(string pathToModels)
        {
            _headless.ModelsPath =
                pathToModels ?? throw new ArgumentNullException(nameof(pathToModels));
            return this;
        }

        ICompletedHeadless ICompletedHeadless.ModelName(string modelName)
        {
            _headless.ModelName = modelName ?? throw new ArgumentNullException(nameof(modelName));
            return this;
        }

        ICompletedHeadless ICompletedHeadless.EnvironmentPath(string pathToEnvironmentIni)
        {
            _headless.EnvironmentIniPath = pathToEnvironmentIni
                ?? throw new ArgumentNullException(nameof(pathToEnvironmentIni));
            return this;
        }

        ICompletedHeadless ICompletedHeadless.BinDirectory(string pathToBinDirectory)
        {
            _headless.TsBinDirectory = pathToBinDirectory
                ?? throw new ArgumentNullException(nameof(pathToBinDirectory));
            return this;
        }

        public IHeadlessWithModelsPath ModelsPath(string pathToModels)
        {
            _headless.ModelsPath =
                pathToModels ?? throw new ArgumentNullException(nameof(pathToModels));
            return this;
        }

        public IHeadlessWithModelName ModelName(string modelName)
        {
            _headless.ModelName = modelName ?? throw new ArgumentNullException(nameof(modelName));
            return this;
        }

        public IHeadlessWithEnvironmentPath EnvironmentPath(string pathToEnvironmentIni)
        {
            _headless.EnvironmentIniPath = pathToEnvironmentIni
                ?? throw new ArgumentNullException(nameof(pathToEnvironmentIni));
            return this;
        }

        public ICompletedHeadless RolePath(string pathToRoleIni)
        {
            _headless.RoleIniPath = pathToRoleIni;
            return this;
        }

        public Headless Build()
        {
            var headless = new Headless(_headless);
            return headless;
        }
    }

    #endregion

    public void Dispose()
        => TeklaService?.Dispose();

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"TS binary directory: {TsBinDirectory}");
        sb.AppendLine($"TS models path: {ModelsPath}");
        sb.AppendLine($"TS model name: {ModelName}");
        sb.AppendLine($"TS environment ini path: {EnvironmentIniPath}");
        sb.AppendLine($"TS role ini path: {RoleIniPath}");

        return sb.ToString();
    }
}