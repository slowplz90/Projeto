using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FootBallAwards
{
    public partial class App : Application
    {
        public static MasterDetailPage MDPage { get; set; }
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginUI());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
