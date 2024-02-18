namespace MultiTekla.Core.Headless;

public interface IEmptyHeadless
{
    public IHeadlessWithBinDirectory BinDirectory(string pathToBinDirectory);
}

public interface IHeadlessWithBinDirectory
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
    public ICompletedHeadless EnvironmentPath(string environmentIniPath);
    public ICompletedHeadless RolePath(string roleIniPath);

    public Headless.Tekla Build();
}