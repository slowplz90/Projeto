namespace ApiAutenticação.Models
{
    public class User
    {
        public string username { get; set; } = string.Empty;
        public string email { get; set; }
        public string password { get; set; }
       // public byte[] PasswordHash { get; set; }
       // public byte[] PasswordSalt { get; set; }
    }
}
