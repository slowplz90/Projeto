using ApiAutenticação.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ApiAutenticação.Controllers
{

    public class VotacoesController
    {

        public static Votacoes votacoes = new Votacoes();
        private readonly IConfiguration _config;

        public VotacoesController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("inserirVotacoes")]
        public async Task<ActionResult<Votacoes>> RegistoVotacoes(Votacoes request)
        {
            votacoes.nome_jogador = request.nome_jogador;
            votacoes.id_user = request.id_user;
            votacoes.id_jogo = request.id_jogo;

            SqlConnection con = new SqlConnection(_config.GetConnectionString("DBCon").ToString());
            SqlCommand cmd = new SqlCommand("INSERT INTO Votacoes(nome_jogador,id_user,id_jogo) VALUES('" + votacoes.nome_jogador + "','" + votacoes.id_user + "','" + votacoes.id_jogo + "')", con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            return votacoes;
        }

        /* 
         [HttpPost("selectVotacoes")]
         public async Task<ActionResult<Votacoes>> SelectVotacoes(Votacoes request)
         {
             SqlConnection con = new SqlConnection(_config.GetConnectionString("DBCon").ToString());
             SqlCommand votacoes = new SqlCommand("SELECT * FROM votacoes", con);
             return votacoes;
         } */
    }
}