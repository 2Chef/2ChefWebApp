namespace WebApp.Kernel.DomainPathProvider
{
    public interface IDomainPathProvider
    {
        Task<string> GetDomain();
    }
}
