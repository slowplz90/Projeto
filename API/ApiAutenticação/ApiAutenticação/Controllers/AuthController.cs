using ApiAutenticação.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace ApiAutenticação.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("registo")]
        public async Task<ActionResult<User>> Registo(UserDTO request)
        {
            user.username = request.username;
            user.email = request.email;
            user.password = request.password;

            SqlConnection con = new SqlConnection(_config.GetConnectionString("DBCon").ToString());
            SqlCommand cmd = new SqlCommand("INSERT INTO users(username,Email,password) VALUES('" + user.username + "','" + user.email + "','" + user.password + "')", con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserLogin request)
        {
            user.username = request.username;
            user.password = request.password;
            SqlDataReader reader;

            SqlConnection con = new SqlConnection(_config.GetConnectionString("DBCon").ToString());
            con.Open();
            
            using (SqlCommand cmd = new SqlCommand("SELECT username, password FROM users WHERE username = '" + user.username + "' AND password = '" + user.password + "' ", con))
            {
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    string token = CriarToken(user);
                    con.Close();
                    reader.Close();
                    return Ok(token);
                }
                else
                {
                    return BadRequest("Dados Inválidos");
                }
            }


        }

        private string CriarToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.username)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _config.GetSection("appsettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

    }
}
