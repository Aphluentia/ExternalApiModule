namespace PublicAPI.Services
{
    public interface IBrokerProvider
    {
        public Task<(bool, string?)> CreateConnection(string ModuleId, string WebPlatformId, int ModuleType);
        public Task<(bool, string?)> CloseConnection(string ModuleId, string WebPlatformId, int ModuleType);
    }
}
