using ApiAutenticação.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ApiAutenticação.Controllers
{
        public class JogadorController 
    {
        public static jogador jogador = new jogador();
        private readonly IConfiguration _config;

        public JogadorController(IConfiguration config)
            {
                _config = config;
            }


        [HttpPost("inserirJogadores")]
        public async Task<ActionResult<jogador>> RegistoJogador(jogador request)
        {
            jogador.nome = request.nome;
            jogador.idade = request.idade;
            jogador.equipa = request.equipa;
            jogador.nacionalidade = request.nacionalidade;


            SqlConnection con = new SqlConnection(_config.GetConnectionString("DBCon").ToString());
            SqlCommand cmd = new SqlCommand("INSERT INTO jogador(nome, idade,equipa,nacionalidade, num) VALUES('" + jogador.nome + "', '" + jogador.idade + "','" + jogador.equipa + "', '" + jogador.nacionalidade + "', '" + jogador.num + "')", con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            return jogador;
        }

        [HttpGet("jogadoresequipa")]
        public async Task<ActionResult<List<jogador>>> jogadorEquipa(jogador request)
        {
            SqlDataReader reader;
            List<jogador> playersList = new List<jogador>();
            using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DBCon").ToString()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT nome, idade, equipa, nacionalidade, num FROM jogador WHERE equipa = '" + request.equipa + "'", con))
                {
                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            jogador.nome = reader.GetString(0);
                            jogador.idade = reader.GetInt32(1);
                            jogador.equipa = reader.GetString(2);
                            jogador.nacionalidade = reader.GetString(3);

                            playersList.Add(new jogador { nome = reader.GetString(0), idade = reader.GetInt32(1), equipa = reader.GetString(2), nacionalidade = reader.GetString(3) });
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                    reader.Close();
                }
            }
            return playersList;
        }

        [HttpGet("selectJogador")]
        public async Task<ActionResult<List<jogador>>> LoadJogador(jogador request)
        {
            SqlDataReader reader;
            List<jogador> playersList = new List<jogador>();
            using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DBCon").ToString()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT nome, idade, equipa, nacionalidade FROM jogador", con))
                {
                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            jogador.nome = reader.GetString(0);
                            jogador.idade = reader.GetInt32(1);
                            jogador.equipa = reader.GetString(2);
                            jogador.nacionalidade = reader.GetString(3);

                            playersList.Add(new jogador { nome = reader.GetString(0), idade = reader.GetInt32(1), equipa = reader.GetString(2), nacionalidade = reader.GetString(3) });
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                    reader.Close();
                }
            }
            return playersList;
        }
    }
 }
