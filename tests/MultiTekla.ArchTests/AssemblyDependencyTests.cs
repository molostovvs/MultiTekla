using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.NUnit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace MultiTekla.ArchTests;

public class AssemblyDependencyTests
{
    private static readonly Architecture Architecture = new ArchLoader()
        .LoadAssemblies(
                System.Reflection.Assembly.Load("MultiTekla.CLI"),
                System.Reflection.Assembly.Load("MultiTekla.Contracts"),
                System.Reflection.Assembly.Load("MultiTekla.Core"),
                System.Reflection.Assembly.Load("MultiTekla.Plugins"))
        .Build();

    private readonly IObjectProvider<IType> _contractsAssembly = Types().That().ResideInAssembly(typeof(Contracts.PluginBase).Assembly).As("Contracts");

    private readonly IObjectProvider<IType> _coreAssembly = Types().That().ResideInAssembly(typeof(Core.PluginManager).Assembly).As("Core");

    private readonly IObjectProvider<IType> _pluginsAssembly = Types().That().ResideInAssembly(typeof(Plugins.Model.ModelCommand).Assembly).As("Plugins");

    private readonly IObjectProvider<IType> _cliAssembly = Types().That().ResideInAssembly(typeof(CLI.Program).Assembly).As("Cli");

    [Test]
    public void Contracts_depends_only_on_system_and_clifx()
    {
        var result = Types()
            .That()
            .Are(_contractsAssembly)
            .Should()
            .DependOnAny("System.+", true)
            .OrShould()
            .DependOnAny("CliFx.+", true);

        result.Check(Architecture);
    }

    [Test]
    public void Core_depends_only_on_contracts()
    {
        var result = Types()
            .That()
            .Are(_coreAssembly)
            .Should()
            .DependOnAny(_contractsAssembly)
            .OrShould()
            .DependOnAny("System.+", true);

        result.Check(Architecture);
    }

    [Test]
    public void Plugins_depends_only_on_contracts_system_and_clifx()
    {
        var result = Types()
            .That()
            .Are(_pluginsAssembly)
            .Should()
            .DependOnAny(_contractsAssembly)
            .OrShould()
            .DependOnAny("System.+", true)
            .OrShould()
            .DependOnAny("CliFx.+", true);

        result.Check(Architecture);
    }

    [Test]
    public void Cli_depends_only_on_core_and_contracts()
    {
        var result = Types()
            .That()
            .Are(_cliAssembly)
            .Should()
            .DependOnAny(_coreAssembly)
            .OrShould()
            .DependOnAny(_contractsAssembly);

        result.Check(Architecture);
    }
}
