/******************************************************************************\
 * SelectDestinations - checkbox tree view of destinations for selecting
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
    public partial class SelectDestinations : Form
    {
        protected DbFace dbface;

        public SelectDestinations(DbFace dbface, StatusManager status)
        {
            this.dbface = dbface;

            InitializeComponent();

            // Select all countries and their airports
            List<Airport> airports = Airport.LoadAllAirports(dbface, "country asc");

            if (airports == null)
            {
                status.AddNow("Could not access airport information", 0, 0);
                this.Close();
                return;
            }
			
            TreeNode node = null;   // the current node we're adding t
            int lastchecked = 0, lastunchecked = 0;
            foreach (Airport airport in airports)
            {
                string country = airport.Country;
                if (node == null || node.Text != country)
                {
                    if (node != null)
                    {
                        if (lastchecked == 0)
                            node.StateImageIndex = 0;
                        else if (lastunchecked == 0)
                            node.StateImageIndex = 1;
                        else
                            node.StateImageIndex = 2;
                    }

                    lastchecked = lastunchecked = 0;

                    // need to add the country
                    node = destinations.Nodes.Add(country, country);
                }

                // Add this
                TreeNode child = node.Nodes.Add(airport.Code, airport.Title + " (" + airport.Code + ")");
                child.StateImageIndex = airport.IsEnabled ? 1 : 0;
                if (airport.IsEnabled)
                    lastchecked++;
                else
                    lastunchecked++;
            }

            if (lastchecked == 0)
                node.StateImageIndex = 0;
            else if (lastunchecked == 0)
                node.StateImageIndex = 1;
            else
                node.StateImageIndex = 2;
        }

        private void submit_Click(object sender, EventArgs e)
        {
            // record the destination selections
            foreach (TreeNode node in destinations.Nodes)
            {
                foreach (TreeNode child in node.Nodes)
                {
                    string code = child.Name;
                    Airport airport = Airport.GetAirport(code, dbface);
                    airport.IsEnabled = child.StateImageIndex == 1;
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            foreach (TreeNode node in destinations.Nodes) {
                node.StateImageIndex = checkBox1.Checked ? 1 : 0;
                destinations.UpdateChildren(node);
            }
        }
    }
}