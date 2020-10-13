using GalaSoft.MvvmLight.Command;
using GUI.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace GUI.ViewModel
{
    class MainViewModel : ObservableObject
    {

        public ICommand searchTitle { get; set; }

        public string filmTitle { get; set; }

        public MainViewModel ()
        {
            searchTitle = new RelayCommand(() =>
           {
               filmTitle = "het werkt!!";
           });
        }

    }
}
