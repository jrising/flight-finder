/******************************************************************************\
 * Flight - Represents a round-trip flight on a particular set of dates
 * ----------------------------------------------------------------------------
 * Copyright (C) 2009  James Rising
 * 
 * Licensed under the GNU General Public License, Version 3 (see license.txt)
\******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace ffly
{
    public class Flight
    {
        protected uint id;   // might not be set (then it's 0)
        // three letter code of origin
        protected string origin;
        // three letter code of destination
        protected string destination;
        // destination country (for better display)
        protected string destcountry;
        // date of departure
        protected DateTime date_leave;
        // date of return
        protected DateTime date_return;
        // price, in dollars
        protected int price;
        // distance, in kilometers
        protected int km;
        // the date when this was entered in the database
        protected DateTime last_checked;

        // Create a flight from known information
        public Flight(string origin, string destination, string destcountry, DateTime date_leave, DateTime date_return, int price, int km, DateTime last_checked)
        {
            this.origin = origin;
            this.destination = destination;
            this.destcountry = destcountry;
            this.date_leave = date_leave;
            this.date_return = date_return;
            this.price = price;
            this.km = km;
            this.last_checked = last_checked;
        }

        // Create a flight from a row in the flights database
        public Flight(Dictionary<string, object> row)
        {
            this.id = (uint) row["id"];
            this.origin = (string)row["origin"];
            this.destination = (string)row["destination"];
            this.destcountry = (string)row["destcountry"];
            this.date_leave = (DateTime)row["date_leave"];
            this.date_return = (DateTime)row["date_return"];
            this.price = (int)row["price"];
            this.km = (int)row["km"];
            this.last_checked = (DateTime)row["created"];
        }

        // get the id from the database, or 0
        public uint Id
        {
            get
            {
                return id;
            }
        }

        // the three letter origin airport code
        public string Origin
        {
            get
            {
                return origin;
            }
        }

        // the three letter destination airport code
        public string Destination
        {
            get
            {
                return destination;
            }
        }

        // the date of departure
        public DateTime DateLeave
        {
            get
            {
                return date_leave;
            }
        }

        // the date of return
        public DateTime DateReturn
        {
            get
            {
                return date_return;
            }
        }

        // the price, in dollars
        public int Price
        {
            get
            {
                return price;
            }
            set
            {
                price = value;
            }
        }

        // the distance, in kilometers
        public int Km
        {
            get
            {
                return km;
            }
        }

        // the date when the price was checked
        public DateTime LastChecked
        {
            get
            {
                return last_checked;
            }
        }

        // a score-- cents per mile
        public double Score
        {
            get
            {
                return 500 * price / (8.0 * 2 * km);
            }
        }

        // a string representation, suitable for the results display
        public override string ToString()
        {
            string prstr = "$" + price.ToString();
            if (price < 1000)
                prstr = " " + prstr;

            string longdest = destination + ", " + destcountry;
            if (longdest.Length > 17)
                longdest = longdest.Substring(0, 16) + ".";
            longdest = longdest.PadLeft(17);

            return string.Format("From {0} to {1} for {2}, on {3} - {4} ({5:F1} c/mi)", origin, longdest, prstr, date_leave.ToString("MM/dd"), date_return.ToString("MM/dd"), Score);
        }

        // a long representation, with all the relevant information
        public string ToLongString(DbFace dbface)
        {
            Airport originAirport = Airport.GetAirport(origin, dbface);
            Airport destinationAirport = Airport.GetAirport(destination, dbface);
            return string.Format("From {0} to {1} for ${1}, {2} km, from {3} to {4} ({6:F1} c/mi)", originAirport.ToString(), destinationAirport.ToString(), price, km, date_leave.ToString("MM/dd"), date_return.ToString("MM/dd"), Score);
        }
    }
}
