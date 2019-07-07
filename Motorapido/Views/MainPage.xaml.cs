using Motorapido.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Motorapido.Views
    {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
        {
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();
        public MainPage()
            {
            InitializeComponent();

            MasterBehavior = MasterBehavior.Popover;

          //  MenuPages.Add((int)MenuItemType.Login, (NavigationPage)Detail);
            }

        public async Task NavigateFromMenu(int id)
            {
            if (!MenuPages.ContainsKey(id))
                {
                switch (id)
                    {
              
                    case (int)MenuItemType.Login:
                        MenuPages.Add(id, new NavigationPage(new LoginPage()));
                        break;
                    case (int)MenuItemType.Cadastrar:
                        MenuPages.Add(id, new NavigationPage(new CadastrarPage()));
                        break;
                    case (int)MenuItemType.Viagens:
                        MenuPages.Add(id, new NavigationPage(new ViagensPage()));
                        break;
                    case (int)MenuItemType.Histórico:
                        MenuPages.Add(id, new NavigationPage(new HistóricoPage()));
                        break;

                    case (int)MenuItemType.Recomendar:
                        MenuPages.Add(id, new NavigationPage(new RecomendarPage()));
                        break;

                    case (int)MenuItemType.GPS:
                        MenuPages.Add(id, new NavigationPage(new GPSPage()));
                        break;

                    case (int)MenuItemType.Sair:
                        MenuPages.Add(id, new NavigationPage(new SairPage()));
                        break;
                    }
                }

            var newPage = MenuPages[id];

            if (newPage != null && Detail != newPage)
                {
                Detail = newPage;

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);

                IsPresented = false;
                }
            }
        }
    }