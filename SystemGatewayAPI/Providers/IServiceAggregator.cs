using SystemGateway.Providers;

namespace SystemGateway.Providers
{
    public interface IServiceAggregator
    {
        IDatabaseProvider DatabaseProvider { get; }
        ISecurityManagerProvider SecurityManagerProvider { get; }
        IOperationsProvider OperationsManagerProvider { get; }
    }
}
