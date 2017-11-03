using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace XmlEditor
{
    public partial class MainForm : Form
    {
        #region Private fields
        
        private XmlDocument _xmlDoc;
        private TreeView _currentTreeView;
        
        #endregion

        #region Constructor

        public MainForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Private methods

        private void OpenFile()
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                try
                {
                    _xmlDoc = new XmlDocument();
                    _xmlDoc.Load(openFileDialog.FileName);
                }
                catch (FileNotFoundException e)
                {
                    MessageBox.Show("File can not be found. Please try again");
                }
        }

        private void LoadTreeFromXmlDocument()
        {
            try
            {
                _currentTreeView.Nodes.Clear();

                foreach (XmlNode node in _xmlDoc.DocumentElement.ChildNodes)
                    AddNode(_currentTreeView.Nodes, node);

                _currentTreeView.ExpandAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException?.ToString() ?? ex.Message);
            }
        }

        private static void GetAttributes(XmlNode node, StringBuilder text)
        {
            text.AppendFormat(node.Attributes?["name"]?.Value ?? (!node.Name.Equals("#text") ? node.Name : node.InnerText));

            if (node.Attributes == null || node.Attributes.Count < 2)
                return;

            int i;
            text.AppendFormat(" (");
            for (i = 1; i < node.Attributes.Count - 1; i++)
                text.AppendFormat(node.Attributes[i].Name + "=" + node.Attributes[i].Value + ", ");
            text.AppendFormat(node.Attributes[i].Name + "=" + node.Attributes[i].Value + ")\n");
        }

        private static void AddNode(TreeNodeCollection nodes, XmlNode node)
        {
            var text = new StringBuilder();
            GetAttributes(node, text);

            if (node.HasChildNodes)
            {
                TreeNode newNode = nodes.Add(text.ToString());

                XmlNodeList childNodes = node.ChildNodes;
                for (int i = 0; i <= childNodes.Count - 1; i++)
                {
                    XmlNode childNode = node.ChildNodes[i];
                    AddNode(newNode.Nodes, childNode);
                }
            }
            else
                nodes.Add(text.ToString());
        }

        private void EditAttribute(TreeNode node)
        {
            
        }

        #endregion

        #region Event handlers

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFile();

                saveAsToolStripMenuItem.Enabled = true;
                saveToolStripMenuItem.Enabled = true;                
                var treeView = new TreeView();
                var page = new TabPage(Path.GetFileName(_xmlDoc.BaseURI))
                {
                    Width = tabs.Width,
                    Height = tabs.Height
                };
                treeView.Width = tabs.Width;
                treeView.Height = tabs.Height;
                treeView.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                page.Controls.Add(treeView);
                tabs.TabPages.Add(page);
                closeTabToolStripMenuItem.Enabled = true;
                treeView.MouseDoubleClick += treeView_MouseDoubleClick;
                _currentTreeView = treeView;

                LoadTreeFromXmlDocument();
            }
            catch (Exception)
            {
                MessageBox.Show("Error");
            }
                                
        }        

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }                

        private void treeView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var localTreeView = (TreeView)sender;
            var node = localTreeView.SelectedNode;
            EditAttribute(node);
        }        

        private void closeTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabs.TabPages.Count > 1)
            {                
                tabs.SelectedIndex = tabs.SelectedIndex - 1 > 0 ? tabs.SelectedIndex - 1 : tabs.SelectedIndex;
                _currentTreeView = tabs.SelectedTab.Controls[0] as TreeView;
                tabs.TabPages.Remove(tabs.SelectedTab);                                
            }

            if (tabs.TabPages.Count == 1)
            {
                saveAsToolStripMenuItem.Enabled = false;
                saveToolStripMenuItem.Enabled = false;
                closeTabToolStripMenuItem.Enabled = false;                                                                
            }            
        }

        private void tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {                            
                _currentTreeView = tabs.SelectedTab?.Controls[0] as TreeView;
            }
            catch (ArgumentOutOfRangeException) { }
        }

        #endregion
    }
}
