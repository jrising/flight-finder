/******************************************************************************\
 * FlightViewBox - Information box describing a single flight
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
    public partial class FlightViewBox : Form
    {
        // the flight this box describes
        protected Flight flight;

        public FlightViewBox(Flight flight, DbFace dbface)
        {
            InitializeComponent();

            this.flight = flight;

            // Fill out all the data files
            lblOrigin.Text = Airport.GetAirport(flight.Origin, dbface).ToString();
            lblDestination.Text = Airport.GetAirport(flight.Destination, dbface).ToString();
            lblLeave.Text = flight.DateLeave.ToString("M/d/yyyy");
            lblReturn.Text = flight.DateReturn.ToString("M/d/yyyy");
            lblPrice.Text = "$" + flight.Price.ToString();
            lblDistance.Text = flight.Km.ToString() + " km";
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