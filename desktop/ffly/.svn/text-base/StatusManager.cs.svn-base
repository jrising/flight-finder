/******************************************************************************\
 * StatusManager - threadsafe manager of the status and progress bars
 * ----------------------------------------------------------------------------
 * Copyright (C) 2009  James Rising
 * 
 * Licensed under the GNU General Public License, Version 3 (see license.txt)
\******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace ffly
{
    public class StatusManager
    {
        protected StatusStrip strip;
        protected ToolStripStatusLabel label;
        protected ToolStripProgressBar progress;

        protected Queue<string> messages;
        protected int diffTodo;
        protected int diffDone;
        protected Stopwatch watch;

        public StatusManager(StatusStrip strip, ToolStripStatusLabel label, ToolStripProgressBar progress)
        {
            this.strip = strip;
            this.label = label;
            this.progress = progress;

            label.Text = "Initializing...";
            watch = new Stopwatch();
            watch.Start();
            progress.Minimum = 0;
            progress.Step = 1;
            progress.Maximum = 1;

            messages = new Queue<string>();
        }

        public void AddNow(string message, int max, int value)
        {
            lock (this)
            {
                label.Text = message;
                progress.Style = ProgressBarStyle.Blocks;
                progress.Maximum = max;
                progress.Value = value;
                messages.Clear();
            }
        }

        public void EnterContinuousMode()
        {
            progress.Style = ProgressBarStyle.Marquee;
        }

        public void Add(string message)
        {
            lock (this)
            {
                messages.Enqueue(message);
            }
			Log.Info(message);
        }

        public void Update()
        {
            lock (this)
            {
                if (messages.Count > 0)
                {
                    if (watch.ElapsedMilliseconds > 1000 / messages.Count)
                    {
                        label.Text = messages.Dequeue();
                        watch.Reset();
                        watch.Start();
                    }
                }
            }
        }
    }
}
