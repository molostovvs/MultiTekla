using System.Collections.Generic;
using System.Composition.Convention;
using System.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using MultiTekla.Contracts;
using MultiTekla.Core.Headless;
using ICommand = CliFx.ICommand;

namespace MultiTekla.Core;

public class PluginManager
{
    private string PluginsFolderName { get; }

    public PluginManager(string pluginsFolderName = "plugins")
        => PluginsFolderName = pluginsFolderName;

    private IReadOnlyList<Assembly> LoadPluginAssemblies()
        => Directory.GetFiles(
                Path.Combine(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty,
                    PluginsFolderName
                ),
                "*.dll"
            )
           .Select(Assembly.LoadFrom)
           .Append(Assembly.GetExecutingAssembly())
           .ToList();

    public ContainerConfiguration ConfigurePluginComposition()
    {
        var assemblies = LoadPluginAssemblies();

        var cb = new ConventionBuilder();

        cb.ForType<TeklaPlugin>().Export<PluginBase>().Shared();

        cb.ForTypesDerivedFrom<IHeadlessConfigPlugin>().Export<IHeadlessConfigPlugin>();

        cb.ForTypesDerivedFrom<PluginBase>()
           .Export()
           .ImportProperties(
                p => p.Name.Contains("Plugin") && p.DeclaringType != typeof(TeklaPlugin)
            );

        cb.ForTypesDerivedFrom<ICommand>()
           .Export<ICommand>()
           .ImportProperties(p => p?.Name.Contains("Plugin") ?? false);

        var configuration = new ContainerConfiguration().WithAssemblies(assemblies, cb);
        return configuration;
    }
}