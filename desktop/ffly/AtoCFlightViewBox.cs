/******************************************************************************\
 * AtoCFlightViewBox - Information box describing a pair of flights
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
    public partial class AtoCFlightViewBox : Form
    {
        // the flight this box describes
        protected AtoCFlight flight;

        public AtoCFlightViewBox(AtoCFlight flight, DbFace dbface)
        {
            InitializeComponent();

            this.flight = flight;

            // Fill out all the data files
            lblPointA.Text = Airport.GetAirport(flight.PointA, dbface).ToString();
            lblPointB.Text = Airport.GetAirport(flight.PointB, dbface).ToString();
            lblPointC.Text = Airport.GetAirport(flight.PointC, dbface).ToString();
            lblDate_AtoB.Text = flight.Date_AtoB.ToString("M/d/yyyy");
            lblDate_BtoC.Text = flight.Date_BtoC.ToString("M/d/yyyy");
            lblPrice.Text = "$" + flight.Price_AtoB.ToString() + " + $" + flight.Price_BtoC.ToString();
            lblDistance.Text = flight.Km_AtoB.ToString() + " km + " + flight.Km_BtoC.ToString() + " km";
            TimeSpan span = DateTime.Now.Subtract(flight.LastChecked);
            if (span.Days > 0)
                lblLastChecked.Text = span.Days + " days and " + span.Hours + " hours ago";
            else
                lblLastChecked.Text = span.Hours + " hours ago";

            lblLink.Text = "http://www.orbitz.com/";
        }

        // This will bring them to Orbitz
        private void lblLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.orbitz.com/"); 
        }
    }
}