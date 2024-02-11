using System;
using CliFx;

namespace MultiTekla.Contracts;

public interface ICommandFor<TPlugin> : ICommand
{
    public Lazy<TPlugin> Plugin { get; set; }
}