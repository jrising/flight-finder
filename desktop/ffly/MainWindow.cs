/******************************************************************************\
 * MainWindow - Search window and primary interface
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using MIL.Html;
using System.Threading;
using System.Configuration;

namespace ffly
{
    public partial class MainWindow : Form
    {
        // The database connection used by the UI
        protected DbFace masterdbface;
        // the source of results -- currently always orbitz
        protected string source;
        // a username, generated from the host name or ip
        protected string user;

        // an ordered list of results
        protected OrderedListBox results;
        // an interface to the status information
        protected StatusManager status;

        // are we currently doing a search?
        protected bool searching;
                    
        public MainWindow()
        {
            // The announcements may include configuration errors and community-wide notices
            Announcements announcer = new Announcements();

            InitializeComponent();

            // set up our connection to the database
            masterdbface = new DbFace();
            source = "orbitz";
            user = GetIpAddress();

            // read the configuration file
            DateTime earlyDate, lateDate;
            if (!DateTime.TryParse(ConfigurationManager.AppSettings["earliest"], out earlyDate))
            {
                announcer.AddMessage("Could not understand 'earliest' configuration date format; resetting to today.");
                earlyDate = DateTime.Now;
            }
            if (!DateTime.TryParse(ConfigurationManager.AppSettings["latest"], out lateDate))
            {
                announcer.AddMessage("Could not understand 'latest' configuration date format; resetting to five months from now.");
                lateDate = DateTime.Now.AddMonths(5);
            }
            // adjust the dates to maintain the difference
            if (earlyDate < DateTime.Now)
            {
                TimeSpan diff = lateDate.Subtract(earlyDate);
                earlyDate = DateTime.Now;
                lateDate = DateTime.Now.Add(diff);
            }
            else if (lateDate < DateTime.Now)
            {
                announcer.AddMessage("Invalid 'latest' date; resetting to five months from 'earliest'.");
                lateDate = earlyDate.AddMonths(5);
            }
            earliest.Value = earlyDate;
	        latest.Value = lateDate;

            int shortNumber, longNumber;
            if (!int.TryParse(ConfigurationManager.AppSettings["shortest"], out shortNumber))
            {
                announcer.AddMessage("The 'shortest' configuration is not a number; resetting to 5");
                shortNumber = 5;
            }
            if (!int.TryParse(ConfigurationManager.AppSettings["longest"], out longNumber))
            {
                announcer.AddMessage("The 'longest' configuration is not a number; resetting to 12");
                longNumber = 12;
            }

            shortest.Text = shortNumber.ToString();
            longest.Text = longNumber.ToString();

            txtOrigins.Text = ConfigurationManager.AppSettings["origins"];
            if (txtOrigins.Text == null || txtOrigins.Text.Length == 0) {
                announcer.AddMessage("The 'origins' configuration is missing: resetting to JFK, LGA, EWR, DCA, BWI, IAD, BOS");
                txtOrigins.Text = "JFK, LGA, EWR, DCA, BWI, IAD, BOS";
            }

            txtPointA.Text = ConfigurationManager.AppSettings["pointas"];
            if (txtPointA.Text == null || txtPointA.Text.Length == 0) {
                announcer.AddMessage("The 'pointas' configuration is missing: resetting to JFK, LGA, EWR");
                txtPointA.Text = "JFK, LGA, EWR";
            }
            txtPointC.Text = ConfigurationManager.AppSettings["pointcs"];
            if (txtPointC.Text == null || txtPointC.Text.Length == 0) {
                announcer.AddMessage("The 'pointcs' configuration is missing: resetting to DCA, BWI, IAD");
                txtPointC.Text = "DCA, BWI, IAD";
            }

			if (ConfigurationManager.AppSettings["mode"] != null)
				cmbSearch.SelectedText = ConfigurationManager.AppSettings["mode"];
			
			Airport.DisableCodes(ConfigurationManager.AppSettings["disabled"], masterdbface);

            int simulNumber;
            if (!int.TryParse(ConfigurationManager.AppSettings["simultaneous"], out simulNumber))
            {
                announcer.AddMessage("The 'simultaneous' configuration is not a number: resetting to 2");
                simulNumber = 2;
            }
            else if (simulNumber > 9)
            {
                announcer.AddMessage("The 'simultaneous' configuration is more than 9; resetting to 9");
                simulNumber = 9;
            }
            numSimul.Value = simulNumber;

            cmbSearch.SelectedIndex = 0;

            results = new OrderedListBox(lstResults);
            status = new StatusManager(statusStrip1, toolStripStatusLabel1, toolStripProgressBar1);

            lstResults.Click += this.lstResults_SelectedIndexChanged;
            FormClosing += Form1_Closing;
            Application.Idle += OnIdle;
            status.AddNow("Initialized.", 1, 0);

            // display the annoucements box
            DateTime lastrun = DateTime.Now;
            if (!DateTime.TryParse(ConfigurationManager.AppSettings["lastrun"], out lastrun))
                lastrun = DateTime.Now;

            if (announcer.Prepare(masterdbface, MyDate(lastrun)))
                announcer.Show();
        }

        //  On close, save the configuration
        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            searching = false;
            status.AddNow("Closing...", 1, 0);

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            // record all of the configuration values
			if (config.AppSettings.Settings["earliest"] != null)
	            config.AppSettings.Settings["earliest"].Value = MyDate(earliest.Value);
			if (config.AppSettings.Settings["latest"] != null)
	            config.AppSettings.Settings["latest"].Value = MyDate(latest.Value);
			if (config.AppSettings.Settings["shorest"] != null)
	            config.AppSettings.Settings["shortest"].Value = shortest.Text;
			if (config.AppSettings.Settings["longest"] != null)
	            config.AppSettings.Settings["longest"].Value = longest.Text;
			if (config.AppSettings.Settings["origins"] != null)
	            config.AppSettings.Settings["origins"].Value = txtOrigins.Text;
			if (config.AppSettings.Settings["pointas"] != null)
	            config.AppSettings.Settings["pointas"].Value = txtPointA.Text;
			if (config.AppSettings.Settings["pointcs"] != null)
	            config.AppSettings.Settings["pointcs"].Value = txtPointC.Text;
			if (config.AppSettings.Settings["mode"] != null)
	            config.AppSettings.Settings["mode"].Value = cmbSearch.SelectedText;
			if (config.AppSettings.Settings["disabled"] != null)
	            config.AppSettings.Settings["disabled"].Value = Airport.GetDisabledCodes();
			if (config.AppSettings.Settings["lastrun"] != null)
	            config.AppSettings.Settings["lastrun"].Value = MyDate(DateTime.Now);

            config.Save(ConfigurationSaveMode.Minimal);
        }

        // On idle, update results and status from the worker threads
        private void OnIdle(object sender, EventArgs e)
        {
            results.Update();
            status.Update();
        }

        // Start or stop a search
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (this.btnSearch.Text == "Search")
            {
                status.EnterContinuousMode();
                this.btnSearch.Text = "Stop";

                // Look up stored results, and check old results
                results.Reset();
                searching = true;
                
                Thread lookup = new Thread(LookupResults);
                lookup.Start();

                // query additional results
                for (int ii = 0; ii < numSimul.Value; ii++)
                {
					Thread search = null;
					if (cmbSearch.SelectedIndex == 0)
						search = new Thread(CollectNewRoundTrips);
					else if (cmbSearch.SelectedIndex == 1)
						search = new Thread(CollectNewAtoCs);
                    search.Start();
                }
            }
            else
            {
                status.AddNow("Search Stopped.", 1, 1);
                this.btnSearch.Text = "Search";
                searching = false;
            }
        }

        // Look up stored results
        public void LookupResults()
        {
            DbFace dbface = new DbFace();
            int start = 0;

            while (results.Count < 600)
            {
                try
                {
                    status.Add("Looking up stored results...");

                    // find anything that fits our criteria
                    int nextstart = 0;

					if (cmbSearch.SelectedIndex == 0) {
						List<Flight> flights = FindStoredFlights(dbface, "km / price desc", start, 200, 600 - results.Count, out nextstart);
	                    foreach (Flight flight in flights) {
	                        lock (results) {
	                            results.Add(flight.Score, flight);
	                        }
	                    }
					} else if (cmbSearch.SelectedIndex == 1) {
						List<AtoCFlight> flights = FindStoredAtoCFlights(dbface, "(atob.km + btoc.km) / (atob.price + btoc.price) desc", start, 200, 600 - results.Count, out nextstart);
	                    foreach (AtoCFlight flight in flights) {
	                        lock (results) {
	                            results.Add(flight.Score, flight);
	                        }
	                    }
					}

                    if (nextstart == 0)
                        break;
                    start = nextstart;
                }
                catch (Exception ex)
                {
                    // Fail!
                    status.Add("Error: " + ex.Message);
                    return;
                }
            }

            status.Add("Stored results query complete.");

            // Check old flights from now on
            start = 0;
            while (searching)
            {
                int nextstart = 0;
				if (cmbSearch.SelectedIndex == 0)
	                CheckOldFlight(dbface, start, out nextstart);
				else if (cmbSearch.SelectedIndex == 1)
					CheckOldAtoCFlight(dbface, start, out nextstart);
                if (nextstart == 0)
                    break;  // nothing more to check
                start = nextstart;
            }
        }

        // Check a previously queried flight for the current price
        // if nextstart = 0, don't try any more
        public bool CheckOldFlight(DbFace dbface, int start, out int nextstart)
        {
            List<Flight> flights = FindStoredFlights(dbface, "(km / price) * datediff(now(), created) desc", start, 10, 1, out nextstart);
            if (flights.Count == 0)
            {
                nextstart = 0;
                return false;
            }

            Flight flight = flights[0];

            // Did this happen today?  if so, we're done
            TimeSpan ago = DateTime.Now.Subtract(flight.LastChecked);
            if (ago.TotalDays < 1)
            {
                nextstart = 0;
                return false;
            }

            // Lookup this price
            int price = GetPrice(flight.Origin, flight.Destination, flight.DateLeave, flight.DateReturn);
            if (price == int.MaxValue)
                return false; // failed

            // succeeded!  update the database
            dbface.Execute(string.Format("update flights set price = {0}, created = now(), user = '{1}' where id = {2}", price, user, flight.Id), false);

            // replace in the results
            results.Remove(flight.Score, flight.ToString());
            flight.Price = price;
            results.Add(flight.Score, flight);
            
            nextstart--;    // because won't be there!
            status.Add(string.Format("Old result updated: {0} to {1} is ${2}", flight.Origin, flight.Destination, price));
            return true;
        }

		// Check a previously queried a-to-c flight for the current price
        // if nextstart = 0, don't try any more
        public bool CheckOldAtoCFlight(DbFace dbface, int start, out int nextstart)
        {
            List<AtoCFlight> flights = FindStoredAtoCFlights(dbface, "((atob.km + btoc.km) / (atob.price + btoc.price)) * (datediff(now(), atob.date_created) * datediff(now(), btoc.date_created)) desc", start, 10, 1, out nextstart);
            if (flights.Count == 0)
            {
                nextstart = 0;
                return false;
            }

            AtoCFlight flight = flights[0];

            // Did this happen today?  if so, we're done
            TimeSpan ago = DateTime.Now.Subtract(flight.LastChecked);
            if (ago.TotalDays < 1)
            {
                nextstart = 0;
                return false;
            }

            // Lookup this prices
            int price_atob = GetPrice(flight.PointA, flight.PointB, flight.Date_AtoB, null);
            if (price_atob == int.MaxValue)
                return false; // failed
            int price_btoc = GetPrice(flight.PointB, flight.PointC, flight.Date_BtoC, null);
            if (price_btoc == int.MaxValue)
                return false; // failed

            // succeeded!  update the database
            dbface.Execute(string.Format("update flights set price = {0}, created = now(), user = '{1}' where id = {2}", price_atob, user, flight.Id_AtoB), false);
            dbface.Execute(string.Format("update flights set price = {0}, created = now(), user = '{1}' where id = {2}", price_btoc, user, flight.Id_BtoC), false);

            // replace in the results
            results.Remove(flight.Score, flight.ToString());
            flight.Price_AtoB = (uint) price_atob;
            flight.Price_BtoC = (uint) price_btoc;
            results.Add(flight.Score, flight);
            
            nextstart--;    // because won't be there!
            status.Add(string.Format("Old result updated: {0} to {1} to {2} is ${3}", flight.PointA, flight.PointB, flight.PointC, price_atob + price_btoc));
            return true;
        }

        // Find all flights that fit the search criteria
        // if nextstart = 0, don't try again
        public List<Flight> FindStoredFlights(DbFace dbface, string order, int start, int limit, int maxgive, out int nextstart)
        {
            List<Flight> flights = new List<Flight>();

			string[] origins = Airport.ParseAirportCodes(txtOrigins.Text);
			List<Dictionary<string, object>> rows = dbface.AssocEnumerate(string.Format("select flights.*, airports.country as destcountry from flights left join airports on flights.destination = airports.code where (origin = '{0}') and date_leave >= '{1}' and date_leave <= '{2}' and datediff(date_return, date_leave) >= {3} and datediff(date_return, date_leave) <= {4} order by {5} limit {6}, {7}", string.Join("' or origin = '", origins), MyDate(earliest.Value), MyDate(latest.Value), GetNum(shortest.Text, false).Value, GetNum(longest.Text, false).Value, order, start, limit));
				
            nextstart = start;
            foreach (Dictionary<string, object> row in rows)
            {
                nextstart++;

                Flight flight = new Flight(row);
                // Is this airport enabled?
                if (Airport.IsAirportEnabled(flight.Destination))
                {
                    flights.Add(flight);
                    if (flights.Count == maxgive)
                        return flights;
                }
            }

            if (rows.Count < limit)
                nextstart = 0;
            return flights;
        }

        // Find all flights that fit the search criteria
        // if nextstart = 0, don't try again
        public List<AtoCFlight> FindStoredAtoCFlights(DbFace dbface, string order, int start, int limit, int maxgive, out int nextstart)
        {
            List<AtoCFlight> flights = new List<AtoCFlight>();

			string[] pointas = Airport.ParseAirportCodes(txtPointA.Text);
            string[] pointcs = Airport.ParseAirportCodes(txtPointC.Text);
            List<Dictionary<string, object>> rows = dbface.AssocEnumerate(string.Format("select atob.id as atob_id, atob.origin as atob_origin, atob.destination as atob_destination, atob.date as atob_date, atob.price as atob_price, atob.km as atob_km, atob.date_created as atob_date_created, btoc.id as btoc_id, btoc.origin as btoc_origin, btoc.destination as btoc_destination, btoc.date as btoc_date, btoc.price as btoc_price, btoc.km as btoc_km, btoc.date_created as btoc_date_created from oneways atob left join oneways btoc on atob.destination = btoc.origin where (atob.origin = '{0}') and (btoc.destination = '{1}') and atob.date >= '{2}' and atob.date <= '{3}' and btoc.date >= '{4}' and btoc.date <= '{5}' and datediff(btoc.date, atob.date) >= {6} and datediff(btoc.date, atob.date) <= {7} order by {8} limit {9}, {10}", string.Join("' or atob.origin = '", pointas), string.Join("' or btoc.destination = '", pointcs), MyDate(earliest.Value), MyDate(latest.Value.AddDays(-GetNum(shortest.Text, false).Value)), MyDate(earliest.Value.AddDays(GetNum(shortest.Text, false).Value)), MyDate(latest.Value), GetNum(shortest.Text, false).Value, GetNum(longest.Text, false).Value, order, start, limit));
				
            nextstart = start;
            foreach (Dictionary<string, object> row in rows)
            {
                nextstart++;

                AtoCFlight flight = new AtoCFlight(row);
                // Is this airport enabled?
                if (Airport.IsAirportEnabled(flight.PointB))
                {
                    flights.Add(flight);
                    if (flights.Count == maxgive)
                        return flights;
                }
            }

            if (rows.Count < limit)
                nextstart = 0;
            return flights;
        }

		// Collect new results given the search criteria
        public void CollectNewRoundTrips()
        {
            DbFace dbface = new DbFace();
            Random randgen = new Random();

            string[] origins = Airport.ParseAirportCodes(txtOrigins.Text);

            while (searching)
            {
                if (!TestDbFace(ref dbface))
                    continue;
                if (origins.Length < 1)
                {
                    status.Add("ERROR: Please add some origin airports!");
                    return;
                }

                // set up the specific query parameters
                string origin = origins[randgen.Next(origins.Length)];
                string destination = Airport.GetSalientAirportCode(dbface);
                status.Add(string.Format("Checking price from {0} to {1}", origin, destination));

                if (origin == destination)
                    continue;

                TimeSpan range = latest.Value.Subtract(earliest.Value);
                DateTime start = earliest.Value.AddDays(randgen.NextDouble() * range.Days);
       
                int shortdays = GetNum(shortest.Text, false).Value;
                int longdays = GetNum(longest.Text, false).Value;
                DateTime end = start.AddDays(randgen.Next(shortdays, longdays));

                // Try to look up the price
                int price = GetPrice(origin, destination, start, end);
                if (price == int.MaxValue)
                {
                    status.Add(string.Format("Failed to get price from {0} to {1}", origin, destination));
                    continue;
                }
                status.Add(string.Format("Price from {0} to {1}: ${2}", origin, destination, price));

                // get the distance involved
                int distance = GetDistance(origin, destination, dbface);

                RecordOption(origin, destination, start, end, price, distance, dbface);
            }
        }

        public void CollectNewAtoCs()
        {
            DbFace dbface = new DbFace();
            Random randgen = new Random();

            string[] pointas = Airport.ParseAirportCodes(txtPointA.Text);
            string[] pointcs = Airport.ParseAirportCodes(txtPointC.Text);

            while (searching)
            {
                if (!TestDbFace(ref dbface))
                    continue;
                if (pointas.Length < 1)
                {
                    status.Add("ERROR: Please add some point A airports!");
                    return;
                }
                if (pointcs.Length < 1)
                {
                    status.Add("ERROR: Please add some point C airports!");
                    return;
                }

                // set up the specific query parameters
                string pointa = pointas[randgen.Next(pointas.Length)];
                string pointb = Airport.GetSalientAirportCode(dbface);
                string pointc = pointcs[randgen.Next(pointcs.Length)];

                if (pointa == pointb || pointb == pointc)
                    continue;

                status.Add(string.Format("Checking price from {0} to {1}", pointa, pointb));

                TimeSpan range = latest.Value.Subtract(earliest.Value);
                DateTime start = earliest.Value.AddDays(randgen.NextDouble() * range.Days);

                // Try to look up the price
                int priceatob = GetPrice(pointa, pointb, start, null);
                if (priceatob == int.MaxValue)
                {
                    status.Add(string.Format("Failed to get price from {0} to {1}", pointa, pointb));
                    continue;
                }

				int distanceatob = GetDistance(pointa, pointb, dbface);
				
                status.Add(string.Format("Checking price from {0} to {1}", pointb, pointc));

                int shortdays = GetNum(shortest.Text, false).Value;
                int longdays = GetNum(longest.Text, false).Value;
                DateTime end = start.AddDays(randgen.Next(shortdays, longdays));

                // Try to look up the price
                int pricebtoc = GetPrice(pointb, pointc, end, null);
                if (pricebtoc == int.MaxValue)
                {
                    status.Add(string.Format("Failed to get price from {0} to {1}", pointb, pointc));
                    continue;
                }

				int distancebtoc = GetDistance(pointb, pointc, dbface);

				status.Add(string.Format("Price from {0} to {1} to {2}: ${3}", pointa, pointb, pointc, priceatob + pricebtoc));

                RecordAtoCOption(pointa, pointb, pointc, start, end, priceatob, pricebtoc, distanceatob, distancebtoc, dbface);
            }
        }

        // Get the lowest price for a particular flight
        // end can be null, and then searches oneway price
        public int GetPrice(string from, string to, DateTime start, DateTime? end)
        {			
			string url = "http://www.orbitz.com/";
			
			if (end != null) {
            	url = "http://www.orbitz.com/shop/home?type=air&_ar.rt.leaveSlice.originRadius=0&_ar.rt.leaveSlice.destinationRadius=0&ar.rt.leaveSlice.time=Anytime&ar.rt.returnSlice.time=Anytime&_ar.rt.flexAirSearch=0&ar.rt.numAdult=1&ar.rt.numSenior=0&ar.rt.numChild=0&_ar.rt.nonStop=0&_ar.rt.narrowSel=0&ar.rt.narrow=airlines&ar.rt.cabin=C&search=Search+Flights";
				url = WebUtilities.AddUrlParam(url, "ar.type", "roundTrip");
				url = WebUtilities.AddUrlParam(url, "ar.rt.leaveSlice.orig.key", from);
				url = WebUtilities.AddUrlParam(url, "ar.rt.leaveSlice.dest.key", to);
				url = WebUtilities.AddUrlParam(url, "ar.rt.leaveSlice.date", start.ToString("MM/dd/yy"));
				url = WebUtilities.AddUrlParam(url, "ar.rt.returnSlice.date", end.Value.ToString("MM/dd/yy"));
			} else {
            	url = "http://www.orbitz.com/shop/airsearch?type=air&_ar.ow.leaveSlice.originRadius=0&_ar.ow.leaveSlice.destinationRadius=0&ar.ow.leaveSlice.time=Anytime&ar.ow.returnSlice.time=Anytime&_ar.ow.flexAirSearch=0&ar.ow.numAdult=1&ar.rt.numSenior=0&ar.ow.numChild=0&_ar.ow.nonStop=0&_ar.ow.narrowSel=0&ar.ow.narrow=airlines&ar.ow.cabin=C&search=Search+Flights";
				url = WebUtilities.AddUrlParam(url, "ar.type", "oneWay");
				url = WebUtilities.AddUrlParam(url, "ar.ow.leaveSlice.orig.key", from);
				url = WebUtilities.AddUrlParam(url, "ar.ow.leaveSlice.dest.key", to);
				url = WebUtilities.AddUrlParam(url, "ar.ow.leaveSlice.date", start.ToString("MM/dd/yy"));
			}

            // post the form
            CookieContainer cookies = new CookieContainer();
    	    string html = WebUtilities.GetPage(url, ref cookies);
            if (html.StartsWith("ERROR:"))
            {
                Thread.Sleep(1000);     // probably an internal orbitz error
                return int.MaxValue;    // failed!
            }

            HtmlNodeCollection nodes = WebUtilities.ParseHtml(html);

            // look for the lowest price
            List<MIL.Html.HtmlElement> elts = WebUtilities.FindAll(nodes, "span", "class", "mainPrice");

            int minimum = int.MaxValue;
            foreach (MIL.Html.HtmlElement elt in elts)
            {
                // find the price
                string div = elt.HTML;
                if (div.Contains("$"))
                {
                    div = div.Substring(div.IndexOf('$') + 1);
                    int? num = GetNum(div, false);
                    if (num.HasValue)
                        minimum = Math.Min(minimum, num.Value);
                }
            }

            return minimum;
        }

        // Get the distance between two airports
        public int GetDistance(string from, string to, DbFace dbface)
        {
            lock (dbface)
            {
                // Try to get the distance from the database
                int? distance = dbface.GetValue<int>(string.Format("select km from distances where origin = '{0}' and destination = '{1}'", from, to));
                if (distance.HasValue)
                    return distance.Value;

				if (from == "NYC")
					from = "JFK";
				if (from == "LON")
					from = "LHR";
				
                // Otherwise, query www.world-airport.codes.com
                CookieContainer cookies = new CookieContainer();
                string output = WebUtilities.GetPage("http://www.world-airport-codes.com/dist/?a1=" + from + "&a2=" + to, ref cookies);

                int kiloi = output.IndexOf("kilometres");
                if (kiloi < 0)
                    return 0;   // error!

                string kilostr = output.Substring(kiloi - 13, 13);
                int? value = GetNum(kilostr, true);

                if (!value.HasValue)
                {
                    status.Add(string.Format("Could not find distance from {0} to {1}", from, to));
                    return 0;
                }

                dbface.Execute(string.Format("insert into distances (origin, destination, km) values ('{0}', '{1}', {2})", from, to, value.Value), false);
                return value.Value;
            }
        }

        // Record a new flight option
        public void RecordOption(string origin, string destination, DateTime start, DateTime end, int price, int distance, DbFace dbface)
        {
            lock (dbface)
            {
                dbface.Execute(string.Format("insert into flights (origin, destination, date_leave, date_return, price, km, source, user) values ('{0}', '{1}', '{2}', '{3}', {4}, {5}, '{6}', '{7}') on duplicate key update price = {4}, source = '{6}', user = '{7}'", origin, destination, MyDate(start), MyDate(end), price, distance, source, user), false);
            }

            lock (results)
            {
                Flight flight = new Flight(origin, destination, Airport.GetAirport(destination, dbface).Country, start, end, price, distance, DateTime.Now);
                results.Add(flight.Score, flight);
            }
        }

        // Record a new flight option
        public void RecordAtoCOption(string pointa, string pointb, string pointc, DateTime start, DateTime end, int priceatob, int pricebtoc, int distanceatob, int distancebtoc, DbFace dbface)
        {
            lock (dbface)
            {
                dbface.Execute(string.Format("insert into oneways (origin, destination, date, price, km, source, user) values ('{0}', '{1}', '{2}', {3}, {4}, '{5}', '{6}') on duplicate key update price = {3}, source = '{5}', user = '{6}'", pointa, pointb, MyDate(start), priceatob, distanceatob, source, user), false);
                dbface.Execute(string.Format("insert into oneways (origin, destination, date, price, km, source, user) values ('{0}', '{1}', '{2}', {3}, {4}, '{5}', '{6}') on duplicate key update price = {3}, source = '{5}', user = '{6}'", pointb, pointc, MyDate(end), pricebtoc, distancebtoc, source, user), false);
            }

            lock (results)
            {
				AtoCFlight flight = new AtoCFlight(pointa, pointb, pointc, start, end, (uint) priceatob, (uint) pricebtoc, (uint) distanceatob, (uint) distancebtoc, DateTime.Now);
				results.Add(flight.Score, flight);
            }
        }

        // Check or refresh dbface
        public bool TestDbFace(ref DbFace dbface) {
            if (!string.IsNullOrEmpty(dbface.LastError))
            {
                dbface.Dispose();
                dbface = new DbFace();
                // try it out now-- might fail!
                dbface.GetConnection();
                if (!string.IsNullOrEmpty(dbface.LastError))
                {
                    status.Add("ERROR: Count not connect to the database.");
                    Thread.Sleep(1000);
                    return false;
                }
            }

            return true;
        }

        // A flight was clicked on-- show the data
        protected void lstResults_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            object item = results.SelectedItem;
            if (item is string)
                return; // ignore

			if (results.SelectedItem is Flight) {
	            Flight flight = (Flight) results.SelectedItem;
				if (flight != null) {
	    	        FlightViewBox viewbox = new FlightViewBox(flight, masterdbface);
	        	    viewbox.Show();
				}
			} else {
	            AtoCFlight flight = (AtoCFlight) results.SelectedItem;
				if (flight != null) {
	    	        AtoCFlightViewBox viewbox = new AtoCFlightViewBox(flight, masterdbface);
	        	    viewbox.Show();
				}
			}
		}

        // helper function for getting a number from a string
        public int? GetNum(string str, bool all)
        {
            string num = "";
            foreach (char c in str)
            {
                if (char.IsDigit(c))
                    num += c;
                else if (!all && c != ',')
                    break;
            }

            int value;
            if (int.TryParse(num, out value))
                return value;
            else
                return null;
        }

        // helper function for producing a sql-style date
        public string MyDate(DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }

        // get a user id-- an ip address or host name
        public string GetIpAddress()
        {
            string strHostName = Dns.GetHostName();

            // Then using host name, get the IP address list..
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;

            for (int ii = 0; ii < addr.Length; ii++)
            {
                string ip = addr[ii].ToString();
                if (!string.IsNullOrEmpty(ip) && !ip.StartsWith("192.168"))
                    return ip;
            }

            return strHostName;
        }

        // show the select destinations window
        private void selectDestinations_Click(object sender, EventArgs e)
        {
            SelectDestinations selector = new SelectDestinations(masterdbface, status);
            if (!selector.IsDisposed)
                selector.ShowDialog();
        }

        private void btnDestinations2_Click(object sender, EventArgs e)
        {
            SelectDestinations selector = new SelectDestinations(masterdbface, status);
            if (!selector.IsDisposed)
                selector.ShowDialog();
        }

        // show the add origins window
        private void btnAddOrigin_Click(object sender, EventArgs e)
        {
            AirportFinder finder = new AirportFinder(txtOrigins, masterdbface, status);
            if (!finder.IsDisposed)
                finder.Show();
        }

        private void btnAddPointA_Click(object sender, EventArgs e)
        {
            AirportFinder finder = new AirportFinder(txtPointA, masterdbface, status);
            if (!finder.IsDisposed)
                finder.Show();
        }

        private void btnAddPointC_Click(object sender, EventArgs e)
        {
            AirportFinder finder = new AirportFinder(txtPointC, masterdbface, status);
            if (!finder.IsDisposed)
                finder.Show();
        }

        private void cmbSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSearch.SelectedItem.ToString() == "Round Trip Search")
            {
                this.panelRoundTrip.Visible = true;
                this.panelAtoC.Visible = false;
            }
            else
            {
                this.panelRoundTrip.Visible = false;
                this.panelAtoC.Visible = true;
            }
        }
    }
}