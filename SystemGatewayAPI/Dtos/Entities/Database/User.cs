namespace SystemGatewayAPI.Dtos.Entities.Database
{
    public class User
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int Age { get; set; }
        public string ProfilePicture { get; set; }
    }
}
