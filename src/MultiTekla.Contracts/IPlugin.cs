namespace MultiTekla.Contracts;

public interface IPlugin<out TResult>
{
    public TResult Run();
}