/******************************************************************************\
 * OriginFinder - window for searching for airports and adding to origins
 * ----------------------------------------------------------------------------
 * Copyright (C) 2009  James Rising
 * 
 * Licensed under the GNU General Public License, Version 3 (see license.txt)
\******************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ffly
{
    public partial class AirportFinder : Form
    {
        protected TextBox txtOrigins;

        protected List<Airport> airports;
        protected Dictionary<string, Airport> catalog;
		
		protected Dictionary<string, Airport>.Enumerator? idleEnumerator;

        public AirportFinder(TextBox txtOrigins, DbFace dbface, StatusManager status)
        {
            this.txtOrigins = txtOrigins;
			idleEnumerator = null;

            InitializeComponent();

            // Collect All Airports
            airports = Airport.LoadAllAirports(dbface, "country asc");
            if (airports == null)
            {
                status.AddNow("Could not access airport information", 0, 0);
                this.Close();
                return;
            }

            txtSearch.Text = "";

            catalog = new Dictionary<string,Airport>();

            // Display all airports
            foreach (Airport airport in airports)
            {
                string line = airport.ToString();
                catalog.Add(line.ToLower(), airport);
                lstAirports.Items.Add(airport);
            }
        }

		// On idle, do more filtering options
        private void OnIdle(object sender, EventArgs e)
        {
			if (idleEnumerator == null)
				return;
			
            string text = txtSearch.Text.ToLower();
			
			int added = 0;
			while (idleEnumerator != null && added < 2) {
				if (!idleEnumerator.Value.MoveNext())
					idleEnumerator = null;
				else {
					KeyValuePair<string, Airport> entry = idleEnumerator.Value.Current;
	                if (entry.Key.Contains(text)) {
    	                lstAirports.Items.Add(entry.Value);
						added++;
					}
				}
            }
		}

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string text = txtSearch.Text.ToLower();
            
			// Search all airports
			List<Airport> matches = new List<Airport>();
			foreach (KeyValuePair<string, Airport> entry in catalog)
	            if (entry.Key.Contains(text))
    	           matches.Add(entry.Value);

			lstAirports.Items.Clear();			
			lstAirports.Items.AddRange(matches.ToArray());
        }

        private void lstAirports_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clean up and add it to the list
            string[] codes = Airport.ParseAirportCodes(txtOrigins.Text);
            
            // Is this already in our list?
            Airport selected = (Airport)lstAirports.SelectedItem;
            foreach (string code in codes)
                if (code == selected.Code)
                    return;

			if (codes.Length == 0)
		        txtOrigins.Text = selected.Code;
			else
	            txtOrigins.Text = string.Join(", ", codes) + ", " + selected.Code;
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}