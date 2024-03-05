namespace MultiTekla.Plugins.Headless.Performance;

public class PerformancePlugin : PluginBase
{
    protected override void Run()
        => new TSM.Model().GetConnectionStatus();
}