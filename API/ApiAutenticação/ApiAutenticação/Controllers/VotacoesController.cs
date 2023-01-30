using ApiAutenticação.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ApiAutenticação.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotacoesController : Controller
    {

        public static Votacoes votacoes = new Votacoes();
        private readonly IConfiguration _config;

        public VotacoesController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("inserirVotacoes")]
        public async Task<ActionResult<Votacoes>> InserirVotacoes(Votacoes request)
        {
                votacoes.id_user = request.id_user;
                votacoes.id_jogo = request.id_jogo;
                votacoes.nome_jogador = request.nome_jogador;

                SqlConnection con = new SqlConnection(_config.GetConnectionString("DBCon").ToString());
                SqlCommand cmd = new SqlCommand("INSERT INTO votacoes(id_user,id_jogo,nome_jogador) VALUES('" + votacoes.id_user + "','" + votacoes.id_jogo + "','" + votacoes.nome_jogador + "')", con);
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                return votacoes;
        }

        [HttpGet("verificarVotacao")]
        public async Task<ActionResult<bool>> VerificarVotacao(int id_user, int id_jogo)
        {
            SqlConnection con = new SqlConnection(_config.GetConnectionString("DBCon").ToString());
            SqlCommand cmd = new SqlCommand("SELECT id_user, id_jogo, nome_jogador FROM Votacoes WHERE id_user='" + id_user + "' AND id_jogo='" + id_jogo + "'", con);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                con.Close();
                return true;
            }
            else
            {
                con.Close();
                return false;
            }
        }

    }
}