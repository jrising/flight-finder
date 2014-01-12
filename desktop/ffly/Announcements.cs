/******************************************************************************\
 * Announcements - Window for startup announcements
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
    public partial class Announcements : Form
    {
        // All the messages added
        protected List<string> userMessages;

        public Announcements()
        {
            InitializeComponent();

            this.userMessages = new List<string>();
        }

        // Add a user message
        public void AddMessage(string message)
        {
            userMessages.Add(message);
        }

        // if prepare is false, don't show it!
        public bool Prepare(DbFace dbface, string lastrun)
        {
            // Try to look up any new annoucements
            List<Dictionary<string, object>> rows = dbface.AssocEnumerate(string.Format("select * from announces where date_created > '{0} 23:59' order by id asc", DbFace.EscapeSingles(lastrun)));
            if (rows != null && rows.Count == 0)
                rows = null;

            // Are there any messages to display?
            if (userMessages.Count == 0 && rows == null)
                return false;

            // fill out the messages box
            StringBuilder messages = new StringBuilder();
            // basic definitions
            messages.Append("{\\rtf1\\ansi\\deff0{\\colortbl;\\red0\\green0\\blue0;\\red128\\green128\\blue0;}");
            foreach (string message in userMessages) {
                messages.AppendLine("\\cf2");
                messages.AppendLine("\\bullet  " + message + "\\line");
            }

            // Add in any annoucements
            if (rows != null)
            {
                foreach (Dictionary<string, object> row in rows)
                {
                    messages.AppendLine("\\bullet {\\b " + row["subject"] + "}\\line\\line");
                    messages.AppendLine(row["body"] + "\\line\\line");
                }
            }
            messages.AppendLine("}");

            rtbMessages.Rtf = messages.ToString();
            return true;
        }

        private void btnDismiss_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}