using Microsoft.Identity.Client;
using MongoDB.Bson.IO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace FootBallAwards.Models
{
    internal class RestService : IRestService
    {
        Uri requestUrl = new Uri("http://localhost:5222");
        Uri urlLogin = new Uri("http://localhost:5222/api/Auth/login");
        Uri requestRegister = new Uri("http://localhost:5222/api/Auth/registo");
    }
}
