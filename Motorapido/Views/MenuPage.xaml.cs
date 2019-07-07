using Motorapido.Models;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Motorapido.Views
    {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
        {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        List<HomeMenuItem> menuItems;
        public MenuPage()
            {

            InitializeComponent();

            string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "imagem");
           
            FileInfo fi = new FileInfo(file);
        
            if (fi.Exists && fi.Length > 0 && Preferences.Get("Autenticado", "default_value" == "true"))

                imagem.Source = file;

            else

                imagem.Source = "my_pick.png";

            if (Preferences.Get("UserName", "default_value")!="default_value")
                {
                username.IsVisible = true;

                username.Text = Preferences.Get("UserName", "default_value");

                }


            if (Preferences.Get("Cadastrado", "default_value") == "true" && Preferences.Get("Autenticado", "default_value") == "true")
            
            {
                menuItems = new List<HomeMenuItem>
                {
            
                new HomeMenuItem {Id = MenuItemType.Viagens, Title="Viagens" },
                new HomeMenuItem {Id = MenuItemType.Histórico, Title="Histórico" },
                new HomeMenuItem {Id = MenuItemType.Recomendar, Title="Compartilhar" },

                new HomeMenuItem {Id = MenuItemType.GPS, Title="Definições GPS" },

                new HomeMenuItem {Id = MenuItemType.Sair, Title="Sair" }
                };
            }
            else

            {
                menuItems = new List<HomeMenuItem>
                {

                new HomeMenuItem {Id = MenuItemType.Login, Title="Logar" },
                new HomeMenuItem {Id = MenuItemType.Cadastrar, Title="Cadastrar" },
                new HomeMenuItem {Id = MenuItemType.GPS, Title="Definições GPS" }
                };
            
            }


            ListViewMenu.ItemsSource = menuItems;

            ListViewMenu.SelectedItem = menuItems[0];
            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var id = (int)((HomeMenuItem)e.SelectedItem).Id;
                await RootPage.NavigateFromMenu(id);
            };
            }
        }
    }