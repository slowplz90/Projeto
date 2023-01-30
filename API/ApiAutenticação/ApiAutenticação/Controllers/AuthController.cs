using ApiAutenticação.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.SwaggerGen;

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
            //PassWordHash(request.password, request.email, out byte[] passwordHash, out byte[] passwordSalt);
            user.username = request.username;
            user.email = request.email;
            user.password = request.password;
            //user.PasswordHash= passwordHash;
            //user.PasswordSalt= passwordSalt;

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

            SqlConnection con = new SqlConnection(_config.GetConnectionString("DBCon").ToString());
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM users WHERE username = '" + user.username + "' AND password = '" + user.password + "' ", con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string token = CriarToken(user);
                con.Close();
                return Ok(token);

            }
            else
            {
                return BadRequest("Dados Inválidos");
            }

        }

        [HttpGet("userid")]
        public async Task<ActionResult<int>> GetUserId(string username)
        {
            SqlConnection con = new SqlConnection(_config.GetConnectionString("DBCon").ToString());
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT Id FROM users WHERE username = '" + username + "'", con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int userId = Convert.ToInt32(ds.Tables[0].Rows[0]["Id"]);
                con.Close();
                return Ok(userId);
            }
            else
            {
                return BadRequest("Username não encontrado");
            }
        }

        private string CriarToken(User user) 
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.username, user.password)
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
