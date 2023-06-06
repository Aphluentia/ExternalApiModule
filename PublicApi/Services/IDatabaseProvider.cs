namespace PublicAPI.Services
{
    public interface IDatabaseProvider
    {

        public Task<(bool, string?)> Get(string endpoint);
        public Task<(bool, string?)> Post(string endpoint, object body);
        public Task<(bool, string?)> Put(string endpoint, object body);
        public Task<(bool, string?)> Delete(string endpoint);
    }
}
