using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    [Serializable]
    public class Film 
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public int Length { get; set; }
        public string Description { get; set; }
        public int review { get; set; }
        public int TicketsLeft { get; set; }

        public Film(string Title, int Length, string Description, int Tickets)
        {
            this.Title = Title;
            this.Date = new DateTime();
            this.Length = Length;
            this.Description = Description;
            this.review = 4;
            this.TicketsLeft = Tickets;
        }

        public string getTitle()
        {
            return this.Title;
        }

        public DateTime getDate()
        {
            return this.Date;
        }

        public int getLength()
        {
            return this.Length;
        }

        public string getDescription ()
        {
            return this.Description;
        }

        public int getTicketsLeft()
        {
            return this.TicketsLeft;
        }
    }
}
