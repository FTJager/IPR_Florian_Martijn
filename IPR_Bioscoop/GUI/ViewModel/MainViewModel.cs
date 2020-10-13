﻿using GalaSoft.MvvmLight.Command;
using GUI.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace GUI.ViewModel
{
    class MainViewModel : ViewModelBase
    {

        private string _filmTitle;
        public string filmTitle
        {
            get => _filmTitle;
            set => SetProperty(ref _filmTitle, value);
        }

        public ICommand searchTitle { get; set; }

        public MainViewModel ()
        {
            _filmTitle = "filmTitle";
            searchTitle = new RelayCommand(() =>
           {
               filmTitle = "werkt het?";
           });
        }

    }
}
