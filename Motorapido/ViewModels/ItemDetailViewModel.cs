using System;

using Motorapido.Models;

namespace Motorapido.ViewModels
    {
    public class ItemDetailViewModel : BaseViewModel
        {
        public Item Item { get; set; }
        public ItemDetailViewModel(Item item = null)
            {
            Title = item?.Text;
            Item = item;
            }
        }
    }
