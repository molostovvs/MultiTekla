using System.IO;
using System.Text;
using MultiTekla.Plugins.Config;
using TSS = Tekla.Structures.Service;

namespace MultiTekla.Plugins.Core;

public sealed class Headless : IDisposable
{
    public TSS.TeklaStructuresService? TeklaService { get; private set; }
    public HeadlessConfig Config { get; private set; } = new();

    public void Initialize(
        DirectoryInfo? modelPath = null,
        string? license = null,
        string? trimbleIdentityIdToken = null,
        string? trimbleAccessToken = null,
        bool useExistingLogin = false,
        string? organizationId = null)
    {
        modelPath ??= new DirectoryInfo(Path.Combine(Config.ModelsPath, Config.ModelName));

        TeklaService?.Initialize(
            modelPath,
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
        Config.TeklaBinPath = headless.Config.TeklaBinPath;
        Config.ModelsPath = headless.Config.ModelsPath;
        Config.EnvironmentIniPath = headless.Config.EnvironmentIniPath;
        Config.RoleIniPath = headless.Config.RoleIniPath;

        TeklaService = new TSS.TeklaStructuresService(
            new DirectoryInfo(Config.TeklaBinPath),
            "English",
            new FileInfo(Config.EnvironmentIniPath),
            new FileInfo(Config.RoleIniPath)
        );
    }

    #region Builder

    public class BuildHeadless : IEmptyHeadless, ICompletedHeadless, IHeadlessWithBinDirectory,
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
            var headless = new Headless
            {
                Config =
                {
                    TeklaBinPath = @"C:\TeklaStructures\2022.0\bin\",
                    EnvironmentIniPath =
                        @"C:\TeklaStructures\2022.0\Environments\default\env_Default_environment.ini",
                    RoleIniPath =
                        @"C:\TeklaStructures\2022.0\Environments\default\role_Steel_Detailer.ini",
                    ModelsPath = @"C:\TeklaStructuresModels\test-model",
                    ModelName = "test-model",
                },
            };

            return new BuildHeadless(headless);
        }

        public IHeadlessWithBinDirectory BinDirectory(string pathToBinDirectory)
        {
            _headless.Config.TeklaBinPath = pathToBinDirectory
                ?? throw new ArgumentNullException(nameof(pathToBinDirectory));
            return this;
        }

        ICompletedHeadless ModelPath(string modelPath)
        {
            _headless.Config.ModelsPath =
                modelPath ?? throw new ArgumentNullException(nameof(modelPath));
            return this;
        }

        ICompletedHeadless ICompletedHeadless.EnvironmentPath(string environmentIniPath)
        {
            _headless.Config.EnvironmentIniPath = environmentIniPath
                ?? throw new ArgumentNullException(nameof(environmentIniPath));
            return this;
        }

        ICompletedHeadless ICompletedHeadless.BinDirectory(string pathToBinDirectory)
        {
            _headless.Config.TeklaBinPath = pathToBinDirectory
                ?? throw new ArgumentNullException(nameof(pathToBinDirectory));
            return this;
        }

        public IHeadlessWithEnvironmentPath EnvironmentPath(string environmentIniPath)
        {
            _headless.Config.EnvironmentIniPath = environmentIniPath
                ?? throw new ArgumentNullException(nameof(environmentIniPath));
            return this;
        }

        public ICompletedHeadless RolePath(string roleIniPath)
        {
            _headless.Config.RoleIniPath = roleIniPath;
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
        => Config.ToString();
}