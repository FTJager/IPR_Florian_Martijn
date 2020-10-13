using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GUI.Utils
{
    class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
