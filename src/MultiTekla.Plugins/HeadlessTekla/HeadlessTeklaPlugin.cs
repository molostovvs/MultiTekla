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

        if (Config.ModelPath is null or "")
            throw new ArgumentNullException(
                nameof(Config.ModelPath),
                $"{Config.ModelPath} should not be null or empty"
            );

        if (!Directory.Exists(Config.ModelPath))
            throw new ArgumentNullException(
                nameof(Config.ModelPath),
                $"Directory with path {nameof(Config.ModelPath)} doesn't exist"
            );

        headlessTs.Initialize(new DirectoryInfo(Config.ModelPath));

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