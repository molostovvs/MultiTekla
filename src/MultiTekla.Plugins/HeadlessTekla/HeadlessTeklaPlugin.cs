using System.Diagnostics;
using System.IO;
using System.Reflection;
using MultiTekla.Plugins.Config;
using MultiTekla.Plugins.Core;

namespace MultiTekla.Plugins;

public class HeadlessTeklaPlugin : IPlugin<TimeSpan>
{
    public HeadlessConfig? Config { get; set; }

    public TimeSpan Run()
    {
        if (Config is null)
            throw new ArgumentNullException(
                nameof(Config),
                "You should provide config file when running headless tekla"
            );

        if (Config.TeklaBinPath is null || Config.EnvironmentIniPath is null
            || Config.RoleIniPath is null)
            throw new ArgumentException(
                $"Config file is invalid, check {nameof(Config.TeklaBinPath)}, {
                    nameof(Config.EnvironmentIniPath)}, {nameof(Config.RoleIniPath)}"
            );

        AppDomain.CurrentDomain.AssemblyResolve +=
            (_, a) => TeklaBinResolve(a, @"C:\TeklaStructures\2022.0\bin\");

        var sw = new Stopwatch();
        sw.Start();

        var headlessTs = Headless.BuildHeadless.With()
           .BinDirectory(Config.TeklaBinPath)
           .EnvironmentPath(Config.EnvironmentIniPath)
           .RolePath(Config.RoleIniPath)
           .Build();

        if (Config.ModelsPath is null or "")
            throw new ArgumentNullException(
                nameof(Config.ModelsPath),
                $"{Config.ModelsPath} should not be null or empty"
            );

        if (!Directory.Exists(Config.ModelsPath))
            throw new ArgumentNullException(
                nameof(Config.ModelsPath),
                $"Directory with path {nameof(Config.ModelsPath)} doesn't exist"
            );

        var initPath = Path.Combine(Config.ModelsPath, Config.ModelName);

        if (!Directory.Exists(initPath))
            Directory.CreateDirectory(initPath);

        headlessTs.Initialize(new DirectoryInfo(initPath));

        sw.Stop();

        return sw.Elapsed;
    }

    private static Assembly? TeklaBinResolve(ResolveEventArgs args, string tsBinDirectory)
    {
        var requestedAssembly = new AssemblyName(args.Name);

        return File.Exists(Path.Combine(tsBinDirectory,      requestedAssembly.Name + ".dll"))
            ? Assembly.LoadFile(Path.Combine(tsBinDirectory, requestedAssembly.Name + ".dll"))
            : null;
    }
}