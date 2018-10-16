using CustomWindowsForm.CLASS;
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

namespace CustomWindowsForm.FORMS
{
    public partial class KOT_LIST : Form
    {
        bool form_shown = false;
        public KOT_LIST()
        {
            InitializeComponent();
            form_shown = true;
            CMB_STATUS.SelectedIndex = 0;
            CMB_TYPE.SelectedIndex = 0;
            form_shown = false;
            LOAD_DAY_ORDERS();
        }
        bool isTopPanelDragged = false;

        bool isWindowMaximized = false;
        Point offset;
        Size _normalWindowSize;
        Point _normalWindowLocation = Point.Empty;
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
            if (e.Clicks == 2)
            {
                isTopPanelDragged = false;
               // _MaxButton_Click(sender, e);
            }
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

        private void LOAD_DAY_ORDERS()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter adp = new MySqlDataAdapter("SELECT ko.order_no AS order_no,CONCAT_WS(' ',g.first_name,g.last_name) AS guest_name,IFNULL(ko.reservation_no,'-') AS reservation_no,IFNULL(rm.room_name,'-') AS room_name,IFNULL(rt.table_no,'-') AS table_no,ko.total_price AS total_price,ko.special_note FROM kot_order ko LEFT JOIN guest g ON ko.guest_id=g.guest_id LEFT JOIN room rm ON ko.room_id=rm.room_id LEFT JOIN resturant_table rt ON ko.table_id=rt.table_id WHERE ko.added_date=CURDATE() AND ko.order_status=@order_status AND ko.order_status<>'Canceled' ORDER BY ko.order_no", CONNECTION.CON))
                {
                    adp.SelectCommand.Parameters.Clear();
                    adp.SelectCommand.Parameters.AddWithValue("@order_status", CMB_STATUS.Text);
                    DataTable tbl = new DataTable();
                    adp.Fill(tbl);
                    DGV_KOT.DataSource = null;
                    if (tbl.Rows.Count > 0)
                    {
                        DGV_KOT.AutoGenerateColumns = false;
                        DGV_KOT.DataSource = tbl;
                    }
                }
            }
            catch (Exception EX)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), EX.Message, MessageAlertImage.Error());
                mdg.ShowDialog();
            }
        }

        private void LOAD_DAY_ORDERS_BY_TYPE()
        {
            try
            {
                CONNECTION.open_connection();
                string query = "";
                if (CMB_TYPE.SelectedIndex == 1)
                    query = "SELECT ko.order_no AS order_no,CONCAT_WS(' ',g.first_name,g.last_name) AS guest_name,IFNULL(ko.reservation_no,'-') AS reservation_no,IFNULL(rm.room_name,'-') AS room_name,IFNULL(rt.table_no,'-') AS table_no,ko.total_price AS total_price,ko.special_note FROM kot_order ko LEFT JOIN guest g ON ko.guest_id=g.guest_id LEFT JOIN room rm ON ko.room_id=rm.room_id LEFT JOIN resturant_table rt ON ko.table_id=rt.table_id WHERE ko.added_date=CURDATE() AND ko.order_status=@order_status  AND ko.order_status<>'Canceled' AND ko.room_id IS NOT NULL AND ko.table_id IS NULL";
                else if (CMB_TYPE.SelectedIndex == 2)
                    query = "SELECT ko.order_no AS order_no,CONCAT_WS(' ',g.first_name,g.last_name) AS guest_name,IFNULL(ko.reservation_no,'-') AS reservation_no,IFNULL(rm.room_name,'-') AS room_name,IFNULL(rt.table_no,'-') AS table_no,ko.total_price AS total_price,ko.special_note FROM kot_order ko LEFT JOIN guest g ON ko.guest_id=g.guest_id LEFT JOIN room rm ON ko.room_id=rm.room_id LEFT JOIN resturant_table rt ON ko.table_id=rt.table_id WHERE ko.added_date=CURDATE() AND ko.order_status=@order_status AND ko.order_status<>'Canceled' AND ko.table_id IS NOT NULL";
                else if (CMB_TYPE.SelectedIndex == 3)
                    query = "SELECT ko.order_no AS order_no,CONCAT_WS(' ',g.first_name,g.last_name) AS guest_name,IFNULL(ko.reservation_no,'-') AS reservation_no,IFNULL(rm.room_name,'-') AS room_name,IFNULL(rt.table_no,'-') AS table_no,ko.total_price AS total_price,ko.special_note FROM kot_order ko LEFT JOIN guest g ON ko.guest_id=g.guest_id LEFT JOIN room rm ON ko.room_id=rm.room_id LEFT JOIN resturant_table rt ON ko.table_id=rt.table_id WHERE ko.added_date=CURDATE() AND ko.order_status=@order_status AND ko.order_status<>'Canceled' AND ko.reservation_no IS NULL";


                using (MySqlDataAdapter adp = new MySqlDataAdapter(query, CONNECTION.CON))
                {
                    adp.SelectCommand.Parameters.Clear();
                    adp.SelectCommand.Parameters.AddWithValue("@order_status", CMB_STATUS.Text);
                    DataTable tbl = new DataTable();
                    adp.Fill(tbl);
                    DGV_KOT.DataSource = null;
                    if (tbl.Rows.Count > 0)
                    {
                        DGV_KOT.AutoGenerateColumns = false;
                        DGV_KOT.DataSource = tbl;
                    }
                }
            }
            catch (Exception EX)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), EX.Message, MessageAlertImage.Error());
                mdg.ShowDialog();
            }
        }

        private void FILTER_ORDER_BY_ID()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter adp = new MySqlDataAdapter("SELECT ko.order_no AS order_no,CONCAT_WS(' ',g.first_name,g.last_name) AS guest_name,IFNULL(ko.reservation_no,'-') AS reservation_no,IFNULL(rm.room_name,'-') AS room_name,IFNULL(rt.table_no,'-') AS table_no,ko.total_price AS total_price,ko.special_note FROM kot_order ko LEFT JOIN guest g ON ko.guest_id=g.guest_id LEFT JOIN room rm ON ko.room_id=rm.room_id LEFT JOIN resturant_table rt ON ko.table_id=rt.table_id WHERE ko.added_date=CURDATE() AND ko.order_no LIKE @order_no AND ko.order_status<>'Canceled' ORDER BY ko.order_no", CONNECTION.CON))
                {
                    adp.SelectCommand.Parameters.Clear();
                    adp.SelectCommand.Parameters.AddWithValue("@order_no", "%" + TXT_ORDER_NO.Text + "%");
                    DataTable tbl = new DataTable();
                    adp.Fill(tbl);
                    DGV_KOT.DataSource = null;
                    if (tbl.Rows.Count > 0)
                    {
                        DGV_KOT.AutoGenerateColumns = false;
                        DGV_KOT.DataSource = tbl;
                    }
                }
            }
            catch (Exception EX)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), EX.Message, MessageAlertImage.Error());
                mdg.ShowDialog();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
          
        }

        private void CMB_TYPE_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (form_shown == false)
            {
                if (CMB_TYPE.SelectedIndex >= 0)
                {
                    if (CMB_TYPE.SelectedIndex == 0)
                    {
                        LOAD_DAY_ORDERS();
                    }
                    else
                    {
                        LOAD_DAY_ORDERS_BY_TYPE();
                    }
                }
            }
        }

        private void CMB_STATUS_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (form_shown == false)
            {
                if (CMB_STATUS.SelectedIndex >= 0)
                {
                    if (CMB_TYPE.SelectedIndex == 0)
                    {
                        LOAD_DAY_ORDERS();
                    }
                    else
                    {
                        LOAD_DAY_ORDERS_BY_TYPE();
                    }
                }
            }
        }

        private void TXT__TextChanged(object sender, EventArgs e)
        {
            if (TXT_ORDER_NO.TextLength > 3)
            {
                FILTER_ORDER_BY_ID();
            }
            else
            {
                DGV_KOT.DataSource = null;
            }
        }

        private void shapedButton3_Click(object sender, EventArgs e)
        {
            LOAD_DAY_ORDERS();
        }

        private void BTN_EDIT_ORDER_Click(object sender, EventArgs e)
        {
            if (DGV_KOT.SelectedRows.Count > 0)
            {
                string KOT_NO = DGV_KOT.SelectedRows[0].Cells[0].Value.ToString();
                string GUEST_NAME= DGV_KOT.SelectedRows[0].Cells[1].Value.ToString();
                string RESERVATION_NO= DGV_KOT.SelectedRows[0].Cells[2].Value.ToString();
                string ROOM_NO= DGV_KOT.SelectedRows[0].Cells[3].Value.ToString();
                string TABLE_NO= DGV_KOT.SelectedRows[0].Cells[4].Value.ToString();

                KOT_ITEMS_EDIT f = new KOT_ITEMS_EDIT(KOT_NO,GUEST_NAME,RESERVATION_NO,ROOM_NO,TABLE_NO);
                f.Show();
            }
        }

        private void KOT_LIST_Activated(object sender, EventArgs e)
        {
            if (CMB_STATUS.SelectedIndex >= 0)
            {
                if (CMB_TYPE.SelectedIndex == 0)
                {
                    LOAD_DAY_ORDERS();
                }
                else
                {
                    LOAD_DAY_ORDERS_BY_TYPE();
                }
            }
        }


        private string CANCEL_ORDER(string order_no)
        {
            MySqlCommand cmd = new MySqlCommand();
            MySqlTransaction myTrans = null;

            try
            {
                CONNECTION.open_connection();
                cmd.Connection = CONNECTION.CON;
                myTrans = CONNECTION.CON.BeginTransaction(IsolationLevel.ReadCommitted);
                cmd.Transaction = myTrans;

                cmd.CommandText = "UPDATE kot_order SET order_status=@order_status WHERE order_no=@order_no";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@order_no", order_no);
                cmd.Parameters.AddWithValue("@order_status", "Canceled");
                cmd.ExecuteNonQuery();

                using (MySqlDataAdapter adp = new MySqlDataAdapter("SELECT koi.item_stock_id,koi.order_qty,koi.unit_price,koi.total_price,i.qty_handle FROM kot_order_item koi JOIN stock s ON s.stock_id=koi.item_stock_id JOIN item i ON s.item_code=i.item_id WHERE koi.order_no=@order_no", CONNECTION.CON))
                {
                    adp.SelectCommand.Parameters.Clear();
                    adp.SelectCommand.Parameters.AddWithValue("@order_no", order_no);
                    DataTable tbl = new DataTable();
                    adp.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        foreach (DataRow row in tbl.Rows)
                        {
                            if (row.Field<string>(4) == "1")
                            {
                                cmd.CommandText = "UPDATE stock SET qty=qty+@order_qty WHERE stock_id=@stock_id";
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@order_qty", row.Field<double>(1));
                                cmd.Parameters.AddWithValue("@stock_id", row.Field<Int32>(0));
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }

                myTrans.Commit();
                return "done";
            }
            catch (Exception ex)
            {
                myTrans.Rollback();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), ex.Message, MessageAlertImage.Error());
                mdg.ShowDialog();
                return "error";
            }
        }

        private void BTN_CANCEL_ORDER_Click(object sender, EventArgs e)
        {
            if (DGV_KOT.SelectedRows.Count > 0)
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure, Do you want to cancel the order ?", "Cancel Order", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    string result = CANCEL_ORDER(DGV_KOT.SelectedRows[0].Cells["Column1"].Value.ToString());
                    if (result == "done")
                    {
                        MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "ORDER CANCELED SUCCESSFULLY", MessageAlertImage.Success());
                        mdg.ShowDialog();
                    }
                }
            }
            else
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Warning(), "PLEASE SELECT ORDER TO CANCEL", MessageAlertImage.Warning());
                mdg.ShowDialog();
            }
        }
    }
}
