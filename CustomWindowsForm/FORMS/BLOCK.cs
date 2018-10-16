using CrystalDecisions.Shared.Json;
using HYFLEX_HMS.CLASS;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HYFLEX_HMS.FORMS
{
    public partial class BLOCK : Form
    {
        public BLOCK()
        {
            InitializeComponent();
            timer1.Start();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            Application.Exit();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           lbl_time.Text = DateTime.Now.ToString("yyyy-MM-dd - hh.mm.ss.fff");
           
        }

        private void label5_MouseEnter(object sender, EventArgs e)
        {
            label5.BackColor = Color.Maroon;
        }

        private void label5_MouseLeave(object sender, EventArgs e)
        {
            label5.BackColor = Color.Black;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }
        string res;
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            
        }

        private void BLOCK_Load(object sender, EventArgs e)
        {
            LBL_KEY.Text = CLS_REGISTER.getMachineID();
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromSeconds(15);

            var timer = new System.Threading.Timer((V) =>
            {
               CLS_REGISTER.CheckStatus();
            }, null, startTimeSpan, periodTimeSpan);
        }
    }
}
