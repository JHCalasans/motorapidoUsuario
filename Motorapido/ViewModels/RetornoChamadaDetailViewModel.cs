using System;

using Motorapido.Models;

namespace Motorapido.ViewModels
    {
    public class RetornoChamadaDetailViewModel : BaseViewModel
        {
        public RetornoChamada RetornoChamada { get; set; }
        public RetornoChamadaDetailViewModel(RetornoChamada retornochamada = null)
            {
            Title = "Chamada: " + retornochamada?.codChamada.ToString();
            RetornoChamada = retornochamada;
            }
        }
    }
