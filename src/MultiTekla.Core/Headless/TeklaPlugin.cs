using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using MultiTekla.Contracts;

namespace MultiTekla.Core.Headless;

/// <summary>
/// Plugin for headless tekla initialization
/// </summary>
public class TeklaPlugin : PluginBase<TimeSpan>
{
    /// <summary>
    /// Initialize the headless tekla/>
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">Config is null -or- Config file is invalid -or- Directory doesnt exist with path Config.ModelsPath + Config.ModelName
    /// </exception>
    protected override TimeSpan Run()
    {
        if (Config is null)
            throw new ArgumentNullException(
                nameof(Config),
                "You should provide config file when running tekla tekla"
            );

        if (Config.TeklaBinPath is null or "" || Config.EnvironmentIniPath is null or ""
            || Config.RoleIniPath is null or "")
            throw new ArgumentNullException(
                $"Config file is invalid, check {nameof(Config.TeklaBinPath)}, {
                    nameof(Config.EnvironmentIniPath)}, {nameof(Config.RoleIniPath)
                    }"
            );

        AppDomain.CurrentDomain.AssemblyResolve +=
            (_, a) => TeklaBinResolve(a, @"C:\TeklaStructures\2022.0\bin\");

        var sw = new Stopwatch();
        sw.Start();

        var headlessTs = Tekla.BuildHeadless.With()
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

    /// <summary>
    /// Resolves assembly loading for Tekla Structures binaries.
    /// </summary>
    /// <param name="args">The <see cref="ResolveEventArgs"/> containing the event data.</param>
    /// <param name="tsBinDirectory">The directory path where Tekla Structures binaries are located.</param>
    /// <returns>
    /// The loaded assembly if found in the specified directory; otherwise, <see langword="null"/>.
    /// </returns>
    /// <remarks>
    /// This method is used to resolve assembly loading events for Tekla Structures binaries.
    /// It attempts to load the requested assembly from the specified directory.
    /// </remarks>
    private static Assembly? TeklaBinResolve(ResolveEventArgs args, string tsBinDirectory)
    {
        var requestedAssembly = new AssemblyName(args.Name);

        return File.Exists(Path.Combine(tsBinDirectory,      requestedAssembly.Name + ".dll"))
            ? Assembly.LoadFile(Path.Combine(tsBinDirectory, requestedAssembly.Name + ".dll"))
            : null;
    }
}