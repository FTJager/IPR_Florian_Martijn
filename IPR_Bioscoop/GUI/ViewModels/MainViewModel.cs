using GUI.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace GUI.ViewModels
{
    public class MainViewModel : ObserverableObject
    {

        public ObserverableObject ViewModel { get; set; }

        public MainViewModel()
        {
            ViewModel = new ItemListViewModel(this);
        }

    }
}
