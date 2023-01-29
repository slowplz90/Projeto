using FootBallAwards.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FootBallAwards.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class onzeInicial : ContentPage
    {

        public List<Jogador> HomeTeamPlayers { get; set; }
        public List<Jogador> AwayTeamPlayers { get; set; }

        public onzeInicial(List<Jogador> homeTeamPlayers, List<Jogador> awayTeamPlayers)
        {
            InitializeComponent();
            HomeTeamPlayers = homeTeamPlayers;
            AwayTeamPlayers = awayTeamPlayers;
            BindingContext = this;
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
    }
}