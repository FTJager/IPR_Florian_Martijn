using GUI.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace GUI.ViewModels
{
    public class ItemListViewModel : ObserverableObject
    {

        private MainViewModel mainViewModel { get; set; }

        public ItemListViewModel (MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
        }

        public string Film { get; set; } = "TENET";
    }
}
