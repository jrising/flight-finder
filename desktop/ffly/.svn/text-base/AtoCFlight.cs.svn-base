/******************************************************************************\
 * AtoCFlight - Represents two one-way flights on particular dates
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
    public class AtoCFlight
    {
        protected uint id_atob;   // might not be set (then it's 0)
        protected uint id_btoc;   // might not be set (then it's 0)
        // three letter code of origin
        protected string pointa;
        // three letter code of destination
        protected string pointb;
        // destination country (for better display)
        protected string pointc;
        // date of departure
        protected DateTime date_atob;
        // date of return
        protected DateTime date_btoc;
        // prices, in dollars
        protected uint price_atob;
        protected uint price_btoc;
        // distances, in kilometers
        protected uint km_atob;
        protected uint km_btoc;
        // the date when this was entered in the database
        protected DateTime last_checked;

        // Create a flight from known information
        public AtoCFlight(string pointa, string pointb, string pointc, DateTime date_atob, DateTime date_btoc, uint price_atob, uint price_btoc, uint km_atob, uint km_btoc, DateTime last_checked)
        {
            this.pointa = pointa;
            this.pointb = pointb;
            this.pointc = pointc;
            this.date_atob = date_atob;
            this.date_btoc = date_btoc;
            this.price_atob = price_atob;
            this.price_btoc = price_btoc;
            this.km_atob = km_atob;
            this.km_btoc = km_btoc;
            this.last_checked = last_checked;
        }

        // Create a flight from a row in the flights database
        public AtoCFlight(Dictionary<string, object> row)
        {
            this.id_atob = (uint) row["atob_id"];
            this.id_btoc = (uint) row["btoc_id"];
            this.pointa = (string)row["atob_origin"];
            this.pointb = (string)row["atob_destination"];
			if (pointb != (string)row["btoc_origin"])
				throw new Exception("atob's B <> btoc's B");		
            this.pointc = (string)row["btoc_destination"];
            this.date_atob = (DateTime)row["atob_date"];
            this.date_btoc = (DateTime)row["btoc_date"];
            this.price_atob = (uint)row["atob_price"];
            this.price_btoc = (uint)row["btoc_price"];
            this.km_atob = (uint)row["atob_km"];
            this.km_btoc = (uint)row["btoc_km"];
            this.last_checked = DateTime.Compare((DateTime)row["atob_date_created"], (DateTime)row["btoc_date_created"]) > 0 ? (DateTime)row["atob_date_created"] : (DateTime)row["btoc_date_created"];
        }

        // get the id from the database, or 0
        public uint Id_AtoB
        {
            get
            {
                return id_atob;
            }
        }

		public uint Id_BtoC
        {
            get
            {
                return id_btoc;
            }
        }

        // the three letter origin airport code
        public string PointA
        {
            get
            {
                return pointa;
            }
        }

		// the three letter destination airport code
        public string PointB
        {
            get
            {
                return pointb;
            }
        }
		
		// the three letter final airport code
        public string PointC
        {
            get
            {
                return pointc;
            }
        }


        // the date of departure
        public DateTime Date_AtoB
        {
            get
            {
                return date_atob;
            }
        }

        // the date of return
        public DateTime Date_BtoC
        {
            get
            {
                return date_btoc;
            }
        }

        // the price, in dollars
        public uint Price_AtoB
        {
            get
            {
                return price_atob;
            }
            set
            {
                price_atob = value;
            }
        }

		
	    // the price, in dollars
        public uint Price_BtoC
        {
            get
            {
                return price_btoc;
            }
            set
            {
                price_btoc = value;
            }
        }

		// the distance, in kilometers
        public uint Km_AtoB
        {
            get
            {
                return km_atob;
            }
        }

		// the distance, in kilometers
        public uint Km_BtoC
        {
            get
            {
                return km_btoc;
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
                return 500 * (price_atob + price_btoc) / (8.0 * (km_atob + km_btoc));
            }
        }

        // a string representation, suitable for the results display
        public override string ToString()
        {
            string prstr = "$" + (price_atob + price_btoc).ToString();
            if (price_atob + price_btoc < 1000)
                prstr = " " + prstr;

            return string.Format("From {0} to {1} to {2} for {3}, on {4} - {5} ({6:F1} c/mi)", pointa, pointb, pointc, prstr, date_atob.ToString("MM/dd"), date_btoc.ToString("MM/dd"), Score);
        }

        // a long representation, with all the relevant information
        public string ToLongString(DbFace dbface)
        {
            Airport pointaAirport = Airport.GetAirport(pointa, dbface);
            Airport pointbAirport = Airport.GetAirport(pointb, dbface);
            Airport pointcAirport = Airport.GetAirport(pointc, dbface);
            return string.Format("From {0} to {1} for ${1}, {2} km, on {3}; then to {4} for ${5}, {6} km on {7} ({8:F1} c/mi)", pointaAirport.ToString(), pointbAirport.ToString(), price_atob, km_atob, date_atob.ToString("MM/dd"), pointcAirport.ToString(), price_btoc, km_btoc, date_btoc.ToString("MM/dd"), Score);
        }
    }
}