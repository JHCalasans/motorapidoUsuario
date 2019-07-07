using System;
using System.Collections.Generic;
using System.Text;

namespace Motorapido.Models
    {
    public enum MenuItemType
        {

        Login, 
        Cadastrar,
        Viagens,
        Histórico,
        Recomendar,
        GPS,
        Sair
        }

    public class HomeMenuItem
        {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }

        public string Image { get; set; }
        }
    }
