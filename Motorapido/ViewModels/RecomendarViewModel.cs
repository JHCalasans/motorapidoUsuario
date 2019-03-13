using System;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Essentials;


using System.Threading.Tasks;
using System.Collections.Generic;
using System.Xml;
using System.IO;


namespace Motorapido.ViewModels
{
    public class RecomendarViewModel : BaseViewModel
    {
        
        public async Task ShareText(string text)
            {
            await Share.RequestAsync(new ShareTextRequest
                {
                Text = text,
                Title = "Compartilhar"
                });
            }
    

        public RecomendarViewModel()
        {
            Title = "Compartilhar";

            string t = "http://motorapido ... link para a Play ou App Store.....";
      
            OpenWebCommand = new Command(async () => await ShareText(t));
		

        }

    
        public ICommand OpenWebCommand { get; }
    }



}