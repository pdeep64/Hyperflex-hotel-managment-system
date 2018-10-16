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
    public partial class SPECIAL_NOTE : Form
    {
        public SPECIAL_NOTE(bool update_note,string order_no,string sp_note)
        {
            InitializeComponent();
            Rectangle rcScreen = Screen.PrimaryScreen.WorkingArea;
            this.Location = new System.Drawing.Point((rcScreen.Left + rcScreen.Right) / 2 - (this.Width / 2), 0);

            if (update_note==true)
            {
                LBL_ORDER_NO.Text = order_no;
                TXT_SP_NOTE.Text = sp_note;
                BTN_OK.ButtonText = "UPDATE";
                BTN_OK.TextLocation_X = 15;
            }
        }

        bool isTopPanelDragged = false;

        Point offset;
        Size _normalWindowSize;
        Point _normalWindowLocation = Point.Empty;

        public static bool SAVE_ORDER=false;
        public static string SP_NOTE = "";

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

        private string UPDATE_NOTE()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlCommand cmd = new MySqlCommand("UPDATE kot_order SET special_note=@special_note WHERE order_no=@order_no", CONNECTION.CON))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@order_no", LBL_ORDER_NO.Text);
                    if(TXT_SP_NOTE.Text!=string.Empty)
                        cmd.Parameters.AddWithValue("@special_note", TXT_SP_NOTE.Text);
                    else
                        cmd.Parameters.AddWithValue("@special_note", "-");
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        return "done";
                    }
                    else
                    {
                        MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), "SYSTEM ERROR. PLEASE CONTACT SYSTEM ADMIN", MessageAlertImage.Error());
                        mdg.ShowDialog();
                        return "error";
                    }
                }
            }
            catch (Exception ex)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), ex.Message, MessageAlertImage.Error());
                mdg.ShowDialog();
                return "error";
            }
            finally
            {
                CONNECTION.close_connection();
            }
        }

        private void BTN_OK_Click_1(object sender, EventArgs e)
        {
            if (BTN_OK.ButtonText == "SAVE")
            {
                SAVE_ORDER = true;
                if (TXT_SP_NOTE.Text == string.Empty)
                    SP_NOTE = "-";
                else
                    SP_NOTE = TXT_SP_NOTE.Text;
            }
            else
            {
                string result = UPDATE_NOTE();
                if (result == "done")
                {
                    MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "ORDER NOTE UPDATED SUCCESSFULLY", MessageAlertImage.Success());
                    mdg.ShowDialog();
                    this.Close();
                }
            }

            this.Close();
        }

        private void BTN_CANCEL_Click(object sender, EventArgs e)
        {
            SAVE_ORDER = false;
            this.Close();
        }
    }
}
