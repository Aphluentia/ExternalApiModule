namespace PublicAPI.Services.Base
{
    public interface IServiceAggregator
    {
        IDatabaseProvider DatabaseProvider { get; }
        ISessionProvider SessionProvider { get; }
        IBrokerProvider BrokerProvider { get; }
    }
}
