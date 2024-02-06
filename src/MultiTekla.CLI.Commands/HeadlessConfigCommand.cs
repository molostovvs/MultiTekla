using System;
using System.Threading.Tasks;
using CliFx.Attributes;
using CliFx.Infrastructure;
using MultiTekla.Contracts;
using MultiTekla.Plugins;

namespace MultiTekla.CLI.Commands;

[Command("headless config")]
public class HeadlessConfigCommand : ICommandFor<HeadlessConfigPlugin>
{
    public ValueTask ExecuteAsync(IConsole console)
        => throw new NotImplementedException();

    public Lazy<HeadlessConfigPlugin> Plugin { get; set; }
}