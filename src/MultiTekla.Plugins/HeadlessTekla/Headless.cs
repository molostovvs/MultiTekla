using System;
using System.IO;
using System.Text;
using TSS = Tekla.Structures.Service;

namespace MultiTekla.Plugins;

public sealed class Headless : IDisposable
{
    public TSS.TeklaStructuresService? TeklaService { get; private set; }
    public string TeklaBinPath { get; private set; }
    public string EnvironmentIniPath { get; private set; }
    public string RoleIniPath { get; private set; }
    public string ModelPath { get; private set; }

    public void Initialize(
        DirectoryInfo? modelName = null,
        string? license = null,
        string? trimbleIdentityIdToken = null,
        string? trimbleAccessToken = null,
        bool useExistingLogin = false,
        string? organizationId = null)
    {
        modelName ??= new DirectoryInfo(ModelPath);

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
        TeklaBinPath = headless.TeklaBinPath;
        ModelPath = headless.ModelPath;
        EnvironmentIniPath = headless.EnvironmentIniPath;
        RoleIniPath = headless.RoleIniPath;
        TeklaService = new TSS.TeklaStructuresService(
            new DirectoryInfo(TeklaBinPath),
            "EN",
            new FileInfo(EnvironmentIniPath),
            new FileInfo(RoleIniPath)
        );
    }

    #region Builder

    public class BuildHeadless : IEmptyHeadless, ICompletedHeadless, IHeadlessWithBinDirectory,
                                 IHeadlessWithModelPath,
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
                TeklaBinPath = @"C:\TeklaStructures\2022.0\bin\",
                ModelPath = @"C:\TeklaStructuresModels\test-model",
                EnvironmentIniPath =
                    @"C:\TeklaStructures\2022.0\Environments\default\env_Default_environment.ini",
                RoleIniPath =
                    @"C:\TeklaStructures\2022.0\Environments\default\role_Steel_Detailer.ini",
            };

            return new BuildHeadless(headless);
        }

        public IHeadlessWithBinDirectory BinDirectory(string pathToBinDirectory)
        {
            _headless.TeklaBinPath = pathToBinDirectory
                ?? throw new ArgumentNullException(nameof(pathToBinDirectory));
            return this;
        }

        ICompletedHeadless ICompletedHeadless.ModelPath(string modelPath)
        {
            _headless.ModelPath = modelPath ?? throw new ArgumentNullException(nameof(modelPath));
            return this;
        }

        ICompletedHeadless ICompletedHeadless.EnvironmentPath(string environmentIniPath)
        {
            _headless.EnvironmentIniPath = environmentIniPath
                ?? throw new ArgumentNullException(nameof(environmentIniPath));
            return this;
        }

        ICompletedHeadless ICompletedHeadless.BinDirectory(string pathToBinDirectory)
        {
            _headless.TeklaBinPath = pathToBinDirectory
                ?? throw new ArgumentNullException(nameof(pathToBinDirectory));
            return this;
        }

        public IHeadlessWithEnvironmentPath EnvironmentPath(string environmentIniPath)
        {
            _headless.EnvironmentIniPath = environmentIniPath
                ?? throw new ArgumentNullException(nameof(environmentIniPath));
            return this;
        }

        public ICompletedHeadless RolePath(string roleIniPath)
        {
            _headless.RoleIniPath = roleIniPath;
            return this;
        }

        public Headless Build()
        {
            var headless = new Headless(_headless);
            return headless;
        }

        IHeadlessWithModelPath IHeadlessWithBinDirectory.ModelPath(string modelPath)
        {
            _headless.ModelPath = modelPath ?? throw new ArgumentNullException(nameof(modelPath));
            return this;
        }
    }

    #endregion

    public void Dispose()
        => TeklaService?.Dispose();

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"TS binary directory: {TeklaBinPath}");
        sb.AppendLine($"TS model path: {ModelPath}");
        sb.AppendLine($"TS environment ini path: {EnvironmentIniPath}");
        sb.AppendLine($"TS role ini path: {RoleIniPath}");

        return sb.ToString();
    }
}