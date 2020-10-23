using GalaSoft.MvvmLight.Command;
using GUI.Utils;
using Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;


namespace GUI.ViewModel
{
    class MainViewModel : ViewModelBase
    {

        private List<Film> MainMovieList { get; set; }

        private string _username;
        public string username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        private Client.Client client = new Client.Client();

        private DateTime _date;
        public DateTime date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }


        private string _filmTitle;
        public string filmTitle
        {
            get => _filmTitle;
            set => SetProperty(ref _filmTitle, value);
        }

        public ICommand searchTitle { get; set; }
        public ICommand getUsername { get; set; }
        public ICommand getAllFilms { get; set; }

        public MainViewModel()
        {
            _filmTitle = "Search for Title";
            _username = "username";
            _date = DateTime.Today;

            searchTitle = new RelayCommand(() =>
            {
                client.GetMovies();
                while (!client.requestDone)
                {

                }
                List<Film> movies = client.films;

                List<Film> moviesWithName = new List<Film>();
                foreach(Film film in client.films)
                {
                    if (film.Title == filmTitle) moviesWithName.Add(film);
                }

                MainMovieList = moviesWithName;
            });

            getUsername = new RelayCommand(() =>
            {
                client.Login(username);
            });

            getAllFilms = new RelayCommand(() =>
            {
                client.GetMovies();
                while (!client.requestDone)
                {

                }
                List<Film> movies = client.films;

                MainMovieList = movies;
            });
        }
    }
}
