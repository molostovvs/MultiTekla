namespace MultiTekla.Plugins.Headless.Performance;

public class PerformancePlugin : PluginBase<bool>
{
    protected override bool Run()
        => new TSM.Model().GetConnectionStatus();
}