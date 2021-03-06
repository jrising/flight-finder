namespace ffly
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.earliest = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.latest = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.shortest = new System.Windows.Forms.MaskedTextBox();
            this.longest = new System.Windows.Forms.MaskedTextBox();
            this.numSimul = new System.Windows.Forms.NumericUpDown();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lstResults = new System.Windows.Forms.ListBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.txtPointA = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtPointC = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnDestinations2 = new System.Windows.Forms.Button();
            this.btnAddPointA = new System.Windows.Forms.Button();
            this.btnAddPointC = new System.Windows.Forms.Button();
            this.btnAddOrigin = new System.Windows.Forms.Button();
            this.btnDestinations = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtOrigins = new System.Windows.Forms.TextBox();
            this.panelRoundTrip = new System.Windows.Forms.Panel();
            this.panelAtoC = new System.Windows.Forms.Panel();
            this.cmbSearch = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.numSimul)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.panelRoundTrip.SuspendLayout();
            this.panelAtoC.SuspendLayout();
            this.SuspendLayout();
            // 
            // earliest
            // 
            this.earliest.Location = new System.Drawing.Point(64, 37);
            this.earliest.MaxDate = new System.DateTime(2017, 12, 31, 0, 0, 0, 0);
            this.earliest.MinDate = new System.DateTime(2013, 1, 1, 0, 0, 0, 0);
            this.earliest.Name = "earliest";
            this.earliest.Size = new System.Drawing.Size(193, 20);
            this.earliest.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Earliest:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Latest:";
            // 
            // latest
            // 
            this.latest.Location = new System.Drawing.Point(64, 63);
            this.latest.MaxDate = new System.DateTime(2017, 12, 31, 0, 0, 0, 0);
            this.latest.MinDate = new System.DateTime(2013, 1, 31, 0, 0, 0, 0);
            this.latest.Name = "latest";
            this.latest.Size = new System.Drawing.Size(193, 20);
            this.latest.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(144, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(245, 25);
            this.label1.TabIndex = 5;
            this.label1.Text = "Community Flight Finder";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Shortest stay (in days):";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(172, 94);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Longest:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(277, 94);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(73, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "Simultaneous:";
            // 
            // shortest
            // 
            this.shortest.Location = new System.Drawing.Point(133, 89);
            this.shortest.Mask = "###";
            this.shortest.Name = "shortest";
            this.shortest.Size = new System.Drawing.Size(33, 20);
            this.shortest.TabIndex = 19;
            // 
            // longest
            // 
            this.longest.Location = new System.Drawing.Point(226, 89);
            this.longest.Mask = "###";
            this.longest.Name = "longest";
            this.longest.Size = new System.Drawing.Size(31, 20);
            this.longest.TabIndex = 20;
            // 
            // numSimul
            // 
            this.numSimul.Location = new System.Drawing.Point(356, 89);
            this.numSimul.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.numSimul.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSimul.Name = "numSimul";
            this.numSimul.Size = new System.Drawing.Size(34, 20);
            this.numSimul.TabIndex = 21;
            this.numSimul.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(396, 87);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(119, 23);
            this.btnSearch.TabIndex = 24;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lstResults
            // 
            this.lstResults.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstResults.FormattingEnabled = true;
            this.lstResults.ItemHeight = 13;
            this.lstResults.Items.AddRange(new object[] {
            "Click Search to see possible flights here!"});
            this.lstResults.Location = new System.Drawing.Point(12, 115);
            this.lstResults.Name = "lstResults";
            this.lstResults.Size = new System.Drawing.Size(503, 158);
            this.lstResults.TabIndex = 26;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 278);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(527, 22);
            this.statusStrip1.TabIndex = 27;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(410, 17);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // txtPointA
            // 
            this.txtPointA.Location = new System.Drawing.Point(52, 0);
            this.txtPointA.Name = "txtPointA";
            this.txtPointA.Size = new System.Drawing.Size(138, 20);
            this.txtPointA.TabIndex = 30;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 3);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 13);
            this.label7.TabIndex = 31;
            this.label7.Text = "Point A:";
            // 
            // txtPointC
            // 
            this.txtPointC.Location = new System.Drawing.Point(52, 26);
            this.txtPointC.Name = "txtPointC";
            this.txtPointC.Size = new System.Drawing.Size(138, 20);
            this.txtPointC.TabIndex = 32;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 29);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 13);
            this.label8.TabIndex = 33;
            this.label8.Text = "Point C:";
            // 
            // btnDestinations2
            // 
            this.btnDestinations2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDestinations2.Location = new System.Drawing.Point(211, 0);
            this.btnDestinations2.Name = "btnDestinations2";
            this.btnDestinations2.Size = new System.Drawing.Size(41, 46);
            this.btnDestinations2.TabIndex = 34;
            this.btnDestinations2.Text = "Destinations";
            this.btnDestinations2.UseVisualStyleBackColor = true;
            this.btnDestinations2.Click += new System.EventHandler(this.btnDestinations2_Click);
            // 
            // btnAddPointA
            // 
            this.btnAddPointA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddPointA.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddPointA.Location = new System.Drawing.Point(189, 0);
            this.btnAddPointA.Name = "btnAddPointA";
            this.btnAddPointA.Size = new System.Drawing.Size(16, 20);
            this.btnAddPointA.TabIndex = 35;
            this.btnAddPointA.Text = "+";
            this.btnAddPointA.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAddPointA.UseVisualStyleBackColor = true;
            this.btnAddPointA.Click += new System.EventHandler(this.btnAddPointA_Click);
            // 
            // btnAddPointC
            // 
            this.btnAddPointC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddPointC.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddPointC.Location = new System.Drawing.Point(189, 26);
            this.btnAddPointC.Name = "btnAddPointC";
            this.btnAddPointC.Size = new System.Drawing.Size(16, 20);
            this.btnAddPointC.TabIndex = 36;
            this.btnAddPointC.Text = "+";
            this.btnAddPointC.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAddPointC.UseVisualStyleBackColor = true;
            this.btnAddPointC.Click += new System.EventHandler(this.btnAddPointC_Click);
            // 
            // btnAddOrigin
            // 
            this.btnAddOrigin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddOrigin.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddOrigin.Location = new System.Drawing.Point(234, 24);
            this.btnAddOrigin.Name = "btnAddOrigin";
            this.btnAddOrigin.Size = new System.Drawing.Size(18, 21);
            this.btnAddOrigin.TabIndex = 29;
            this.btnAddOrigin.Text = "+";
            this.btnAddOrigin.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAddOrigin.UseVisualStyleBackColor = true;
            this.btnAddOrigin.Click += new System.EventHandler(this.btnAddOrigin_Click);
            // 
            // btnDestinations
            // 
            this.btnDestinations.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDestinations.Location = new System.Drawing.Point(17, 19);
            this.btnDestinations.Name = "btnDestinations";
            this.btnDestinations.Size = new System.Drawing.Size(81, 26);
            this.btnDestinations.TabIndex = 28;
            this.btnDestinations.Text = "Destinations";
            this.btnDestinations.UseVisualStyleBackColor = true;
            this.btnDestinations.Click += new System.EventHandler(this.selectDestinations_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Possible Origins:";
            // 
            // txtOrigins
            // 
            this.txtOrigins.Location = new System.Drawing.Point(104, 0);
            this.txtOrigins.Multiline = true;
            this.txtOrigins.Name = "txtOrigins";
            this.txtOrigins.Size = new System.Drawing.Size(148, 45);
            this.txtOrigins.TabIndex = 11;
            // 
            // panelRoundTrip
            // 
            this.panelRoundTrip.Controls.Add(this.label6);
            this.panelRoundTrip.Controls.Add(this.btnDestinations);
            this.panelRoundTrip.Controls.Add(this.btnAddOrigin);
            this.panelRoundTrip.Controls.Add(this.txtOrigins);
            this.panelRoundTrip.Location = new System.Drawing.Point(263, 37);
            this.panelRoundTrip.Name = "panelRoundTrip";
            this.panelRoundTrip.Size = new System.Drawing.Size(252, 48);
            this.panelRoundTrip.TabIndex = 37;
            // 
            // panelAtoC
            // 
            this.panelAtoC.Controls.Add(this.btnDestinations2);
            this.panelAtoC.Controls.Add(this.txtPointA);
            this.panelAtoC.Controls.Add(this.btnAddPointC);
            this.panelAtoC.Controls.Add(this.label7);
            this.panelAtoC.Controls.Add(this.btnAddPointA);
            this.panelAtoC.Controls.Add(this.txtPointC);
            this.panelAtoC.Controls.Add(this.label8);
            this.panelAtoC.Location = new System.Drawing.Point(263, 37);
            this.panelAtoC.Name = "panelAtoC";
            this.panelAtoC.Size = new System.Drawing.Size(252, 48);
            this.panelAtoC.TabIndex = 38;
            this.panelAtoC.Visible = false;
            // 
            // cmbSearch
            // 
            this.cmbSearch.DisplayMember = "Round Trip Search";
            this.cmbSearch.FormattingEnabled = true;
            this.cmbSearch.Items.AddRange(new object[] {
            "Round Trip Search",
            "A-to-C Search"});
            this.cmbSearch.Location = new System.Drawing.Point(396, 10);
            this.cmbSearch.Name = "cmbSearch";
            this.cmbSearch.Size = new System.Drawing.Size(121, 21);
            this.cmbSearch.TabIndex = 39;
            this.cmbSearch.SelectedIndexChanged += new System.EventHandler(this.cmbSearch_SelectedIndexChanged);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 300);
            this.Controls.Add(this.panelRoundTrip);
            this.Controls.Add(this.cmbSearch);
            this.Controls.Add(this.panelAtoC);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.lstResults);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.numSimul);
            this.Controls.Add(this.longest);
            this.Controls.Add(this.shortest);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.latest);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.earliest);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainWindow";
            this.Text = "Community Flight Finder";
            ((System.ComponentModel.ISupportInitialize)(this.numSimul)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panelRoundTrip.ResumeLayout(false);
            this.panelRoundTrip.PerformLayout();
            this.panelAtoC.ResumeLayout(false);
            this.panelAtoC.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker earliest;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker latest;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.MaskedTextBox shortest;
        private System.Windows.Forms.MaskedTextBox longest;
        private System.Windows.Forms.NumericUpDown numSimul;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ListBox lstResults;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.TextBox txtPointA;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtPointC;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnDestinations2;
        private System.Windows.Forms.Button btnAddPointA;
        private System.Windows.Forms.Button btnAddPointC;
        private System.Windows.Forms.Button btnAddOrigin;
        private System.Windows.Forms.Button btnDestinations;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtOrigins;
        private System.Windows.Forms.Panel panelRoundTrip;
        private System.Windows.Forms.Panel panelAtoC;
        private System.Windows.Forms.ComboBox cmbSearch;
    }
}

