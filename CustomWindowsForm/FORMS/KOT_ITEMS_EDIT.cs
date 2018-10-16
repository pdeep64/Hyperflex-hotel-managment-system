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
    public partial class KOT_ITEMS_EDIT : Form
    {
        double reg_tax_amount = 0;
        double tax_amount = 0;

        public KOT_ITEMS_EDIT(string KOT_NO, string GUEST_NAME,string RESERVATION_NO,string ROOM_NO,string TABLE_NO)
        {
            InitializeComponent();
            LBL_ORDER_NO.Text = KOT_NO;
            LBL_GUEST_NAME.Text = GUEST_NAME;
            LBL_RESERVATION_NO.Text = RESERVATION_NO;
            LBL_ROOM_NO.Text = ROOM_NO;
            LBL_TABLE_NO.Text = TABLE_NO;
            reg_tax_amount = CLS_TAX.GetTotalTaxPercentage();
            LOAD_ORDER_DETAILS(KOT_NO);
        }

        private void LOAD_ORDER_DETAILS(string order_no)
        {
            try
            {
                CONNECTION.open_connection();

                using (MySqlDataAdapter adp = new MySqlDataAdapter("SELECT r.tax_status FROM kot_order ko LEFT JOIN reservation r ON ko.reservation_no=r.reservation_id WHERE ko.order_no=@order_no", CONNECTION.CON))
                {
                    adp.SelectCommand.Parameters.Clear();
                    adp.SelectCommand.Parameters.AddWithValue("@order_no", order_no);
                    DataTable tbl = new DataTable();
                    adp.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        if (tbl.Rows[0].Field<string>(0) == "1")
                            tax_amount = reg_tax_amount;
                        else if(tbl.Rows[0].Field<string>(0) == "0")
                            tax_amount = 0;
                        else
                            tax_amount = reg_tax_amount;
                    }
                    else
                        tax_amount = reg_tax_amount;
                }

                using (MySqlDataAdapter adp = new MySqlDataAdapter("SELECT special_note FROM kot_order WHERE order_no=@order_no", CONNECTION.CON))
                {
                    adp.SelectCommand.Parameters.Clear();
                    adp.SelectCommand.Parameters.AddWithValue("@order_no", order_no);
                    DataTable tbl = new DataTable();
                    adp.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        foreach (DataRow row in tbl.Rows)
                        {
                            TXT_SPECIAL_NOTE.Text = row[0].ToString();
                        }
                    }
                }

                using (MySqlDataAdapter adp = new MySqlDataAdapter("SELECT koi.item_stock_id,s.item_code,i.barcode,i.item_name,koi.order_qty,koi.unit_price,koi.total_price,i.qty_handle,koi.cost_price FROM kot_order_item koi JOIN stock s ON s.stock_id=koi.item_stock_id JOIN item i ON s.item_code=i.item_id WHERE koi.order_no=@order_no", CONNECTION.CON))
                {
                    adp.SelectCommand.Parameters.Clear();
                    adp.SelectCommand.Parameters.AddWithValue("@order_no", order_no);
                    DataTable tbl = new DataTable();
                    adp.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        foreach (DataRow row in tbl.Rows)
                        {
                            int x = DGV_ORDER_ITEMS.Rows.Add();
                            DGV_ORDER_ITEMS.Rows[x].Cells[0].Value = row.Field<Int32>(0).ToString();
                            DGV_ORDER_ITEMS.Rows[x].Cells[1].Value = row.Field<Int32>(1).ToString();
                            DGV_ORDER_ITEMS.Rows[x].Cells[2].Value = row.Field<string>(2);
                            DGV_ORDER_ITEMS.Rows[x].Cells[3].Value = row.Field<string>(3);
                            DGV_ORDER_ITEMS.Rows[x].Cells[4].Value = row.Field<double>(4).ToString();
                            DGV_ORDER_ITEMS.Rows[x].Cells[5].Value = row.Field<double>(5).ToString("F2");
                            DGV_ORDER_ITEMS.Rows[x].Cells[6].Value = row.Field<double>(6).ToString("F2");
                            DGV_ORDER_ITEMS.Rows[x].Cells[7].Value = row.Field<string>(7);
                            DGV_ORDER_ITEMS.Rows[x].Cells[8].Value = row.Field<double>(8).ToString("F2");
                        }
                        LBL_TOT_PRICE.Text = GET_ORDER_TOTAL_PRICE().ToString("F2");
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

        private double GET_ORDER_TOTAL_PRICE()
        {
            double tot_price = 0;
            foreach (DataGridViewRow row in DGV_ORDER_ITEMS.Rows)
            {
                tot_price += Convert.ToDouble(row.Cells[6].Value);
            }
            return tot_price;
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

        private void button1_Click(object sender, EventArgs e)
        {
          
        }

        private void BTN_PLUS_QTY_Click(object sender, EventArgs e)
        {
            if (DGV_ORDER_ITEMS.SelectedRows.Count > 0)
            {
                double new_qty = Convert.ToDouble(DGV_ORDER_ITEMS.SelectedRows[0].Cells[4].Value) + 1;
                DGV_ORDER_ITEMS.SelectedRows[0].Cells[4].Value = new_qty.ToString();
                double new_price = new_qty * Convert.ToDouble(DGV_ORDER_ITEMS.SelectedRows[0].Cells[5].Value);
                DGV_ORDER_ITEMS.SelectedRows[0].Cells[6].Value = new_price.ToString("F2");

                LBL_TOT_PRICE.Text = GET_ORDER_TOTAL_PRICE().ToString("F2");
            }
        }

        private void BTN_MINUS_QTY_Click(object sender, EventArgs e)
        {
            if (DGV_ORDER_ITEMS.SelectedRows.Count > 0)
            {
                if (Convert.ToDouble(DGV_ORDER_ITEMS.SelectedRows[0].Cells[4].Value) > 0)
                {
                    double new_qty = Convert.ToDouble(DGV_ORDER_ITEMS.SelectedRows[0].Cells[4].Value) - 1;
                    DGV_ORDER_ITEMS.SelectedRows[0].Cells[4].Value = new_qty.ToString();
                    double new_price = new_qty * Convert.ToDouble(DGV_ORDER_ITEMS.SelectedRows[0].Cells[5].Value);
                    DGV_ORDER_ITEMS.SelectedRows[0].Cells[6].Value = new_price.ToString("F2");
                }

                LBL_TOT_PRICE.Text = GET_ORDER_TOTAL_PRICE().ToString("F2");
            }
        }

        private void BTN_REMOVE_Click(object sender, EventArgs e)
        {
            if (Convert.ToDouble(DGV_ORDER_ITEMS.SelectedRows[0].Cells[4].Value) > 0)
            {
                DGV_ORDER_ITEMS.Rows.RemoveAt(DGV_ORDER_ITEMS.SelectedRows[0].Index);
                LBL_TOT_PRICE.Text = GET_ORDER_TOTAL_PRICE().ToString("F2");
            }
        }

        private string UPDATE_ORDER()
        {
            MySqlCommand cmd = new MySqlCommand();
            MySqlTransaction myTrans = null;

            try
            {
                CONNECTION.open_connection();
                cmd.Connection = CONNECTION.CON;
                myTrans = CONNECTION.CON.BeginTransaction(IsolationLevel.ReadCommitted);
                cmd.Transaction = myTrans;

                cmd.CommandText = "UPDATE kot_order SET special_note=@special_note,tax_percentage=@tax_percentage,total_price=@total_price WHERE order_no=@order_no";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@order_no", LBL_ORDER_NO.Text);
                cmd.Parameters.AddWithValue("@special_note", TXT_SPECIAL_NOTE.Text);
                cmd.Parameters.AddWithValue("@tax_percentage", tax_amount);
                cmd.Parameters.AddWithValue("@total_price", Convert.ToDouble(LBL_TOT_PRICE.Text));
                cmd.ExecuteNonQuery();

                using (MySqlDataAdapter adp = new MySqlDataAdapter("SELECT koi.item_stock_id,koi.order_qty,koi.unit_price,koi.total_price,i.qty_handle FROM kot_order_item koi JOIN stock s ON s.stock_id=koi.item_stock_id JOIN item i ON s.item_code=i.item_id WHERE koi.order_no=@order_no", CONNECTION.CON))
                {
                    adp.SelectCommand.Parameters.Clear();
                    adp.SelectCommand.Parameters.AddWithValue("@order_no", LBL_ORDER_NO.Text);
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

                        cmd.CommandText = "DELETE FROM kot_order_item WHERE order_no=@order_no";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@order_no", LBL_ORDER_NO.Text);
                        cmd.ExecuteNonQuery();
                    }
                }


                foreach (DataGridViewRow row in DGV_ORDER_ITEMS.Rows)
                {
                    cmd.CommandText = "INSERT INTO kot_order_item(item_stock_id,order_qty,unit_price,total_price,order_no,cost_price) VALUES(@item_stock_id,@order_qty,@unit_price,@total_price,@order_no,@cost_price)";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@item_stock_id", row.Cells[0].Value);
                    cmd.Parameters.AddWithValue("@order_qty", Convert.ToDouble(row.Cells[4].Value));
                    cmd.Parameters.AddWithValue("@unit_price", Convert.ToDouble(row.Cells[5].Value));
                    cmd.Parameters.AddWithValue("@total_price", Convert.ToDouble(row.Cells[6].Value));
                    cmd.Parameters.AddWithValue("@order_no", LBL_ORDER_NO.Text);
                    cmd.Parameters.AddWithValue("@cost_price", Convert.ToDouble(row.Cells[8].Value));
                    cmd.ExecuteNonQuery();

                    if (row.Cells[7].Value.ToString() == "1")
                    {
                        cmd.CommandText = "UPDATE stock SET qty=qty-@order_qty WHERE stock_id=@stock_id";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@order_qty", Convert.ToDouble(row.Cells[4].Value));
                        cmd.Parameters.AddWithValue("@stock_id", row.Cells[0].Value);
                        cmd.ExecuteNonQuery();
                    }
                }

                cmd.CommandText = "INSERT INTO kot_order_update_log(updated_by,updated_date,updated_time) VALUES(@updated_by,@updated_date,@updated_time)";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@updated_by", CLS_CURRENT_LOGGER.LOGGED_IN_USERID);
                cmd.Parameters.AddWithValue("@updated_date", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@updated_time", DateTime.Now.ToString("HH:mm:ss"));
                cmd.ExecuteNonQuery();

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

        private void BTN_SAVE_Click(object sender, EventArgs e)
        {
            string result = UPDATE_ORDER();
            if (result == "done")
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "ORDER UPDATED SUCCESSFULLY", MessageAlertImage.Success());
                mdg.ShowDialog();
            }
        }
    }
}
