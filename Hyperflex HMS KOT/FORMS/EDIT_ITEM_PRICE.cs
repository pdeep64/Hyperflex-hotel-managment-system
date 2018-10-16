using Hyperflex_HMS_KOT.CLASS;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Hyperflex_HMS_KOT.FORMS
{
    public partial class EDIT_ITEM_PRICE : Form
    {
        double TAX_PRS = CLS_TAX.GetTotalTaxPercentage();

        public EDIT_ITEM_PRICE(string item_name,double price)
        {
            InitializeComponent();
            Rectangle rcScreen = Screen.PrimaryScreen.WorkingArea;
            this.Location = new System.Drawing.Point((rcScreen.Left + rcScreen.Right) / 2 - (this.Width / 2), 0);
            LBL_ITEM_NAME.Text = item_name;
            TXT_NEW_PRICE.Text = price.ToString("F2");
            LBL_PRICE_WITH_TAX.Text = (price + (price * (TAX_PRS / 100))).ToString("F2");
        }

        bool isTopPanelDragged = false;

        Point offset;
        Size _normalWindowSize;
        Point _normalWindowLocation = Point.Empty;

        public static bool ADD_ITEM=false;
        public static double NEW_PRICE = 0;

        private void CenterButton(System.Windows.Forms.Button btn_ok)
        {
            btn_ok.Location = new Point((btn_ok.Parent.ClientSize.Width / 2) - 35);
            btn_ok.Refresh();
        }
        private void btn_ok_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void _CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
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
        }
        private void _MaxButton_Click(object sender, EventArgs e)
        {
           
        }
        private void TopPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (isTopPanelDragged)
            {
                Point newPoint = TopPanel.PointToScreen(new Point(e.X, e.Y));
                newPoint.Offset(offset);
                this.Location = newPoint;
            }
        }

        private void TopPanel_MouseUp(object sender, MouseEventArgs e)
        {
            isTopPanelDragged = false;
            if (this.Location.Y <= 5)
            {
                    _normalWindowSize = this.Size;
                    _normalWindowLocation = this.Location;

                    Rectangle rect = Screen.PrimaryScreen.WorkingArea;
                    this.Location = new Point(0, 0);
                    this.Size = new System.Drawing.Size(rect.Width, rect.Height);
            }
        }


        private void BTN_OK_Click_1(object sender, EventArgs e)
        {
            ADD_ITEM = true;
            NEW_PRICE = Convert.ToDouble(TXT_NEW_PRICE.Text);
            this.Close();
        }

        private void BTN_CANCEL_Click(object sender, EventArgs e)
        {
            ADD_ITEM = false;
            this.Close();
        }

        private void EDIT_ITEM_PRICE_Shown(object sender, EventArgs e)
        {
            TXT_NEW_PRICE.Focus();
            TXT_NEW_PRICE.SelectAll();
        }

        private void TXT_NEW_PRICE_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&(e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && ((sender as System.Windows.Forms.TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void TXT_NEW_PRICE_Leave(object sender, EventArgs e)
        {
            if (TXT_NEW_PRICE.Text != string.Empty)
                TXT_NEW_PRICE.Text = Convert.ToDouble(TXT_NEW_PRICE.Text).ToString("F2");
            else
                TXT_NEW_PRICE.Text = "0.00";
        }

        private void TXT_NEW_PRICE_TextChanged(object sender, EventArgs e)
        {
            double sp = 0;
            if (TXT_NEW_PRICE.Text != string.Empty)
                sp = Convert.ToDouble(TXT_NEW_PRICE.Text);

            LBL_PRICE_WITH_TAX.Text = (sp + (sp * (TAX_PRS / 100))).ToString("F2");
        }
    }
}
