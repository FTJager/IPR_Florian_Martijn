using GalaSoft.MvvmLight.Command;
using GUI.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace GUI.ViewModels
{
    public class MainViewModel : ObserverableObject
    {

        public ObserverableObject ViewModel { get; set; }

        public MainViewModel()
        {
            ViewModel = new ItemListViewModel(this);
        }


        private ICommand mSearchCommand;
        public ICommand SearchCommand
        {
            get
            {
                if(mSearchCommand == null)
                {
                    mSearchCommand = new RelayCommand(() =>
                    {
                        Search("WIP");
                    });
                }
                return mSearchCommand;
            }
        }

        private void Search (string Title)
        {
            Console.WriteLine(Title);
        }
    }
}
