/******************************************************************************\
 * TriStateTreeView - modified from a comment on a project on CodeProject
 * ----------------------------------------------------------------------------
 * Licensed under the GNU General Public License, Version 3 (see license.txt)
\******************************************************************************/
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace ffly
{
    public class TriStateTreeView : TreeView
    {
        private const int STATE_UNCHECKED = 0; //unchecked state
        private const int STATE_CHECKED = 1; //checked state
        private const int STATE_MIXED = 2; //mixed state (indeterminate)
        private ImageList _InternalStateImageList; //state image list

        //create a new ThreeStateTreeView
        public TriStateTreeView()
            : base()
        {
        }

        //initialize all nodes state image
        public void InitializeNodesState(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.StateImageIndex < 0)
                    node.StateImageIndex = 0;
                if (node.Nodes.Count != 0)
                {
                    InitializeNodesState(node.Nodes);
                }
            }
        }

        //update children state image with the parent value
        public void UpdateChildren(TreeNode parent)
        {
            int state = parent.StateImageIndex;
            foreach (TreeNode node in parent.Nodes)
            {
                node.StateImageIndex = state;
                if (node.Nodes.Count != 0)
                {
                    UpdateChildren(node);
                }
            }
        }

        //update parent state image base on the children state
        public void UpdateParent(TreeNode child)
        {
            TreeNode parent = child.Parent;

            if (parent == null)
            {
                return;
            }

            if (child.StateImageIndex == STATE_MIXED)
            {
                parent.StateImageIndex = STATE_MIXED;
            }
            else if (IsChildrenChecked(parent))
            {
                parent.StateImageIndex = STATE_CHECKED;
            }
            else if (IsChildrenUnchecked(parent))
            {
                parent.StateImageIndex = STATE_UNCHECKED;
            }
            else
            {
                parent.StateImageIndex = STATE_MIXED;
            }
            UpdateParent(parent);
        }

        //returns a value indicating if all children are checked
        public static bool IsChildrenChecked(TreeNode parent)
        {
            return IsAllChildrenSame(parent, STATE_CHECKED);
        }

        //returns a value indicating if all children are unchecked
        public static bool IsChildrenUnchecked(TreeNode parent)
        {
            return IsAllChildrenSame(parent, STATE_UNCHECKED);
        }

        //returns a value indicating if all children are in the same state
        public static bool IsAllChildrenSame(TreeNode parent, int state)
        {
            foreach (TreeNode node in parent.Nodes)
            {
                if (node.StateImageIndex != state)
                {
                    return false;
                }
                if (node.Nodes.Count != 0 && !IsAllChildrenSame(node, state))
                {
                    return false;
                }

            }
            return true;
        }

        //build the checked, unchecked and indeterminate images
        private static Image GetStateImage(CheckBoxState state, Size imageSize)
        {
            Bitmap bmp = new Bitmap(16, 16);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                Point pt = new Point((16 - imageSize.Width) / 2, (16 - imageSize.Height) / 2);
                CheckBoxRenderer.DrawCheckBox(g, pt, state);
            }
            return bmp;
        }


        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (_InternalStateImageList == null)
            {
                _InternalStateImageList = new ImageList();
                using (Graphics g = base.CreateGraphics())
                {
                    Size glyphSize = CheckBoxRenderer.GetGlyphSize(g, CheckBoxState.UncheckedNormal);
                    _InternalStateImageList.Images.Add(GetStateImage(CheckBoxState.UncheckedNormal, glyphSize));
                    _InternalStateImageList.Images.Add(GetStateImage(CheckBoxState.CheckedNormal, glyphSize));
                    _InternalStateImageList.Images.Add(GetStateImage(CheckBoxState.MixedNormal, glyphSize));
                    _InternalStateImageList.Images.Add(GetStateImage(CheckBoxState.UncheckedDisabled, glyphSize));
                    _InternalStateImageList.Images.Add(GetStateImage(CheckBoxState.CheckedDisabled, glyphSize));
                    _InternalStateImageList.Images.Add(GetStateImage(CheckBoxState.MixedDisabled, glyphSize));
                }
            }
            base.StateImageList = _InternalStateImageList;

            InitializeNodesState(base.Nodes);
        }

        //check if user click on the state image
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (e.Button == MouseButtons.Left)
            {
                TreeViewHitTestInfo info = base.HitTest(e.Location);
                if (info.Node != null && info.Location == TreeViewHitTestLocations.StateImage)
                {
                    TreeNode node = info.Node;
                    switch (node.StateImageIndex)
                    {
                        case STATE_UNCHECKED:
                        case STATE_MIXED:
                            node.StateImageIndex = STATE_CHECKED;
                            break;
                        case STATE_CHECKED:
                            node.StateImageIndex = STATE_UNCHECKED;
                            break;
                    }
                    UpdateChildren(node);
                    UpdateParent(node);
                }
            }
        }

        //check if user press the space key
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Space)
            {
                if (base.SelectedNode != null)
                {
                    TreeNode node = base.SelectedNode;
                    switch (node.StateImageIndex)
                    {
                        case STATE_UNCHECKED:
                        case STATE_MIXED:
                            node.StateImageIndex = STATE_CHECKED;
                            break;
                        case STATE_CHECKED:
                            node.StateImageIndex = STATE_UNCHECKED;
                            break;
                    }
                    UpdateChildren(node);
                    UpdateParent(node);
                }
            }
        }

        //swap between enabled and disabled images
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);


            for (int i = 0; i < 3; i++)
            {
                Image img = _InternalStateImageList.Images[0];
                _InternalStateImageList.Images.RemoveAt(0);
                _InternalStateImageList.Images.Add(img);

            }
        }
    }
}