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
            jogador.num = request.num;


            SqlConnection con = new SqlConnection(_config.GetConnectionString("DBCon").ToString());
            SqlCommand cmd = new SqlCommand("INSERT INTO jogador(nome,idade,equipa,nacionalidade,num) VALUES('" + jogador.nome + "','" + jogador.idade + "','" + jogador.equipa + "', '" + jogador.nacionalidade + "', '" + jogador.num + "')", con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            return jogador;
        }


        [HttpGet("selectJogador")]
        public async Task<ActionResult<List<jogador>>> LoadJogador(jogador request)
        {
            SqlDataReader reader;
            List<jogador> playersList = new List<jogador>();
            using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DBCon").ToString()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT id, nome, idade, equipa, nacionalidade, num FROM jogador", con))
                {
                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            jogador.id = reader.GetInt32(0);
                            jogador.nome = reader.GetString(1);
                            jogador.idade = reader.GetInt32(2);
                            jogador.equipa = reader.GetString(3);
                            jogador.nacionalidade = reader.GetString(4);
                            jogador.num = reader.GetInt32(5);

                            playersList.Add(new jogador { id = reader.GetInt32(0), nome = reader.GetString(1), idade = reader.GetInt32(2), equipa = reader.GetString(3), nacionalidade = reader.GetString(4), num = reader.GetInt32(5) });
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

        [HttpGet("selectJogadorEquipa")]
        public async Task<ActionResult<List<jogador>>> LoadJogadorEquipa(JogEquipa request)
        {
            SqlDataReader reader;
            List<jogador> playersList = new List<jogador>();
            using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DBCon").ToString()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT id, nome, idade, equipa, nacionalidade, num FROM jogador WHERE equipa = '" + request.equipa + "'", con))
                {
                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            jogador.id = reader.GetInt32(0);
                            jogador.nome = reader.GetString(1);
                            jogador.idade = reader.GetInt32(2);
                            jogador.equipa = reader.GetString(3);
                            jogador.nacionalidade = reader.GetString(4);
                            jogador.num = reader.GetInt32(5);

                            playersList.Add(new jogador { id = reader.GetInt32(0), nome = reader.GetString(1), idade = reader.GetInt32(2), equipa = reader.GetString(3), nacionalidade = reader.GetString(4), num = reader.GetInt32(5) });
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
