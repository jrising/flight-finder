/******************************************************************************\
 * Airport - Represents a possible origin or desintation
 * ----------------------------------------------------------------------------
 * Copyright (C) 2009  James Rising
 * 
 * This file is part of FFlight, which is free software: you can redistribute
 * it and/or modify it under the terms of the GNU General Public License as 
 * published by the Free Software Foundation, either version 3 of the License,
 * or (at your option) any later version.
 * 
 * FFlight is distributed in the hope that it will be useful, but WITHOUT ANY
 * WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
 * FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more
 * details (license.txt).
 * 
 * You should have received a copy of the GNU General Public License along with
 * FFlight.  If not, see <http://www.gnu.org/licenses/>.
\******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace ffly
{
    public class Airport
    {
        /***** Static Methods *****/

        // Single airport objects for each known airport
        protected static Dictionary<string, Airport> cache;
        // How many airports are currently disabled (maintained by IsEnabled)
        protected static int countDisabled;

        static Airport()
        {
            cache = new Dictionary<string, Airport>();
            countDisabled = 0;
        }

        // Get an airport, either from the cache or the database
        public static Airport GetAirport(string code, DbFace dbface) {
            lock (cache)
            {
                // look in the cache
                Airport airport;
                if (cache.TryGetValue(code, out airport))
                    return airport;

                // look in the database
                List<Dictionary<string, object>> rows = dbface.AssocEnumerate(string.Format("select * from airports where code = '{0}' limit 1", code));
                if (rows == null || rows.Count != 1)
                    return new Airport(code, "Unknown", "Unknown");

                Dictionary<string, object> row = rows[0];

                airport = new Airport((string)row["code"], (string)row["title"], (string)row["country"]);
                cache[code] = airport;

                return airport;
            }
        }

        // Remember the information for an airport
        public static Airport RecordAirport(Dictionary<string, object> row)
        {
            lock (cache)
            {
                string code = (string) row["code"];

                // look in the cache
                Airport airport;
                if (cache.TryGetValue(code, out airport))
                    return airport;

                // Add it to the cache
                airport = new Airport(code, (string)row["title"], (string)row["country"]);
                cache[code] = airport;

                return airport;
            }
        }

        // Get a random airport, based on salience.  Defaults to BOS, if there's an error.
        public static string GetSalientAirportCode(DbFace dbface)
        {
            lock (dbface)
            {
                List<Dictionary<string, object>> rows = dbface.AssocEnumerate("select * from airports order by salience * rand() desc limit " + (countDisabled + 1));
                if (rows == null)
                    return "BOS";

                foreach (Dictionary<string, object> row in rows)
                {
                    Airport airport = RecordAirport(row);
                    if (!airport.enabled)
                        continue;

                    return airport.code;
                }

                return "BOS";
            }
        }

        public static List<Airport> LoadAllAirports(DbFace dbface, string order)
        {
            string sql = "select * from airports";
            if (!string.IsNullOrEmpty(order))
                sql += " order by " + order;

            List<Dictionary<string, object>> rows = dbface.AssocEnumerate(sql);
            if (rows == null)
                return null; // failed!

            List<Airport> airports = new List<Airport>();
            foreach (Dictionary<string, object> row in rows)
                airports.Add(Airport.RecordAirport(row));

            return airports;
        }

        // Is this airport enabled?  It is if we don't know otherwise.
        public static bool IsAirportEnabled(string code) {
            lock (cache)
            {
                // look in the cache
                Airport airport;
                if (cache.TryGetValue(code, out airport))
                    return airport.IsEnabled;

                return true;    // it's never been disabled
            }
        }

        // Get all the airports that are currently disabled
        public static string GetDisabledCodes()
        {
            List<string> codes = new List<string>();

            foreach (KeyValuePair<string, Airport> kvp in cache)
                if (!kvp.Value.IsEnabled)
                    codes.Add(kvp.Key);

            return string.Join(", ", codes.ToArray());
        }

        // Disable the airports described in this list
        public static void DisableCodes(string list, DbFace dbface)
        {
			if (list == null)
				return;
			
            string[] codes = ParseAirportCodes(list);
            if (codes.Length > 20)
            {
                // First cache all airports
                LoadAllAirports(dbface, null);
            }

            foreach (string code in codes)
                GetAirport(code, dbface).IsEnabled = false;
        }

        // Split up a list of airport codes
        public static string[] ParseAirportCodes(string list)
        {
            return list.Split(new char[] { ' ', ',', ';', '\'' }, StringSplitOptions.RemoveEmptyEntries);
        }
        
        /***** Airport Instance *****/

        protected string code;      // three letter airport code
        protected string title;     // a title for the airport
        protected string country;   // the country where it is

        protected bool enabled;     // Is this airport available for a destination?

        // Protected constructor-- all Airports are gotten through GetAirport or RecordAirport
        protected Airport(string code, string title, string country)
        {
            this.code = code;
            this.title = title;
            this.country = country;

            enabled = true;
        }

        // get airport code
        public string Code
        {
            get
            {
                return code;
            }
        }

        // get airport title
        public string Title
        {
            get
            {
                return title;
            }
        }

        // get airport country
        public string Country
        {
            get
            {
                return country;
            }
        }

        // is this airport available in the search?
        public bool IsEnabled
        {
            get
            {
                return enabled;
            }
            set
            {
                // maintain the countDisabled value
                if (enabled != value) {
                    if (enabled)
                        countDisabled++;
                    else
                        countDisabled--;
                }
                enabled = value;
            }
        }

        // A string describing all relevant information
        public override string ToString()
        {
            return title + ", " + country + " (" + code + ")";
        }
    }
}
