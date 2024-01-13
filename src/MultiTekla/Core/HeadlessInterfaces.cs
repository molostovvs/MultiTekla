namespace MultiTekla.Core;

public interface IEmptyHeadless
{
    public IHeadlessWithBinDirectory BinDirectory(string pathToBinDirectory);
}

public interface IHeadlessWithBinDirectory
{
    public IHeadlessWithModelsPath ModelsPath(string pathToModels);
}

public interface IHeadlessWithModelsPath
{
    public IHeadlessWithModelName ModelName(string modelName);
}

public interface IHeadlessWithModelName
{
    public IHeadlessWithEnvironmentPath EnvironmentPath(string pathToEnvironmentIni);
}

public interface IHeadlessWithEnvironmentPath
{
    public ICompletedHeadless RolePath(string pathToRoleIni);
}

public interface ICompletedHeadless
{
    public ICompletedHeadless BinDirectory(string pathToBinDirectory);
    public ICompletedHeadless ModelsPath(string pathToModels);
    public ICompletedHeadless ModelName(string modelName);
    public ICompletedHeadless EnvironmentPath(string pathToEnvironmentIni);
    public ICompletedHeadless RolePath(string pathToRoleIni);

    public Headless Build();
}