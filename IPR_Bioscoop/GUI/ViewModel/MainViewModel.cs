using Client;
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
        private List<Film> _MainMovieList;
        public List<Film> MainMovieList 
        {
            get => _MainMovieList;
            set => SetProperty(ref _MainMovieList, value);
        }

        private Film _film;
        public Film film 
        {
            get => _film;
            set => SetProperty(ref _film, value);
        }

        private int _amountTickets;
        public int amountTickets
        {
            get => _amountTickets;
            set => SetProperty(ref _amountTickets, value);
        }

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
        public ICommand orderTickets { get; set; }

        public MainViewModel()
        {
            _filmTitle = "Search for Title";
            _username = "username";
            _date = DateTime.Today;
            _amountTickets = 0;
            _MainMovieList = new List<Film>();
            _film = new Film("", 0, "", 0);

            // When te Search title button is pressed it searches for the title in the list of films and returns it
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
                client.requestDone = false;
            });

            //When the login button is pressed the username is sent to the server to connect
            getUsername = new RelayCommand(() =>
            {
                client.Login(username);
            });

            //When the get films button is pressed it returns a list of all the films
            getAllFilms = new RelayCommand(() =>
            {
                client.GetMovies();
                while (!client.requestDone)
                {

                }
                List<Film> movies = client.films;

                MainMovieList = movies;

                client.requestDone = false;
            });

            //When the button is pressed it orders a new ticket
            orderTickets = new RelayCommand(() =>
           {
               client.orderTickets(film.Title, amountTickets);
               while (!client.requestDone)
               {

               }
               List<Film> movies = client.films;

               MainMovieList = movies;
               client.requestDone = false;
           });
        }
    }
}
