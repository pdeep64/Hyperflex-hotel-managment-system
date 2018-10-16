using CustomWindowsForm.CLASS;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HYFLEX_HMS.Properties;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using HYFLEX_HMS.CLASS;

namespace CustomWindowsForm.FORMS
{
    public partial class CHECKED_OUT : Form
    {
        public CHECKED_OUT()
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

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        private void CAL_TOTAL_USD()
        {
            Double ROOM_CHARGES = Convert.ToDouble(LBL_TOTAL_ROOM_PRICE.Text);
            Double MEAL_ADULT = Convert.ToDouble(LBL_TOAL_MEAL_PRICE_ADULT.Text);
            Double MEAL_CHILD = Convert.ToDouble(LBL_TOTAL_MEAL_CHILD.Text);
            Double ADDITIONAL = Convert.ToDouble(LBL_ADDITIONAL_SERVICE_CHARGE.Text);
            Double MINI_BAR = Convert.ToDouble(LBL_MINI_BAR_ITEMS.Text);
            Double KOT = Convert.ToDouble(lbl_kot_price.Text);
            lbl_kot_price.Text = GET_KOT_TOTAL().ToString("F2");
            Double SUB_TOAL = ROOM_CHARGES + MEAL_ADULT + MEAL_CHILD + ADDITIONAL+ MINI_BAR+ KOT;
            LBL_TOT_CHARGE_USD.Text = SUB_TOAL.ToString("F2");
            //LBL_TOT_CHARGE_USD_WITH_TAX.Text = (SUB_TOAL+((SUB_TOAL * Settings.Default.tax_percentage) / 100)).ToString("F2");
            LBL_TOT_CHARGE_LKR.Text = (SUB_TOAL*Settings.Default.exchange_rate_lrk).ToString("F2");
            LBL_TOTAL_PAYABLE.Text = SUB_TOAL.ToString("F2");
        }
        private void label9_Click(object sender, EventArgs e)
        {

        }
        //private void LOAD_AVAILABLE_ROOMS()
        //{
        //    try
        //    {
        //        CONNECTION.open_connection();
        //        using (MySqlDataAdapter da = new MySqlDataAdapter("SELECT room_id,room_name,room_packages.package_name,room_packages.description,room_packages.condition,room_packages.room_package_price,room_packages.package_color FROM room INNER JOIN room_packages ON (room.room_package_id=room_packages.room_package_id) WHERE room_id NOT IN(SELECT room.room_id  FROM room INNER JOIN room_packages ON (room.room_package_id = room_packages.room_package_id) INNER JOIN recerved_rooms ON (recerved_rooms.room_id = room.room_id) INNER JOIN reservation ON (reservation.reservation_id = recerved_rooms.reservation_no) WHERE (reservation.arrival_date BETWEEN @frm_Date AND @todate) OR (reservation.depature_Date > @frm_Date AND reservation.depature_Date<= @todate))", CONNECTION.CON))
        //        {
        //            da.SelectCommand.Parameters.Clear();
        //            da.SelectCommand.Parameters.AddWithValue("@frm_Date", DTP_ARRIVAL_DATE.Value.ToShortDateString());
        //            da.SelectCommand.Parameters.AddWithValue("@todate", DTP_DEPATURE_DATE.Value.ToShortDateString());
        //            DataTable DT = new DataTable();
        //            da.Fill(DT);
        //            if (DT.Rows.Count > 0)
        //            {
        //                flowLayoutPanel1.Controls.Clear();
        //                flowLayoutPanel1.Visible = false;
        //                foreach (DataRow DR in DT.Rows)
        //                {

        //                    Font = new Font("Consolas", 8, FontStyle.Bold);
        //                    Panel BTN = new Panel();
        //                    BTN.Name = DR[0].ToString();
        //                    BTN.Click += (sender2, e2) => AddSelectedRoom(sender2, e2, DR[0].ToString(), DR[1].ToString(), DR[2].ToString(), Convert.ToDouble(DR[5].ToString()));
        //                    BTN.Size = new Size(100, 100);
        //                    Color back = ColorTranslator.FromHtml(DR[6].ToString());
        //                    BTN.BackColor = back;

        //                    //DESCRIPTION
        //                    Label LBL_TYPE = new Label();
        //                    LBL_TYPE.Height = 30;
        //                    LBL_TYPE.BackColor = Color.DodgerBlue;
        //                    LBL_TYPE.Text = DR[2].ToString();
        //                    LBL_TYPE.AutoSize = false;
        //                    LBL_TYPE.Width = 96;
        //                    LBL_TYPE.Location = new Point(2, 2);
        //                    LBL_TYPE.MouseClick += (sender2, e2) => AddSelectedRoom(sender2, e2, DR[0].ToString(), DR[1].ToString(), DR[2].ToString(), Convert.ToDouble(DR[5].ToString()));
        //                    LBL_TYPE.TextAlign = ContentAlignment.BottomCenter;

        //                    //CONDITION
        //                    Label LBL_CONDITION = new Label();
        //                    LBL_CONDITION.Height = 15;
        //                    LBL_CONDITION.BackColor = Color.DeepSkyBlue;
        //                    LBL_CONDITION.Text = DR[4].ToString();
        //                    LBL_CONDITION.AutoSize = false;
        //                    LBL_CONDITION.Width = 96;
        //                    LBL_CONDITION.MouseClick += (sender2, e2) => AddSelectedRoom(sender2, e2, DR[0].ToString(), DR[1].ToString(), DR[2].ToString(), Convert.ToDouble(DR[5].ToString()));
        //                    LBL_CONDITION.Location = new Point(2, 30);
        //                    LBL_CONDITION.TextAlign = ContentAlignment.BottomCenter;

        //                    //PRICE
        //                    Label LBL_PRICE = new Label();
        //                    LBL_PRICE.Height = 15;
        //                    LBL_PRICE.BackColor = Color.DodgerBlue;
        //                    LBL_PRICE.AutoSize = false;
        //                    LBL_PRICE.Width = 96;
        //                    LBL_PRICE.Location = new Point(2, 50);
        //                    LBL_PRICE.MouseClick += (sender2, e2) => AddSelectedRoom(sender2, e2, DR[0].ToString(), DR[1].ToString(), DR[2].ToString(), Convert.ToDouble(DR[5].ToString()));
        //                    LBL_PRICE.Text = "LKR: " + Convert.ToDouble(DR[5].ToString()).ToString("F2");
        //                    LBL_PRICE.TextAlign = ContentAlignment.BottomCenter;

        //                    //ROOM NAME
        //                    Label LBL_ID = new Label();
        //                    LBL_ID.Height = 15;
        //                    LBL_ID.Text = DR[1].ToString();
        //                    LBL_ID.AutoSize = false;
        //                    LBL_ID.Width = 96;
        //                    LBL_ID.Location = new Point(2, 70);
        //                    LBL_ID.MouseClick += (sender2, e2) => AddSelectedRoom(sender2, e2, DR[0].ToString(), DR[1].ToString(), DR[2].ToString(), Convert.ToDouble(DR[5].ToString()));
        //                    LBL_ID.TextAlign = ContentAlignment.BottomCenter;
        //                    LBL_ID.BackColor = Color.DeepSkyBlue;

        //                    BTN.Controls.Add(LBL_ID);
        //                    BTN.Controls.Add(LBL_TYPE);
        //                    BTN.Controls.Add(LBL_CONDITION);
        //                    BTN.Controls.Add(LBL_PRICE);

        //                    BTN.MouseEnter += BTN_MouseEnter1;
        //                    BTN.MouseLeave += BTN_MouseLeave2;
        //                    flowLayoutPanel1.Controls.Add(BTN);

        //                }
        //                flowLayoutPanel1.Visible = true;
        //            }
        //        }
        //    }
        //    catch (Exception EX)
        //    {
        //        MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), EX.Message, MessageAlertImage.Error());
        //        mdg.ShowDialog();
        //    }
        //}
        private void BTN_MouseLeave2(object sender, EventArgs e)
        {
            Panel btn = (Panel)sender;
            btn.BackColor = Color.White;
        }

        private void BTN_MouseEnter1(object sender, EventArgs e)
        {
            Panel btn = (Panel)sender;
            btn.BackColor = Color.Red;
        }
        private void AddSelectedRoom(object sender, EventArgs e, string room_id, string room_name, string description, double PRICE)
        {
            bool ADD = false;
            double totalPrice = 0;
            foreach (ListViewItem F in LST_SELECTED_ROOM.Items)
            {
                totalPrice = totalPrice + Convert.ToDouble(F.SubItems[3].Text);
                if (F.Text == room_id)
                {
                    F.Remove();
                    ADD = true;
                    break;
                }
            }
            if (ADD == false)
            {
                ListViewItem LST = new ListViewItem(room_id);
                {
                    LST.SubItems.Add(room_name);
                    LST.SubItems.Add(description);
                    LST.SubItems.Add(PRICE.ToString("f2"));
                    LST.BackColor = Color.Green;
                    LST.ForeColor = Color.White;
                }
                LST_SELECTED_ROOM.Items.Add(LST);
            }
            TXT_NO_OF_ROOMS.Text = LST_SELECTED_ROOM.Items.Count.ToString();
            LBL_TOT_CHARGE_LKR.Text = (totalPrice * Settings.Default.exchange_rate_lrk).ToString("F2");
            LBL_TOT_CHARGE_USD.Text = totalPrice.ToString("F2");
            CAL_AMOUNT();
        }
        //private void LOAD_AVAILABLE_ROOMS()
        //{
        //    try
        //    {
        //        CONNECTION.open_connection();
        //        using (MySqlDataAdapter da = new MySqlDataAdapter("SELECT id, room_name FROM room WHERE current_status = 'AVAILABLE'", CONNECTION.CON))
        //        {
        //            DataTable DT = new DataTable();
        //            da.Fill(DT);
        //            if (DT.Rows.Count > 0)
        //            {
        //               foreach(DataRow DR in DT.Rows)
        //                {
        //                    ShapedButton BTN = new ShapedButton();
        //                    BTN.Name = DR[0].ToString();
        //                    BTN.ButtonText= DR[1].ToString();
        //                    BTN.Text = DR[1].ToString();
        //                    BTN.TextLocation_X = 25;
        //                    BTN.TextLocation_Y = 20;
        //                    BTN.borderWidth = 1;
        //                    BTN.ShowButtontext = true;
        //                    BTN.Click += BTN_Click;
        //                    BTN.Size = new Size(75,60);
        //                    flowLayoutPanel1.Controls.Add(BTN);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception EX)
        //    {
        //        MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), EX.Message, MessageAlertImage.Error());
        //        mdg.ShowDialog();
        //    }
        //}

        private void BTN_Click(object sender, EventArgs e)
        {
            bool ADD = false;
            string[] tokens = sender.ToString().Split(':');
            foreach(ListViewItem F in LST_SELECTED_ROOM.Items)
            {
                if(F.Text== tokens[1])
                {
                    F.Remove();
                    ADD = true;
                    break;
                }
            }
            if (ADD==false)
            {
                ListViewItem LST = new ListViewItem(tokens[1]);
                {
                    LST.SubItems.Add(tokens[1]);
                    LST.BackColor = Color.Green;
                }
                LST_SELECTED_ROOM.Items.Add(LST);
            }
            TXT_NO_OF_ROOMS.Text = LST_SELECTED_ROOM.Items.Count.ToString();
        }

        private void CHECKED_IN_Load(object sender, EventArgs e)
        {
           
            string QRY1 = "SELECT meal_type_id, TYPE FROM meal_types";
            CLS_METHODS.FILL_COMBOBOX(CMB_MEAL_TYPE, QRY1, "TYPE", "meal_type_id", -1);

            string QRY2 = "SELECT agent_id, agent_name FROM agent";
            CLS_METHODS.FILL_COMBOBOX(CMB_AGENT, QRY2, "agent_name", "agent_id", -1);

            string QRY3 = "SELECT id,bank_name FROM bank";
            CLS_METHODS.FILL_COMBOBOX(CMB_CHEQUE_BANK, QRY3, "bank_name", "id", -1);
            TXT_GUEST_NAME.Focus();
            CAL_AMOUNT();
        }

        private void shapedButton1_Click(object sender, EventArgs e)
        {

        }

        private void DTP_ARRIVAL_DATE_ValueChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    TXT_NO_OF_NIGHTS.Text = Convert.ToUInt32((DTP_DEPATURE_DATE.Value - DTP_ARRIVAL_DATE.Value).TotalDays).ToString();
            //    //LOAD_AVAILABLE_ROOMS();
            //    CAL_AMOUNT();
            //}
            //catch (Exception)
            //{
            //}
        }

        private void DTP_DEPATURE_DATE_ValueChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    TXT_NO_OF_NIGHTS.Text = Convert.ToUInt32((DTP_DEPATURE_DATE.Value - DTP_ARRIVAL_DATE.Value).TotalDays).ToString();
            //    //LOAD_AVAILABLE_ROOMS();
            //    CAL_AMOUNT();
            //}
            //catch (Exception)
            //{
            //}
        }
        private void LOAD_GUEST_LIST_TO_LIST()
        {
            try
            {
                using (MySqlDataAdapter DA = new MySqlDataAdapter("SELECT reservation.reservation_id,guest.first_name , guest.last_name,guest.is_temp,reservation.added_date ,guest.guest_id,reservation.tax_status FROM guest INNER JOIN reservation ON (guest.guest_id = reservation.guest_id) WHERE  reservation.status=@status AND (guest.first_name LIKE @NAME OR reservation.reservation_id LIKE @NAME OR reservation.added_date=@NAME OR reservation.added_date=@NAME) ", CONNECTION.CON))
                {
                    DA.SelectCommand.Parameters.Clear();
                    DA.SelectCommand.Parameters.AddWithValue("@NAME", "%" + TXT_GUEST_NAME.Text+"%");
                    DA.SelectCommand.Parameters.AddWithValue("@status", "CHECKED IN");
                    DataTable DT = new DataTable();
                    DA.Fill(DT);
                    if (DT.Rows.Count > 0)
                    {
                        LST_GUEST_LIST.Items.Clear();
                        foreach (DataRow DR in DT.Rows)
                        {
                            ListViewItem LST = new ListViewItem(DR.Field<string>(0).ToString());
                            {
                                LST.SubItems.Add(DR.Field<string>(1));
                                LST.SubItems.Add(DR.Field<string>(2));
                                LST.SubItems.Add(DR.Field<string>(3));
                                LST.SubItems.Add(DR.Field<DateTime>(4).ToShortDateString());
                                LST.SubItems.Add(DR.Field<string>(5));
                                LST.SubItems.Add(DR.Field<string>(6));

                            }
                            LST_GUEST_LIST.Items.Add(LST);
                        }

                        if (LST_GUEST_LIST.Items.Count > 0)
                        {
                            LST_GUEST_LIST.Visible = true;
                            LST_GUEST_LIST.Size = new Size(394, 300);
                        }
                        else
                        {
                            LST_GUEST_LIST.Hide();
                        }
                    }
                    else
                    {
                        LST_GUEST_LIST.Items.Clear();
                    }
                }
            }
            catch (Exception EX)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), EX.Message, MessageAlertImage.Error());
                mdg.ShowDialog();
            }
        }
        private void TXT_GUEST_NAME_TextChanged(object sender, EventArgs e)
        {
            if (TXT_GUEST_NAME.Text.Length > 1)
            {
                LOAD_GUEST_LIST_TO_LIST();
            }
            else
            {
                LST_GUEST_LIST.Hide();
            }
        }

        private void TXT_GUEST_NAME_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (LST_GUEST_LIST.Items.Count > 0)
                {
                    LST_GUEST_LIST.Focus();
                    LST_GUEST_LIST.Items[0].Selected = true;

                }
            }
        }

        private void GET_RESERVED_NO_DATA()
        {
            try
            {
                using (MySqlDataAdapter DA = new MySqlDataAdapter("SELECT reservation.reservation_id , guest.guest_id , reservation.no_of_rooms , reservation.no_of_adult , reservation.no_of_child , reservation.arrival_date , reservation.depature_Date , reservation.no_of_nights ,IFNULL( agent.agent_id,-1) AS agent_id , agent.agent_name , reservation.additional_note , reservation.added_date , reservation.added_time , recerved_rooms.room_id , recerved_rooms.room_charge , room.room_name , room_packages.package_name , room_packages.description,reservation.meal_type_id,reservation.no_of_nights  FROM reservation INNER JOIN recerved_rooms ON (reservation.reservation_id = recerved_rooms.reservation_no) LEFT JOIN agent ON (agent.agent_id = reservation.agent_id) INNER JOIN guest ON (reservation.guest_id = guest.guest_id) INNER JOIN room ON (room.room_id = recerved_rooms.room_id) INNER JOIN room_packages ON (room.room_package_id = room_packages.room_package_id) WHERE reservation.reservation_id=@reservation_id", CONNECTION.CON))
                {
                    DA.SelectCommand.Parameters.Clear();
                    DA.SelectCommand.Parameters.AddWithValue("@reservation_id",RESERVATION_NO);
                    DataTable DT = new DataTable();
                    DA.Fill(DT);
                    if (DT.Rows.Count > 0)
                    {
                        LST_SELECTED_ROOM.Items.Clear();
                        foreach (DataRow DR in DT.Rows)
                        {
                            ListViewItem LST = new ListViewItem(DR.Field<int>("room_id").ToString());
                            {
                                LST.SubItems.Add(DR.Field<string>("room_name"));
                                LST.SubItems.Add(DR.Field<string>("package_name"));
                                LST.SubItems.Add(DR.Field<double>("room_charge").ToString("F2"));
                            }
                            LST_SELECTED_ROOM.Items.Add(LST);
                        }

                        TXT_NO_OF_ADULT.Text = DT.Rows[0].Field<int>("no_of_adult").ToString();
                        TXT_NO_OF_CHILD.Text = DT.Rows[0].Field<int>("no_of_child").ToString();
                        TXT_NO_OF_ROOMS.Text = DT.Rows[0].Field<int>("no_of_rooms").ToString();
                        DTP_ARRIVAL_DATE.Value= DT.Rows[0].Field<DateTime>("arrival_date")  ;
                        DTP_DEPATURE_DATE.Value = DT.Rows[0].Field<DateTime>("depature_Date");
                        TXT_NO_OF_NIGHTS.Text= DT.Rows[0].Field<int>("no_of_nights").ToString();
                        CMB_MEAL_TYPE.SelectedValue= DT.Rows[0].Field<int>("meal_type_id");

                        if (DT.Rows[0].Field<Int64>("agent_id") ==-1)
                        {
                            CMB_AGENT.Enabled = false;
                            CMB_AGENT.SelectedIndex = -1;
                        }
                        else
                        {
                            CMB_AGENT.Enabled = true;
                            CMB_AGENT.SelectedValue = DT.Rows[0].Field<Int64>("agent_id");
                        }
                       
                    }
                    else
                    {
                        LST_SELECTED_ROOM.Items.Clear();
                    }
                }

                using (MySqlDataAdapter DA = new MySqlDataAdapter("SELECT additional_service.additional_serivice_id , additional_service_list.service_name , additional_service.additional_serivice_qty , additional_service.additional_serivice_price FROM additional_service INNER JOIN additional_service_list ON (additional_service.additional_serivice_id = additional_service_list.id) WHERE reservation_id=@reservation_id", CONNECTION.CON))
                {
                    DA.SelectCommand.Parameters.Clear();
                    DA.SelectCommand.Parameters.AddWithValue("@reservation_id", RESERVATION_NO);
                    DataTable DT = new DataTable();
                    DA.Fill(DT);
                    if (DT.Rows.Count > 0)
                    {
                        LST_ADDITINAL_SERVICE.Items.Clear();
                        foreach (DataRow DR in DT.Rows)
                        {
                            ListViewItem LST = new ListViewItem(DR.Field<int>(0).ToString());
                            {
                                LST.SubItems.Add(DR.Field<string>(1));
                                LST.SubItems.Add(DR.Field<double>(2).ToString("F2"));
                                LST.SubItems.Add(DR.Field<double>(3).ToString("F2"));
                            }
                            LST_ADDITINAL_SERVICE.Items.Add(LST);
                        }  

                    }
                    else
                    {
                        LST_ADDITINAL_SERVICE.Items.Clear();
                    }
                }
            }
            catch (Exception EX)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), EX.Message, MessageAlertImage.Error());
                mdg.ShowDialog();
            }
        }

        String RESERVATION_NO = String.Empty;
        String GUEST_ID = String.Empty;
        String FIRST_NAME = String.Empty;
        String LAST_NAME = String.Empty;
        String IS_TEMP = String.Empty;
        String RECERVED_DATE = String.Empty;
        String TAX_DATA = String.Empty;


        private void LST_GUEST_LIST_KeyDown(object sender, KeyEventArgs e)
        {
           
            if (e.KeyCode == Keys.Up)
            {
                if (LST_GUEST_LIST.Items[0].Selected == true)
                {
                    TXT_GUEST_NAME.Focus();
                }
            }

            if (e.KeyCode == Keys.Enter)
            {

                RESERVATION_NO = LST_GUEST_LIST.SelectedItems[0].SubItems[0].Text;
                FIRST_NAME = LST_GUEST_LIST.SelectedItems[0].SubItems[1].Text;
                LAST_NAME = LST_GUEST_LIST.SelectedItems[0].SubItems[2].Text;
                IS_TEMP = LST_GUEST_LIST.SelectedItems[0].SubItems[3].Text;
                RECERVED_DATE = LST_GUEST_LIST.SelectedItems[0].SubItems[4].Text;
                GUEST_ID = LST_GUEST_LIST.SelectedItems[0].SubItems[5].Text;
                TAX_DATA= LST_GUEST_LIST.SelectedItems[0].SubItems[6].Text;
                if(TAX_DATA=="1")
                {
                    LBL_TAX_INFO.Text = "TAX INCLUDED";
                }
                else
                {
                    LBL_TAX_INFO.Text = "TAX NOT INCLUDED";
                }
                LBL_RESERVED_DATE.Text = RECERVED_DATE;
                LBL_SELECTED_ID_NO.Text = GUEST_ID;
                LBL_SELECTED_FIRST_NAME.Text = FIRST_NAME;
                LBL_SELECTED_LAST_NAME.Text = LAST_NAME;
                LBL_SELECTED_RECERVATION_NO.Text = RESERVATION_NO;
                GET_RESERVED_NO_DATA();
                //LOAD_AVAILABLE_ROOMS();
                TXT_GUEST_NAME.Text = RESERVATION_NO;
                LST_GUEST_LIST.Hide();
                TXT_NO_OF_ADULT.Focus();
                TXT_DISCOUNT.Focus();
                CAL_AMOUNT();
            }
        }
        private Double  GET_MEAL_PACKAGES_PRICE_ADULT()
        {
            CONNECTION.open_connection();
            using (MySqlDataAdapter DA=new MySqlDataAdapter("SELECT adult_meal_price,child_meal_price FROM meal_types WHERE meal_type_id=@meal_type_id", CONNECTION.CON))
            {
                DA.SelectCommand.Parameters.Clear();
                DA.SelectCommand.Parameters.AddWithValue("@meal_type_id", CMB_MEAL_TYPE.SelectedValue);
                DataTable DT = new DataTable();
                DA.Fill(DT);
                if(DT.Rows.Count>0)
                {
                   
                    return DT.Rows[0].Field<Double>(0);
                }
                else
                {
                    return 0;
                }
            }
            

        }
        private Double GET_MEAL_PACKAGES_PRICE_CHILD()
        {
            CONNECTION.open_connection();
            using (MySqlDataAdapter DA = new MySqlDataAdapter("SELECT adult_meal_price,child_meal_price FROM meal_types WHERE meal_type_id=@meal_type_id", CONNECTION.CON))
            {
                DA.SelectCommand.Parameters.Clear();
                DA.SelectCommand.Parameters.AddWithValue("@meal_type_id", CMB_MEAL_TYPE.SelectedValue);
                DataTable DT = new DataTable();
                DA.Fill(DT);
                if (DT.Rows.Count > 0)
                {

                    return DT.Rows[0].Field<Double>(1);
                }
                else
                {
                    return 0;
                }
            }

        }
        private Double GET_KOT_TOTAL()
        {
            CONNECTION.open_connection();
            using (MySqlDataAdapter DA = new MySqlDataAdapter("SELECT total_price FROM kot_order WHERE reservation_no=@reservation_no", CONNECTION.CON))
            {
                DA.SelectCommand.Parameters.Clear();
                DA.SelectCommand.Parameters.AddWithValue("@reservation_no", LBL_SELECTED_RECERVATION_NO.Text);
                DataTable DT = new DataTable();
                DA.Fill(DT);
                if (DT.Rows.Count > 0)
                {

                    return DT.Rows[0].Field<Double>(0);
                }
                else
                {
                    return 0;
                }
            }

        }
        private void CAL_AMOUNT()
        {
            try
            {
                double A_FOOD = 0;
                double C_FOOD = 0;
                if (CMB_MEAL_TYPE.SelectedIndex == -1)
                {
                    A_FOOD = 0;
                    C_FOOD = 0;
                }
                else
                {
                    if (TAX_DATA == "1")
                    {
                        A_FOOD = GET_MEAL_PACKAGES_PRICE_ADULT() + GET_MEAL_PACKAGES_PRICE_ADULT() * TAX / 100;
                        C_FOOD = GET_MEAL_PACKAGES_PRICE_CHILD() + GET_MEAL_PACKAGES_PRICE_CHILD() * TAX / 100;
                    }
                    else
                    {
                        A_FOOD = GET_MEAL_PACKAGES_PRICE_ADULT();
                        C_FOOD = GET_MEAL_PACKAGES_PRICE_CHILD();
                    }
                   
                }
                double DD = 0;
                double ADULTS = Convert.ToDouble(TXT_NO_OF_ADULT.Text);
                double CHILD = Convert.ToDouble(TXT_NO_OF_CHILD.Text);
                double NO_OF_DAYS = Convert.ToDouble(TXT_NO_OF_NIGHTS.Text);

                foreach (ListViewItem LST in LST_SELECTED_ROOM.Items)
                {
                    DD = DD + Convert.ToDouble(LST.SubItems[3].Text);
                }



                LBL_TOTAL_ROOM_PRICE.Text = (DD*Convert.ToDouble(TXT_NO_OF_NIGHTS.Text)).ToString("F2");
                LBL_TOAL_MEAL_PRICE_ADULT.Text = (A_FOOD * ADULTS* NO_OF_DAYS).ToString("F2");
                LBL_TOTAL_MEAL_CHILD.Text = (C_FOOD * CHILD* NO_OF_DAYS).ToString("F2");
                
                double AC = 0;
                foreach (ListViewItem LST in LST_ADDITINAL_SERVICE.Items)
                {
                    AC = AC + Convert.ToDouble(LST.SubItems[3].Text);
                }
                double MB_ITEM = 0;
                foreach (ListViewItem LST in LST_MINI_BAR_ITEM.Items)
                {
                    MB_ITEM = MB_ITEM + Convert.ToDouble(LST.SubItems[3].Text);
                }

                LBL_MINI_BAR_ITEMS.Text= MB_ITEM.ToString("F2");
                LBL_ADDITIONAL_SERVICE_CHARGE.Text= AC.ToString("F2");
                
                CAL_TOTAL_USD();
            }
            catch (Exception)
            {

            }

        }
        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CLEAR()
        {
            LBL_RESERVED_DATE.Text = "N/A";
            LBL_SELECTED_ID_NO.Text = "N/A"; ;
            LBL_SELECTED_FIRST_NAME.Text = "N/A";
            LBL_SELECTED_LAST_NAME.Text = "N/A"; 
            LBL_SELECTED_RECERVATION_NO.Text = "N/A"; 
            TXT_NO_OF_ADULT.Text ="0";
            TXT_NO_OF_CHILD.Text = "0";
            TXT_NO_OF_ROOMS.Text = "0";
            DTP_ARRIVAL_DATE.Value = DateTime.Now;
            DTP_DEPATURE_DATE.Value = DateTime.Now;
           // TXT_NOTE.Text = "N/A";
            CMB_AGENT.SelectedValue = -1;
            LST_GUEST_LIST.Items.Clear();

            LBL_TOTAL_ROOM_PRICE.Text = "0.00";
            LBL_TOAL_MEAL_PRICE_ADULT.Text = "0.00";
            LBL_TOTAL_MEAL_CHILD.Text = "0.00";
            LST_SELECTED_ROOM.Items.Clear();
            LBL_TOT_CHARGE_LKR.Text = "0.00";
            LBL_TOT_CHARGE_USD.Text = "0.00";
           // CMB_ADDITIONAL_SERVICE.SelectedIndex = -1;
            //TXT_ADDITIONAL_QTY.Text = "0.00";
            LST_ADDITINAL_SERVICE.Items.Clear();
          //  LBL_TOT_CHARGE_USD_WITH_TAX.Text = "0.00";
        }
        private void BTN_NEW_Click(object sender, EventArgs e)
        {
            DialogResult DS = MessageBox.Show("DO YOU WANT TO CLEAR THIS DATA" + Environment.NewLine + "ARE YOU SURE ?", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DS == DialogResult.Yes)
            {
                CLEAR_ALL();
                BTN_SAVE.Enabled = true;
                TXT_GUEST_NAME.Focus();
            }
            
        }
        private void RemoveSelectedRoom()
        {
            CONNECTION.open_connection();
            using (MySqlCommand DA = new MySqlCommand("DELETE FROM recerved_rooms WHERE reservation_no=@reservation_no AND room_id=@room_id", CONNECTION.CON))
            {
                DA.Parameters.Clear();
                DA.Parameters.AddWithValue("@reservation_no", LBL_SELECTED_RECERVATION_NO.Text);
                DA.Parameters.AddWithValue("@room_id",Convert.ToInt32(LST_SELECTED_ROOM.SelectedItems[0].SubItems[0].Text));
                int y= DA.ExecuteNonQuery();
                if(y>0)
                {
                    using (MySqlCommand A = new MySqlCommand("UPDATE reservation SET no_of_rooms=@no_of_rooms", CONNECTION.CON))
                    {
                        A.Parameters.Clear();
                        A.Parameters.AddWithValue("@no_of_rooms", LST_SELECTED_ROOM.Items.Count-1);
                        A.ExecuteNonQuery();
                    }

                    MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "SELECTED ROOM REMOVE FROM THIS RESERVATION!", MessageAlertImage.Success());
                    mdg.ShowDialog();
                }
                else
                {
                    MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), "SELECTED ROOM REMOVE FAILED!", MessageAlertImage.Error());
                    mdg.ShowDialog();
                }
            }

            
        }

        private void LST_SELECTED_ROOM_KeyDown(object sender, KeyEventArgs e)
        {
            //try
            //{
            //    if (e.KeyCode == Keys.Delete)
            //    {
            //        RemoveSelectedRoom();
            //        LST_SELECTED_ROOM.Items.RemoveAt(LST_SELECTED_ROOM.SelectedItems[0].Index);
            //        double totalPrice = 0;
            //        foreach (ListViewItem lst in LST_SELECTED_ROOM.Items)
            //        {
            //            totalPrice = totalPrice + Convert.ToDouble(lst.SubItems[3].Text);
            //        }
            //        LBL_TOT_CHARGE_LKR.Text = (totalPrice * Settings.Default.exchange_rate_lrk).ToString("F2");
            //        LBL_TOT_CHARGE_USD.Text = totalPrice.ToString("F2");
            //        CAL_AMOUNT();
            //        //LOAD_AVAILABLE_ROOMS();
            //    }
            //}
            //catch (Exception)
            //{
            //}
        }

        private void CMB_MEAL_TYPE_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CMB_MEAL_TYPE.SelectedIndex != -1)
            {
                CAL_AMOUNT();
            }
            else
            {
                LBL_TOAL_MEAL_PRICE_ADULT.Text = "0.00";
                LBL_TOTAL_MEAL_CHILD.Text = "0.00";
            }
        }

        private void CMB_MEAL_TYPE_Leave(object sender, EventArgs e)
        {
            CAL_AMOUNT();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        //private Double GET_ADDITIONAL_SERVICE_PRICE()
        //{
        //    CONNECTION.open_connection();
        //    using (MySqlDataAdapter DA = new MySqlDataAdapter("SELECT service_price FROM additional_service_list WHERE id=@id", CONNECTION.CON))
        //    {
        //        DA.SelectCommand.Parameters.Clear();
        //        DA.SelectCommand.Parameters.AddWithValue("@id", CMB_ADDITIONAL_SERVICE.SelectedValue);
        //        DataTable DT = new DataTable();
        //        DA.Fill(DT);
        //        if (DT.Rows.Count > 0)
        //        {

        //            return DT.Rows[0].Field<Double>(0);
        //        }
        //        else
        //        {
        //            return 0;
        //        }
        //    }

        //}
        private void BTN_MAKE_NEW_AGENT_Click(object sender, EventArgs e)
        {
            
        }

        private void LST_ADDITINAL_SERVICE_KeyDown(object sender, KeyEventArgs e)
        {
            //try
            //{
            //    if (e.KeyCode == Keys.Delete)
            //    {
            //        LST_ADDITINAL_SERVICE.Items.RemoveAt(LST_ADDITINAL_SERVICE.SelectedItems[0].Index);
            //        CAL_AMOUNT();
            //    }
            //}
            //catch (Exception)
            //{
            //}
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void LST_SELECTED_ROOM_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void TXT_NO_OF_ADULT_Leave(object sender, EventArgs e)
        {
            if(TXT_NO_OF_ADULT.Text==string.Empty)
            {
                TXT_NO_OF_ADULT.Text = "0";
            }
            CAL_AMOUNT();
        }

        private void TXT_NO_OF_CHILD_Leave(object sender, EventArgs e)
        {
            if (TXT_NO_OF_CHILD.Text == string.Empty)
            {
                TXT_NO_OF_CHILD.Text = "0";
            }
            CAL_AMOUNT();
        }
        private void LOCK_CONTROLLERS()
        {
            groupBox2.Enabled = false;
            groupBox3.Enabled = false;
            groupBox4.Enabled = false;
            BTN_SAVE.Enabled = false;
            LST_ADDITINAL_SERVICE.Enabled = false;
            LST_GUEST_LIST.Enabled = false;
            LST_SELECTED_ROOM.Enabled = false;
            BTN_SAVE.Enabled = false;
            BTN_NEW.Enabled = false;
            TXT_GUEST_NAME.Enabled = false;
            TXT_NO_OF_ADULT.Enabled = false;
            TXT_NO_OF_CHILD.Enabled = false;
            CMB_AGENT.Enabled = false; ;
            CMB_MEAL_TYPE.Enabled = false;

        }
        private void UNLOCK_CONTROLLERS()
        {
            groupBox2.Enabled = true;
            groupBox3.Enabled = true;
            groupBox4.Enabled = true;
            BTN_SAVE.Enabled = true;
            BTN_SAVE.Enabled = true;
            LST_ADDITINAL_SERVICE.Enabled = true;
            LST_GUEST_LIST.Enabled = true;
            LST_SELECTED_ROOM.Enabled = true;
            BTN_SAVE.Enabled = true;
            BTN_NEW.Enabled = true;
            TXT_GUEST_NAME.Enabled = true;
            TXT_NO_OF_ADULT.Enabled = true;
            TXT_NO_OF_CHILD.Enabled = true;
            TXT_NO_OF_NIGHTS.Enabled = true;
            TXT_NO_OF_ROOMS.Enabled = true;
            CMB_AGENT.Enabled = true; ;
            CMB_MEAL_TYPE.Enabled = true;
        }

        private Double GET_AGENT_COMMITIONS()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter DA = new MySqlDataAdapter("SELECT commison_rate FROM agent WHERE agent_id=@agent_id", CONNECTION.CON))
                {
                    DA.SelectCommand.Parameters.Clear();
                    DA.SelectCommand.Parameters.AddWithValue("@agent_id", CMB_AGENT.SelectedValue);
                    DataTable DT = new DataTable();
                    DA.Fill(DT);
                    if (DT.Rows.Count > 0)
                    {

                        return DT.Rows[0].Field<Double>(0);
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception)
            {
                return 0;
            }

        }
        private Double GET_KOT_CHARGES()
        {
            CONNECTION.open_connection();
            using (MySqlDataAdapter DA = new MySqlDataAdapter("SELECT total_price FROM kot_order WHERE reservation_no=@reservation_no", CONNECTION.CON))
            {
                DA.SelectCommand.Parameters.Clear();
                DA.SelectCommand.Parameters.AddWithValue("@reservation_no", LBL_SELECTED_RECERVATION_NO.Text);
                DataTable DT = new DataTable();
                DA.Fill(DT);
                if (DT.Rows.Count > 0)
                {
                    return DT.Rows[0].Field<Double>(0);
                }
                else
                {
                    return 0;
                }
            }

        }
     
        MySqlCommand cmd = CONNECTION.CON.CreateCommand();
        MySqlTransaction myTrans;
        private void SAVE_CHECKED_OUT()
        {
            try
            {

                Cursor.Current = Cursors.WaitCursor;
                LOCK_CONTROLLERS();
                CONNECTION.open_connection();
                myTrans = CONNECTION.CON.BeginTransaction();
                cmd.Connection = CONNECTION.CON;
                cmd.Transaction = myTrans;

                double discount = Convert.ToDouble(TXT_DISCOUNT.Text);
                double cash = Convert.ToDouble(TXT_CASH.Text);
                double cheque = Convert.ToDouble(TXT_CHEQUE.Text);
                double card = Convert.ToDouble(TXT_CARD.Text);

                cmd.CommandText = "UPDATE reservation SET STATUS=@status,agent_id=@agent_id WHERE reservation_id=@reservation_id";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@reservation_id", LBL_SELECTED_RECERVATION_NO.Text);
                cmd.Parameters.AddWithValue("@agent_id", CMB_AGENT.SelectedValue);
                cmd.Parameters.AddWithValue("@status", "CHECKED OUT");
                cmd.ExecuteNonQuery();

                foreach (ListViewItem dgvr in LST_SELECTED_ROOM.Items)
                {
                    cmd.CommandText = "UPDATE room SET current_status=@current_status WHERE room_id=@room_id";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@room_id", Convert.ToInt32(dgvr.SubItems[0].Text));
                    cmd.Parameters.AddWithValue("@current_status", "CHECKED OUT");
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "UPDATE room SET current_status=@current_status WHERE room_id=@room_id";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@room_id", Convert.ToInt32(dgvr.SubItems[0].Text));
                    cmd.Parameters.AddWithValue("@current_status", "CHECKED OUT");
                    cmd.ExecuteNonQuery();
                }

                cmd.CommandText = "UPDATE recervation_price SET agent_commitions=@agent_commitions,paid=@paid,STATUS=@status,discount=@discount,mini_bar_item_price=@mini_bar_item_price,additional_service_charge=@additional_service_charge,kot_charges=@kot_charges,sub_toal_with_tax=@sub_toal_with_tax WHERE reservation_no=@reservation_no";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@reservation_no", LBL_SELECTED_RECERVATION_NO.Text);
                cmd.Parameters.AddWithValue("@discount", Convert.ToDouble(TXT_DISCOUNT.Text));
                cmd.Parameters.AddWithValue("@kot_charges", GET_KOT_CHARGES());
                cmd.Parameters.AddWithValue("@additional_service_charge", Convert.ToDouble(LBL_ADDITIONAL_SERVICE_CHARGE.Text));
                cmd.Parameters.AddWithValue("@sub_toal_with_tax", Convert.ToDouble(LBL_TOT_CHARGE_USD.Text));
                cmd.Parameters.AddWithValue("@mini_bar_item_price", Convert.ToDouble(LBL_MINI_BAR_ITEMS.Text));
                if (CMB_AGENT.SelectedIndex>-1)
                {
                    cmd.Parameters.AddWithValue("@agent_commitions", (Convert.ToDouble(LBL_TOT_CHARGE_USD.Text)* GET_AGENT_COMMITIONS())/100);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@agent_commitions", 0);
                }
                cmd.Parameters.AddWithValue("@paid", Convert.ToDouble( cash+card+cheque));
                cmd.Parameters.AddWithValue("@status", "COMPLETE");
                cmd.ExecuteNonQuery();

                foreach (ListViewItem dgvr in LST_MINI_BAR_ITEM.Items)
                {
                    cmd.CommandText = "INSERT INTO purchased_mini_bar_item (reservation_no, stock_id, qty, price) VALUES (@reservation_no, @stock_id, @qty, @price)";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@reservation_no", LBL_SELECTED_RECERVATION_NO.Text);
                    cmd.Parameters.AddWithValue("@stock_id", dgvr.SubItems[0].Text);
                    cmd.Parameters.AddWithValue("@qty", Convert.ToDouble(dgvr.SubItems[2].Text));
                    cmd.Parameters.AddWithValue("@price", Convert.ToDouble(dgvr.SubItems[3].Text)/Convert.ToDouble(dgvr.SubItems[2].Text));
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "UPDATE stock SET qty=qty-@qty WHERE stock_id=@stock_id";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@qty", Convert.ToDouble(dgvr.SubItems[2].Text));
                    cmd.Parameters.AddWithValue("@stock_id", dgvr.SubItems[0].Text);
                    cmd.ExecuteNonQuery();
                }

                if (cheque > 0)
                {
                    cmd.CommandText = "INSERT INTO received_cheque (reservation_no, amount, cheque_date, issued_date, guest_id, bank_id, cheque_no, issued_time, issued_by ) VALUES ( @reservation_no, @amount, @cheque_date, @issued_date, @guest_id, @bank_id, @cheque_no, CURTIME(), @issued_by)";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@reservation_no", LBL_SELECTED_RECERVATION_NO.Text);
                    cmd.Parameters.AddWithValue("@amount", cheque);
                    cmd.Parameters.AddWithValue("@cheque_date", DTP_CHEQUE_DATE.Value.ToShortDateString());
                    cmd.Parameters.AddWithValue("@issued_date", DateTime.Now.ToShortDateString());
                    cmd.Parameters.AddWithValue("@guest_id", GUEST_ID);
                    cmd.Parameters.AddWithValue("@bank_id", CMB_CHEQUE_BANK.SelectedValue);
                    cmd.Parameters.AddWithValue("@cheque_no", TXT_CHEQUE_NO.Text);
                    //cmd.Parameters.AddWithValue("@issued_time", DateTime.Now.ToShortTimeString());
                    cmd.Parameters.AddWithValue("@issued_by", CLS_CURRENT_LOGGER.LOGGED_IN_USERID);
                    cmd.ExecuteNonQuery();
                }

                string type = string.Empty;
                if (cash > 0)
                {
                    type = type + "CASH,";
                }
                if (cheque > 0)
                {
                    type = type + "CHEQUE,";
                }
                if (card > 0)
                {
                    type = type + "CARD,";
                }

                cmd.CommandText = "INSERT INTO account(income_type, payment_type, payment, added_date, added_time, note,card_details ) VALUES ( @income_type, @payment_type, @payment, @added_date, CURTIME(), @note,@card_details)";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@income_type", "INCOME");
                cmd.Parameters.AddWithValue("@payment_type", type); 
                cmd.Parameters.AddWithValue("@card_details", "CARD NO: "+TXT_CARD_NO.Text+" OWNER NAME: "+TXT_OWNER.Text);  
                cmd.Parameters.AddWithValue("@payment", cash + cheque + card);
                cmd.Parameters.AddWithValue("@added_date", DateTime.Now.ToShortDateString());
                cmd.Parameters.AddWithValue("@note", "AMOUNT RECEIVED FOR: " + LBL_SELECTED_RECERVATION_NO.Text);
                cmd.ExecuteNonQuery();

                if(CMB_AGENT.SelectedIndex>-1)
                {
                    double AGENT_RATE = CLS_METHODS.GET_DATA("SELECT commison_rate FROM agent WHERE agent_id='"+ CMB_AGENT.SelectedIndex + "'");
                    double COMMITION = Convert.ToDouble(LBL_TOT_CHARGE_USD.Text) * AGENT_RATE / 100;
                    cmd.CommandText = "INSERT INTO agent_acc (account_no, reservation_no, agent_id, commition, paid, due) VALUES (@account_no, @reservation_no, @agent_id, @commition, @paid, @due)";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@account_no", CLS_METHODS.GET_MAX_INT_ID("SELECT MAX(id) FROM account"));
                    cmd.Parameters.AddWithValue("@reservation_no", LBL_SELECTED_RECERVATION_NO.Text);
                    cmd.Parameters.AddWithValue("@agent_id", CMB_AGENT.SelectedIndex);
                    cmd.Parameters.AddWithValue("@commition", COMMITION);
                    cmd.Parameters.AddWithValue("@paid", 0);
                    cmd.Parameters.AddWithValue("@due", 0);
                    cmd.ExecuteNonQuery();
                }


                myTrans.Commit();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "SELECTED RESERVATION  SUCCESSFULLY CHECKED OUT!", MessageAlertImage.Success());
                mdg.ShowDialog();

            }
            catch (Exception EX)
            {
                Cursor.Current = Cursors.Default;
                UNLOCK_CONTROLLERS();
                myTrans.Rollback();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), EX.Message, MessageAlertImage.Error());
                mdg.ShowDialog();
            }
            finally
            {
                UNLOCK_CONTROLLERS();
                BTN_NEW.Focus();
                Cursor.Current = Cursors.Default;
                CONNECTION.close_connection();
            }
        }
        private void CLEAR_ALL()
        {
            LBL_RESERVED_DATE.Text = "N/A";
            LBL_SELECTED_ID_NO.Text = "N/A"; ;
            LBL_SELECTED_FIRST_NAME.Text = "N/A";
            LBL_SELECTED_LAST_NAME.Text = "N/A";
            LBL_SELECTED_RECERVATION_NO.Text = "N/A";
            LBL_TAX_INFO.Text = "N/A";
            TXT_NO_OF_ADULT.Text = "0";
            TXT_NO_OF_CHILD.Text = "0";
            TXT_NO_OF_ROOMS.Text = "0";
            DTP_ARRIVAL_DATE.Value = DateTime.Now;
            DTP_DEPATURE_DATE.Value = DateTime.Now;
            CMB_AGENT.SelectedValue = -1;
            LST_GUEST_LIST.Items.Clear();
            TXT_DISCOUNT.Text = "0.00";
            LBL_TOTAL_PAYABLE.Text = "0.00";
            LBL_TOTAL_ROOM_PRICE.Text = "0.00";
            LBL_TOAL_MEAL_PRICE_ADULT.Text = "0.00";
            LBL_TOTAL_MEAL_CHILD.Text = "0.00";
            LST_SELECTED_ROOM.Items.Clear();
            LBL_TOT_CHARGE_LKR.Text = "0.00";
            LBL_TOT_CHARGE_USD.Text = "0.00";
            LST_ADDITINAL_SERVICE.Items.Clear();
            LST_MINI_BAR_ITEM.Items.Clear();
            PNL_MINI_BAR.Hide();
            LBL_ADDITIONAL_SERVICE_CHARGE.Text = "0.00";
            TXT_CASH.Text = "0.00";
            TXT_CARD.Text = "0.00";
            TXT_CHEQUE.Text = "0.00";
            TXT_NO_OF_NIGHTS.Text = "0";
            TAX_DATA = String.Empty;
            CMB_MEAL_TYPE.SelectedIndex = -1;
            TXT_CHEQUE_NO.Clear();
            DTP_CHEQUE_DATE.Value = DateTime.Now;
            CMB_CHEQUE_BANK.SelectedIndex = -1;
            TXT_CARD.Clear();
            TXT_CARD_NO.Clear();
            TXT_OWNER.Clear();

        }
        private void TXT_NO_OF_ADULT_TextChanged(object sender, EventArgs e)
        {
            //CAL_AMOUNT();
        }

        private void TXT_NO_OF_CHILD_TextChanged(object sender, EventArgs e)
        {
          ///  CAL_AMOUNT();
        }

        private void LBL_TOTAL_ROOM_PRICE_TextChanged(object sender, EventArgs e)
        {
        }

        private void BTN_SAVE_Click(object sender, EventArgs e)
        {
            if(LBL_SELECTED_RECERVATION_NO.Text== "N/A")
            {
                TXT_GUEST_NAME.Focus();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), "PLEASE SELECT RESERVATION!", MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
            else if(Convert.ToDouble(LBL_TOTAL_PAYABLE.Text)>0)
            {
                TXT_GUEST_NAME.Focus();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), "ENTERD AMOUNT IS LESS THAN TOTAL AMOUNT!", MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
            else if (Convert.ToDouble(TXT_CHEQUE.Text) > 0 && CMB_CHEQUE_BANK.SelectedIndex==-1)
            {
                CMB_CHEQUE_BANK.Focus();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), "PLEASE SELECT BANK!", MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
            else
            {
                SAVE_CHECKED_OUT();
                BTN_SAVE.Enabled = false;
                BTN_PRINT.Focus();
                BTN_PRINT.PerformClick();
            }
           
        }

        private void TXT_NO_OF_ADULT_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void TXT_NO_OF_CHILD_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void TXT_NO_OF_ROOMS_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void DTP_ARRIVAL_DATE_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void DTP_DEPATURE_DATE_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void CMB_MEAL_TYPE_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void CMB_ADDITIONAL_SERVICE_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void TXT_ADDITIONAL_QTY_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void CHECKED_IN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Add)
            {
                TXT_DISCOUNT.Focus();
            }

            if (e.KeyCode == Keys.F10)
            {
                BTN_SAVE.Focus();
            }
        }

        private void CMB_CHEQUE_BANK_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label31_Click(object sender, EventArgs e)
        {

        }

        private void label30_Click(object sender, EventArgs e)
        {

        }

        private void label29_Click(object sender, EventArgs e)
        {

        }

        private void TXT_CHEQUE_NO_TextChanged(object sender, EventArgs e)
        {

        }

        private void DTP_CHEQUE_DATE_ValueChanged(object sender, EventArgs e)
        {

        }

        private void TXT_NO_OF_ROOMS_TextChanged(object sender, EventArgs e)
        {

        }

        private void TXT_DISCOUNT_Leave(object sender, EventArgs e)
        {
            if(Convert.ToDouble( TXT_DISCOUNT.Text)>0)
            {
                LBL_TOTAL_PAYABLE.Text = (Convert.ToDouble(LBL_TOT_CHARGE_USD.Text) - Convert.ToDouble(TXT_DISCOUNT.Text)).ToString("F2");
            }
            else
            {
                LBL_TOTAL_PAYABLE.Text = LBL_TOT_CHARGE_USD.Text;
            }
        }

        private void TXT_DISCOUNT_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToDouble(TXT_DISCOUNT.Text) > 0)
                {
                    LBL_TOTAL_PAYABLE.Text = (Convert.ToDouble(LBL_TOT_CHARGE_USD.Text) - Convert.ToDouble(TXT_DISCOUNT.Text)).ToString("F2");
                }
                else
                {
                    LBL_TOTAL_PAYABLE.Text = LBL_TOT_CHARGE_USD.Text;
                }
            }
            catch (Exception)
            {
            }
        }
        private void CHECK_TEXTBOX()
        {
            if (TXT_CASH.Text == String.Empty)
            {
                TXT_CASH.Text = "0.00";
            }
            if (TXT_CARD.Text == String.Empty)
            {
                TXT_CARD.Text = "0.00";
            }
            if (TXT_CHEQUE.Text == String.Empty)
            {
                TXT_CHEQUE.Text = "0.00";
            }
        }
        private void TXT_CASH_TextChanged(object sender, EventArgs e)
        {
            CHECK_TEXTBOX();
            LBL_TOTAL_PAYABLE.Text = (Convert.ToDouble(LBL_TOT_CHARGE_USD.Text) - Convert.ToDouble(TXT_DISCOUNT.Text)-Convert.ToDouble(TXT_CASH.Text) - Convert.ToDouble(TXT_CHEQUE.Text) - Convert.ToDouble(TXT_CARD.Text)).ToString("F2");
        }

        private void TXT_CHEQUE_TextChanged(object sender, EventArgs e)
        {
            CHECK_TEXTBOX();
            LBL_TOTAL_PAYABLE.Text = (Convert.ToDouble(LBL_TOT_CHARGE_USD.Text)-Convert.ToDouble(TXT_DISCOUNT.Text) - Convert.ToDouble(TXT_CASH.Text) - Convert.ToDouble(TXT_CHEQUE.Text) - Convert.ToDouble(TXT_CARD.Text)).ToString("F2");
        }

        private void TXT_CARD_TextChanged(object sender, EventArgs e)
        {
            CHECK_TEXTBOX();
            LBL_TOTAL_PAYABLE.Text = (Convert.ToDouble(LBL_TOT_CHARGE_USD.Text)-Convert.ToDouble(TXT_DISCOUNT.Text) - Convert.ToDouble(TXT_CASH.Text) - Convert.ToDouble(TXT_CHEQUE.Text) - Convert.ToDouble(TXT_CARD.Text)).ToString("F2");
        }

        private void TXT_NO_OF_NIGHTS_TextChanged(object sender, EventArgs e)
        {

        }

        private void CMB_AGENT_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void shapedButton1_Click_1(object sender, EventArgs e)
        {
            if(LBL_SELECTED_RECERVATION_NO.Text!= "N/A")
            {
                REPORT.RESERVATION_NO = LBL_SELECTED_RECERVATION_NO.Text;
                REPORT_VIEWER RV = new REPORT_VIEWER();
                RV.ShowDialog();
            }
            else
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), "NO ANY CHECKEDOUT FOUND!", MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
            
        }

        private void TXT_DISCOUNT_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                TXT_CASH.Focus();
            }
        }

        private void TXT_CASH_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TXT_CHEQUE.Focus();
            }
        }

        private void TXT_CHEQUE_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TXT_CARD.Focus();
            }
        }

        private void TXT_CARD_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TXT_CHEQUE_NO.Focus();
            }
        }

        private void TXT_CHEQUE_NO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TXT_CARD_NO.Focus();
            }
        }

        private void TXT_CARD_NO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CMB_CHEQUE_BANK.Focus();
            }
        }

        private void CMB_CHEQUE_BANK_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TXT_OWNER.Focus();
            }
        }

        private void TXT_OWNER_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DTP_CHEQUE_DATE.Focus();
            }
        }

        private void DTP_CHEQUE_DATE_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BTN_SAVE.Focus();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if(LBL_SELECTED_RECERVATION_NO.Text!="N/A" && PNL_GUEST_INFO.Visible==false)
            {
                PNL_GUEST_INFO.Size = new Size(335, 175);
                PNL_GUEST_INFO.Visible = true;
            }
            else
            {
                PNL_GUEST_INFO.Visible = false;
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), "PLEASE LOAD RESERVATION FOR DETAILS!", MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
        }


        private void LOAD_MINI_BAR_ITEMS()
        {
            try
            {
                CONNECTION.open_connection();
                string SQL = string.Empty;
                if(TAX_DATA=="1")
                {
                    SQL = "SELECT S.stock_id, CONCAT(i.item_name,' -(',ROUND (s.sales_price+s.sales_price*@tax/100,2),' USD)') AS STRING , s.qty,s.sales_price+s.sales_price*@tax/100 FROM item AS i, stock AS s WHERE i.item_id = s.item_code AND i.item_type_id = '2' AND i.item_status = 'ENABLE' ";
                }
                else
                {
                    SQL = "SELECT S.stock_id, CONCAT(i.item_name,' -(',ROUND (s.sales_price),' USD)') AS STRING , s.qty, s.sales_price FROM item AS i, stock AS s WHERE i.item_id = s.item_code AND i.item_type_id = '2' AND i.item_status = 'ENABLE' ";
                }
                using (MySqlDataAdapter da = new MySqlDataAdapter(SQL, CONNECTION.CON))
                {
                    da.SelectCommand.Parameters.Clear();
                    da.SelectCommand.Parameters.AddWithValue("@tax", Settings.Default.tax_percentage);
                    DataTable DT = new DataTable();
                    da.Fill(DT);
                    if (DT.Rows.Count > 0)
                    {
                        CMB_MINI_BAR_ITEM.DataSource = DT;
                        CMB_MINI_BAR_ITEM.DisplayMember = "STRING";
                        CMB_MINI_BAR_ITEM.ValueMember = "stock_id";
                        CMB_MINI_BAR_ITEM.SelectedIndex = -1;
                    }
                    else
                    {
                        CMB_MINI_BAR_ITEM.DataSource = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), ex.Message, MessageAlertImage.Error());
                mdg.ShowDialog();
            }
        }
        private void button1_Click_2(object sender, EventArgs e)
        {
            if(PNL_MINI_BAR.Visible==false)
            {
                LOAD_MINI_BAR_ITEMS();
                PNL_MINI_BAR.Size = new Size(353, 106);
                PNL_MINI_BAR.Visible = true;
            }
            else
            {

                PNL_MINI_BAR.Visible = false;
            }
        }

        private void hyflexTextbox1_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void hyflexTextbox1_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void LST_MINI_BAR_ITEMS_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private String ITEM_CODE = String.Empty;
        private String ITEM_NAME = String.Empty;
        private String QTY = String.Empty;
        private String PRICE = String.Empty;

        private void LST_MINI_BAR_ITEMS_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void CLEAR_MINI_BAR_ITEMS()
        {
            CMB_MINI_BAR_ITEM.SelectedIndex = -1;
            TXT_MINI_BAT_ITEM_QTY.Focus();
            TXT_MINI_BAT_ITEM_QTY.Text = "0.000";
        }
        private String GET_NAME()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlCommand da = new MySqlCommand("SELECT item.item_name FROM stock INNER JOIN item ON (stock.item_code = item.item_id) WHERE stock.stock_id=@stock_id", CONNECTION.CON))
                {
                    da.Parameters.Clear();
                    da.Parameters.AddWithValue("@stock_id", CMB_MINI_BAR_ITEM.SelectedValue.ToString());
                    return da.ExecuteScalar().ToString();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        private String GET_PRICE()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlCommand da = new MySqlCommand("SELECT sales_price FROM stock WHERE stock_id=@item_id", CONNECTION.CON))
                {
                    da.Parameters.Clear();
                    da.Parameters.AddWithValue("@item_id", CMB_MINI_BAR_ITEM.SelectedValue.ToString());
                    return da.ExecuteScalar().ToString();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        Double TAX = Settings.Default.tax_percentage;
        private void button2_Click(object sender, EventArgs e)
        {
            if(CMB_MINI_BAR_ITEM.SelectedIndex==-1)
            {
                CMB_MINI_BAR_ITEM.Focus();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), "PLEASE SELECT ITEM!", MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
            else if(Convert.ToDouble(TXT_MINI_BAT_ITEM_QTY.Text)==0)
            {
                TXT_MINI_BAT_ITEM_QTY.Focus();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), "PLEASE ENTER QTY!", MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
            else
            {
                ListViewItem LST = new ListViewItem(CMB_MINI_BAR_ITEM.SelectedValue.ToString());
                {
                    Double TOTAL = 0;
                    if (TAX_DATA=="1")
                    {
                        TOTAL = (Convert.ToDouble(TXT_MINI_BAT_ITEM_QTY.Text) * Convert.ToDouble(GET_PRICE()) + ((Convert.ToDouble(TXT_MINI_BAT_ITEM_QTY.Text) * Convert.ToDouble(GET_PRICE()) * TAX / 100)));
                    }
                    else
                    {
                        TOTAL = Convert.ToDouble(TXT_MINI_BAT_ITEM_QTY.Text) * Convert.ToDouble(GET_PRICE());
                    }
                   
                    LST.SubItems.Add(GET_NAME());
                    LST.SubItems.Add(Convert.ToDouble(TXT_MINI_BAT_ITEM_QTY.Text).ToString("F3"));
                    LST.SubItems.Add(TOTAL.ToString("F2"));
                }
                LST_MINI_BAR_ITEM.Items.Add(LST);
                CLEAR_MINI_BAR_ITEMS();
                CAL_AMOUNT();
                CMB_MINI_BAR_ITEM.Focus();
            }
            
        }

        private void PNL_MINI_BAR_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CMB_MINI_BAR_ITEM_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                TXT_MINI_BAT_ITEM_QTY.Focus();
            }
        }

        private void TXT_MINI_BAT_ITEM_QTY_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BTN_ADD_MINI_BAR_ITEM.Focus();
            }
            
        }

        private void LST_MINI_BAR_ITEM_KeyDown(object sender, KeyEventArgs e)
        {
            LST_MINI_BAR_ITEM.Items.RemoveAt(LST_MINI_BAR_ITEM.SelectedItems[0].Index);
            CAL_AMOUNT();
        }

        private void BTN_CLOSE_Click(object sender, EventArgs e)
        {
            PNL_MINI_BAR.Hide();
        }
    }
}
