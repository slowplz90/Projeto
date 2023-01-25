using Azure.Messaging;
using FootBallAwards.Models;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FootBallAwards
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Jogos : ContentPage
	{
        public class jogos
        {
            public int id { get; set; }
            public string data { get; set; }
            public string e_casa { get; set; }
            public string e_fora { get; set; }
            public string estadio { get; set; }
        }
        
        public Jogos()
        {
            InitializeComponent();
            LoadJogos();    
            var i = Id;
        }
        public async void LoadJogos()
        {
                var content = "";
                HttpClient client = new HttpClient();
                var UrlJog = "http://10.0.2.2:5222/selectjogos";
                client.BaseAddress = new Uri(UrlJog);
           
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(UrlJog);
                content = await response.Content.ReadAsStringAsync();   
                var Items = JsonConvert.DeserializeObject<List<jogos>>(content);
                ListView1.ItemsSource = Items;
                BindingContext = this;
        }

        private void ListView1_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }

        private void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {
            var selecionado = ListView1.SelectedItem;
            selecionado.ToJson(); //Enviar dados para API usar equipa casa e fora para divir os 11's Iniciais
            //Fazer request a API e enviar estes dados
            //Abrir outro ecrã com os 11's Iniciais
        }
    }
}