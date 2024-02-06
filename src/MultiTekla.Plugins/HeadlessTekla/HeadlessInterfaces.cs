namespace MultiTekla.Plugins;

public interface IEmptyHeadless
{
    public IHeadlessWithBinDirectory BinDirectory(string pathToBinDirectory);
}

public interface IHeadlessWithBinDirectory
{
    public IHeadlessWithModelPath ModelPath(string modelPath);
}

public interface IHeadlessWithModelPath
{
    public IHeadlessWithEnvironmentPath EnvironmentPath(string environmentIniPath);
}

public interface IHeadlessWithEnvironmentPath
{
    public ICompletedHeadless RolePath(string roleIniPath);
}

public interface ICompletedHeadless
{
    public ICompletedHeadless BinDirectory(string pathToBinDirectory);
    public ICompletedHeadless ModelPath(string modelPath);
    public ICompletedHeadless EnvironmentPath(string environmentIniPath);
    public ICompletedHeadless RolePath(string roleIniPath);

    public Headless Build();
}