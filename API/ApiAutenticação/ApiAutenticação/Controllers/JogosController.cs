using ApiAutenticação.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace ApiAutenticação.Controllers
{
    public class JogosController
    {

        public static jogos jogos = new jogos();
        private readonly IConfiguration _config;

        public JogosController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("inserirJogos")]
        public async Task<ActionResult<jogos>> RegistoJogos(jogos request)
        {
            jogos.e_fora = request.e_fora;
            jogos.e_casa = request.e_casa;
            jogos.data = request.data;
            jogos.estadio = request.estadio;

            SqlConnection con = new SqlConnection(_config.GetConnectionString("DBCon").ToString());
            SqlCommand cmd = new SqlCommand("INSERT INTO jogos(equipa_casa,equipa_fora,data,estadio) VALUES('" + jogos.e_casa + "','" + jogos.e_fora + "','" + jogos.data + "', '" + jogos.estadio + "')", con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            return jogos;
        }

        [HttpGet("selectjogos")]
        public async Task<ActionResult<List<jogos>>> Loadjogos(jogos request)
        {
            SqlDataReader reader;
            List<jogos> gamesList = new List<jogos>();
            using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DBCon").ToString()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT equipa_casa, equipa_fora, data, estadio FROM jogos", con))
                {
                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            jogos.e_casa = reader.GetString(0);
                            jogos.e_fora = reader.GetString(1);
                            jogos.data = reader.GetString(2);
                            jogos.estadio = reader.GetString(3);

                            gamesList.Add(new jogos { e_casa = reader.GetString(0), e_fora = reader.GetString(1), data = reader.GetString(2), estadio = reader.GetString(3) });
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                        gamesList.Add(new jogos { e_casa = "N/A", e_fora = "N/A", data = "N/A", estadio = "N/A" });
                    }
                    reader.Close();
                }
            }
            return gamesList;
        }
    }
}
