using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Motorapido.Views
    {
    public partial class LoginPage : ContentPage
        {
        void Login_Clicked(object sender, System.EventArgs e)
            {


            bool flag = true;





            if (string.IsNullOrEmpty(Email.Text))

                {

                EmailObrigatório.IsVisible = true;

                flag = false;

                }

            else
                {

                EmailObrigatório.IsVisible = false;

                }

            if (string.IsNullOrEmpty(Senha.Text))

                {


                SenhaObrigatório.IsVisible = true;

                flag = false;

                }

            else
                {


                SenhaObrigatório.IsVisible = false;



                }

            if (!flag) return;


            Preferences.Set("Autenticado", "true");

            Application.Current.MainPage = new MainPage { Detail = new NavigationPage(new ViagensPage()) };




            }

        public LoginPage()
            {
            InitializeComponent();


        

            }


        protected async override void OnAppearing()
            {
            base.OnAppearing();


            if (Preferences.Get("Cadastrado", "default_value") != "true")

                {

                // para Modal ......... MainPage = new CadastrarPage();

                Application.Current.MainPage = new MainPage { Detail = new NavigationPage(new CadastrarPage()) };
                }




            }
        }
    }
