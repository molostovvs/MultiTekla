using System.Collections.Generic;

namespace MultiTekla.Contracts;

public interface IHeadlessConfigPlugin
{
    public HeadlessConfig GetConfigWithName(string configFileName);
    public IReadOnlyList<string> GetAllConfigNames();
    public (bool success, string configFileName) Remove(string configNameToRemove);
}