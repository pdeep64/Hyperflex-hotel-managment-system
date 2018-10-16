using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HYFLEX_HMS.FORMS
{
    public partial class ABOUNT_WE : Form
    {
        public ABOUNT_WE()
        {
            InitializeComponent();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            ((Form)this.TopLevelControl).Close();
        }
    }
}
