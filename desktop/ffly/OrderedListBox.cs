/******************************************************************************\
 * OrderedListBox - threadsafely maintains the items of a list box in an order
 * ----------------------------------------------------------------------------
 * Copyright (C) 2009  James Rising
 * 
 * Licensed under the GNU General Public License, Version 3 (see license.txt)
\******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ffly
{
    public class OrderedListBox
    {
        protected ListBox listbox;

        protected LinkedList<KeyValuePair<double, object>> ordered;
        protected Queue<KeyValuePair<double, object>> pending;
        protected Queue<KeyValuePair<double, object>> toremove;

        public OrderedListBox(ListBox listbox)
        {
            this.listbox = listbox;
            ordered = new LinkedList<KeyValuePair<double, object>>();
            pending = new Queue<KeyValuePair<double, object>>();
            toremove = new Queue<KeyValuePair<double, object>>();
        }

        public object SelectedItem
        {
            get
            {
                return listbox.SelectedItem;
            }
        }

        public void Reset()
        {
            listbox.Items.Clear();
            ordered = new LinkedList<KeyValuePair<double, object>>();
            pending = new Queue<KeyValuePair<double, object>>();
            toremove = new Queue<KeyValuePair<double, object>>();
        }

        public int Count
        {
            get
            {
                return ordered.Count;
            }
        }

        public void Add(double score, object value)
        {
            lock (pending)
            {
                pending.Enqueue(new KeyValuePair<double, object>(score, value));
            }
        }

        // find the element that matches this
        public void Remove(double score, object value)
        {
            lock (toremove)
            {
                toremove.Enqueue(new KeyValuePair<double, object>(score, value));
            }
        }

        // Only called from UI thread
        public void Update()
        {
			int updates = 0;
			
			// First remove any
            while (toremove.Count > 0 && updates < 2)
            {
                KeyValuePair<double, object> remove = toremove.Dequeue();

                // Find where this was
                int index = 0;
                for (LinkedListNode<KeyValuePair<double, object>> elt = ordered.First; elt != null; elt = elt.Next)
                {
                    if (elt.Value.Key == remove.Key && elt.Value.Value.ToString() == remove.Value.ToString())
                    {
                        ordered.Remove(elt);
                        break;
                    }
                    index++;
                }
                if (index == ordered.Count)
                    continue; // not found!

                listbox.Items.RemoveAt(index);
				updates++;
            }

            while (pending.Count > 0 && updates < 2)
            {
                KeyValuePair<double, object> add = pending.Dequeue();

                // Find where to put this:
                int index = 0;
                for (LinkedListNode<KeyValuePair<double, object>> elt = ordered.First; elt != null; elt = elt.Next)
                {
                    if (elt.Value.Key > add.Key)
                    {
                        ordered.AddBefore(elt, add);
                        break;
                    }
                    index++;
                }
                if (index == ordered.Count)
                    ordered.AddLast(add);

                listbox.Items.Insert(index, add.Value);
				updates++;
            }
        }
    }
}
