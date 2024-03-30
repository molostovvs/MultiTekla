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

    private readonly IObjectProvider<IType> _contractsAssembly = Types().That().ResideInAssembly("MultiTekla.Contracts").As("Contracts");

    private readonly IObjectProvider<IType> _coreAssembly = Types().That().ResideInAssembly("MultiTekla.Core").As("Core");

    private readonly IObjectProvider<IType> _pluginsAssembly = Types().That().ResideInAssembly("MultiTekla.Plugin").As("Plugins");

    private readonly IObjectProvider<IType> _cliAssembly = Types().That().ResideInAssembly("MultiTekla.CLI").As("Cli");

    [Test]
    public void Contracts_depends_only_on_system_libraries()
    {
        var result = Types().That()
            .Are(_contractsAssembly)
            .Should()
            .OnlyDependOn(Types().That().ResideInNamespace("System"));

        result.Check(Architecture);
    }

    [Test]
    public void Core_depends_only_on_contracts()
    {
        var result = Types().That()
            .Are(_coreAssembly)
            .Should()
            .DependOnAny(Types().That().Are(_contractsAssembly));

        result.Check(Architecture);
    }

    [Test]
    public void Plugins_depends_only_on_contracts()
    {
        var result = Types().That()
            .Are(_pluginsAssembly)
            .Should()
            .OnlyDependOn(Types().That().Are(_contractsAssembly));

        result.Check(Architecture);
    }

    [Test]
    public void Cli_depends_only_on_core_and_contracts()
    {
        var result = Types().That()
            .Are(_cliAssembly)
            .Should()
            .OnlyDependOn(Types().That().Are(_contractsAssembly).Or().Are(_coreAssembly));

        result.Check(Architecture);
    }
}
