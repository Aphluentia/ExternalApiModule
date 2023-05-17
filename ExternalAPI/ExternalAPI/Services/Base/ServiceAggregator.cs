namespace ExternalAPI.Services.Base
{
    public class ServiceAggregator : IServiceAggregator
    {
        private readonly IDatabaseProvider _DatabaseProvider;
        private readonly ISessionProvider _SessionProvider;
        private readonly IBrokerProvider _BrokerProvider;

        public ServiceAggregator(IDatabaseProvider _DatabaseProviderInput, ISessionProvider _SessionProviderInput, IBrokerProvider brokerProvider)
        {
            _DatabaseProvider = _DatabaseProviderInput;
            _SessionProvider = _SessionProviderInput;
            _BrokerProvider = brokerProvider;
        }

        public IDatabaseProvider DatabaseProvider => _DatabaseProvider;

        public ISessionProvider SessionProvider => _SessionProvider;
        public IBrokerProvider BrokerProvider => _BrokerProvider;
    }
}
