using Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace JsonCommands
{
    public class Commands
    {
        //COMMANDS
        public static string Login(string name)
        {
			var data = new
			{
				username = name
			};

			var login = new
			{
				id = "login/login",
				data = data
			};
			var jsonLogin = JsonSerializer.Serialize(login);
			return jsonLogin;
		}

		public static string GetMovies()
        {
			var data = new
			{

			};

			var movies = new
			{
				id = "movies/get",
				data = data
			};
			var jsonMovies = JsonSerializer.Serialize(movies);
			return jsonMovies;
        }

		public static string OrderMovie(string title, int amount)
        {
			var data = new
			{
				title = title,
				amount = amount
			};

			var order = new
			{
				id = "movies/order",
				data = data
			};
			var jsonOrder = JsonSerializer.Serialize(order);
			return jsonOrder;
		}

        //RESPONSES
		public static string GetMoviesResponse(List<Film> films)
        {
			var data = new
			{
				movies = films
			};

			var movies = new
			{
				id = "movies/getResponse",
				data = data
			};
			var jsonMovies = JsonSerializer.Serialize(movies);
			return jsonMovies;
        }

		public static string OrderResponse(bool success)
        {
			Object data;

            if (success)
            {
				 data = new
				{
					status = "success"
				};
            }
            else
            {
				 data = new
				{
					status = "error"
				};
			}

			var order = new
			{
				id = "movies/orderResponse",
				data = data
			};
			var jsonOrder = JsonSerializer.Serialize(order);
			return jsonOrder;
		}
    }
}
