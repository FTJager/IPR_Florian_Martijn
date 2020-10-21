using GUI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel viewModel;

        public MainWindow()
        {
            viewModel = new MainViewModel();

            DataContext = viewModel;
            InitializeComponent();

            SearchButton.Click += SearchButton_Click;
            LoginButton.Click += LoginButton_Click;
            getFilmsButton.Click += GetFilmsButton_Click;
        }

        private async void GetFilmsButton_Click(object sender, RoutedEventArgs e)
        {
            DatePicker.IsEnabled = false;
            getFilmsButton.IsEnabled = false;
            DateTime date = (DateTime)DatePicker.SelectedDate;

            bool isValid = await getFilmsAsync(date);

            if (isValid)
            {
                date = DateTime.Today;
            }
            else
            {
                DatePicker.IsEnabled = true;
                getFilmsButton.IsEnabled = true;
            }
        }

        private async Task<bool> getFilmsAsync(DateTime date)
        {
            return await Task.Run (() => getDate(date));
        }

        private bool getDate (DateTime date)
        {
            viewModel.date = date;
            return true;
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            titleSearchBox.IsEnabled = false;
            SearchButton.IsEnabled = false;
            string Title = titleSearchBox.Text;

            bool isValid = await SearchAsync(Title);

            if (isValid)
            {
                Title = "working";
            }
            else
            {
                titleSearchBox.IsEnabled = true;
                SearchButton.IsEnabled = true;
            }
        }

        private async Task<bool> SearchAsync(string title)
        {
            return await Task.Run(() => Search(Title));
        }

        private bool Search(string Title)
        {
            Thread.Sleep(100);
            viewModel.updateTitle(Title);
            return true;
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            usernameBox.IsEnabled = false;
            LoginButton.IsEnabled = false;
            string username = usernameBox.Text;

            bool isValid = await LoginAsync(username);

            if (isValid)
            {
                username = "working";
            }
            else
            {
                usernameBox.IsEnabled = true;
                LoginButton.IsEnabled = true;
            }
        }

        private async Task<bool> LoginAsync(string username)
        {
            return await Task.Run(() => Login(username));
        }

        private bool Login(string username)
        {
            Thread.Sleep(100);
            viewModel.updateUsername(username);
            return true; ;
        }
    }
}
