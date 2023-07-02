namespace BioShop.Data.ViewModels.UserModels
{
    public class UserDTO
    {
        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Role { get; set; }

        public string Token { get; set; }
    }
}