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

            imagem.Source = file;
            username.Text = Preferences.Get("UserName", "default_value");


            menuItems = new List<HomeMenuItem>
            {
              
                new HomeMenuItem {Id = MenuItemType.Login, Title="Logar" },
                new HomeMenuItem {Id = MenuItemType.Cadastrar, Title="Cadastrar" },
                new HomeMenuItem {Id = MenuItemType.Viagens, Title="Viagens" },
                new HomeMenuItem {Id = MenuItemType.Histórico, Title="Histórico" },
                new HomeMenuItem {Id = MenuItemType.Recomendar, Title="Compartilhar" }
            };

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