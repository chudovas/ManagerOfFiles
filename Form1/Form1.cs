using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace FileManager
{
    public partial class Form1 : Form, IView
    {
        IEntity selectedItem;

        Login log;
        string userName, password;

        EntityDriveInfo[] drivers;

        public event EventHandler GetBackClick;
        public event EventHandler<EntityDriveInfo> DiskClick;
        public event EventHandler<IEntity> ElementClick;
        public event EventHandler<IEntity> EncryptElementClick;
        public event EventHandler<IEntity> DecryptElementClick;

        public event EventHandler<string[]> LoadElementClick;
        public event EventHandler<List<IEntity>> ArchiveElementClick;
        public event EventHandler<IEntity> ExtractArchiveElementClick;

        public event EventHandler<IEntity> FindPersDataThreadClick;
        public event EventHandler<IEntity> FindPersDataForEachClick;
        public event EventHandler<IEntity> FindPersDataTaskClick;
        public event EventHandler<IEntity> FindPersDataAsicnClick;

        public event EventHandler<string> FindElementClick;
        public event EventHandler<IEntity> StatisticOfElementClick;
        public event EventHandler<List<IEntity>> CopyElementClick;
        public event EventHandler<List<IEntity>> DeleteElementClick;
        public event EventHandler<KeyValuePair<IEntity, string>> RenameElementClick;
        public event EventHandler PastElementClick;
        public event EventHandler<string> CreateFileClick;
        public event EventHandler<string> CreateDirectoryClick;

        public Form1()
        {                                                                                
            InitializeComponent();
            drivers = EntityFunctions.GetDrivers();

            foreach (var disc in drivers)
                listBoxOfDisks.Items.Add(disc);

            fontDialog1.ShowColor = true;

            listBoxOfDisks.DoubleClick += new EventHandler(listBoxOfDisks_DoubleClick);
            listViewOfFiles.DoubleClick += new EventHandler(listViewOfFiles_DoubleClick);
        }

        public void LoginAndPassOk()
        {
            Enabled = true;
            log.Close();
        }

        private void listBoxOfDisks_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxOfDisks.SelectedItem == null)
                return;

            DiskClick(this, (EntityDriveInfo)listBoxOfDisks.SelectedItem);
        }

        private void listViewOfFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewOfFiles.SelectedItems.Count == 0)
            {
                selectedItem = null;
                return;
            }

            selectedItem = (IEntity)listViewOfFiles.SelectedItems[0].Tag;
        }

        private void listViewOfFiles_DoubleClick(object sender, EventArgs e)
        {
            if (listViewOfFiles.SelectedItems.Count == 0)
                return;

            ElementClick(this, (IEntity)(listViewOfFiles.SelectedItems[0].Tag));
        }

        delegate void Updat(IEnumerable<IEntity> items, string fullPath);

        public void ViewDirect(IEnumerable<IEntity> items, string fullPath)
        {
            try
            {
                listViewOfFiles.Invoke(new Updat((it, fP) =>
                {
                    listViewOfFiles.Clear();

                    foreach (IEntity info in it)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = info.GetName();
                        item.ToolTipText = info.GetFullName();

                        switch (info.GetType())
                        {
                            case EntityType.DIRECTORY:
                                item.ImageIndex = 0;
                                break;

                            case EntityType.FILE:
                                item.ImageIndex = 1;
                                break;

                            case EntityType.ZIPDIRECTORY:
                                item.ImageIndex = 0;
                                break;

                            case EntityType.ZIPFILE:
                                item.ImageIndex = 1;
                                break;

                            case EntityType.ZIP:
                                item.ImageIndex = 2;
                                break;
                        }

                        item.Tag = info;
                        listViewOfFiles.Items.Add(item);
                    }
                }), items, fullPath);

                textBox1.Invoke(new Updat((it, fP) =>
                {
                    textBox1.Text = fullPath;
                }), items, fullPath);
        }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("You havn't got access to this files!");
                GetBackClick(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error! " + ex.Message);
                GetBackClick(this, EventArgs.Empty);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetBackClick(this, e);
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            so.font = fontDialog1.Font;
            so.fontColor = fontDialog1.Color;
            ChangeOptions();
        }

        private void backgroundImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Bitmap files(*.bmp) | *.bmp | Image files(*.jpg) | *.jpg";
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            so.im = new Bitmap(openFileDialog1.FileName);
            ChangeOptions();
        }

        private void usernameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != null && textBox2.Text != "")
            {
                so.username = textBox2.Text;
                ChangeOptions();
            }
            else
                MessageBox.Show("Input correct username!");
        }

        private void encryptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedItem == null)
            {
                MessageBox.Show("Select file!");
                return;
            }

            string key = textBox2.Text;

            if (key == "")
            {
                MessageBox.Show("Input correct key!");
                return;
            }

            selectedItem.cryptKey = key;

            try
            {
                EncryptElementClick(this, selectedItem);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error!" + ex);
            }
        }

        private void decryptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedItem == null)
            {
                MessageBox.Show("Select file!");
                return;
            }

            string key = textBox2.Text;

            if (key == "")
            {
                MessageBox.Show("Input correct key!");
                return;
            }

            selectedItem.cryptKey = key;
            try
            {
                DecryptElementClick(this, selectedItem);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error! " + ex.Message);
            }
        }

        private void passwordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != null && textBox2.Text != "")
            {
                so.password = textBox2.Text;
                ChangeOptions();
            }
            else
                MessageBox.Show("Input correct password!");
        }

        private void loadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] names = textBox2.Text.Split(' ');
            LoadElementClick(this, names);
        }

        private void threadsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FindPersDataThreadClick(this, selectedItem);    
        }

        private void parallelsForEachToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FindPersDataForEachClick(this, selectedItem);
        }

        private void taskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FindPersDataTaskClick(this, selectedItem);
        }

        private void asyncawaitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FindPersDataAsicnClick(this, selectedItem);
        }

        private void findFiledirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FindElementClick(this, textBox2.Text);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewOfFiles.SelectedItems.Count == 0)
                return;
            List<IEntity> l = new List<IEntity>();

            foreach (ListViewItem el in listViewOfFiles.SelectedItems)
                l.Add((IEntity)el.Tag);

            DeleteElementClick(this, l);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewOfFiles.SelectedItems.Count == 0)
                return;
            List<IEntity> l = new List<IEntity>();

            foreach (ListViewItem el in listViewOfFiles.SelectedItems)
                l.Add((IEntity)el.Tag);

            CopyElementClick(this, l);
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PastElementClick(this, EventArgs.Empty);
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                MessageBox.Show("Input text!");
                return;
            }

            RenameElementClick(this, new KeyValuePair<IEntity, string>(selectedItem, textBox2.Text));
        }

        private void statisticToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StatisticOfElementClick(this, selectedItem);
        }

        private void archiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedItem == null)
            {
                MessageBox.Show("Select file!");
                return;
            }

            List<IEntity> l = new List<IEntity>();
            foreach (ListViewItem el in listViewOfFiles.SelectedItems)
                l.Add((IEntity)el.Tag);

            ArchiveElementClick(this, l);
        }

        private void createFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                MessageBox.Show("Input file name!");
                return;
            }

            CreateFileClick(this, textBox2.Text);
        }

        private void createDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                MessageBox.Show("Input directory name!");
                return;
            }

            CreateDirectoryClick(this, textBox2.Text);
        }

        private void extractArchiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedItem == null)
            {
                MessageBox.Show("Select file!");
                return;
            }

            ExtractArchiveElementClick(this, selectedItem);
        }
    }
}
