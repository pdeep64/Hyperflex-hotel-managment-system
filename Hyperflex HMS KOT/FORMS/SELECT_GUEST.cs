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
    public partial class SELECT_GUEST : Form
    {
        string added_guest_id = "";
        public static string SELECTED_GUEST_ID = "";
        public static string SELECTED_GUEST_NAME = "";

        private void LOADGUESTS()
        {
            try
            {
                CONNECTION.open_connection();
                using(MySqlDataAdapter adp=new MySqlDataAdapter("SELECT guest_id,CONCAT_WS(' ',first_name,last_name) AS guest_name FROM guest ORDER BY guest_id", CONNECTION.CON))
                {
                    DataTable tbl = new DataTable();
                    adp.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        CMB_GUEST.DataSource = tbl;
                        CMB_GUEST.DisplayMember = "guest_name";
                        CMB_GUEST.ValueMember = "guest_id";
                    }
                }
            }
            catch (Exception EX)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), EX.Message, MessageAlertImage.Error());
                mdg.ShowDialog();
            }
            finally
            {
                CONNECTION.close_connection();
            }
        }

        private string ADD_NEW_GUEST()
        {
            try
            {
                string guest_id =CLS_GENERATE_ID.GEN_NEXT_GUEST_NO();
                CONNECTION.open_connection();
                using(MySqlCommand cmd=new MySqlCommand("INSERT INTO guest(guest_id,id_no,first_name,last_name) VALUES(@guest_id,@id_no,@first_name,@last_name)", CONNECTION.CON))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@guest_id", guest_id);
                    cmd.Parameters.AddWithValue("@id_no", TXT_NIC_NO.Text);
                    cmd.Parameters.AddWithValue("@first_name", TXT_FIRST_NAME.Text);
                    cmd.Parameters.AddWithValue("@last_name", TXT_LAST_NAME.Text);
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        added_guest_id = guest_id;
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
            catch (Exception EX)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), EX.Message, MessageAlertImage.Error());
                mdg.ShowDialog();
                return "error";
            }
            finally
            {
                CONNECTION.close_connection();
            }
        }

        public SELECT_GUEST(bool update_note,string order_no)
        {
            InitializeComponent();
            Rectangle rcScreen = Screen.PrimaryScreen.WorkingArea;
            this.Location = new System.Drawing.Point((rcScreen.Left + rcScreen.Right) / 2 - (this.Width / 2), 0);
            LOADGUESTS();
            if (update_note==true)
            {
                LBL_ORDER_NO.Text = order_no;
                BTN_SELECT_GUEST.ButtonText = "UPDATE";
                BTN_SELECT_GUEST.TextLocation_X = 15;
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


        private void BTN_OK_Click_1(object sender, EventArgs e)
        {
            if (CMB_GUEST.SelectedIndex >= 0)
            {
                SELECTED_GUEST_ID = CMB_GUEST.SelectedValue.ToString();
                SELECTED_GUEST_NAME = CMB_GUEST.Text;
                this.Close();
            }
            else
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), "PLEASE SELECT GUSEST", MessageAlertImage.Error());
                mdg.ShowDialog();
            }
        }

        private void BTN_CANCEL_Click(object sender, EventArgs e)
        {
            SELECTED_GUEST_ID = "";
            SELECTED_GUEST_NAME = "";
            this.Close();
        }

        private void BTN_SAVE_Click(object sender, EventArgs e)
        {
            string result = ADD_NEW_GUEST();
            if (result == "done")
            {
                LOADGUESTS();
                CMB_GUEST.SelectedValue = added_guest_id;
                added_guest_id = "";
                TXT_FIRST_NAME.Clear();
                TXT_LAST_NAME.Clear();
                TXT_NIC_NO.Clear();
            }
        }
    }
}
