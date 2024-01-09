using System.Diagnostics;
using System.Reflection;
using MultiTekla.Core;

try
{
    var sw = new Stopwatch();
    sw.Start();

    AppDomain.CurrentDomain.AssemblyResolve +=
        (_, a) => TeklaBinResolve(a, @"C:\TeklaStructures\2022.0\bin\");
    var headless = Headless.BuildHeadless.Default().Build();
    headless.Initialize();

    Console.Clear();
    sw.Stop();
    Console.WriteLine($"Headless initialization took {sw.ElapsedMilliseconds}ms");

    headless.RunPlugins();
    Console.ReadKey();
}
catch (Exception ex)
{
    Console.WriteLine(ex);
    Console.ReadKey();
}

return;

static Assembly? TeklaBinResolve(ResolveEventArgs args, string tsBinDirectory)
{
    var requestedAssembly = new AssemblyName(args.Name);

    return File.Exists(Path.Combine(tsBinDirectory,      requestedAssembly.Name + ".dll"))
        ? Assembly.LoadFile(Path.Combine(tsBinDirectory, requestedAssembly.Name + ".dll")) : null;
}