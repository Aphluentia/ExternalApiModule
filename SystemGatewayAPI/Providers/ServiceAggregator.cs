namespace SystemGateway.Providers
{
    public class ServiceAggregator : IServiceAggregator
    {
        private readonly IDatabaseProvider _DatabaseProvider;
        private readonly ISecurityManagerProvider _SecurityManagerProvider;
        private readonly IOperationsProvider _OperationsManagerProvider;

        public ServiceAggregator(IDatabaseProvider _DatabaseProviderInput, ISecurityManagerProvider _SecurityManagerProviderInput, IOperationsProvider _OperationsManagerProviderInput)
        {
            _DatabaseProvider = _DatabaseProviderInput;
            _SecurityManagerProvider = _SecurityManagerProviderInput;
            _OperationsManagerProvider = _OperationsManagerProviderInput;
        }

        public IDatabaseProvider DatabaseProvider => _DatabaseProvider;
        public ISecurityManagerProvider SecurityManagerProvider => _SecurityManagerProvider;
        public IOperationsProvider OperationsManagerProvider => _OperationsManagerProvider;

    }
}
