using System;

namespace MultiTekla.Contracts;

/// <summary>
/// The base class for all plugins within the application.
/// </summary>
public abstract class PluginBase
{
    /// <summary>
    /// Indicates whether the plugin operates in headless mode.
    /// </summary>
    public bool IsHeadlessMode { get; set; }

    /// <summary>
    /// Config for the plugin if it runs in headless mode.
    /// This can be <see langword="null"/> if no configuration is provided and plugin runs in non-headless mode.
    /// </summary>
    public HeadlessConfig? Config { get; set; }

    /// <summary>
    /// This method runs the plugin based on whether it's running in headless mode or not.
    /// If <see cref="IsHeadlessMode"/> is true, then it starts a headless version of tekla
    /// by calling <see cref="StartHeadless"/>  and then calls <see cref="Run"/>.
    /// Otherwise, it just calls <see cref="Run"/>.
    /// </summary>
    public virtual void RunPlugin()
    {
        if (IsHeadlessMode)
            StartHeadless();
        Run();
    }

    /// <summary>
    /// This method starts headless tekla. If no headless tekla plugin has been initialized yet,
    /// it throws an <see cref="ArgumentNullException"/>
    /// </summary>
    protected virtual void StartHeadless()
    {
        if (HeadlessTeklaPlugin is null)
            throw new ArgumentNullException(
                $"{nameof(HeadlessTeklaPlugin)} plugin was not initialized"
            );

        var headless = HeadlessTeklaPlugin.Value;
        headless.Config = Config;
        headless.Run();
    }

    /// <summary>
    /// Method with the actual logic for running a plugin.
    /// </summary>
    protected abstract void Run();

    /// <summary>
    /// Lazily loaded instance of the `HeadlessTeklaPlugin` class, used specifically in headless mode scenarios.
    /// </summary>
    [System.Composition.Import(AllowDefault = true)]
    [System.Composition.ImportMetadataConstraint("name", "TeklaPlugin")]
    public Lazy<PluginBase>? HeadlessTeklaPlugin { get; set; }
}
