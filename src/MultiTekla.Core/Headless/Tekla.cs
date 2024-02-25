using System;
using System.IO;
using MultiTekla.Contracts;
using TSS = Tekla.Structures.Service;

namespace MultiTekla.Core.Headless;

public sealed class Tekla : IDisposable
{
    private TSS.TeklaStructuresService? TeklaService { get; set; }
    private HeadlessConfig Config { get; set; } = new();

    public void Initialize(
        DirectoryInfo? modelPath = null,
        string? license = null,
        string? trimbleIdentityIdToken = null,
        string? trimbleAccessToken = null,
        bool useExistingLogin = false,
        string? organizationId = null)
    {
        if (Config.ModelsPath is null)
            throw new ArgumentNullException($"{nameof(Config.ModelsPath)} must be provided");

        if (Config.ModelName is null)
            throw new ArgumentNullException($"{nameof(Config.ModelName)} must be provided");

        modelPath ??= new DirectoryInfo(Path.Combine(Config.ModelsPath, Config.ModelName));

        TeklaService?.Initialize(
            modelPath,
            "FULL",
            trimbleIdentityIdToken,
            trimbleAccessToken,
            useExistingLogin,
            organizationId
        );
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Tekla() {}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    private Tekla(Tekla tekla)
    {
        Config = tekla.Config;

        TeklaService = new TSS.TeklaStructuresService(
            new DirectoryInfo(
                Config.TeklaBinPath ?? throw new ArgumentNullException(nameof(Config.TeklaBinPath))
            ),
            "English",
            new FileInfo(
                Config.EnvironmentIniPath
                ?? throw new ArgumentNullException(nameof(Config.EnvironmentIniPath))
            ),
            new FileInfo(
                Config.RoleIniPath ?? throw new ArgumentNullException(nameof(Config.RoleIniPath))
            )
        );
    }

    #region Builder

    public class BuildHeadless : IEmptyHeadless, ICompletedHeadless, IHeadlessWithBinDirectory,
                                 IHeadlessWithEnvironmentPath, IHeadlessWithRolePath,
                                 IHeadlessWithModelsPath
    {
        private readonly Tekla _tekla = new();

        private BuildHeadless() {}

        private BuildHeadless(Tekla tekla)
            => _tekla = tekla;

        public static IEmptyHeadless With()
            => new BuildHeadless();

        public static ICompletedHeadless Default()
        {
            var headless = new Tekla
            {
                Config = new()
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
            _tekla.Config.TeklaBinPath = pathToBinDirectory
                ?? throw new ArgumentNullException(nameof(pathToBinDirectory));
            return this;
        }

        public ICompletedHeadless Config(HeadlessConfig config)
        {
            _tekla.Config = config ?? throw new ArgumentNullException(nameof(config));
            return this;
        }

        public IHeadlessWithModelsPath ModelsPath(string modelsPath)
        {
            _tekla.Config.ModelsPath =
                modelsPath ?? throw new ArgumentNullException(nameof(modelsPath));
            return this;
        }

        ICompletedHeadless ICompletedHeadless.ModelsPath(string modelsPath)
        {
            _tekla.Config.ModelsPath =
                modelsPath ?? throw new ArgumentNullException(nameof(modelsPath));
            return this;
        }

        public ICompletedHeadless ModelName(string modelName)
        {
            _tekla.Config.ModelName =
                modelName ?? throw new ArgumentNullException(nameof(modelName));
            return this;
        }

        ICompletedHeadless ICompletedHeadless.EnvironmentPath(string environmentIniPath)
        {
            _tekla.Config.EnvironmentIniPath = environmentIniPath
                ?? throw new ArgumentNullException(nameof(environmentIniPath));
            return this;
        }

        ICompletedHeadless ICompletedHeadless.BinDirectory(string pathToBinDirectory)
        {
            _tekla.Config.TeklaBinPath = pathToBinDirectory
                ?? throw new ArgumentNullException(nameof(pathToBinDirectory));
            return this;
        }

        public IHeadlessWithEnvironmentPath EnvironmentPath(string environmentIniPath)
        {
            _tekla.Config.EnvironmentIniPath = environmentIniPath
                ?? throw new ArgumentNullException(nameof(environmentIniPath));
            return this;
        }

        public ICompletedHeadless RolePath(string roleIniPath)
        {
            _tekla.Config.RoleIniPath = roleIniPath;
            return this;
        }

        public Tekla Build()
        {
            var headless = new Tekla(_tekla);
            return headless;
        }

        IHeadlessWithRolePath IHeadlessWithEnvironmentPath.RolePath(string roleIniPath)
        {
            _tekla.Config.RoleIniPath =
                roleIniPath ?? throw new ArgumentNullException(nameof(roleIniPath));
            return this;
        }
    }

    #endregion

    public void Dispose()
        => TeklaService?.Dispose();

    public override string ToString()
        => Config.ToString();
}