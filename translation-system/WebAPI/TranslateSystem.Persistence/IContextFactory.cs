namespace TranslateSystem.Persistence
{
    public interface IContextFactory
    {
        ApplicationContext CreateContext();
    }
}