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

namespace FootBallAwards
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Registro : ContentPage
	{
		public Users user { get; set; }
		private const string UrlReg = "http://10.0.2.2:5222/api/Auth/registo";

        public string userName { get; set; }
		public string Email { get; set; }
		public string password { get; set; }
		public Registro ()
		{
			InitializeComponent ();
		}
        private async void Button_Clicked_1(object sender, EventArgs e)
        {
			if(txtconfirmpassword.Text == txtpassword.Text)
			{
                user = new Users
                {
                    userName = txtusername.Text.TrimStart().TrimEnd(),
                    Email = txtemail.Text.TrimStart().TrimEnd(),
                    Password = txtpassword.Text.TrimStart().TrimEnd(),
                };

                var serializeItem = JsonConvert.SerializeObject(user);
                StringContent body = new StringContent(serializeItem, Encoding.UTF8, "application/json");
                HttpClient client = new HttpClient();
                var result = await client.PostAsync(UrlReg, body);
                string data = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    var answer = await DisplayAlert("Registo efetuado com sucesso", "Deseja fazer login?", "Sim", "Não");
                    if (answer)
                    {
                        Navigation.PushAsync(new LoginUI());
                    }
                    else
                    {

                    }
                }
            }
            else
            {
               var answer = await DisplayAlert("Passwords não são iguais","Insira os dados de novo","Ok","Cancel");
                if(answer)
                {
                    Navigation.PushAsync(new Registro());
                }
            }
        }
    }
}