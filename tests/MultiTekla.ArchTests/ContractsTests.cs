using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.NUnit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace MultiTekla.ArchTests;

public class ContractsTests
{
    private static readonly Architecture Architecture = new ArchLoader()
       .LoadAssemblies(
            System.Reflection.Assembly.Load("MultiTekla.CLI"),
            System.Reflection.Assembly.Load("MultiTekla.Contracts"),
            System.Reflection.Assembly.Load("MultiTekla.Core"),
            System.Reflection.Assembly.Load("MultiTekla.Plugins")
        )
       .Build();

    private readonly IObjectProvider<IType> _contractsAssembly =
        Types().That().ResideInAssembly(typeof(Contracts.PluginBase).Assembly).As("Contracts");

    [Test]
    public void Test()
    {
        var result = PropertyMembers()
            .That()
            .HavePublicGetter()
            .And()
            .AreDeclaredIn(_contractsAssembly)
            .And()
            .HaveNameEndingWith("Plugin")
            .Should()
            .HaveAttributeWithArguments(typeof(System.Composition.ImportMetadataConstraintAttribute),
                                        new object[] { "name", });

        result.Check(Architecture);
    }
}
