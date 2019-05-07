using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Workshop4
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        // user logs in - open package form
        private void btnLogIn_Click(object sender, EventArgs e)
        {
            frmMain openPackageForm = new frmMain();
            openPackageForm.ShowDialog();
            this.Close();
        }

        // user exits
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
