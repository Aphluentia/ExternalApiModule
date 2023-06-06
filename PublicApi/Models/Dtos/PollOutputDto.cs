namespace PublicAPI.Models.Dtos
{
    public class PollOutputDto
    {
        public bool HasUpdates { get; set; }

        public PollOutputDto(bool _HasUpdates) { this.HasUpdates = _HasUpdates; }
        public PollOutputDto() { HasUpdates = false; }
    }
}
