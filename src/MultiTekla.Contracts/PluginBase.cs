using System;

namespace MultiTekla.Contracts;

/// <summary>
/// Represents a base class for plugins that can be run in headless mode.
/// </summary>
public abstract class PluginBase<TResult> : IPlugin
{
    /// <summary>
    /// A property that determines whether the plugin should run in headless mode or not. Defaults to <see langword="false"/>;
    /// </summary>
    public bool Headless { get; set; }

    /// <summary>
    /// Config for the plugin. This can be <see langword="null"/> if no configuration is provided and plugin runs in non-headless mode.
    /// </summary>
    public HeadlessConfig? Config { get; set; }

    /// <summary>
    /// Gets or sets the name of the model in which headless tekla would be initialized. Defaults to <see langword="null"/>
    /// </summary>
    public string? ModelName { get; set; }

    /// <summary>
    /// This method runs the plugin based on whether it's running in headless mode or not.
    /// If <see cref="Headless"/> is true, then it starts a headless version of tekla by calling <see cref="StartHeadless"/>  and then calls <see cref="Run"/>.
    /// Otherwise, it just calls <see cref="Run"/>.
    /// </summary>
    public virtual TResult RunPlugin()
    {
        if (Headless)
            StartHeadless();
        return Run();
    }

    /// <summary>
    /// This method starts headless tekla. If no headless tekla plugin has been initialized yet, it throws an <see cref="ArgumentNullException"/>
    /// </summary>
    /// <returns>Returns the time (in seconds) taken to initialize headless Tekla</returns>
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

    /// <summary>
    /// Method with the actual logic for running a plugin.
    /// </summary>
    /// <returns> <see cref="TResult"/> </returns>
    protected abstract TResult Run();

    /// <summary>
    /// Headless tekla plugin, provided by app. This can be null if an application composition error has occurred.
    /// </summary>
    public Lazy<PluginBase<TimeSpan>>? HeadlessTeklaPlugin { get; set; }
}