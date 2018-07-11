using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileManager
{
    public partial class ResOfFindData : Form
    {
        HashSet<string> list = new HashSet<string>();

        public ResOfFindData()
        {
            InitializeComponent();
        }

        public void AddListEl(string str)
        {
            list.Add(str);
        }

        public void CopyOllInListView()
        {
            foreach (string str in list)
            {
                ListViewItem it = new ListViewItem();
                it.Text = str;
                it.ImageIndex = 0;

                listViewOfFiles.Items.Add(it);
            }
        }

        private void listViewOfFiles_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
