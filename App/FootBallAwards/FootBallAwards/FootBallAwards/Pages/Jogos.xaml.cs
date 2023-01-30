using Azure.Messaging;
using FootBallAwards.Models;
using FootBallAwards.Pages;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Numerics;
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

        public List<jogos> Items;
        public jogos selectedItem;
        JogoID jogo = new JogoID();

        public Jogos()
        {
            InitializeComponent();
            LoadJogos();
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
            Items = JsonConvert.DeserializeObject<List<jogos>>(content);
            ListView1.ItemsSource = Items;
            BindingContext = this;
        }

        private void ListView1_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ListView1.ItemTapped += (sender, e) =>
            {
                selectedItem = e.Item as jogos;
            };
        }



        private async void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {

            if (selectedItem != null)
            {
                string e_casa = selectedItem.e_casa;
                string e_fora = selectedItem.e_fora;
                JogoID.jogoID = selectedItem.id;

                var homeTeamPlayers = await GetHomeTeamPlayers(e_casa);
                var awayTeamPlayers = await GetAwayTeamPlayers(e_fora);

                Navigation.PushAsync(new onzeInicial(homeTeamPlayers, awayTeamPlayers));

            }
            else
            {
                DisplayAlert("Erro", "Nenhum item selecionado", "Ok");
            }
        }

        private async Task<List<Jogador>> GetHomeTeamPlayers(string homeTeam)
        {
            var client = new HttpClient();
            var response = await client.GetAsync("http://10.0.2.2:5222/selectJogadorEquipa?equipa=" + homeTeam);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var players = JsonConvert.DeserializeObject<List<Jogador>>(responseString);
                return players;
            }
            else
            {
                // Se não receber dados
                return null;
            }
        }

        private async Task<List<Jogador>> GetAwayTeamPlayers(string awayTeam)
        {
            var client = new HttpClient();
            var response = await client.GetAsync("http://10.0.2.2:5222/selectJogadorEquipa?equipa=" + awayTeam);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var players = JsonConvert.DeserializeObject<List<Jogador>>(responseString);
                return players;
            }
            else
            {
                // Se não receber dados
                return null;
            }
        }
    }
} 