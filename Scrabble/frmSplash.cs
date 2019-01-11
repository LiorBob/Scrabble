using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;



namespace Scrabble
{
    public partial class frmSplash : Form
    {
        public frmSplash()
        {
            InitializeComponent();

            lblVersion.Text = "Version " + Application.ProductVersion.Substring(0, 3);
            lblCopyright.Text = lblCopyright.Text.Replace("(C)", "\u00A9");
        }

        private void tmrSplash_Tick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}