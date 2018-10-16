using CustomWindowsForm.CLASS;
using CustomWindowsForm.FORMS;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using HYFLEX_HMS;
using HYFLEX_HMS.Properties;
using HYFLEX_HMS.FORMS;
using Google.Apis.Calendar.v3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3.Data;
using System.Threading;
using Google.Apis.Util.Store;
using Google.Apis.Services;
using HYFLEX_HMS.CLASS;

namespace CustomWindowsForm
{
    public partial class DASHBOARD : Form
    {
        public DASHBOARD()
        {
            InitializeComponent();
        }

        private void BlackForm_Load(object sender, EventArgs e)
        {
            timer1.Start();
            if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                LBL_1.ForeColor = Color.White;
                LBL_2.ForeColor = Color.White;
            }
            else if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
            {
                LBL_1.ForeColor = Color.Yellow;
                LBL_2.ForeColor = Color.Yellow;
            }
            else if (DateTime.Now.DayOfWeek == DayOfWeek.Thursday)
            {
                LBL_1.ForeColor = Color.Green;
                LBL_2.ForeColor = Color.Green;
            }
            else if (DateTime.Now.DayOfWeek == DayOfWeek.Wednesday)
            {
                LBL_1.ForeColor = Color.Orange;
                LBL_2.ForeColor = Color.Orange;
            }
            else if (DateTime.Now.DayOfWeek == DayOfWeek.Thursday)
            {
                LBL_1.ForeColor = Color.Red;
                LBL_2.ForeColor = Color.Red;
            }
            else if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                LBL_1.ForeColor = Color.Blue;
                LBL_2.ForeColor = Color.Blue;
            }
            else if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
            {
                LBL_1.ForeColor = Color.Purple;
                LBL_2.ForeColor = Color.Purple;
            }
            LBL_EXCHANGE_RATE.Text = Convert.ToDouble(HYFLEX_HMS.Properties.Settings.Default.exchange_rate_lrk).ToString("F2");

            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromSeconds(30);

            var timer = new System.Threading.Timer((V) =>
            {
                CLS_REGISTER.CheckStatus();
            }, null, startTimeSpan, periodTimeSpan);
        }

        public void ShowControllers()
        {

        }

        bool isTopPanelDragged = false;
        bool isLeftPanelDragged = false;
        bool isRightPanelDragged = false;
        bool isBottomPanelDragged = false;
        bool isTopBorderPanelDragged = false;

        bool isRightBottomPanelDragged = false;
        bool isLeftBottomPanelDragged = false;
        bool isRightTopPanelDragged = false;
        bool isLeftTopPanelDragged = false;

        bool isWindowMaximized = false;
        Point offset;
        Size _normalWindowSize;
        Point _normalWindowLocation = Point.Empty;



        private void TopBorderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isTopBorderPanelDragged = true;
            }
            else
            {
                isTopBorderPanelDragged = false;
            }
        }


        private void TopBorderPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Y < this.Location.Y)
            {
                if (isTopBorderPanelDragged)
                {
                    if (this.Height < 50)
                    {
                        this.Height = 50;
                        isTopBorderPanelDragged = false;
                    }
                    else
                    {
                        this.Location = new Point(this.Location.X, this.Location.Y + e.Y);
                        this.Height = this.Height - e.Y;
                    }
                }
            }
        }


        private void TopBorderPanel_MouseUp(object sender, MouseEventArgs e)
        {
            isTopBorderPanelDragged = false;
        }



        private void TopPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isTopPanelDragged = true;
                Point pointStartPosition = this.PointToScreen(new Point(e.X, e.Y));
                offset = new Point();
                offset.X = this.Location.X - pointStartPosition.X;
                offset.Y = this.Location.Y - pointStartPosition.Y;
            }
            else
            {
                isTopPanelDragged = false;
            }
            if (e.Clicks == 2)
            {
                isTopPanelDragged = false;
              //  _MaxButton_Click(sender, e);
            }
        }

        private void TopPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (isTopPanelDragged)
            {
                Point newPoint = TopPanel.PointToScreen(new Point(e.X, e.Y));
                newPoint.Offset(offset);
                this.Location = newPoint;

                if (this.Location.X > 2 || this.Location.Y > 2)
                {
                    if (this.WindowState == FormWindowState.Maximized)
                    {
                        this.Location = _normalWindowLocation;
                        this.Size = _normalWindowSize;
                        toolTip1.SetToolTip(_MaxButton, "Maximize");
                        _MaxButton.CFormState = MinMaxButton.CustomFormState.Normal;
                        isWindowMaximized = false;
                    }
                }
            }
        }


        private void TopPanel_MouseUp(object sender, MouseEventArgs e)
        {
            isTopPanelDragged = false;
            if (this.Location.Y <= 5)
            {
                if (!isWindowMaximized)
                {
                    _normalWindowSize = this.Size;
                    _normalWindowLocation = this.Location;

                    Rectangle rect = Screen.PrimaryScreen.WorkingArea;
                    this.Location = new Point(0, 0);
                    this.Size = new System.Drawing.Size(rect.Width, rect.Height);
                    toolTip1.SetToolTip(_MaxButton, "Restore Down");
                    _MaxButton.CFormState = MinMaxButton.CustomFormState.Maximize;
                    isWindowMaximized = true;
                }
            }
        }



        private void LeftPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.Location.X <= 0 || e.X < 0)
            {
                isLeftPanelDragged = false;
                this.Location = new Point(10, this.Location.Y);
            }
            else
            {
                if (e.Button == MouseButtons.Left)
                {
                    isLeftPanelDragged = true;
                }
                else
                {
                    isLeftPanelDragged = false;
                }
            }
        }

        private void LeftPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.X < this.Location.X)
            {
                if (isLeftPanelDragged)
                {
                    if (this.Width < 100)
                    {
                        this.Width = 100;
                        isLeftPanelDragged = false;
                    }
                    else
                    {
                        this.Location = new Point(this.Location.X + e.X, this.Location.Y);
                        this.Width = this.Width - e.X;
                    }
                }
            }
        }

        private void LeftPanel_MouseUp(object sender, MouseEventArgs e)
        {
            isLeftPanelDragged = false;
        }



        private void RightPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isRightPanelDragged = true;
            }
            else
            {
                isRightPanelDragged = false;
            }
        }

        private void RightPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (isRightPanelDragged)
            {
                if (this.Width < 100)
                {
                    this.Width = 100;
                    isRightPanelDragged = false;
                }
                else
                {
                    this.Width = this.Width + e.X;
                }
            }
        }

        private void RightPanel_MouseUp(object sender, MouseEventArgs e)
        {
            isRightPanelDragged = false;
        }



        private void BottomPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isBottomPanelDragged = true;
            }
            else
            {
                isBottomPanelDragged = false;
            }
        }

        private void BottomPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (isBottomPanelDragged)
            {
                if (this.Height < 50)
                {
                    this.Height = 50;
                    isBottomPanelDragged = false;
                }
                else
                {
                    this.Height = this.Height + e.Y;
                }
            }
        }

        private void BottomPanel_MouseUp(object sender, MouseEventArgs e)
        {
            isBottomPanelDragged = false;
        }


        private void _MinButton_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void _MaxButton_Click(object sender, EventArgs e)
        {
            if (isWindowMaximized)
            {
                this.Location = _normalWindowLocation;
                this.Size = _normalWindowSize;
                toolTip1.SetToolTip(_MaxButton, "Maximize");
                _MaxButton.CFormState = MinMaxButton.CustomFormState.Normal;
                isWindowMaximized = false;
            }
            else
            {
                _normalWindowSize = this.Size;
                _normalWindowLocation = this.Location;

                Rectangle rect = Screen.PrimaryScreen.WorkingArea;
                this.Location = new Point(0, 0);
                this.Size = new System.Drawing.Size(rect.Width, rect.Height);
                toolTip1.SetToolTip(_MaxButton, "Restore Down");
                _MaxButton.CFormState = MinMaxButton.CustomFormState.Maximize;
                isWindowMaximized = true;
            }
        }

        private void _CloseButton_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
        }




        private void RightBottomPanel_1_MouseDown(object sender, MouseEventArgs e)
        {
            isRightBottomPanelDragged = true;
        }

        private void RightBottomPanel_1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isRightBottomPanelDragged)
            {
                if (this.Width < 100 || this.Height < 50)
                {
                    this.Width = 100;
                    this.Height = 50;
                    isRightBottomPanelDragged = false;
                }
                else
                {
                    this.Width = this.Width + e.X;
                    this.Height = this.Height + e.Y;
                }
            }
        }


        private void RightBottomPanel_1_MouseUp(object sender, MouseEventArgs e)
        {
            isRightBottomPanelDragged = false;
        }

        private void RightBottomPanel_2_MouseDown(object sender, MouseEventArgs e)
        {
            RightBottomPanel_1_MouseDown(sender, e);
        }

        private void RightBottomPanel_2_MouseMove(object sender, MouseEventArgs e)
        {
            RightBottomPanel_1_MouseMove(sender, e);
        }

        private void RightBottomPanel_2_MouseUp(object sender, MouseEventArgs e)
        {
            RightBottomPanel_1_MouseUp(sender, e);
        }



        private void LeftBottomPanel_1_MouseDown(object sender, MouseEventArgs e)
        {
            isLeftBottomPanelDragged = true;
        }

        private void LeftBottomPanel_1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.X < this.Location.X)
            {
                if (isLeftBottomPanelDragged || this.Height < 50)
                {
                    if (this.Width < 100)
                    {
                        this.Width = 100;
                        this.Height = 50;
                        isLeftBottomPanelDragged = false;
                    }
                    else
                    {
                        this.Location = new Point(this.Location.X + e.X, this.Location.Y);
                        this.Width = this.Width - e.X;
                        this.Height = this.Height + e.Y;
                    }
                }
            }
        }

        private void LeftBottomPanel_1_MouseUp(object sender, MouseEventArgs e)
        {
            isLeftBottomPanelDragged = false;
        }

        private void LeftBottomPanel_2_MouseDown(object sender, MouseEventArgs e)
        {
            LeftBottomPanel_1_MouseDown(sender, e);
        }

        private void LeftBottomPanel_2_MouseMove(object sender, MouseEventArgs e)
        {
            LeftBottomPanel_1_MouseMove(sender, e);
        }

        private void LeftBottomPanel_2_MouseUp(object sender, MouseEventArgs e)
        {
            LeftBottomPanel_1_MouseUp(sender, e);
        }




        private void RightTopPanel_1_MouseDown(object sender, MouseEventArgs e)
        {
            isRightTopPanelDragged = true;
        }

        private void RightTopPanel_1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Y < this.Location.Y || e.X < this.Location.X)
            {
                if (isRightTopPanelDragged)
                {
                    if (this.Height < 50 || this.Width < 100)
                    {
                        this.Height = 50;
                        this.Width = 100;
                        isRightTopPanelDragged = false;
                    }
                    else
                    {
                        this.Location = new Point(this.Location.X, this.Location.Y + e.Y);
                        this.Height = this.Height - e.Y;
                        this.Width = this.Width + e.X;
                    }
                }
            }
        }

        private void RightTopPanel_1_MouseUp(object sender, MouseEventArgs e)
        {
            isRightTopPanelDragged = false;
        }

        private void RightTopPanel_2_MouseDown(object sender, MouseEventArgs e)
        {
            RightTopPanel_1_MouseDown(sender, e);
        }

        private void RightTopPanel_2_MouseMove(object sender, MouseEventArgs e)
        {
            RightTopPanel_1_MouseMove(sender, e);
        }

        private void RightTopPanel_2_MouseUp(object sender, MouseEventArgs e)
        {
            RightTopPanel_1_MouseUp(sender, e);
        }





        private void LeftTopPanel_1_MouseDown(object sender, MouseEventArgs e)
        {
            isLeftTopPanelDragged = true;
        }

        private void LeftTopPanel_1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.X < this.Location.X || e.Y < this.Location.Y)
            {
                if (isLeftTopPanelDragged)
                {
                    if (this.Width < 100 || this.Height < 50)
                    {
                        this.Width = 100;
                        this.Height = 100;
                        isLeftTopPanelDragged = false;
                    }
                    else
                    {
                        this.Location = new Point(this.Location.X + e.X, this.Location.Y);
                        this.Width = this.Width - e.X;
                        this.Location = new Point(this.Location.X, this.Location.Y + e.Y);
                        this.Height = this.Height - e.Y;
                    }
                }
            }

        }

        private void LeftTopPanel_1_MouseUp(object sender, MouseEventArgs e)
        {
            isLeftTopPanelDragged = false;
        }

        private void LeftTopPanel_2_MouseDown(object sender, MouseEventArgs e)
        {
            LeftTopPanel_1_MouseDown(sender, e);
        }

        private void LeftTopPanel_2_MouseMove(object sender, MouseEventArgs e)
        {
            LeftTopPanel_1_MouseMove(sender, e);
        }

        private void LeftTopPanel_2_MouseUp(object sender, MouseEventArgs e)
        {
            LeftTopPanel_1_MouseUp(sender, e);
        }

        private void WindowTextLabel_MouseDown(object sender, MouseEventArgs e)
        {
            TopPanel_MouseDown(sender, e);
        }

        private void WindowTextLabel_MouseMove(object sender, MouseEventArgs e)
        {
            TopPanel_MouseMove(sender, e);
        }

        private void WindowTextLabel_MouseUp(object sender, MouseEventArgs e)
        {
            TopPanel_MouseUp(sender, e);
        }

        private void BTN_Q1_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(CATEGORY))
                {
                    f.Activate();
                    return;
                }
            }
            Form CATEGORY = new CATEGORY();
            CATEGORY.MdiParent = this;
            CATEGORY.Show();
        }

        private void shapedButton1_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(ITEMS))
                {
                    f.Activate();
                    return;
                }
            }
            Form ITEMS = new ITEMS();
            ITEMS.MdiParent = this;
            ITEMS.Show();
        }

        private void shapedButton2_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(SUPPLIER))
                {
                    f.Activate();
                    return;
                }
            }
            Form ITEMS = new SUPPLIER();
            ITEMS.MdiParent = this;
            ITEMS.Show();

        }

        private void shapedButton3_Click(object sender, EventArgs e)
        {

        }

        private void shapedButton3_Click_1(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(GRN))
                {
                    f.Activate();
                    return;
                }
            }
            Form ITEMS = new GRN();
            ITEMS.MdiParent = this;
            ITEMS.Show();
        }

        private void shapedButton4_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(VIEW_STOCK))
                {
                    f.Activate();
                    return;
                }
            }
            Form ITEMS = new VIEW_STOCK();
            ITEMS.MdiParent = this;
            ITEMS.Show();
        }

        private void shapedButton5_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(NEW_GUEST))
                {
                    f.Activate();
                    return;
                }
            }
            Form ITEMS = new NEW_GUEST();
            ITEMS.MdiParent = this;
            ITEMS.Show();
        }

        private void shapedButton7_Click(object sender, EventArgs e)
        {
           
        }
        bool TYPE = true;
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                LBL_TIME.Text = "Today is: " + DateTime.Now.ToLongDateString() + " Time: " + DateTime.Now.ToLongTimeString();
                lbl_vurrent_logger.Text = CLS_CURRENT_LOGGER.LOGGED_IN_USER_TYPE;
                if ((Convert.ToDouble(LBL_EXCHANGE_RATE.Text) != HYFLEX_HMS.Properties.Settings.Default.exchange_rate_lrk) && TYPE == false)
                {
                    LBL_EXCHANGE_RATE.Text = HYFLEX_HMS.Properties.Settings.Default.exchange_rate_lrk.ToString("F2");
                }
            }
            catch (Exception)
            {

            }
        }

        private void BTN_SERVATION_Click(object sender, EventArgs e)
        {
           
        }
        ToolTip TT;
        private void LBL_EXCHANGE_RATE_Leave(object sender, EventArgs e)
        {
            TYPE = false;
        }

        private void LBL_EXCHANGE_RATE_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TYPE = false;
                double LAST_SAVED_PRICE = Convert.ToDouble(HYFLEX_HMS.Properties.Settings.Default.exchange_rate_lrk);
                if (Convert.ToDouble(LBL_EXCHANGE_RATE.Text) > 0)
                {
                    HYFLEX_HMS.Properties.Settings.Default.exchange_rate_lrk = Convert.ToDouble(LBL_EXCHANGE_RATE.Text);
                    HYFLEX_HMS.Properties.Settings.Default.Save();
                    MessageBox.Show("RATE UPDATED!. NEW RATE IS : " + HYFLEX_HMS.Properties.Settings.Default.exchange_rate_lrk.ToString("F2"), "ALERT", MessageBoxButtons.OK);
                }
            }
        }

        private void LBL_TIME_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void lbl_vurrent_logger_Click(object sender, EventArgs e)
        {

        }
        private const string urlPattern = "http://rate-exchange-1.appspot.com/currency?from={0}&to={1}";
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        public double CurrencyConversion(string fromCurrency, string toCurrency)
        {
            using (var webClient = new System.Net.WebClient())
            {
                string URL = "http://free.currencyconverterapi.com/api/v5/convert?q=" + fromCurrency + "_" + toCurrency + "&compact=y";
                var json = webClient.DownloadString(URL);
                var obj = JsonConvert.DeserializeObject<RootObject>(json);

                return obj.USD_LKR.val;
            }

        }
        public class USDLKR
        {
            public double val { get; set; }
        }

        public class RootObject
        {
            public USDLKR USD_LKR { get; set; }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if(CheckForInternetConnection()==true)
            {
                double res = 1 * CurrencyConversion("USD", "LKR");
                HYFLEX_HMS.Properties.Settings.Default.exchange_rate_lrk = res;
                HYFLEX_HMS.Properties.Settings.Default.Save();
                LBL_EXCHANGE_RATE.Text = HYFLEX_HMS.Properties.Settings.Default.exchange_rate_lrk.ToString("F2");
                MessageBox.Show("Latest Rate Updated!");
            }
        }

        private void LBL_EXCHANGE_RATE_TextChanged(object sender, EventArgs e)
        {
            TYPE = true;
        }

        private void rOOMPACKAGESToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(ROOM_PACKAGE))
                {
                    f.Activate();
                    return;
                }
            }
            Form ROOM_PACKAGES = new ROOM_PACKAGE();
            ROOM_PACKAGES.MdiParent = this;
            ROOM_PACKAGES.Show();
        }

        private void rOOMSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(ROOMS))
                {
                    f.Activate();
                    return;
                }
            }
            Form room = new ROOMS();
            room.MdiParent = this;
            room.Show();
        }

        private void mEALPLANToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(MEAL_PLAN))
                {
                    f.Activate();
                    return;
                }
            }
            Form meal_plans = new MEAL_PLAN();
            meal_plans.MdiParent = this;
            meal_plans.Show();
        }

        private void aDDITIONALSERVICESToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(ADDITIONAL_SERVICE))
                {
                    f.Activate();
                    return;
                }
            }
            Form additional_services = new ADDITIONAL_SERVICE();
            additional_services.MdiParent = this;
            additional_services.Show();
        }

        private void pURCHASEGRNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(GRN))
                {
                    f.Activate();
                    return;
                }
            }
            Form ITEMS = new GRN();
            ITEMS.MdiParent = this;
            ITEMS.Show();
        }

        private void sTOCKToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(VIEW_STOCK))
                {
                    f.Activate();
                    return;
                }
            }
            Form ITEMS = new VIEW_STOCK();
            ITEMS.MdiParent = this;
            ITEMS.Show();
        }

        private void aDDRESERVATIONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(RESERVATION))
                {
                    f.Activate();
                    return;
                }
            }
            Form ITEMS = new RESERVATION();
            ITEMS.MdiParent = this;
            ITEMS.Show();
        }

        private void cHECKINToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(CHECKED_IN))
                {
                    f.Activate();
                    return;
                }
            }
            Form ITEMS = new CHECKED_IN();
            ITEMS.MdiParent = this;
            ITEMS.Show();
        }

        private void iTEMCATEGORYToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(CATEGORY))
                {
                    f.Activate();
                    return;
                }
            }
            Form CATEGORY = new CATEGORY();
            CATEGORY.MdiParent = this;
            CATEGORY.Show();
        }

        private void iTEMSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(ITEMS))
                {
                    f.Activate();
                    return;
                }
            }
            Form ITEMS = new ITEMS();
            ITEMS.MdiParent = this;
            ITEMS.Show();
        }

        private void sUPPLIERSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(SUPPLIER))
                {
                    f.Activate();
                    return;
                }
            }
            Form ITEMS = new SUPPLIER();
            ITEMS.MdiParent = this;
            ITEMS.Show();
        }

        private void rOOMOVERVIEWToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(ROOMS_OVERVIEW))
                {
                    f.Activate();
                    return;
                }
            }
            Form overview = new ROOMS_OVERVIEW();
            overview.MdiParent = this;
            overview.Show();
        }

        private void tRAVELAGENTSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(TRAVEL_AGENT))
                {
                    f.Activate();
                    return;
                }
            }
            Form agent = new TRAVEL_AGENT();
            agent.MdiParent = this;
            agent.Show();

        }

        private void oTHERTRANSACRIONCATEGORIESToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(OTHER_TRANS_CATEGORY))
                {
                    f.Activate();
                    return;
                }
            }
            Form ot_cat = new OTHER_TRANS_CATEGORY();
            ot_cat.MdiParent = this;
            ot_cat.Show();
        }

        private void btn_checked_out_Click(object sender, EventArgs e)
        {
            

        }

        private void vIEWRESERVATIONLISTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(VIEW_RESERVATIONS))
                {
                    f.Activate();
                    return;
                }
            }
            Form ot_cat = new VIEW_RESERVATIONS();
            ot_cat.MdiParent = this;
            ot_cat.Show();
        }

        private void rEQUESTADDITIONALSERVICESToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(REQUEST_ADDITIONAL_SERVICES))
                {
                    f.Activate();
                    return;
                }
            }
            Form add_sv = new REQUEST_ADDITIONAL_SERVICES();
            add_sv.MdiParent = this;
            add_sv.Show();
        }

        private void rESERVATIONCALToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(CALENDAR))
                {
                    f.Activate();
                    return;
                }
            }
            Form cal = new CALENDAR();
            cal.MdiParent = this;
            cal.Show();
        }

        private void pAYMENTSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(CHECKBILL_PRINT))
                {
                    f.Activate();
                    return;
                }
            }
            Form cal = new CHECKBILL_PRINT();
            cal.MdiParent = this;
            cal.Show();
        }

        private void toolStripMenuItem23_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(ABOUNT_WE))
                {
                    f.Activate();
                    return;
                }
            }
            Form cal = new ABOUNT_WE();
            cal.MdiParent = this;
            cal.Show();
        }

        private void rOOMTRANSFERToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void eDITKITCHENORDERSToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void eDITRESERVATIONToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        static string[] Scopes = { CalendarService.Scope.CalendarReadonly };
        static string ApplicationName = "Google Calendar API .NET Quickstart";

       

        private void toolStripMenuItem22_Click(object sender, EventArgs e)
        {

        //    try
        //    {
        //        UserCredential credential;

        //        using (var stream =
        //            new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
        //        {
        //            string credPath = "token.json";
        //            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
        //                GoogleClientSecrets.Load(stream).Secrets,
        //                Scopes,
        //                "user",
        //                CancellationToken.None,
        //                new FileDataStore(credPath, true)).Result;
        //            Console.WriteLine("Credential file saved to: " + credPath);
        //        }

        //        // Create Google Calendar API service.
        //        var service = new CalendarService(new BaseClientService.Initializer()
        //        {
        //            HttpClientInitializer = credential,
        //            ApplicationName = ApplicationName,
        //        });

        //        // Define parameters of request.
        //        EventsResource.ListRequest request = service.Events.List("primary");
        //        request.TimeMin = DateTime.Now;
        //        request.ShowDeleted = false;
        //        request.SingleEvents = true;
        //        request.MaxResults = 10;
        //        request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

        //        // List events.
        //        Events events = request.Execute();
        //        Console.WriteLine("Upcoming events:");
        //        if (events.Items != null && events.Items.Count > 0)
        //        {
        //            foreach (var eventItem in events.Items)
        //            {
        //                string when = eventItem.Start.DateTime.ToString();
        //                if (String.IsNullOrEmpty(when))
        //                {
        //                    when = eventItem.Start.Date;
        //                }
        //                Console.WriteLine("{0} ({1})", eventItem.Summary, when);
        //            }
        //        }
        //        else
        //        {
        //            Console.WriteLine("No upcoming events found.");
        //        }
        //        Console.Read();
        //    }
        //    catch (Exception)
        //    {

        //    }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(SETTINGS))
                {
                    f.Activate();
                    return;
                }
            }
            Form room = new SETTINGS();
            room.MdiParent = this;
            room.Show();
        }

        private void rOOMCLEANINGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(VIEW_ROOM_CLEANINGS))
                {
                    f.Activate();
                    return;
                }
            }
            Form room = new VIEW_ROOM_CLEANINGS();
            room.MdiParent = this;
            room.Show();
        }

        private void tRAVELAGENTPAYMENTSToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void dAILYMENUToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(DAILY_MENU))
                {
                    f.Activate();
                    return;
                }
            }
            Form room = new DAILY_MENU();
            room.MdiParent = this;
            room.Show();
        }

        private void cHECKOUTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(CHECKED_OUT))
                {
                    f.Activate();
                    return;
                }
            }
            Form room = new CHECKED_OUT();
            room.MdiParent = this;
            room.Show();
        }

        private void kOTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(KOT_LIST))
                {
                    f.Activate();
                    return;
                }
            }
            Form room = new KOT_LIST();
            room.MdiParent = this;
            room.Show();
        }

        private void nORMALBILLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(CHECKBILL_PRINT))
                {
                    f.Activate();
                    return;
                }
            }
            Form cal = new CHECKBILL_PRINT();
            cal.MdiParent = this;
            cal.Show();
        }

        private void sPILTBILLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(CHECKBILL_PRINT))
                {
                    f.Activate();
                    return;
                }
            }
            Form cal = new CHECKBILL_PRINT();
            cal.MdiParent = this;
            cal.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(RESERVATION))
                {
                    f.Activate();
                    return;
                }
            }
            Form ITEMS = new RESERVATION();
            ITEMS.MdiParent = this;
            ITEMS.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(CHECKED_OUT))
                {
                    f.Activate();
                    return;
                }
            }
            Form ot_cat = new CHECKED_OUT();
            ot_cat.MdiParent = this;
            ot_cat.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(CHECKED_IN))
                {
                    f.Activate();
                    return;
                }
            }
            Form ITEMS = new CHECKED_IN();
            ITEMS.MdiParent = this;
            ITEMS.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(DAILY_MENU))
                {
                    f.Activate();
                    return;
                }
            }
            Form room = new DAILY_MENU();
            room.MdiParent = this;
            room.Show();
        }

        private void btn_NewGRN_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(GRN))
                {
                    f.Activate();
                    return;
                }
            }
            Form room = new GRN();
            room.MdiParent = this;
            room.Show();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(VIEW_STOCK))
                {
                    f.Activate();
                    return;
                }
            }
            Form room = new VIEW_STOCK();
            room.MdiParent = this;
            room.Show();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(CHECKBILL_PRINT))
                {
                    f.Activate();
                    return;
                }
            }
            Form room = new CHECKBILL_PRINT();
            room.MdiParent = this;
            room.Show();
        }

        
        private void pnl_quickLaunch_DragLeave(object sender, EventArgs e)
        {
            
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(VIEW_ROOM_CLEANINGS))
                {
                    f.Activate();
                    return;
                }
            }
            Form room = new VIEW_ROOM_CLEANINGS();
            room.MdiParent = this;
            room.Show();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == typeof(SETTINGS))
                {
                    f.Activate();
                    return;
                }
            }
            Form room = new SETTINGS();
            room.MdiParent = this;
            room.Show();
        }
    }
}
