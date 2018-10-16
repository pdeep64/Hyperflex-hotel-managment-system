using Hyperflex_HMS_KOT.CLASS;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Hyperflex_HMS_KOT.FORMS
{
    public partial class KOT_DASHBOARD : Form
    {
        bool form_shown = false;
        public KOT_DASHBOARD()
        {
            InitializeComponent();
            form_shown = true;
            CMB_STATUS.SelectedIndex = 0;
            CMB_TYPE.SelectedIndex = 0;
            form_shown = false;
            LOAD_DAY_ORDERS();
        }

        private void LOAD_DAY_ORDERS()
        {
            try
            {
                CONNECTION.open_connection();
                using(MySqlDataAdapter adp=new MySqlDataAdapter("SELECT ko.order_no AS order_no,CONCAT_WS(' ',g.first_name,g.last_name) AS guest_name,IFNULL(ko.reservation_no,'-') AS reservation_no,IFNULL(rm.room_name,'-') AS room_name,IFNULL(rt.table_no,'-') AS table_no,ko.total_price AS total_price,ko.special_note FROM kot_order ko LEFT JOIN guest g ON ko.guest_id=g.guest_id LEFT JOIN room rm ON ko.room_id=rm.room_id LEFT JOIN resturant_table rt ON ko.table_id=rt.table_id WHERE ko.added_date=CURDATE() AND ko.order_status=@order_status AND ko.order_status<>'Canceled' ORDER BY ko.order_no", CONNECTION.CON))
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
                else if(CMB_TYPE.SelectedIndex == 2)
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

        private void LOAD_DAY_ORDERS_BY_STATUS()
        {
            try
            {
                CONNECTION.open_connection();
                string query = "";
                if (CMB_TYPE.SelectedIndex == 1)
                    query = "SELECT ko.order_no AS order_no,CONCAT_WS(' ',g.first_name,g.last_name) AS guest_name,IFNULL(ko.reservation_no,'-') AS reservation_no,IFNULL(rm.room_name,'-') AS room_name,IFNULL(rt.table_no,'-') AS table_no,ko.total_price AS total_price,ko.special_note FROM kot_order ko LEFT JOIN reservation r ON ko.reservation_no=r.reservation_id LEFT JOIN guest g ON r.guest_id=g.guest_id LEFT JOIN room rm ON ko.room_id=rm.room_id LEFT JOIN resturant_table rt ON ko.table_id=rt.table_id WHERE ko.added_date=CURDATE() AND ko.order_status=@order_status AND ko.order_status<>'Canceled' AND ko.room_id IS NOT NULL AND ko.table_id IS NULL";
                else if (CMB_TYPE.SelectedIndex == 2)
                    query = "SELECT ko.order_no AS order_no,CONCAT_WS(' ',g.first_name,g.last_name) AS guest_name,IFNULL(ko.reservation_no,'-') AS reservation_no,IFNULL(rm.room_name,'-') AS room_name,IFNULL(rt.table_no,'-') AS table_no,ko.total_price AS total_price,ko.special_note FROM kot_order ko LEFT JOIN reservation r ON ko.reservation_no=r.reservation_id LEFT JOIN guest g ON r.guest_id=g.guest_id LEFT JOIN room rm ON ko.room_id=rm.room_id LEFT JOIN resturant_table rt ON ko.table_id=rt.table_id WHERE ko.added_date=CURDATE() AND ko.order_status=@order_status AND ko.order_status<>'Canceled' AND ko.table_id IS NOT NULL";

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
            Application.Exit();
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
                _MaxButton_Click(sender, e);
            }
        }
        private void _MaxButton_Click(object sender, EventArgs e)
        {
            if (isWindowMaximized)
            {
                this.Location = _normalWindowLocation;
                this.Size = _normalWindowSize;
                isWindowMaximized = false;
            }
            else
            {
                _normalWindowSize = this.Size;
                _normalWindowLocation = this.Location;

                Rectangle rect = Screen.PrimaryScreen.WorkingArea;
                this.Location = new Point(0, 0);
                this.Size = new System.Drawing.Size(rect.Width, rect.Height);
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
                    isWindowMaximized = true;
                }
            }
        }

        private void KOT_DASHBOARD_Load(object sender, EventArgs e)
        {

        }

        private void BTN_NEW_ORDER_Click(object sender, EventArgs e)
        {
            Form KITECHEN_ORDER = new KOT(false,"N/A");
            KITECHEN_ORDER.Show();
        }

        private void CMB_TYPE_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (form_shown==false)
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

        private void PD_KOT_BILL_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            string ORDER_NO = DGV_KOT.SelectedRows[0].Cells["Column1"].Value.ToString();
            string RESERVATION_NO = DGV_KOT.SelectedRows[0].Cells["Column3"].Value.ToString();
            string ROOM_NO = DGV_KOT.SelectedRows[0].Cells["Column4"].Value.ToString();
            string TABLE_NO = DGV_KOT.SelectedRows[0].Cells["Column5"].Value.ToString();
            double TOTAL_AMOUNT = Convert.ToDouble(DGV_KOT.SelectedRows[0].Cells["Column6"].Value.ToString());
            string DESCRIPTION = DGV_KOT.SelectedRows[0].Cells["Column7"].Value.ToString();
            DataTable tbl = new DataTable();

            CONNECTION.open_connection();
            using (MySqlDataAdapter adp = new MySqlDataAdapter("SELECT i.item_name,ki.order_qty,ki.unit_price,ki.total_price FROM kot_order_item ki JOIN stock s ON ki.item_stock_id=s.stock_id JOIN item i ON s.item_code=i.item_id WHERE ki.order_no=@order_no ", CONNECTION.CON))
            {
                adp.SelectCommand.Parameters.Clear();
                adp.SelectCommand.Parameters.AddWithValue("@order_no", ORDER_NO);
                adp.Fill(tbl);
            }
            CONNECTION.close_connection();


            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Far;
            stringFormat.LineAlignment = StringAlignment.Near;
            string bill_font = "Consolas";
            Graphics graphics = e.Graphics;
            Font font = new Font("Consolas", 11);
            float fontHeight = font.GetHeight();
            //int startX = 10;
            //int startY = 0;
            int Offset = 0;                                     //=
            //String underLine = "=====================================";                                    //-
            string underLineSingle = "----------------------------------------------------------------------------";

            float xx = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString("KITCHEN ORDER TICKET", new Font(bill_font, 11, FontStyle.Bold)).Width / 2);

            graphics.DrawString("KITCHEN ORDER TICKET", new Font(bill_font, 11, FontStyle.Bold), new SolidBrush(Color.Black), xx, 10 + Offset);
            Offset = Offset + 12;

            graphics.DrawString("ORDER NO : " + ORDER_NO, new Font(bill_font, 8, FontStyle.Regular), new SolidBrush(Color.Black), 1, 30 + Offset);
            Offset = Offset + 12;

            graphics.DrawString("RESERVATION NO : " + RESERVATION_NO, new Font(bill_font, 8, FontStyle.Regular), new SolidBrush(Color.Black), 1, 30 + Offset);
            Offset = Offset + 12;

            if (ROOM_NO != "-")
            {
                graphics.DrawString("ROOM NO : " + ROOM_NO, new Font(bill_font, 8, FontStyle.Regular), new SolidBrush(Color.Black), 1, 30 + Offset);
                Offset = Offset + 12;
            }

            if (TABLE_NO != "-")
            {
                graphics.DrawString("TABLE NO : " + TABLE_NO, new Font(bill_font, 8, FontStyle.Regular), new SolidBrush(Color.Black), 1, 30 + Offset);
                Offset = Offset + 12;
            }

            graphics.DrawString("ORDER DATE : " + DateTime.Now.ToString(), new Font(bill_font, 8, FontStyle.Regular), new SolidBrush(Color.Black), 1, 30 + Offset);
            Offset = Offset + 12;

            graphics.DrawString("ORDER ADDED BY : " + CLS_CURRENT_LOGGER.LOGGED_IN_USER_NAME, new Font(bill_font, 8, FontStyle.Regular), new SolidBrush(Color.Black), 1, 30 + Offset);
            Offset = Offset + 12;

            graphics.DrawString(underLineSingle, new Font(bill_font, 7), new SolidBrush(Color.Black), 1, 35 + Offset);
            Offset = Offset + 6;

            graphics.DrawString("ITEM NAME", new Font(bill_font, 8, System.Drawing.FontStyle.Regular), new SolidBrush(Color.Black), 1, 37 + Offset);
            graphics.DrawString("AMOUNT", new Font(bill_font, 8, System.Drawing.FontStyle.Regular), new SolidBrush(Color.Black), 205, 37 + Offset);//220
            Offset = Offset + 6;

            graphics.DrawString(underLineSingle, new Font(bill_font, 7), new SolidBrush(Color.Black), 1, 39 + Offset);
            Offset = Offset + 10;

            foreach (DataRow row in tbl.Rows)
            {
                string item_name = row.Field<string>(0);
                double price = row.Field<double>(3);
                double unitPrice = row.Field<double>(2);
                string qty = row.Field<double>(1).ToString();

                if (item_name.Length > 40)
                    item_name = item_name.Substring(0, 39) + "...";
                else
                    item_name = row.Field<string>(0);

                graphics.DrawString(item_name, new Font(bill_font, 8, FontStyle.Regular), new SolidBrush(Color.Black), 1, 41 + Offset);
                Offset = Offset + 10;

                graphics.DrawString("(" + qty + " " + " x " + "   " + (unitPrice).ToString("F2") + ")", new Font(bill_font, 8, FontStyle.Regular), new SolidBrush(Color.Black), 110, 43 + Offset, stringFormat);
                graphics.DrawString(price.ToString("F2"), new Font(bill_font, 8, FontStyle.Regular), new SolidBrush(Color.Black), 248, 43 + Offset, stringFormat);
                Offset = Offset + 13;
            }

            graphics.DrawString(underLineSingle, new Font(bill_font, 7), new SolidBrush(Color.Black), 1, 56 + Offset);
            Offset = Offset + 10;
            graphics.DrawString("NO OF ITEMS: " + tbl.Rows.Count.ToString(), new Font(bill_font, 6, FontStyle.Regular), new SolidBrush(Color.Black), 1, 61 + Offset);
            Offset = Offset + 12;

            graphics.DrawString("TOTAL PRICE ", new Font(bill_font, 10, FontStyle.Bold), new SolidBrush(Color.Black), 1, 71 + Offset);
            graphics.DrawString(TOTAL_AMOUNT.ToString("F2"), new Font(bill_font, 10, FontStyle.Regular), new SolidBrush(Color.Black), 248, 71 + Offset, stringFormat);
            Offset = Offset + 17;

            if (DESCRIPTION != "-")
            {
                graphics.DrawString("SPECIAL NOTE:", new Font(bill_font, 10, FontStyle.Bold), new SolidBrush(Color.Black), 1, 75 + Offset);
                Offset = Offset + 12;

                SizeF sf = graphics.MeasureString(DESCRIPTION, new Font(bill_font, 8, FontStyle.Regular), 280);
                graphics.DrawString(DESCRIPTION, new Font(bill_font, 8, FontStyle.Regular), Brushes.Black, new RectangleF(new PointF(1, 80 + Offset), sf), StringFormat.GenericTypographic);

            }
        }

        private string MARK_ORDER_IN_PREPERATION()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlCommand cmd = new MySqlCommand("UPDATE kot_order SET order_status='In preparation' WHERE order_no=@order_no", CONNECTION.CON))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@order_no", DGV_KOT.SelectedRows[0].Cells["Column1"].Value.ToString());
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

        private void BTN_PRINT_ORDER_Click(object sender, EventArgs e)
        {
            if (DGV_KOT.SelectedRows.Count > 0)
            {
                string result = MARK_ORDER_IN_PREPERATION();
                if (result == "done")
                {
                    PD_KOT_BILL.PrintPage += new PrintPageEventHandler(this.PD_KOT_BILL_PrintPage);

                    //PrintDialog printdlg = new PrintDialog();
                    //PrintPreviewDialog printPrvDlg = new PrintPreviewDialog();

                    //printPrvDlg.Document = PD_KOT_BILL;
                    //printPrvDlg.ShowDialog();

                    try
                    {
                        PD_KOT_BILL.Print();
                    }
                    catch (Exception ex)
                    {
                        MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), ex.Message, MessageAlertImage.Error());
                        mdg.ShowDialog();
                    }
                }
            }
        }

        private void BTN_REFRESH_Click(object sender, EventArgs e)
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

        private string MARK_ORDER_AS_SERVED()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlCommand cmd = new MySqlCommand("UPDATE kot_order SET order_status='Served' WHERE order_no=@order_no", CONNECTION.CON))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@order_no", DGV_KOT.SelectedRows[0].Cells["Column1"].Value.ToString());
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

        private void BTN_SERVED_Click(object sender, EventArgs e)
        {
            if (DGV_KOT.SelectedRows.Count > 0)
            {
                string result = MARK_ORDER_AS_SERVED();
                if (result == "done")
                {
                    MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "ORDER STATUS UPDATED AS SERVED", MessageAlertImage.Success());
                    mdg.ShowDialog();
                }
            }
        }

         
        private void BTN_VIEW_NOTE_Click(object sender, EventArgs e)
        {
            if (DGV_KOT.SelectedRows.Count > 0)
            {
                SPECIAL_NOTE sp_note = new SPECIAL_NOTE(true, DGV_KOT.SelectedRows[0].Cells["Column1"].Value.ToString(), DGV_KOT.SelectedRows[0].Cells["Column7"].Value.ToString());
                sp_note.ShowDialog();
            }          
        }

        private void KOT_DASHBOARD_Activated(object sender, EventArgs e)
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

        private void BTN_EDIT_ORDER_Click(object sender, EventArgs e)
        {
            if (DGV_KOT.SelectedRows.Count > 0)
            {
                KOT k = new KOT(true, DGV_KOT.SelectedRows[0].Cells["Column1"].Value.ToString());
                k.Show();
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
