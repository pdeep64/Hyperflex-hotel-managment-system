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

namespace CustomWindowsForm.FORMS
{
    public partial class CHECKED_IN : Form
    {
        public CHECKED_IN()
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
              //  _MaxButton_Click(sender, e);
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
            Double SUB_TOAL = ROOM_CHARGES + MEAL_ADULT + MEAL_CHILD + ADDITIONAL;
            LBL_TOT_CHARGE_USD.Text = SUB_TOAL.ToString("F2");
           // LBL_TOT_CHARGE_USD_WITH_TAX.Text = (SUB_TOAL+((SUB_TOAL * Settings.Default.tax_percentage) / 100)).ToString("F2");
            LBL_TOT_CHARGE_LKR.Text = (SUB_TOAL*Settings.Default.exchange_rate_lrk).ToString("F2");
        }
        private void label9_Click(object sender, EventArgs e)
        {

        }
        private void LOAD_AVAILABLE_ROOMS()
        {
            try
            {
                double TAX = Settings.Default.tax_percentage;
                CONNECTION.open_connection();
                using (MySqlDataAdapter da = new MySqlDataAdapter("SELECT room_id,room_name,room_packages.package_name,room_packages.description,room_packages.condition,room_packages.room_package_price,room_packages.package_color FROM room INNER JOIN room_packages ON (room.room_package_id=room_packages.room_package_id) WHERE room_id NOT IN(SELECT room.room_id  FROM room INNER JOIN room_packages ON (room.room_package_id = room_packages.room_package_id) INNER JOIN recerved_rooms ON (recerved_rooms.room_id = room.room_id) INNER JOIN reservation ON (reservation.reservation_id = recerved_rooms.reservation_no) WHERE (reservation.arrival_date BETWEEN @frm_Date AND @todate) OR (reservation.depature_Date > @frm_Date AND reservation.depature_Date<= @todate))", CONNECTION.CON))
                {
                    da.SelectCommand.Parameters.Clear();
                    da.SelectCommand.Parameters.AddWithValue("@frm_Date", DTP_ARRIVAL_DATE.Value.ToShortDateString());
                    da.SelectCommand.Parameters.AddWithValue("@todate", DTP_DEPATURE_DATE.Value.ToShortDateString());
                    DataTable DT = new DataTable();
                    da.Fill(DT);
                    if (DT.Rows.Count > 0)
                    {
                        flowLayoutPanel1.Controls.Clear();
                        flowLayoutPanel1.Visible = false;
                        foreach (DataRow DR in DT.Rows)
                        {

                            Font = new Font("Consolas", 8, FontStyle.Bold);
                            Panel BTN = new Panel();
                            BTN.Name = DR[0].ToString();
                            double ROOM_CHRGES_WITH = Convert.ToDouble(DR[5].ToString());
                            double TOTAL = 0;
                            if (TAX_STATUS=="1")
                            {
                                TOTAL = ROOM_CHRGES_WITH + (ROOM_CHRGES_WITH * TAX / 100);
                            }
                            else
                            {
                                TOTAL = ROOM_CHRGES_WITH;
                            }
                            
                            BTN.Click += (sender2, e2) => AddSelectedRoom(sender2, e2, DR[0].ToString(), DR[1].ToString(), DR[2].ToString(), TOTAL);
                            BTN.Size = new Size(100, 100);
                            Color back = ColorTranslator.FromHtml(DR[6].ToString());
                            BTN.BackColor = back;

                            //DESCRIPTION
                            Label LBL_TYPE = new Label();
                            LBL_TYPE.Height = 30;
                            LBL_TYPE.BackColor = Color.DodgerBlue;
                            LBL_TYPE.Text = DR[2].ToString();
                            LBL_TYPE.AutoSize = false;
                            LBL_TYPE.Width = 96;
                            LBL_TYPE.Location = new Point(2, 2);
                            LBL_TYPE.MouseClick += (sender2, e2) => AddSelectedRoom(sender2, e2, DR[0].ToString(), DR[1].ToString(), DR[2].ToString(), TOTAL);
                            LBL_TYPE.TextAlign = ContentAlignment.BottomCenter;

                            //CONDITION
                            Label LBL_CONDITION = new Label();
                            LBL_CONDITION.Height = 15;
                            LBL_CONDITION.BackColor = Color.DeepSkyBlue;
                            LBL_CONDITION.Text = DR[4].ToString();
                            LBL_CONDITION.AutoSize = false;
                            LBL_CONDITION.Width = 96;
                            LBL_CONDITION.MouseClick += (sender2, e2) => AddSelectedRoom(sender2, e2, DR[0].ToString(), DR[1].ToString(), DR[2].ToString(), TOTAL);
                            LBL_CONDITION.Location = new Point(2, 30);
                            LBL_CONDITION.TextAlign = ContentAlignment.BottomCenter;

                            //PRICE
                            Label LBL_PRICE = new Label();
                            LBL_PRICE.Height = 15;
                            LBL_PRICE.BackColor = Color.DodgerBlue;
                            LBL_PRICE.AutoSize = false;
                            LBL_PRICE.Width = 96;
                            LBL_PRICE.Location = new Point(2, 50);
                            LBL_PRICE.MouseClick += (sender2, e2) => AddSelectedRoom(sender2, e2, DR[0].ToString(), DR[1].ToString(), DR[2].ToString(), TOTAL);
                            LBL_PRICE.Text = "LKR: " + TOTAL.ToString("F2");
                            LBL_PRICE.TextAlign = ContentAlignment.BottomCenter;

                            //ROOM NAME
                            Label LBL_ID = new Label();
                            LBL_ID.Height = 15;
                            LBL_ID.Text = DR[1].ToString();
                            LBL_ID.AutoSize = false;
                            LBL_ID.Width = 96;
                            LBL_ID.Location = new Point(2, 70);
                            LBL_ID.MouseClick += (sender2, e2) => AddSelectedRoom(sender2, e2, DR[0].ToString(), DR[1].ToString(), DR[2].ToString(), TOTAL);
                            LBL_ID.TextAlign = ContentAlignment.BottomCenter;
                            LBL_ID.BackColor = Color.DeepSkyBlue;

                            BTN.Controls.Add(LBL_ID);
                            BTN.Controls.Add(LBL_TYPE);
                            BTN.Controls.Add(LBL_CONDITION);
                            BTN.Controls.Add(LBL_PRICE);

                            BTN.MouseEnter += (sender2, e2) => BTN_MouseEnter1(sender2, e2);
                            BTN.MouseLeave += (sender2, e2) => BTN_MouseLeave2(sender2, e2, back);
                            flowLayoutPanel1.Controls.Add(BTN);

                        }
                        flowLayoutPanel1.Visible = true;
                    }
                }
            }
            catch (Exception EX)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), EX.Message, MessageAlertImage.Error());
                mdg.ShowDialog();
            }
        }
        private void BTN_MouseLeave2(object sender, EventArgs e, Color default_color)
        {
            Panel btn = (Panel)sender;
            btn.BackColor = default_color;
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
            double TAX = Convert.ToDouble(Settings.Default.tax_percentage);

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
                    LST.SubItems.Add(PRICE.ToString("F2"));
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

            string QRY3 = "SELECT id,service_name,service_price FROM additional_service_list";
            CLS_METHODS.FILL_COMBOBOX(CMB_ADDITIONAL_SERVICE, QRY3, "service_name", "id", -1);
            TXT_GUEST_NAME.Focus();
            CAL_AMOUNT();
        }

        private void shapedButton1_Click(object sender, EventArgs e)
        {

        }

        private void DTP_ARRIVAL_DATE_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                TXT_NO_OF_NIGHTS.Text = Convert.ToUInt32((DTP_DEPATURE_DATE.Value - DTP_ARRIVAL_DATE.Value).TotalDays).ToString();
                LOAD_AVAILABLE_ROOMS();
                CAL_AMOUNT();
            }
            catch (Exception)
            {
            }
        }

        private void DTP_DEPATURE_DATE_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                TXT_NO_OF_NIGHTS.Text = Convert.ToUInt32((DTP_DEPATURE_DATE.Value - DTP_ARRIVAL_DATE.Value).TotalDays).ToString();
                LOAD_AVAILABLE_ROOMS();
                CAL_AMOUNT();
            }
            catch (Exception)
            {
            }
        }
        private void LOAD_GUEST_LIST_TO_LIST()
        {
            try
            {
                using (MySqlDataAdapter DA = new MySqlDataAdapter("SELECT reservation.reservation_id,guest.first_name , guest.last_name,guest.is_temp,reservation.added_date ,guest.guest_id,reservation.tax_status FROM guest INNER JOIN reservation ON (guest.guest_id = reservation.guest_id) WHERE  reservation.status=@status AND (guest.first_name LIKE @NAME OR reservation.reservation_id LIKE @NAME OR reservation.added_date=@NAME OR reservation.added_date=@NAME) ", CONNECTION.CON))
                {
                    DA.SelectCommand.Parameters.Clear();
                    DA.SelectCommand.Parameters.AddWithValue("@NAME", "%" + TXT_GUEST_NAME.Text+"%");
                    DA.SelectCommand.Parameters.AddWithValue("@status", "PENDING");
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
                using (MySqlDataAdapter DA = new MySqlDataAdapter("SELECT reservation.reservation_id , guest.guest_id , reservation.no_of_rooms , reservation.no_of_adult , reservation.no_of_child , reservation.arrival_date , reservation.depature_Date , reservation.no_of_nights , IFNULL(agent.agent_id,-1) AS agent_id , agent.agent_name , reservation.additional_note , reservation.added_date , reservation.added_time , recerved_rooms.room_id , recerved_rooms.room_charge , room.room_name , room_packages.package_name , room_packages.description FROM reservation INNER JOIN recerved_rooms ON (reservation.reservation_id = recerved_rooms.reservation_no) LEFT JOIN agent ON (agent.agent_id = reservation.agent_id) INNER JOIN guest ON (reservation.guest_id = guest.guest_id) INNER JOIN room ON (room.room_id = recerved_rooms.room_id) INNER JOIN room_packages ON (room.room_package_id = room_packages.room_package_id) WHERE reservation.reservation_id=@reservation_id", CONNECTION.CON))
                {
                    DA.SelectCommand.Parameters.Clear();
                    DA.SelectCommand.Parameters.AddWithValue("@reservation_id", RESERVATION_NO);
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
                        TXT_NOTE.Text = DT.Rows[0].Field<string>("additional_note");

                        if (DT.Rows[0].Field<Int64>("agent_id").ToString() =="-1")
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
        String TAX_STATUS = String.Empty;


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
                TAX_STATUS= LST_GUEST_LIST.SelectedItems[0].SubItems[6].Text;
                LBL_RESERVED_DATE.Text = RECERVED_DATE;
                LBL_SELECTED_ID_NO.Text = GUEST_ID;
                LBL_SELECTED_FIRST_NAME.Text = FIRST_NAME;
                LBL_SELECTED_LAST_NAME.Text = LAST_NAME;
                LBL_SELECTED_RECERVATION_NO.Text = RESERVATION_NO;
                if(TAX_STATUS=="1")
                {
                    LBL_TAX_INFO.Text = "TAX INCLUDED";
                }
                else
                {
                    LBL_TAX_INFO.Text = "TAX NOT INCLUDED";
                }
               
                GET_RESERVED_NO_DATA();
                LOAD_AVAILABLE_ROOMS();
                TXT_GUEST_NAME.Text = RESERVATION_NO;
                LST_GUEST_LIST.Hide();
                TXT_NO_OF_ADULT.Focus();

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
        Double TAX = Settings.Default.tax_percentage;
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
                    if(TAX_STATUS=="1")
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
                CMB_EXTRA_SERVICE_ROOM.Items.Clear();

                foreach (ListViewItem LST in LST_SELECTED_ROOM.Items)
                {
                    DD = DD + Convert.ToDouble(LST.SubItems[3].Text);
                    CMB_EXTRA_SERVICE_ROOM.Items.Add(LST.SubItems[1].Text);
                }

                LBL_TOTAL_ROOM_PRICE.Text = (DD*Convert.ToDouble(TXT_NO_OF_NIGHTS.Text)).ToString("F2");
                LBL_TOAL_MEAL_PRICE_ADULT.Text = (A_FOOD * ADULTS* NO_OF_DAYS).ToString("F2");
                LBL_TOTAL_MEAL_CHILD.Text = (C_FOOD * CHILD* NO_OF_DAYS).ToString("F2");
                double AC = 0;

                foreach (ListViewItem LST in LST_ADDITINAL_SERVICE.Items)
                {
                    AC = AC + Convert.ToDouble(LST.SubItems[4].Text);
                }
                LBL_ADDITIONAL_SERVICE_CHARGE.Text=AC.ToString("F2");
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
            LBL_TAX_INFO.Text = "N/A";
            LBL_RESERVED_DATE.Text = "N/A";
            LBL_SELECTED_ID_NO.Text = "N/A"; ;
            LBL_SELECTED_FIRST_NAME.Text = "N/A";
            LBL_SELECTED_LAST_NAME.Text = "N/A"; 
            LBL_SELECTED_RECERVATION_NO.Text = "N/A";
            TAX_STATUS = String.Empty;
            TXT_NO_OF_ADULT.Text ="0";
            TXT_NO_OF_CHILD.Text = "0";
            TXT_NO_OF_ROOMS.Text = "0";
            DTP_ARRIVAL_DATE.Value = DateTime.Now;
            DTP_DEPATURE_DATE.Value = DateTime.Now;
            TXT_NOTE.Text = "N/A";
            CMB_AGENT.SelectedValue = -1;
            LST_GUEST_LIST.Items.Clear();
            LBL_ADDITIONAL_SERVICE_CHARGE.Text = "0.00";
            CMB_MEAL_TYPE.SelectedIndex = -1;
            CMB_EXTRA_SERVICE_ROOM.SelectedIndex = -1;
            LBL_TOTAL_ROOM_PRICE.Text = "0.00";
            LBL_TOAL_MEAL_PRICE_ADULT.Text = "0.00";
            LBL_TOTAL_MEAL_CHILD.Text = "0.00";
            LST_SELECTED_ROOM.Items.Clear();
            LBL_TOT_CHARGE_LKR.Text = "0.00";
            flowLayoutPanel1.Controls.Clear();
            LBL_TOT_CHARGE_USD.Text = "0.00";
            CMB_ADDITIONAL_SERVICE.SelectedIndex = -1;
            TXT_ADDITIONAL_QTY.Text = "0.00";
            LST_ADDITINAL_SERVICE.Items.Clear();
            //LBL_TOT_CHARGE_USD_WITH_TAX.Text = "0.00";
        }
        private void BTN_NEW_Click(object sender, EventArgs e)
        {
            DialogResult DS = MessageBox.Show("DO YOU WANT TO CLEAR THIS DATA" + Environment.NewLine + "ARE YOU SURE ?", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DS == DialogResult.Yes)
            {
                CLEAR();
                TXT_GUEST_NAME.Clear();
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
            }
        }

        private void LST_SELECTED_ROOM_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Delete)
                {
                    RemoveSelectedRoom();
                    LST_SELECTED_ROOM.Items.RemoveAt(LST_SELECTED_ROOM.SelectedItems[0].Index);
                    double totalPrice = 0;
                    foreach (ListViewItem lst in LST_SELECTED_ROOM.Items)
                    {
                        totalPrice = totalPrice + Convert.ToDouble(lst.SubItems[3].Text);
                    }
                    LBL_TOT_CHARGE_LKR.Text = (totalPrice * Settings.Default.exchange_rate_lrk).ToString("F2");
                    LBL_TOT_CHARGE_USD.Text = totalPrice.ToString("F2");
                    CAL_AMOUNT();
                    LOAD_AVAILABLE_ROOMS();
                }
            }
            catch (Exception)
            {
            }
        }

        private void CMB_MEAL_TYPE_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(CMB_MEAL_TYPE.SelectedIndex!=-1)
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
        private Double GET_ADDITIONAL_SERVICE_PRICE()
        {
            CONNECTION.open_connection();
            using (MySqlDataAdapter DA = new MySqlDataAdapter("SELECT service_price FROM additional_service_list WHERE id=@id", CONNECTION.CON))
            {
                DA.SelectCommand.Parameters.Clear();
                DA.SelectCommand.Parameters.AddWithValue("@id", CMB_ADDITIONAL_SERVICE.SelectedValue);
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
        private string GET_ROOM_ID()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlCommand DA = new MySqlCommand("SELECT room_id FROM room WHERE room_name=@room_name", CONNECTION.CON))
                {
                    DA.Parameters.Clear();
                    DA.Parameters.AddWithValue("@room_name", CMB_EXTRA_SERVICE_ROOM.Text);
                    return DA.ExecuteScalar().ToString();
                }
            }
            catch (Exception)
            {
                return null;
            }

        }
        private void BTN_MAKE_NEW_AGENT_Click(object sender, EventArgs e)
        {
            double TAX = Convert.ToDouble(Settings.Default.tax_percentage);
            if(CMB_ADDITIONAL_SERVICE.SelectedIndex!=-1 && Convert.ToDouble(TXT_ADDITIONAL_QTY.Text)>0)
            {
                ListViewItem LST = new ListViewItem(CMB_ADDITIONAL_SERVICE.SelectedValue.ToString());
                {
                    double TOTAL=0;
                    if (TAX_STATUS=="1")
                    {
                        TOTAL = (GET_ADDITIONAL_SERVICE_PRICE() * Convert.ToDouble(TXT_ADDITIONAL_QTY.Text)) + ((GET_ADDITIONAL_SERVICE_PRICE() * Convert.ToDouble(TXT_ADDITIONAL_QTY.Text)) * TAX / 100);
                    }
                    else
                    {
                        TOTAL = GET_ADDITIONAL_SERVICE_PRICE() * Convert.ToDouble(TXT_ADDITIONAL_QTY.Text);
                    }
                    

                    LST.SubItems.Add(CMB_ADDITIONAL_SERVICE.Text);
                    LST.SubItems.Add(CMB_EXTRA_SERVICE_ROOM.Text);
                    LST.SubItems.Add(Convert.ToDouble( TXT_ADDITIONAL_QTY.Text).ToString("F3"));
                    LST.SubItems.Add(TOTAL.ToString("F2"));
                    LST.SubItems.Add(GET_ROOM_ID());
                }
                LST_ADDITINAL_SERVICE.Items.Add(LST);
                CMB_ADDITIONAL_SERVICE.SelectedIndex = -1;
                TXT_ADDITIONAL_QTY.Text = "0.000";
                CMB_ADDITIONAL_SERVICE.Focus();
                CAL_AMOUNT();
            }
            else if (Convert.ToDouble(TXT_ADDITIONAL_QTY.Text) == 0)
            {
                TXT_ADDITIONAL_QTY.Focus();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), "PLEASE ENTER ADDITIONAL QTY!", MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
            else
            {
                CMB_ADDITIONAL_SERVICE.Focus();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), "PLEASE SELECT ADDITIONAL SERVICE!", MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
        }

        private void LST_ADDITINAL_SERVICE_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Delete)
                {
                    LST_ADDITINAL_SERVICE.Items.RemoveAt(LST_ADDITINAL_SERVICE.SelectedItems[0].Index);
                    CAL_AMOUNT();
                }
            }
            catch (Exception)
            {
            }
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
            flowLayoutPanel1.Enabled = false;
            groupBox2.Enabled = false;
            groupBox3.Enabled = false;
            BTN_SAVE.Enabled = false;
            LST_ADDITINAL_SERVICE.Enabled = false;
            LST_GUEST_LIST.Enabled = false;
            LST_SELECTED_ROOM.Enabled = false;
            TXT_ADDITIONAL_QTY.Enabled = false;
            BTN_SAVE.Enabled = false;
            BTN_NEW.Enabled = false;
            TXT_GUEST_NAME.Enabled = false;
            TXT_NOTE.Enabled = false;
            TXT_NO_OF_ADULT.Enabled = false;
            TXT_NO_OF_CHILD.Enabled = false;
            BTN_MAKE_NEW_AGENT.Enabled = false;
            CMB_ADDITIONAL_SERVICE.Enabled = false;
            CMB_AGENT.Enabled = false; ;
            CMB_MEAL_TYPE.Enabled = false;

        }
        private void UNLOCK_CONTROLLERS()
        {
            flowLayoutPanel1.Enabled = true;
            groupBox2.Enabled = true;
            groupBox3.Enabled = true;
            BTN_SAVE.Enabled = true;
            BTN_SAVE.Enabled = true;
            LST_ADDITINAL_SERVICE.Enabled = true;
            LST_GUEST_LIST.Enabled = true;
            LST_SELECTED_ROOM.Enabled = true;
            TXT_ADDITIONAL_QTY.Enabled = true;
            BTN_SAVE.Enabled = true;
            BTN_NEW.Enabled = true;
            TXT_GUEST_NAME.Enabled = true;
            TXT_NOTE.Enabled = true;
            TXT_NO_OF_ADULT.Enabled = true;
            TXT_NO_OF_CHILD.Enabled = true;
            TXT_NO_OF_NIGHTS.Enabled = true;
            TXT_NO_OF_ROOMS.Enabled = true;
            BTN_MAKE_NEW_AGENT.Enabled = true;
            CMB_ADDITIONAL_SERVICE.Enabled = true;
            CMB_AGENT.Enabled = true; ;
            CMB_MEAL_TYPE.Enabled = true;
        }

        MySqlCommand cmd = CONNECTION.CON.CreateCommand();
        MySqlTransaction myTrans;
        private void SAVE_CHECKED_IN()
        {
            try
            {

                Cursor.Current = Cursors.WaitCursor;
                LOCK_CONTROLLERS();
                CONNECTION.open_connection();
                myTrans = CONNECTION.CON.BeginTransaction();
                cmd.Connection = CONNECTION.CON;
                cmd.Transaction = myTrans;

                cmd.CommandText = "UPDATE reservation SET no_of_rooms=@no_of_rooms,no_of_adult=@no_of_adult,no_of_child=@no_of_child,arrival_date=@arrival_date,depature_Date=@depature_Date,no_of_nights=@no_of_nights,agent_id=@agent_id,additional_note=@additional_note,STATUS=@status,meal_type_id=@meal_type_id WHERE reservation_id=@reservation_id";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@reservation_id", LBL_SELECTED_RECERVATION_NO.Text);
                cmd.Parameters.AddWithValue("@no_of_rooms", Convert.ToInt32(TXT_NO_OF_ROOMS.Text));
                cmd.Parameters.AddWithValue("@no_of_adult", Convert.ToInt32(TXT_NO_OF_ADULT.Text));
                cmd.Parameters.AddWithValue("@no_of_child", Convert.ToInt32(TXT_NO_OF_CHILD.Text));
                cmd.Parameters.AddWithValue("@arrival_date", DTP_ARRIVAL_DATE.Value.ToShortDateString());
                cmd.Parameters.AddWithValue("@depature_Date", DTP_DEPATURE_DATE.Value.ToShortDateString());
                cmd.Parameters.AddWithValue("@no_of_nights", Convert.ToInt32(TXT_NO_OF_NIGHTS.Text));
                cmd.Parameters.AddWithValue("@agent_id", CMB_AGENT.SelectedValue);
                cmd.Parameters.AddWithValue("@additional_note", TXT_NOTE.Text);
                cmd.Parameters.AddWithValue("@status", "CHECKED IN");
                cmd.Parameters.AddWithValue("@meal_type_id", Convert.ToInt32(CMB_MEAL_TYPE.SelectedValue));
                cmd.ExecuteNonQuery();

                cmd.CommandText = "DELETE FROM recerved_rooms WHERE reservation_no=@reservation_no";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@reservation_no", LBL_SELECTED_RECERVATION_NO.Text);
                cmd.ExecuteNonQuery();

                foreach (ListViewItem dgvr in LST_SELECTED_ROOM.Items)
                {
                    cmd.CommandText = "INSERT INTO recerved_rooms(reservation_no,room_id,room_charge) VALUES(@reservation_no,@room_id,@room_charge)";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@reservation_no", LBL_SELECTED_RECERVATION_NO.Text);
                    cmd.Parameters.AddWithValue("@room_id", Convert.ToInt32(dgvr.SubItems[0].Text));
                    cmd.Parameters.AddWithValue("@room_charge", Convert.ToDouble(dgvr.SubItems[3].Text));
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "UPDATE room SET current_status=@current_status WHERE room_id=@room_id";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@room_id", Convert.ToInt32(dgvr.SubItems[0].Text));
                    cmd.Parameters.AddWithValue("@current_status", "CHECKED IN");
                    cmd.ExecuteNonQuery();
                }

                cmd.CommandText = "DELETE FROM additional_service WHERE reservation_id=@reservation_id";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@reservation_id", LBL_SELECTED_RECERVATION_NO.Text);
                cmd.ExecuteNonQuery();

                foreach (ListViewItem dgvr in LST_ADDITINAL_SERVICE.Items)
                {
                    cmd.CommandText = "INSERT INTO additional_service ( reservation_id, additional_serivice_id,additional_serivice_qty, additional_serivice_price,room_id ) VALUES ( @reservation_id, @additional_serivice_id,@additional_serivice_qty, @additional_serivice_price,@room_id )";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@reservation_id", LBL_SELECTED_RECERVATION_NO.Text);
                    cmd.Parameters.AddWithValue("@additional_serivice_id", Convert.ToInt32(dgvr.SubItems[0].Text));
                    cmd.Parameters.AddWithValue("@additional_serivice_qty", Convert.ToDouble(dgvr.SubItems[3].Text));
                    cmd.Parameters.AddWithValue("@additional_serivice_price", Convert.ToDouble(dgvr.SubItems[4].Text));
                    cmd.Parameters.AddWithValue("@room_id", dgvr.SubItems[5].Text);
                    cmd.ExecuteNonQuery();
                }

                cmd.CommandText = "INSERT INTO recervation_price ( reservation_no, total_room_charges, total_adult_food_charges, total_child_food_charges, additional_service_charge, tax_pecentage, sub_toal_with_tax, agent_commitions ) VALUES ( @reservation_no, @total_room_charges, @total_adult_food_charges, @total_child_food_charges, @additional_service_charge, @tax_pecentage, @sub_toal_with_tax, @agent_commitions)";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@reservation_no", LBL_SELECTED_RECERVATION_NO.Text);
                cmd.Parameters.AddWithValue("@total_room_charges", Convert.ToDouble(LBL_TOTAL_ROOM_PRICE.Text));
                cmd.Parameters.AddWithValue("@total_adult_food_charges", Convert.ToDouble(LBL_TOAL_MEAL_PRICE_ADULT.Text));
                cmd.Parameters.AddWithValue("@total_child_food_charges", Convert.ToDouble(LBL_TOTAL_MEAL_CHILD.Text));
                cmd.Parameters.AddWithValue("@additional_service_charge", Convert.ToDouble(LBL_ADDITIONAL_SERVICE_CHARGE.Text));
                cmd.Parameters.AddWithValue("@tax_pecentage", Settings.Default.tax_percentage);
                cmd.Parameters.AddWithValue("@sub_toal_with_tax", Convert.ToDouble(LBL_TOT_CHARGE_USD.Text));
                cmd.Parameters.AddWithValue("@agent_commitions", 0);
                cmd.ExecuteNonQuery();
                myTrans.Commit();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "SELECTED RESERVATION  SUCCESSFULLY CHECKED IN!", MessageAlertImage.Success());
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

        private void TXT_NO_OF_ADULT_TextChanged(object sender, EventArgs e)
        {
            CAL_AMOUNT();
        }

        private void TXT_NO_OF_CHILD_TextChanged(object sender, EventArgs e)
        {
            CAL_AMOUNT();
        }

        private void LBL_TOTAL_ROOM_PRICE_TextChanged(object sender, EventArgs e)
        {
        }

        private void BTN_SAVE_Click(object sender, EventArgs e)
        {
            if(CMB_MEAL_TYPE.SelectedIndex==-1)
            {
                CMB_MEAL_TYPE.Focus();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), "PLEASE SELECT MEAL TYPE!", MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
            else
            {
                SAVE_CHECKED_IN();
                BTN_SAVE.Enabled = false;
                BTN_NEW.Focus();
                BTN_NEW.PerformClick();
            }
           
        }

        private void TXT_NO_OF_ADULT_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                TXT_NO_OF_CHILD.Focus();
            }
        }

        private void TXT_NO_OF_CHILD_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DTP_ARRIVAL_DATE.Focus();
            }
        }

        private void TXT_NO_OF_ROOMS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DTP_ARRIVAL_DATE.Focus();
            }
        }

        private void DTP_ARRIVAL_DATE_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DTP_DEPATURE_DATE.Focus();
            }
        }

        private void DTP_DEPATURE_DATE_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CMB_MEAL_TYPE.Focus();
            }
        }

        private void CMB_MEAL_TYPE_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CMB_ADDITIONAL_SERVICE.Focus();
            }
        }

        private void CMB_ADDITIONAL_SERVICE_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CMB_EXTRA_SERVICE_ROOM.Focus();
            }
        }

        private void TXT_ADDITIONAL_QTY_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BTN_MAKE_NEW_AGENT.Focus();
            }
        }

        private void CHECKED_IN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Add)
            {
                BTN_SAVE.Focus();
            }
        }

        private void CMB_EXTRA_SERVICE_ROOM_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TXT_ADDITIONAL_QTY.Focus();
            }
           
        }

        private void CMB_AGENT_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void CMB_ADDITIONAL_SERVICE_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
