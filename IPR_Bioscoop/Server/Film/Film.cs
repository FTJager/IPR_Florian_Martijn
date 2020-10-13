using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Film
{
    class Film
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public int Length { get; set; }
        public string Description { get; set; }
        public int review { get; set; }
        public int TicketsLeft { get; set; }

        public Film(string Title, DateTime date, int Length, string Description, int review, int Tickets)
        {
            this.Title = Title;
            this.Date = date;
            this.Length = Length;
            this.Description = Description;
            this.review = review;
            this.TicketsLeft = Tickets;
        }

    }
}
