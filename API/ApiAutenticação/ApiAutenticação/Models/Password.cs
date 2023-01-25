using System.Security.Cryptography;
using System.Text;

namespace ApiAutenticação.Models
{
    public class Password
    {

        public string passwordHash(string password)
        {
            var sha = SHA256.Create();
            var asByteArray = Encoding.Default.GetBytes(password);
            var passwordHash = sha.ComputeHash(asByteArray);
            return Convert.ToBase64String(passwordHash);

        }
    }
}
