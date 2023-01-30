using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using FootBallAwards.Models;
using Newtonsoft.Json;

namespace FootBallAwards.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class onzeInicial : ContentPage
    {

        public List<Jogador> HomeTeamPlayers { get; set; }
        public List<Jogador> AwayTeamPlayers { get; set; }
        public Jogador homeselectedItem;
        public Jogador awayselectedItem;

        public votar voto { get; set; }
        private const string UrlReg = "http://10.0.2.2:5222/api/Auth/registo";

        public int id_user { get; set; }
        public int id_jogo { get; set; }
        public string nome_jogador { get; set; }

        public onzeInicial(List<Jogador> homeTeamPlayers, List<Jogador> awayTeamPlayers)
        {
            InitializeComponent();
            HomeTeamPlayers = homeTeamPlayers;
            AwayTeamPlayers = awayTeamPlayers;
            BindingContext = this;

            var currentUserId = UserID.UserId;

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            HomeTeamListView.ItemTapped += (sender, e) =>
            {
                homeselectedItem = e.Item as Jogador;
            };
            AwayTeamListView.ItemTapped += (sender, e) =>
            {
                awayselectedItem = e.Item as Jogador;
            };
        }

        public async void LoadJogadores()
        {
            var content = "";
            HttpClient client = new HttpClient();
            var UrlJog = "http://10.0.2.2:5222/selectJogadorEquipa";
            client.BaseAddress = new Uri(UrlJog);

            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await client.GetAsync(UrlJog);
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            bool itemSelectedInFirstListView = HomeTeamListView.SelectedItem != null;
            bool itemSelectedInSecondListView = AwayTeamListView.SelectedItem != null;

            if (itemSelectedInFirstListView && itemSelectedInSecondListView)
            {
                DisplayAlert("Aviso", "Selecione apenas 1 jogador", "OK");
            }
            else if (itemSelectedInFirstListView)
            {
                int id_user = UserID.UserId;
                int id_jogo = JogoID.jogoID;
                string nome_jogador = homeselectedItem.nome;
                    votar(id_user, id_jogo, nome_jogador);
            }
            else if (itemSelectedInSecondListView)
            {
                int id_user = UserID.UserId;
                int id_jogo = JogoID.jogoID;
                string nome_jogador = awayselectedItem.nome;
                    votar(id_user, id_jogo, nome_jogador);
            }
            else
            {
                DisplayAlert("Aviso", "Selecione pelo menos 1 jogador", "Ok");
            }
    }
       /*public async Task<bool> VerificarVotacao(int id_user, int id_jogo)
        {
            using (var client = new HttpClient())
            {
                var UrlVerifica = "http://10.0.2.2:5222/api/Votacoes/verificarVotacao";
                var verify = new votar
                {
                    id_user = id_user,
                    id_jogo = id_jogo,
                };

                var serializeItem = JsonConvert.SerializeObject(verify);
                StringContent body = new StringContent(serializeItem, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(UrlVerifica, body);
                string data = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }*/

       public async void votar(int id_user, int id_jogo, string nome_jogador)
        {
            string UrlVoto = "http://10.0.2.2:5222/inserirVotacoes";
            var voto = new votar
            {
                id_user = id_user,
                id_jogo = id_jogo,
                nome_jogador = nome_jogador
            };

                var serializedVoto = JsonConvert.SerializeObject(voto);

                var httpContent = new StringContent(serializedVoto, Encoding.UTF8, "application/json");

                HttpClient client = new HttpClient();

                var result = await client.PostAsync("http://10.0.2.2:5222/api/Votacoes/inserirVotacoes", httpContent);
                string data = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode)
                {
                DisplayAlert("Aviso", "Votação Efetuada", "Ok");
                Navigation.PushAsync(new MainPage());
                }
        }

    }
}