using ApiAutenticação.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using NuGet.Protocol;
using System.Data;
using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;

namespace ApiAutenticação.Controllers 
{

    public class EquipaController
    {

        public static Equipa equipa = new Equipa();
        private readonly IConfiguration _config;

        public EquipaController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("inserirEquipa")]
        public async Task<ActionResult<Equipa>> RegistoEquipa(Equipa request)
        {
            equipa.nome = request.nome;
            equipa.estadio = request.estadio;
            equipa.cidade = request.cidade;

            SqlConnection con = new SqlConnection(_config.GetConnectionString("DBCon").ToString());
            SqlCommand cmd = new SqlCommand("INSERT INTO equipas(nome,estadio,cidade) VALUES('" + equipa.nome + "','" + equipa.estadio + "','" + equipa.cidade + "')", con);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            return equipa;
        }


        [HttpGet("selectEquipa")]

        public async Task<ActionResult<List<Equipa>>> LoadEquipa(Equipa request)
        {
            SqlDataReader reader;
            List<Equipa> teamsList = new List<Equipa>();

            using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DBCon").ToString()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT nome, estadio, cidade FROM equipas", con))
                {

                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            equipa.nome = reader.GetString(0);
                            equipa.estadio = reader.GetString(1);
                            equipa.cidade = reader.GetString(2);

                            teamsList.Add(new Equipa { nome = reader.GetString(0), estadio = reader.GetString(1), cidade = reader.GetString(2) });
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                    reader.Close();
                }
            }
            return teamsList;
        }
    }
}
 