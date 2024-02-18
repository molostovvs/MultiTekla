using System;

namespace MultiTekla.Contracts;

public abstract class PluginBase<TResult>
{
    public bool Headless { get; set; }
    public HeadlessConfig? Config { get; set; }
    public string? ModelName { get; set; }

    public virtual TResult RunPlugin()
    {
        if (Headless)
            StartHeadless();
        return Run();
    }

    protected virtual TimeSpan StartHeadless()
    {
        if (HeadlessTeklaPlugin is null)
            throw new ArgumentNullException(
                $"{nameof(HeadlessTeklaPlugin)} Headless plugin was not initialized"
            );

        var headless = HeadlessTeklaPlugin.Value;
        headless.Config = Config;
        return headless.Run();
    }

    protected abstract TResult Run();

    public Lazy<PluginBase<TimeSpan>>? HeadlessTeklaPlugin { get; set; }
}