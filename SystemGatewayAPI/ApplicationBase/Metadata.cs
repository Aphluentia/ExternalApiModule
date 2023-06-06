namespace SystemGateway
{
    public class Metadata
    {
        public bool Success => !Errors.Any();
        public ICollection<Error> Errors { get; set; } = new List<Error>();
        public DateTime ActionTimestamp => DateTime.UtcNow;

    }
}
