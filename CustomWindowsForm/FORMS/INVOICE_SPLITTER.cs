using CustomWindowsForm.CLASS;
using HYFLEX_HMS.CLASS;
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
    public partial class INVOICE_SPLITTER : Form
    {
        public INVOICE_SPLITTER()
        {
            InitializeComponent();
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
            //if (this.Location.Y <= 5)
            //{
            //    if (!isWindowMaximized)
            //    {
            //        _normalWindowSize = this.Size;
            //        _normalWindowLocation = this.Location;

            //        Rectangle rect = Screen.PrimaryScreen.WorkingArea;
            //        this.Location = new Point(0, 0);
            //        this.Size = new System.Drawing.Size(rect.Width, rect.Height);
            //        toolTip1.SetToolTip(_MaxButton, "Restore Down");
            //        _MaxButton.CFormState = MinMaxButton.CustomFormState.Maximize;
            //        isWindowMaximized = true;
            //    }
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
          
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
        private void LOAD_ROOMS()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter DA=new MySqlDataAdapter("SELECT recerved_rooms.reservation_no , room.room_id , room.room_name , recerved_rooms.room_charge FROM recerved_rooms INNER JOIN room ON (recerved_rooms.room_id = room.room_id) WHERE recerved_rooms.reservation_no=@reservation_no", CONNECTION.CON))
                {
                    DA.SelectCommand.Parameters.Clear();
                    DA.SelectCommand.Parameters.AddWithValue("@reservation_no", REPORT.RESERVATION_NO);
                    DataTable dt = new DataTable();
                    DA.Fill(dt);
                    if(dt.Rows.Count>0)
                    {
                        DGV_RESERVED_ROOM.DataSource = dt;
                        DGV_RESERVED_ROOM.AutoGenerateColumns = false;
                    }
                    else
                    {
                        DGV_RESERVED_ROOM.DataSource = null;
                    }

                }
            }
            catch (Exception EX)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), EX.Message, MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
        }

        private void LOAD_MEAL_PRICE()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter DA = new MySqlDataAdapter("SELECT reservation_id,no_of_nights,recervation_price.total_adult_food_charges,recervation_price.total_child_food_charges FROM reservation INNER JOIN recervation_price ON (reservation.reservation_id=recervation_price.reservation_no) WHERE reservation.reservation_id=@reservation_id", CONNECTION.CON))
                {
                    DA.SelectCommand.Parameters.Clear();
                    DA.SelectCommand.Parameters.AddWithValue("@reservation_id", REPORT.RESERVATION_NO);
                    DataTable dt = new DataTable();
                    DA.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        DGV_MEAL.DataSource = dt;
                        DGV_MEAL.AutoGenerateColumns = false;
                    }
                    else
                    {
                        DGV_MEAL.DataSource = null;
                    }
                }
            }
            catch (Exception EX)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), EX.Message, MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
        }
        private void LOAD_KOT()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter DA = new MySqlDataAdapter("SELECT kot_order.order_no , resturant_table.table_no , kot_order.total_price FROM kot_order INNER JOIN resturant_table ON (kot_order.table_id = resturant_table.table_id) WHERE kot_order.reservation_no=@reservation_no", CONNECTION.CON))
                {
                    DA.SelectCommand.Parameters.Clear();
                    DA.SelectCommand.Parameters.AddWithValue("@reservation_no", REPORT.RESERVATION_NO);
                    DataTable dt = new DataTable();
                    DA.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        DGV_KOT.DataSource = dt;
                        DGV_KOT.AutoGenerateColumns = false;
                    }
                    else
                    {
                        DGV_KOT.DataSource = null;
                    }
                }
            }
            catch (Exception EX)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), EX.Message, MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
        }
        private void LOAD_MINI_BAR()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter DA = new MySqlDataAdapter("SELECT reservation.reservation_id, SUM(purchased_mini_bar_item.price) AS price FROM purchased_mini_bar_item INNER JOIN reservation ON ( purchased_mini_bar_item.reservation_no = reservation.reservation_id ) WHERE reservation_id = @reservation_id", CONNECTION.CON))
                {
                    DA.SelectCommand.Parameters.Clear();
                    DA.SelectCommand.Parameters.AddWithValue("@reservation_id", REPORT.RESERVATION_NO);
                    DataTable dt = new DataTable();
                    DA.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        FGV_MINI_BAR.DataSource = dt;
                        FGV_MINI_BAR.AutoGenerateColumns = false;
                    }
                    else
                    {
                        FGV_MINI_BAR.DataSource = null;
                    }
                }
            }
            catch (Exception EX)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), EX.Message, MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
        }
        private void LOAD_INVOICE_INFO()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter DA = new MySqlDataAdapter("SELECT tax_status FROM reservation WHERE reservation_id=@reservation_id", CONNECTION.CON))
                {
                    DA.SelectCommand.Parameters.Clear();
                    DA.SelectCommand.Parameters.AddWithValue("@reservation_id", REPORT.RESERVATION_NO);
                    DataTable dt = new DataTable();
                    DA.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        if(dt.Rows[0].Field<string>(0)=="1")
                        {
                            lbl_status.Text = "VAT INCLUDED";
                        }
                        else
                        {
                            lbl_status.Text = "VAT NOT INCLUDED";
                        }
                    }
                  
                }
            }
            catch (Exception EX)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), EX.Message, MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
        }
        private void LOAD_ADDITIONAL_SERVICE()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter DA = new MySqlDataAdapter("SELECT additional_service.reservation_id ,room.room_name ,SUM( additional_service.additional_serivice_price) AS TOTAL FROM hyperfle_hms.additional_service INNER JOIN hyperfle_hms.additional_service_list ON (additional_service.additional_serivice_id = additional_service_list.id) INNER JOIN hyperfle_hms.room ON (additional_service.room_id = room.room_id) WHERE additional_service.reservation_id=@reservation_id GROUP BY room_name", CONNECTION.CON))
                {
                    DA.SelectCommand.Parameters.Clear();
                    DA.SelectCommand.Parameters.AddWithValue("@reservation_id", REPORT.RESERVATION_NO);
                    DataTable dt = new DataTable();
                    DA.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        DGV_ADDITIONAL_SERVICE.DataSource = dt;
                        DGV_ADDITIONAL_SERVICE.AutoGenerateColumns = false;
                    }
                    else
                    {
                        DGV_ADDITIONAL_SERVICE.DataSource = null;
                    }
                }
            }
            catch (Exception EX)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), EX.Message, MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
        }
        private void INVOICE_SPLITTER_Load(object sender, EventArgs e)
        {
            LOAD_ROOMS();
            LOAD_MEAL_PRICE();
            LOAD_KOT();
            LOAD_MINI_BAR();
            LOAD_ADDITIONAL_SERVICE();
            LOAD_INVOICE_INFO();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        MySqlCommand cmd = CONNECTION.CON.CreateCommand();
        MySqlTransaction myTrans;
        private void save_data()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                CONNECTION.open_connection();
                myTrans = CONNECTION.CON.BeginTransaction();
                cmd.Connection = CONNECTION.CON;
                cmd.Transaction = myTrans;
                String RESERVATION_NO = REPORT.RESERVATION_NO;
                cmd.CommandText = "TRUNCATE spilt_invoice";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO spilt_invoice (reservation_id, description, price, total_price,type ) VALUES ( @reservation_id, @description, @price, @total_price,@type ); ";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@reservation_id", RESERVATION_NO);
                cmd.Parameters.AddWithValue("@description", "TOTAL ROOM CHARGES");
                cmd.Parameters.AddWithValue("@price", Convert.ToDouble(LBL_TOTAL_RESERVED_ROOM.Text));
                cmd.Parameters.AddWithValue("@total_price", Convert.ToDouble(lbl_grand_total.Text));
                cmd.Parameters.AddWithValue("@type", "ROOM");
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO spilt_invoice (reservation_id, description, price, total_price,type ) VALUES ( @reservation_id, @description, @price, @total_price,@type ); ";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@reservation_id", RESERVATION_NO);
                cmd.Parameters.AddWithValue("@description", "TOTAL MEAL CHARGES");
                cmd.Parameters.AddWithValue("@price", Convert.ToDouble(LBL_MEAL_PRICE.Text));
                cmd.Parameters.AddWithValue("@total_price", Convert.ToDouble(lbl_grand_total.Text));
                cmd.Parameters.AddWithValue("@type", "MEAL");
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO spilt_invoice (reservation_id, description, price, total_price,type ) VALUES ( @reservation_id, @description, @price, @total_price,@type ); ";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@reservation_id", RESERVATION_NO);
                cmd.Parameters.AddWithValue("@description", "TOTAL MINI BAR CHARGES");
                cmd.Parameters.AddWithValue("@price", Convert.ToDouble(LBL_MINI_BAR.Text));
                cmd.Parameters.AddWithValue("@total_price", Convert.ToDouble(lbl_grand_total.Text));
                cmd.Parameters.AddWithValue("@type", "MINI BAR");
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO spilt_invoice (reservation_id, description, price, total_price,type ) VALUES ( @reservation_id, @description, @price, @total_price,@type ); ";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@reservation_id", RESERVATION_NO);
                cmd.Parameters.AddWithValue("@description", "TOTAL ADDITIONAL SERVICE CHARGES");
                cmd.Parameters.AddWithValue("@price", Convert.ToDouble(LBL_ADDITIONAL_SERVICE.Text));
                cmd.Parameters.AddWithValue("@total_price", Convert.ToDouble(lbl_grand_total.Text));
                cmd.Parameters.AddWithValue("@type", "ADDITIONAL");
                cmd.ExecuteNonQuery();

                foreach (DataGridViewRow dgvr in DGV_KOT.Rows)
                {
                    if (Convert.ToBoolean(dgvr.Cells[0].Value) == true)
                    {
                        cmd.CommandText = "INSERT INTO spilt_invoice (reservation_id, description, price, total_price,type ) VALUES ( @reservation_id, @description, @price, @total_price,@type ); ";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@reservation_id", RESERVATION_NO);
                        cmd.Parameters.AddWithValue("@description", "KOT - "+dgvr.Cells[1].Value);
                        cmd.Parameters.AddWithValue("@price", Convert.ToDouble(dgvr.Cells[3].Value.ToString()));
                        cmd.Parameters.AddWithValue("@total_price", Convert.ToDouble(lbl_grand_total.Text));
                        cmd.Parameters.AddWithValue("@type", "KOT");
                        cmd.ExecuteNonQuery();
                    }
                    
                }
                myTrans.Commit();
                REPORT_VIEWER_SPILT RVS = new REPORT_VIEWER_SPILT();
                RVS.ShowDialog();

            }
            catch (Exception EX)
            {
                Cursor.Current = Cursors.Default;
                myTrans.Rollback();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), EX.Message, MessageAlertImage.Error());
                 mdg.ShowDialog();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                CONNECTION.close_connection();
            }
        }
        private void shapedButton2_Click(object sender, EventArgs e)
        {
            save_data();
        }

        private void DGV_RESERVED_ROOM_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
           // cal_room_Price();

        }

        public void cal_room_Price()
        {
            try
            {
                Double TOTAL = 0;
                foreach (DataGridViewRow row in DGV_RESERVED_ROOM.Rows)
                {
                    if (Convert.ToBoolean(row.Cells[0].Value) == true)
                    {
                        TOTAL = TOTAL + Convert.ToDouble(row.Cells[4].Value);
                    }
                }
                    LBL_TOTAL_RESERVED_ROOM.Text = TOTAL.ToString("F2");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());                
            }
        }
        public void cal_total()
        {
            try
            {
                Double GrandTotal = 0;
                double Total_room = Convert.ToDouble(LBL_TOTAL_RESERVED_ROOM.Text);
                double Total_food = Convert.ToDouble(LBL_MEAL_PRICE.Text);
                double Total_KOT = Convert.ToDouble(LBL_KOT_PRICE.Text);
                double Total_mini_bar = Convert.ToDouble(LBL_MINI_BAR.Text);
                double Total_additional_service = Convert.ToDouble(LBL_ADDITIONAL_SERVICE.Text);
                GrandTotal = GrandTotal+ (Total_room+Total_food+Total_KOT+Total_mini_bar+Total_additional_service);
                lbl_grand_total.Text = GrandTotal.ToString("f2");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public void cal_Meal_Price()
        {
            Double TOTAL_MEAL = 0;
            foreach (DataGridViewRow row in DGV_MEAL.Rows)
            {
                if (Convert.ToBoolean(row.Cells[0].Value) == true)
                {
                    TOTAL_MEAL = TOTAL_MEAL + (Convert.ToDouble(row.Cells[3].Value) + Convert.ToDouble(row.Cells[4].Value));
                }
            }
                LBL_MEAL_PRICE.Text = TOTAL_MEAL.ToString("F2");

        }
        public void cal_kot_Price()
        {
            Double TOTAL_KOT = 0;
            foreach (DataGridViewRow row in DGV_KOT.Rows)
            {
                if (Convert.ToBoolean(row.Cells[0].Value) == true)
                {
                    TOTAL_KOT = TOTAL_KOT + Convert.ToDouble(row.Cells[3].Value);
                }
            }
            LBL_KOT_PRICE.Text = TOTAL_KOT.ToString("F2");
        }

        public void cal_minibar_Price()
        {
            Double MINI_BAR = 0;
            foreach (DataGridViewRow row in FGV_MINI_BAR.Rows)
            {
                if (Convert.ToBoolean(row.Cells[0].Value) == true)
                {
                    MINI_BAR = MINI_BAR + Convert.ToDouble(row.Cells[2].Value);
                }
            }
            LBL_MINI_BAR.Text = MINI_BAR.ToString("F2");
        }

        public void cal_additional_service_price()
        {
            Double ADIITIONAL_SERVICE = 0;
            foreach (DataGridViewRow row in DGV_ADDITIONAL_SERVICE.Rows)
            {
                if (Convert.ToBoolean(row.Cells[0].Value) == true)
                {
                    ADIITIONAL_SERVICE = ADIITIONAL_SERVICE + Convert.ToDouble(row.Cells[3].Value);
                }
            }
            LBL_ADDITIONAL_SERVICE.Text = ADIITIONAL_SERVICE.ToString("F2");
        }
        public void CheckedSelectedRows(DataGridView GRID)
        {
            int count = 0;
            foreach (DataGridViewRow row in GRID.SelectedRows)
            {
                if (row.Cells[0].Value == null)
                    row.Cells[0].Value = false;
                switch (row.Cells[0].Value.ToString())
                {
                    case "True":
                        row.Cells[0].Value = false;
                        row.DefaultCellStyle.BackColor = SystemColors.Window;
                        break;
                    case "False":
                        row.Cells[0].Value = true;
                        row.DefaultCellStyle.BackColor = Color.Yellow;
                        break;
                }
            }
        }
        private void DGV_RESERVED_ROOM_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            CheckedSelectedRows(DGV_RESERVED_ROOM);
            cal_room_Price();
            cal_total();
        }

        private void DGV_RESERVED_ROOM_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DGV_MEAL_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            CheckedSelectedRows(DGV_MEAL);
            cal_Meal_Price();
            cal_total();
        }

        private void DGV_KOT_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            CheckedSelectedRows(DGV_KOT);
            cal_kot_Price();
            cal_total();
        }

        private void dataGridView4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           // CheckedSelectedRows(DGV_RESERVED_ROOM);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void FGV_MINI_BAR_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            CheckedSelectedRows(FGV_MINI_BAR);
            cal_minibar_Price();
            cal_total();
        }

        private void DGV_ADDITIONAL_SERVICE_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            CheckedSelectedRows(DGV_ADDITIONAL_SERVICE);
            cal_additional_service_price();
            cal_total();
        }
    }
}
