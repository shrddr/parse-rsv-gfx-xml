using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace rsv_xml1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "XML files (*.xml)|*.xml";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                DeserializeTreeView(treeView1, openFileDialog1.FileName);
                //StreamReader reader = new System.IO.StreamReader(openFileDialog1.FileName, System.Text.Encoding.GetEncoding("Windows-1252"), true);
                //XmlTextReader xmlReader = new XmlTextReader(reader);

                //TreeNode current = null;

                //while (xmlReader.Read())
                //{
                //    xmlReader.
                //    switch (xmlReader.NodeType)
                //    {
                //        case XmlNodeType.Element: // The node is an Element.
                //            current = treeView1.Nodes.Add("<" + xmlReader.Name + ">");
                //            break;
                //        case XmlNodeType.Text: // Display the text in each element.
                //            current.Nodes.Add(xmlReader.Value);
                //            break;
                //        case XmlNodeType.EndElement: // Display end of element.
                //            Console.Write("</" + xmlReader.Name);
                //            Console.WriteLine(">");
                //            break;
                //    }
                //}
                        


            }
        }

        public void DeserializeTreeView(TreeView treeView, string fileName)
        {
            StreamReader Encreader = null;
            XmlTextReader reader = null;
            try
            {
                // disabling re-drawing of treeview till all nodes are added
                treeView.BeginUpdate();
                Encreader = new StreamReader(fileName, Encoding.GetEncoding("Windows-1252"), true);
                reader = new XmlTextReader(Encreader);
                TreeNode parentNode = null;
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                       
                        TreeNode newNode = new TreeNode();
                        newNode.Text = reader.Name;

                        bool isEmptyElement = reader.IsEmptyElement;

                        // loading node attributes
                        int attributeCount = reader.AttributeCount;
                        if (attributeCount > 0)
                        {
                            for (int i = 0; i < attributeCount; i++)
                            {
                                reader.MoveToAttribute(i);
                                newNode.Text += String.Format(" {0}={1}", reader.Name, reader.Value);
                            } 
                        }
                        // add new node to Parent Node or TreeView
                        if (parentNode != null)
                            parentNode.Nodes.Add(newNode);
                        else
                            treeView.Nodes.Add(newNode);

                        // making current node 'ParentNode' if its not empty
                        if (!isEmptyElement)
                        {
                            parentNode = newNode;
                        }
                        
                    }
                    // moving up to in TreeView if end tag is encountered
                    else if (reader.NodeType == XmlNodeType.EndElement)
                    {
                        if (true)//reader.Name == XmlNodeTag)
                        {
                            parentNode = parentNode.Parent;
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.XmlDeclaration)
                    {
                        //Ignore Xml Declaration                    
                    }
                    else if (reader.NodeType == XmlNodeType.None)
                    {
                        return;
                    }
                    else if (reader.NodeType == XmlNodeType.Text)
                    {
                        parentNode.Nodes.Add(reader.Value);
                    }

                }
            }
            finally
            {
                // enabling redrawing of treeview after all nodes are added
                treeView.EndUpdate();
                reader.Close();
            }
        }

    }
}
