using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;
using Tekla.Structures;

namespace MultiTekla.Plugins.Model.RunMacro;

public class RunMacroPlugin : PluginBase<bool>
{
    public string? MacroName { get; set; }

    private const string MacroDirectoryAdvancedParameterName = "XS_MACRO_DIRECTORY";

    private const string TeklaMacrosEntrypointAttributeName =
        "Tekla.Macros.Runtime.MacroEntryPointAttribute";

    /*
    // Calling RunMacro() method from Tekla API throws an exception with message
    // "RunMacro cannot be called from UI thread"
    // from tekla forum https://forum.tekla.com/topic/21139-export-drawings-headlessly/
    // we need to implement messaging from scratch for headless tekla
    // for safe RunMacro() method calling
    // at this time messaging implemented only in UI version of Tekla
    protected override bool Run()
    {
        if (MacroName is null or "")
            throw new ArgumentException("You must provide name of macro to run");

        return TSM.Operations.Operation.RunMacro(MacroName);
    }*/
    protected override bool Run()
    {
        if (MacroName is null or "")
            throw new ArgumentException("You must provide name of macro to run");

        var macroDirectories = GetMacroDirectories();

        var compiledMacroPath = FindCompiledMacro(macroDirectories, MacroName);

        if (compiledMacroPath is not null)
            return StartMacro(Assembly.LoadFile(compiledMacroPath));

        var pathToMacroFile = GetPathToMacroFile(macroDirectories, MacroName);

        StartMacro(CompileScript(pathToMacroFile).CompiledAssembly);

        return true;
    }

    private static bool StartMacro(Assembly macroAssembly)
    {
        var macroMethod = macroAssembly.GetTypes()
           .Select(t => t.GetMethod("Run", BindingFlags.Public | BindingFlags.Static))
            //TODO: implement discovering methods without attribute
           .First(
                m => m?.GetCustomAttributes()
                       .First(a => a.ToString() == TeklaMacrosEntrypointAttributeName)
                    is not null
            );

        if (macroMethod is null)
            throw new ApplicationException("Couldn't find a macro method to call.");

        macroMethod.Invoke(null, null);
        return true;
    }

    private static string? FindCompiledMacro(IEnumerable<string> macroDirectories, string macroName)
        => macroDirectories
           .Select(
                macroDirectory
                    => Directory.GetFiles(
                        macroDirectory,
                        macroName + ".dll",
                        SearchOption.AllDirectories
                    )
            )
           .Where(files => files.Length > 0)
           .Select(files => files.First())
           .FirstOrDefault();

    private CompilerResults CompileScript(string pathToScript)
    {
        var provider = new CSharpCodeProvider();
        var compilerParams = new CompilerParameters()
        {
            GenerateInMemory = true,
            TreatWarningsAsErrors = false,
        };

        AddCommonReferences(compilerParams);

        var compileResult = provider.CompileAssemblyFromFile(compilerParams, pathToScript);

        if (compileResult.Errors.HasErrors)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Couldn't compile the macro ${MacroName}");
            sb.AppendLine("The following errors occurred during compilation:");

            foreach (CompilerError error in compileResult.Errors)
                sb.AppendLine(error.ErrorNumber + " " + error.ErrorText);

            throw new ApplicationException(sb.ToString());
        }

        return compileResult;
    }

    private void AddCommonReferences(CompilerParameters compilerParams)
    {
        compilerParams.ReferencedAssemblies.Add(
            Path.Combine(Config.TeklaBinPath, "Tekla.Structures.dll")
        );

        compilerParams.ReferencedAssemblies.Add(
            Path.Combine(Config.TeklaBinPath, "Tekla.Macros.Runtime.dll")
        );

        compilerParams.ReferencedAssemblies.Add(
            Path.Combine(Config.TeklaBinPath, "Tekla.Structures.Model.dll")
        );
    }

    private static string GetPathToMacroFile(IEnumerable<string> macroDirectories, string macroName)
    {
        var pathToScript = string.Empty;

        foreach (var macroDirectory in macroDirectories)
        {
            var files = Directory.GetFiles(
                macroDirectory,
                macroName + ".cs",
                SearchOption.AllDirectories
            );

            if (files.Length <= 0)
                continue;

            pathToScript = files.First();
            break;
        }

        return pathToScript;
    }

    private static IReadOnlyList<string> GetMacroDirectories()
    {
        TeklaStructuresSettings.GetAdvancedOptionPaths(
            MacroDirectoryAdvancedParameterName,
            out List<string> macroDirectories
        );

        return macroDirectories.Where(Directory.Exists).ToList();
    }
}