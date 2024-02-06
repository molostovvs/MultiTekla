using System;
using System.IO;
using System.Reflection;
using MultiTekla.Contracts;

namespace MultiTekla.Plugins;

public class HeadlessTeklaPlugin : IPlugin<object>
{
    /*private HeadlessConfig? Config { get; set; }
    
    public HeadlessTeklaPlugin(HeadlessConfig config)
        => Config = config;*/

    public object Run()
    {
        /*AppDomain.CurrentDomain.AssemblyResolve +=
            (_, a) => TeklaBinResolve(a, @"C:\TeklaStructures\2022.0\bin\");

        var headlessTs = Headless.BuildHeadless.With()
           .BinDirectory(Config?.TeklaBinDirectory)
           .ModelPath(Config?.ModelPath)
           .EnvironmentPath(Config?.EnvironmentIniPath)
           .RolePath(Config?.RoleIniPath)
           .Build();

        headlessTs.Initialize(new DirectoryInfo(Config.ModelPath));*/

        return new object();
    }

    private static Assembly? TeklaBinResolve(ResolveEventArgs args, string tsBinDirectory)
    {
        var requestedAssembly = new AssemblyName(args.Name);

        return File.Exists(Path.Combine(tsBinDirectory,      requestedAssembly.Name + ".dll"))
            ? Assembly.LoadFile(Path.Combine(tsBinDirectory, requestedAssembly.Name + ".dll"))
            : null;
    }
}