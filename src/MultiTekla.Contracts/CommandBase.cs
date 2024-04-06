using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;

// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace MultiTekla.Contracts;

/// <summary>
/// The base class for a command that operates on a plugin of type <typeparamref name="TPlugin"/>.
/// Provides a framework for executing commands with optional headless mode
/// and configuration settings for Tekla applications.
/// </summary>
/// <typeparam name="TPlugin">The type of the plugin this command operates on, constrained to types derived from <see cref="PluginBase"/>.</typeparam>
public abstract class CommandBase<TPlugin> : ICommand where TPlugin : PluginBase
{
    /// <summary>
    /// Value indicating whether the command should run the plugin in headless mode.
    /// </summary>
    /// <value>
    /// <c>true</c> if the command should run in headless mode; otherwise, <c>false</c>. Default is <c>false</c>.
    /// </value>
    public virtual bool IsHeadlessMode { get; init; } = true;

    /// <summary>
    /// The name of the configuration to use for a headless run.
    /// </summary>
    /// <value>
    /// The name of the configuration. Default is <c>default</c>.
    /// </value>
    public virtual string ConfigName { get; init; } = "default";

    /// <summary>
    /// The name of the model to be used by the command.
    /// </summary>
    /// <value>
    /// The name of the model for headless run, or <see langword="null"/> if run not in headless mode.
    /// </value>
    public virtual string? ModelName { get; init; }

    /// <summary>
    /// Executes the command asynchronously using the specified console for output.
    /// </summary>
    /// <param name="console">The console to use for command output.</param>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
    /// <exception cref="ApplicationException">Thrown if the plugin is not initialized.</exception>
    /// <exception cref="ApplicationException">Thrown if the plugin for headless config is not initialized.</exception>
    /// <exception cref="ArgumentException">Thrown if the model name is not provided.</exception>
    public ValueTask ExecuteAsync(IConsole console)
    {
        var plugin =
            Plugins?.FirstOrDefault(p => p.Value.GetType() == typeof(TPlugin))?.Value as TPlugin;

        if (plugin is null)
            throw new ApplicationException($"Plugins is not initialized");

        if (IsHeadlessMode)
        {
            var configPlugin = ConfigPlugin?.Value;

            if (configPlugin is null)
                throw new ApplicationException("Plugins for headless config is not initialized");

            configPlugin.Config = new HeadlessConfig { Name = ConfigName, };
            configPlugin.IsHeadlessMode = false;
            configPlugin.RunPlugin();

            var config = configPlugin.Config;

            if (!string.IsNullOrEmpty(ModelName))
                config.ModelName = ModelName;

            if (string.IsNullOrEmpty(config.ModelName))
                throw new ArgumentException(
                    "You must provide the model name either in the config or in the command"
                );

            plugin.IsHeadlessMode = IsHeadlessMode;
            plugin.Config = config;
        }

        var result = Execute(console, plugin);

        return result;
    }

    /// <summary>
    /// Executes the command with the specified plugin and console.
    /// </summary>
    /// <param name="console">The console to use for command output.</param>
    /// <param name="plugin">The plugin instance to operate on.</param>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation. </returns>
    /// <remarks>
    /// All interaction with Tekla API must be synchronous. Therefore, the <c>return default;</c>
    /// must be specified in the method
    /// </remarks>
    protected abstract ValueTask Execute(IConsole console, TPlugin plugin);

    /// <summary>
    /// The lazily loaded instance of the plugin to be executed.
    /// </summary>
    public IEnumerable<Lazy<PluginBase>>? Plugins { get; set; }

    /// <summary>
    /// The lazily loaded instance of the plugin for retrieving headless configurations.
    /// </summary>
    [ImportMetadataConstraint("name", "GetHeadlessConfigPlugin")]
    public Lazy<PluginBase>? ConfigPlugin { get; set; }
}
