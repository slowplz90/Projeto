using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FootBallAwards
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Master : ContentPage
	{
		public Master ()
		{
			InitializeComponent ();
		}

        private async void Settings_Clicked(object sender, EventArgs e)
        {
            App.MDPage.IsPresented = false;
            await Navigation.PushAsync(new SettingsPage());
        }

        private async void Logout_Clicked(object sender, EventArgs e)
        {
            App.MDPage.IsPresented = false;
        }

        private async void VerVotos_Clicked(object sender, EventArgs e)
        {
            App.MDPage.IsPresented = false;
            await Navigation.PushAsync(new VerVotos());
        }

        private async void Jogos_Clicked(object sender, EventArgs e)
        {
            App.MDPage.IsPresented = false;
            await Navigation.PushAsync(new Jogos());
        }
    }
}