﻿using Azure;
using FootBallAwards.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FootBallAwards
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginUI : ContentPage
    {
        public Users user { get; set; }
        private const string Url = "http://10.0.2.2:5222/api/Auth/login";
        public string userName { get; set; }
        public string password { get; set; }
        public LoginUI()
        {
            InitializeComponent();
        }

        public async void LoginAsync(string username, string password)
        {
            var token = string.Empty;

                var request = new HttpRequestMessage(HttpMethod.Post, Url);

                user = new Users
                {
                    userName = txtusername.Text.TrimStart().TrimEnd(),
                    Password = txtpassword.Text.TrimStart().TrimEnd(),
                };

                var client = new HttpClient();
                //var response = client.SendAsync(request).Result;

                var dados = JsonConvert.SerializeObject(user);
                StringContent body = new StringContent(dados, Encoding.UTF8, "application/json");
                request.Content = body;
                var response = client.SendAsync(request).Result;

                using (HttpContent content = response.Content)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var json = content.ReadAsStringAsync().Result;
                        token = json;
                        Navigation.PushAsync(new MainPage());
                    }
                    else
                    {
                        DisplayAlert("Erro", "Dados Inválidos", "Ok");
                        return;
                    }
                }
        }

        private async Task<int> GetUserId(string username)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("http://10.0.2.2:5222/api/Auth/userid?username=" + username);
                if (response.IsSuccessStatusCode)
                {
                    var userIdString = await response.Content.ReadAsStringAsync();
                    int userId = int.Parse(userIdString);
                    // Use userId as needed
                    return userId;
                }
                else
                {
                    // Handle error
                    return 0;
                }

            }
        }

        //Código do botão de Login
        private async void Button_Clicked(object sender, EventArgs e)
        {
            LoginAsync(txtusername.Text, txtpassword.Text);
            var userId = await GetUserId(txtusername.Text);
            UserID UserId = new UserID();

            UserID.UserId = userId;
        }

        //Código da "label" de registro
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Registro());
        }

        private void checkpass_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (checkpass.IsChecked == true)
            {
                txtpassword.IsPassword = false;
            }
            else
            {
                txtpassword.IsPassword = true;
            }
        }
    }
}