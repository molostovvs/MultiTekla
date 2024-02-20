using MultiTekla.Contracts;

namespace MultiTekla.Core.Headless;

public interface IEmptyHeadless
{
    public IHeadlessWithBinDirectory BinDirectory(string pathToBinDirectory);
    public ICompletedHeadless Config(HeadlessConfig config);
}

public interface IHeadlessWithBinDirectory
{
    public IHeadlessWithEnvironmentPath EnvironmentPath(string environmentIniPath);
}

public interface IHeadlessWithEnvironmentPath
{
    public IHeadlessWithRolePath RolePath(string roleIniPath);
}

public interface IHeadlessWithRolePath
{
    public IHeadlessWithModelsPath ModelsPath(string modelsPath);
}

public interface IHeadlessWithModelsPath
{
    public ICompletedHeadless ModelName(string modelName);
}

public interface ICompletedHeadless
{
    public ICompletedHeadless BinDirectory(string pathToBinDirectory);
    public ICompletedHeadless EnvironmentPath(string environmentIniPath);
    public ICompletedHeadless RolePath(string roleIniPath);
    public ICompletedHeadless ModelsPath(string modelsPath);
    public ICompletedHeadless ModelName(string modelName);

    public Tekla Build();
}