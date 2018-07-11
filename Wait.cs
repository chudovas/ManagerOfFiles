using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileManager
{
    public partial class Wait : Form
    {
        CancellationTokenSource Cans = null;

        public Wait(CancellationTokenSource cans)
        {
            InitializeComponent();
            Cans = cans;
        }

        public Wait()
        {
            InitializeComponent();
        }

        public void ChangeProgress(int progress)
        {
            progressBar2.Value = progress;
            progressBar2.Update();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Cans != null)
                Cans.Cancel();
        }

        private void progressBar2_Click(object sender, EventArgs e)
        {

        }
    }
}
