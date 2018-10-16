using CustomWindowsForm.CLASS;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HYFLEX_HMS.Properties;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CustomWindowsForm.FORMS
{
    public partial class RESERVATION : Form
    {
        public RESERVATION()
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
            //if (isWindowMaximized)
            //{
            //    this.Location = _normalWindowLocation;
            //    this.Size = _normalWindowSize;
            //    toolTip1.SetToolTip(_MaxButton, "Maximize");
            //    _MaxButton.CFormState = MinMaxButton.CustomFormState.Normal;
            //    isWindowMaximized = false;
            //}
            //else
            //{
            //    _normalWindowSize = this.Size;
            //    _normalWindowLocation = this.Location;

            //    Rectangle rect = Screen.PrimaryScreen.WorkingArea;
            //    this.Location = new Point(0, 0);
            //    this.Size = new System.Drawing.Size(rect.Width, rect.Height);
            //    toolTip1.SetToolTip(_MaxButton, "Restore Down");
            //    _MaxButton.CFormState = MinMaxButton.CustomFormState.Maximize;
            //    isWindowMaximized = true;
            //}
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
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
        //private void LOAD_AVAILABLE_ROOMS()
        //{
        //    try
        //    {
        //        CONNECTION.open_connection();
        //        using (MySqlDataAdapter da = new MySqlDataAdapter("SELECT id, room_name, room_condition FROM room WHERE current_status = 'AVAILABLE'", CONNECTION.CON))
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

       
        private void LOAD_AVAILABLE_ROOMS()
        {
            double TAX = CLS_TAX.GetTotalTaxPercentage();
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter da = new MySqlDataAdapter("SELECT room_id, room_name, room_packages.package_name, room_packages.description, room_packages.condition, room_packages.room_package_price, room_packages.package_color FROM room INNER JOIN room_packages ON ( room.room_package_id = room_packages.room_package_id ) WHERE room_id NOT IN (SELECT room.room_id FROM room INNER JOIN room_packages ON ( room.room_package_id = room_packages.room_package_id ) INNER JOIN recerved_rooms ON ( recerved_rooms.room_id = room.room_id ) INNER JOIN reservation ON ( reservation.reservation_id = recerved_rooms.reservation_no ) WHERE ( reservation.arrival_date BETWEEN @frmdate AND @todate ) OR ( reservation.depature_Date > @frmdate AND reservation.depature_Date <= @todate ) OR( reservation.arrival_date < @frmdate AND reservation.`depature_Date` >= @todate ) )", CONNECTION.CON))
                {
                    da.SelectCommand.Parameters.Clear();
                    da.SelectCommand.Parameters.AddWithValue("@frmdate", DTP_ARRIVAL_DATE.Value.ToShortDateString());
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
                            if (CMB_TAX.SelectedIndex==0)
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
                            LBL_TYPE.Location = new Point(2,2);
                            LBL_TYPE.MouseClick += (sender2, e2) => AddSelectedRoom(sender2, e2, DR[0].ToString(), DR[1].ToString(), DR[2].ToString(), TOTAL);
                            LBL_TYPE.TextAlign=ContentAlignment.BottomCenter;

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
                            LBL_PRICE.Text = "LKR: "+ TOTAL.ToString("F2");
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

        //private void LOAD_GUESTS()
        //{
        //    try
        //    {
        //        CONNECTION.open_connection();
        //        using (MySqlDataAdapter da = new MySqlDataAdapter("SELECT id_no,first_name,last_name,mobile_no,gender,passport_no,address,email,country.country_name FROM guest INNER JOIN country ON (country.id=guest.country_id) WHERE id_no=@id_no", CONNECTION.CON))
        //        {
        //            da.SelectCommand.Parameters.Clear();
        //            da.SelectCommand.Parameters.AddWithValue("@id_no", TXT_NIC_NO.Text);
        //            DataTable DT = new DataTable();
        //            da.Fill(DT);
        //            if (DT.Rows.Count > 0)
        //            {
        //                TXT_NIC_NO.DA
        //            }
        //        }
        //    }
        //    catch (Exception EX)
        //    {
        //        MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), EX.Message, MessageAlertImage.Error());
        //        mdg.ShowDialog();
        //    }
        //}
        private void LBL_TYPE_MouseEnter(object sender, EventArgs e)
        {
           
        }

        private void AddSelectedRoom(object sender, EventArgs e, string room_id,string room_name,string description ,double PRICE)
        {
            bool ADD = false;
            double totalPrice = 0;
            double TAX = CLS_TAX.GetTotalTaxPercentage();
            foreach (ListViewItem F in LST_SELECTED_ROOM.Items)
            {
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

            foreach (ListViewItem F in LST_SELECTED_ROOM.Items)
            {
                totalPrice = totalPrice + Convert.ToDouble(F.SubItems[3].Text);
            }

            TXT_NO_OF_ROOMS.Text = LST_SELECTED_ROOM.Items.Count.ToString();
            LBL_TOT_CHARGE_LKR.Text = (totalPrice* HYFLEX_HMS.Properties.Settings.Default.exchange_rate_lrk).ToString("F2");
            LBL_TOT_CHARGE_USD.Text = (totalPrice*Convert.ToDouble(TXT_NO_OF_NIGHTS.Text)).ToString("F2");
        }

        private void BTN_MouseLeave2(object sender, EventArgs e,Color default_color)
        {
            Panel btn = (Panel)sender;
            btn.BackColor = default_color;
        }

        private void BTN_MouseEnter1(object sender, EventArgs e)
        {
            Panel btn = (Panel)sender;
            btn.BackColor = Color.Red;
        }

        private void LBL_ID_MouseLeave(object sender, EventArgs e)
        {
            foreach (Control c in ((Control)flowLayoutPanel1).Controls)
            {
                if (c is Panel)
                {
                    c.BackColor = Color.White;
                    break;
                }
            }

        }

      
        private void BTN_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = Color.Gray;
        }

        private void BTN_MouseEnter(object sender, EventArgs e)
        {
            this.BackColor = Color.Pink;
        }

        private void CHECKED_IN_Load(object sender, EventArgs e)
        {
            LOAD_AVAILABLE_ROOMS();
            CMB_TAX.SelectedIndex = 0;
            string QRY2 = "SELECT agent_id, agent_name FROM agent";
            CLS_METHODS.FILL_COMBOBOX(CMB_AGENT, QRY2, "agent_name", "agent_id", -1);
            TXT_NIC_NO.Focus();
            flowLayoutPanel1.Controls.Clear();
        }

        private void shapedButton1_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }
        private void CHECK_TYPE()
        {
            if (CMB_RESERVE_BY.SelectedIndex == 3)
            {
                LBL_AGENT_NAME.Enabled = true;
                CMB_AGENT.Enabled = true;
                BTN_MAKE_NEW_AGENT.Enabled = true;
            }
            else
            {
                LBL_AGENT_NAME.Enabled = false;
                CMB_AGENT.Enabled = false;
                BTN_MAKE_NEW_AGENT.Enabled = false;
            }
        }
        private void hyflexComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            CHECK_TYPE();
        }
        String TEMP_GUEST_ID;
        private String CHECK_GUEST_DATA()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter da = new MySqlDataAdapter("SELECT id_no,first_name,last_name,guest_id FROM guest WHERE id_no=@id_no", CONNECTION.CON))
                {
                    da.SelectCommand.Parameters.Clear();
                    da.SelectCommand.Parameters.AddWithValue("@id_no", TXT_NIC_NO.Text);
                    DataTable DT = new DataTable();
                    da.Fill(DT);
                    if(DT.Rows.Count>0)
                    {
                        return DT.Rows[0][3].ToString();
                    }
                    else
                    {
                        return "N";
                    }

                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            finally
            {
                CONNECTION.close_connection();
            }
        }
        private void LOCK_CONTROLLERS()
        {
            BTN_SAVE.Enabled = false;
            LBL_RESERVATION_ID.Enabled = false;
            DTP_RESERVATION_DATE.Enabled = false;
            TXT_F_NAME.Enabled = false;
            TXT_L_NAME.Enabled = false;
            TXT_EMAIL.Enabled = false;
            TXT_TEL.Enabled = false;
            TXT_NO_OF_ROOMS.Enabled = false;
            TXT_ADULT.Enabled = false;
            TXT_CHILD.Enabled = false;
           // CMB_PACKAGE.Enabled = false;
            DTP_ARRIVAL_DATE.Enabled = false;
            DTP_DEPATURE_DATE.Enabled = false;
            TXT_NO_OF_NIGHTS.Enabled = false;
            CMB_RESERVE_BY.Enabled = false;
            CMB_AGENT.Enabled = false;
            LBL_AGENT_NAME.Enabled = false;
            LBL_RESERVATION_ID.Enabled = false;
            BTN_MAKE_NEW_AGENT.Enabled = false;
            groupBox2.Enabled = false;
            flowLayoutPanel1.Enabled = false;
            LST_SELECTED_ROOM.Enabled = false;
        }
        private void UNLOCK_CONTROLLERS()
        {
            LBL_RESERVATION_ID.Enabled = true;
            DTP_RESERVATION_DATE.Enabled = true;
            TXT_F_NAME.Enabled = true;
            TXT_L_NAME.Enabled = true;
            TXT_EMAIL.Enabled = true;
            TXT_TEL.Enabled = true;
            TXT_NO_OF_ROOMS.Enabled = true;
            TXT_ADULT.Enabled = true;
            TXT_CHILD.Enabled = true;
         //   CMB_PACKAGE.Enabled = true;
            DTP_ARRIVAL_DATE.Enabled = true;
            DTP_DEPATURE_DATE.Enabled = true;
            TXT_NO_OF_NIGHTS.Enabled = true;
            CMB_RESERVE_BY.Enabled = true;
            CMB_AGENT.Enabled = true;
            LBL_AGENT_NAME.Enabled = true;
            LBL_RESERVATION_ID.Enabled = true;
            BTN_MAKE_NEW_AGENT.Enabled = true;
            groupBox2.Enabled = true;
            flowLayoutPanel1.Enabled = true;
            LST_SELECTED_ROOM.Enabled = true;
        }
        MySqlCommand cmd = CONNECTION.CON.CreateCommand();
        MySqlTransaction myTrans;
        private void SAVE_RESERVATION()
        {
            try
            {

             Cursor.Current = Cursors.WaitCursor;
            LOCK_CONTROLLERS();
            CONNECTION.open_connection();
            myTrans = CONNECTION.CON.BeginTransaction();
            cmd.Connection = CONNECTION.CON;
            cmd.Transaction = myTrans;
            string GUESTID = string.Empty;
            string RES = CHECK_GUEST_DATA();
            string RESERVATION_ID = CLS_GENERATE_ID.GEN_NEXT_RESERVATION_NO();
            if (RES!="N")
            {
                    GUESTID = RES;
            }
            else
            {
                GUESTID = CLS_GENERATE_ID.GEN_NEXT_GUEST_NO();
                cmd.CommandText = "INSERT INTO guest ( guest_id, id_no, first_name, last_name, mobile_no, gender, passport_no, address, email, country_id ) VALUES ( @guest_id, @id_no, @first_name, @last_name, @mobile_no, @gender, @passport_no, @address, @email, @country_id )";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@guest_id", GUESTID);
                cmd.Parameters.AddWithValue("@id_no", TXT_NIC_NO.Text);
                cmd.Parameters.AddWithValue("@first_name", TXT_F_NAME.Text);
                cmd.Parameters.AddWithValue("@last_name", TXT_L_NAME.Text);
                cmd.Parameters.AddWithValue("@mobile_no", TXT_TEL.Text);
                cmd.Parameters.AddWithValue("@gender", "MALE");
                cmd.Parameters.AddWithValue("@passport_no", "N/A");
                cmd.Parameters.AddWithValue("@address", "N/A");
                cmd.Parameters.AddWithValue("@email", TXT_EMAIL.Text);
                cmd.Parameters.AddWithValue("@country_id", 199);
                cmd.ExecuteNonQuery();
            }

                TEMP_GUEST_ID = CLS_METHODS.GET_MAX_STRING_ID("SELECT MAX(guest_id) FROM guest");

                cmd.CommandText = "INSERT INTO reservation ( reservation_id, guest_id, no_of_adult, no_of_child, arrival_date, depature_Date, no_of_nights, reserved_by, agent_id, additional_note,added_date,added_time,added_by,no_of_rooms,tax_status ) VALUES ( @reservation_id, @guest_id, @no_of_adult, @no_of_child, @arrival_date, @depature_Date, @no_of_nights, @reserved_by, @agent_id, @additional_note,@added_date,CURTIME(),@added_by,@no_of_rooms,@tax_status )";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@reservation_id", RESERVATION_ID);
                cmd.Parameters.AddWithValue("@guest_id", GUESTID);
                cmd.Parameters.AddWithValue("@no_of_adult", Convert.ToInt16(TXT_ADULT.Text));
                cmd.Parameters.AddWithValue("@no_of_child", Convert.ToInt16(TXT_CHILD.Text));
                cmd.Parameters.AddWithValue("@arrival_date", DTP_ARRIVAL_DATE.Value.ToShortDateString());
                cmd.Parameters.AddWithValue("@depature_Date", DTP_DEPATURE_DATE.Value.ToShortDateString());
                cmd.Parameters.AddWithValue("@no_of_nights", (DTP_DEPATURE_DATE.Value - DTP_ARRIVAL_DATE.Value).TotalDays);
                cmd.Parameters.AddWithValue("@reserved_by", CMB_RESERVE_BY.Text);
                cmd.Parameters.AddWithValue("@no_of_rooms", Convert.ToInt16(TXT_NO_OF_ROOMS.Text));
                if (CMB_TAX.SelectedIndex == 0)
                {
                    cmd.Parameters.AddWithValue("@tax_status", "1");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@tax_status", "0");
                }
                   
                if (CMB_RESERVE_BY.SelectedIndex == 3)
                {
                    cmd.Parameters.AddWithValue("@agent_id", CMB_AGENT.SelectedValue);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@agent_id", "0");
                }

                cmd.Parameters.AddWithValue("@additional_note", TXT_NOTE.Text);
                cmd.Parameters.AddWithValue("@added_date", DateTime.Now.ToShortDateString());
                cmd.Parameters.AddWithValue("@added_by", CLS_CURRENT_LOGGER.LOGGED_IN_USERID);
                cmd.ExecuteNonQuery();

                foreach (ListViewItem lvi in LST_SELECTED_ROOM.Items)
                {
                    cmd.CommandText = "INSERT INTO recerved_rooms ( reservation_no, room_id, room_charge ) VALUES ( @reservation_no, @room_id, @room_charge)";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@reservation_no", RESERVATION_ID);
                    cmd.Parameters.AddWithValue("@room_id", Convert.ToInt32(lvi.SubItems[0].Text));
                    cmd.Parameters.AddWithValue("@room_charge", Convert.ToDouble(lvi.SubItems[3].Text));
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "UPDATE room SET current_status=@current_status WHERE room_id=@room_id";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@room_id", Convert.ToInt32(lvi.SubItems[0].Text));
                    cmd.Parameters.AddWithValue("@current_status", "RESERVED");
                    cmd.ExecuteNonQuery();
                }

                myTrans.Commit();
            
           
            LBL_RESERVATION_ID.Text = RESERVATION_ID;
            MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "RESERVATION ADDED SUCCESFULLY"+Environment.NewLine+Environment.NewLine + "RESERVATION NO IS : " +  RESERVATION_ID, MessageAlertImage.Success());
            mdg.ShowDialog();
            BTN_NEW.Focus();

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
                Cursor.Current = Cursors.Default;
                CONNECTION.close_connection();
            }

        }
        private void BTN_SAVE_Click(object sender, EventArgs e)
        {
            if(TXT_F_NAME.Text.Length==0)
            {
                TXT_F_NAME.Focus();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), "PLEASE ENTER GUEST NAME!", MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
            else if(TXT_TEL.Text.Length==0)
            {
                TXT_TEL.Focus();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), "PLEASE ENTER TELEPHONE NO!", MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
            else if (TXT_ADULT.Text.Length == 0)
            {
                TXT_ADULT.Focus();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), "PLEASE ENTER NO OF ADULTS!", MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
           
            else if (CMB_RESERVE_BY.SelectedIndex == -1)
            {
                CMB_RESERVE_BY.Focus();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), "PLEASE SELECT RESERVATION TYPE!", MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
            else if (CMB_RESERVE_BY.SelectedIndex == 3 && CMB_AGENT.SelectedIndex==-1)
            {
                CMB_RESERVE_BY.Focus();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), "PLEASE SELECT AGENT!", MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
            else if (LST_SELECTED_ROOM.Items.Count==0)
            {
                
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), "PLEASE SELECT ROOMS!", MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
            else if (Convert.ToInt16(TXT_NO_OF_NIGHTS.Text) == 0)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), "PLEASE SELECT SPEND DATES!", MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
            else if (CMB_TAX.SelectedIndex== -1)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), "PLEASE SELECT TAX TYPE!", MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
            else if (CMB_AGENT.SelectedIndex >-1 && CMB_AGENT.SelectedIndex==-1)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), "PLEASE SELECT AGENT!", MessageAlertImage.Alert());
                mdg.ShowDialog();
            }

            else
            {
                BTN_SAVE.Enabled = false;
                SAVE_RESERVATION();
                BTN_NEW.Focus();
            }
        }
        private void CLEAR_DATA()
        {
            LBL_RESERVATION_ID.Text="N/A";
            DTP_RESERVATION_DATE.Value = DateTime.Now;
            TXT_F_NAME.Clear();
            TXT_L_NAME.Clear();
            TXT_EMAIL.Clear();
            TXT_TEL.Clear();
            TXT_NO_OF_ROOMS.Text = "0";
            TXT_ADULT.Text = "0";
            TXT_CHILD.Text = "0";
            LST_SELECTED_ROOM.Items.Clear();
            TXT_NIC_NO.Clear();
            DTP_ARRIVAL_DATE.Value = DateTime.Now;
            DTP_DEPATURE_DATE.Value = DateTime.Now;
            TXT_NO_OF_NIGHTS.Text = "0";
            CMB_RESERVE_BY.SelectedIndex = -1;
            CMB_AGENT.SelectedIndex = -1;
            LBL_AGENT_NAME.Visible = false;
           // BTN_MAKE_NEW_AGENT.Visible = false;
            TXT_NOTE.Clear();
            TXT_F_NAME.Enabled = true;
            TXT_L_NAME.Enabled = true;
            TXT_TEL.Enabled = true;
            TXT_EMAIL.Enabled = true;
            LBL_SELECTED_ROOM_CONDITION.Text = "N/A";
            LBL_SELECTED_ROOM_PRICE.Text = "0.00";
            LBL_SELECTED_ROOM_DESCRIPTION.Text = "N/A";
            LBL_TOT_CHARGE_LKR.Text = "0.00";
            LBL_TOT_CHARGE_USD.Text = "0.00";
        }

        private void BTN_NEW_Click(object sender, EventArgs e)
        {
           // MessageBox.Show(Settings.Default.tax_percentage.ToString());
           DialogResult DS= MessageBox.Show("DO YOU WANT TO CLEAR THIS DATA"+Environment.NewLine+"ARE YOU SURE ?","ALERT",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if(DS==DialogResult.Yes)
            {
                CLEAR_DATA();
                UNLOCK_CONTROLLERS();
                BTN_SAVE.Enabled = true;
                TXT_NIC_NO.Focus();
            }
        }

        private void CMB_AGENT_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           // string QRY1 = "SELECT room_package_id, package_name FROM room_packages";
           // CLS_METHODS.FILL_COMBOBOX(CMB_PACKAGE, QRY1, "package_name", "room_package_id", -1);
        }

        private void BTN_MAKE_NEW_AGENT_Click(object sender, EventArgs e)
        {
            if (PNL_QUICK_ADD_AGENT.Visible==true)
            {
                PNL_QUICK_ADD_AGENT.Visible = false;
            }
            else
            {
                PNL_QUICK_ADD_AGENT.Visible = true;
                PNL_QUICK_ADD_AGENT.Size = new Size(300, 100);
                TXT_NEW_AGENT_NAME.Focus();
                TXT_NEW_AGENT_NAME.Clear();
            }

            string QRY2 = "SELECT agent_id, agent_name FROM agent";
            CLS_METHODS.FILL_COMBOBOX(CMB_AGENT, QRY2, "agent_name", "agent_id", -1);
        }

        private void CMB_RESERVE_BY_Leave(object sender, EventArgs e)
        {
            CHECK_TYPE();
        }

        private void DTP_ARRIVAL_DATE_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                TXT_NO_OF_NIGHTS.Text = Convert.ToUInt32((DTP_DEPATURE_DATE.Value - DTP_ARRIVAL_DATE.Value).TotalDays).ToString();
                LST_SELECTED_ROOM.Items.Clear();
                LOAD_AVAILABLE_ROOMS();
                calRoomChages();
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
                LST_SELECTED_ROOM.Items.Clear();
                LOAD_AVAILABLE_ROOMS();
                calRoomChages();
               
            }
            catch (Exception)
            {
            }
        }

        private void DTP_RESERVATION_DATE_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                TXT_NIC_NO.Focus();
            }
        }

        private void TXT_F_NAME_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TXT_L_NAME.Focus();
            }
        }

        private void TXT_L_NAME_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TXT_TEL.Focus();
            }
        }

        private void TXT_EMAIL_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TXT_ADULT.Focus();
            }
        }

        private void TXT_TEL_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TXT_EMAIL.Focus();
            }
        }

        private void TXT_NO_OF_ROOMS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TXT_ADULT.Focus();
            }
        }

        private void TXT_ADULT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TXT_CHILD.Focus();
            }
        }

        private void TXT_CHILD_KeyDown(object sender, KeyEventArgs e)
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
                CMB_RESERVE_BY.Focus();
            }
        }

        private void CMB_PACKAGE_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CMB_RESERVE_BY.Focus();
            }
        }

        private void CMB_RESERVE_BY_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (CMB_RESERVE_BY.SelectedIndex == 3)
                    CMB_AGENT.Focus();
                else
                    TXT_NOTE.Focus();
            }
        }

        private void CMB_AGENT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TXT_NOTE.Focus();
            }
        }

        private void TXT_NOTE_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                BTN_SAVE.Focus();
            }
        }


        //private void CAL_ROOM_CHARGES()
        //{
        //    try
        //    {
        //        CONNECTION.open_connection();
        //        using (MySqlDataAdapter DA = new MySqlDataAdapter("SELECT room_package_id,package_name,description,room_packages.condition,room_package_price FROM room_packages WHERE room_package_id=@room_package_id", CONNECTION.CON))
        //        {
        //            DA.SelectCommand.Parameters.Clear();
        //            DA.SelectCommand.Parameters.AddWithValue("@room_package_id",CMB_PACKAGE.SelectedValue);
        //            DataTable DT = new DataTable();
        //            DA.Fill(DT);
        //            if (DT.Rows.Count > 0)
        //            {
        //                double TOT_CHARGE = Convert.ToDouble(DT.Rows[0][4].ToString()) * Convert.ToDouble(TXT_NO_OF_ROOMS.Text) * Convert.ToDouble(TXT_NO_OF_NIGHTS.Text);
        //                LBL_SELECTED_ROOM_CONDITION.Text = DT.Rows[0][3].ToString();
        //                LBL_SELECTED_ROOM_PRICE.Text = Convert.ToDouble(DT.Rows[0][4]).ToString("F2");
        //                LBL_SELECTED_ROOM_DESCRIPTION.Text = DT.Rows[0][2].ToString();
        //                LBL_TOT_CHARGE_LKR.Text = TOT_CHARGE.ToString("F2");
        //                LBL_TOT_CHARGE_USD.Text = (TOT_CHARGE/Properties.Settings.Default.exchange_rate_lrk).ToString("F2");
        //            }
        //            else
        //            {
        //                LBL_TOT_CHARGE_LKR.Text = "0.00";
        //                LBL_TOT_CHARGE_USD.Text = "0.00";
        //            }
        //        }

        //    }
        //    catch (Exception EX)
        //    {

        //        MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), EX.Message, MessageAlertImage.Error());
        //        mdg.ShowDialog();
        //    }
        //}
        private void CMB_PACKAGE_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if(CMB_PACKAGE.SelectedIndex>=0)
            //{
            //    CAL_ROOM_CHARGES();
            //}
        }

        private void TXT_NO_OF_ROOMS_Leave(object sender, EventArgs e)
        {
            if(TXT_NO_OF_ROOMS.Text.Length==0)
            {
                TXT_NO_OF_ROOMS.Text = "0";
            }
        }
        private void SAVE_NEW_AGENT()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlCommand da = new MySqlCommand("INSERT INTO agent ( agent_name) VALUES ( @agent_name)", CONNECTION.CON))
                {
                    da.Parameters.Clear();
                    da.Parameters.AddWithValue("@agent_name", TXT_NEW_AGENT_NAME.Text);
                    if (da.ExecuteNonQuery() > 0)
                    {
                        string QRY2 = "SELECT agent_id, agent_name FROM agent";
                        CLS_METHODS.FILL_COMBOBOX(CMB_AGENT, QRY2, "agent_name", "agent_id", -1);
                        CMB_AGENT.Text = TXT_NEW_AGENT_NAME.Text;
                        PNL_QUICK_ADD_AGENT.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), ex.Message, MessageAlertImage.Error());
                mdg.ShowDialog();
            }
            finally
            {
                CONNECTION.close_connection();
            }
        }
        private void BTN_ADD_NEW_AGENT_Click(object sender, EventArgs e)
        {
            SAVE_NEW_AGENT();
        }

        private void TXT_NEW_AGENT_NAME_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                BTN_ADD_NEW_AGENT.Focus();
            }
        }

        private void TXT_NO_OF_ROOMS_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (CMB_PACKAGE.SelectedIndex >= 0)
            //{
            //    CAL_ROOM_CHARGES();
            //}
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            LOAD_AVAILABLE_ROOMS();
        }

        private void LST_SELECTED_ROOM_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                calRoomChages();
            }
        }

        private void calRoomChages()
        {
            try
            {

                LST_SELECTED_ROOM.Items.RemoveAt(LST_SELECTED_ROOM.SelectedItems[0].Index);
                double totalPrice = 0;
                foreach (ListViewItem lst in LST_SELECTED_ROOM.Items)
                {
                    totalPrice = totalPrice + Convert.ToDouble(lst.SubItems[3].Text);
                }
                LBL_TOT_CHARGE_LKR.Text = (totalPrice * Settings.Default.exchange_rate_lrk).ToString("F2");
                LBL_TOT_CHARGE_USD.Text = (totalPrice*Convert.ToDouble(TXT_NO_OF_NIGHTS.Text)).ToString("F2");

            }
            catch (Exception)
            {

            }
        }
        private void RESERVATION_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F10)
            {
                BTN_SAVE.Focus();
            }
            if (e.KeyCode == Keys.F5)
            {
                if( CMB_TAX.SelectedIndex==-1)
                {
                    CMB_TAX.SelectedIndex = 0;
                }
                else if (CMB_TAX.SelectedIndex == 0)
                {
                    CMB_TAX.SelectedIndex = 1;
                }
                else
                {
                    CMB_TAX.SelectedIndex = 0;
                }
            }
        }
        private void LOAD_GUESTS_TO_LISTVIEW()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter DA = new MySqlDataAdapter("SELECT id_no,first_name,last_name,mobile_no,gender,passport_no,address,email,country.country_name,guest_id FROM guest INNER JOIN country ON (country.id=guest.country_id) WHERE id_no LIKE @id_no", CONNECTION.CON))
                {
                    DA.SelectCommand.Parameters.Clear();
                    DA.SelectCommand.Parameters.AddWithValue("@id_no", "%" + TXT_NIC_NO.Text + "%");
                    DataTable DT = new DataTable();
                    DA.Fill(DT);
                    if (DT.Rows.Count > 0)
                    {
                        LST_GUEST_LIST.Items.Clear();
                        foreach (DataRow DR in DT.Rows)
                        {
                            ListViewItem LST = new ListViewItem(DR.Field<string>(0));
                            {
                                LST.SubItems.Add(DR.Field<string>(1));
                                LST.SubItems.Add(DR.Field<string>(2));
                                LST.SubItems.Add(DR.Field<string>(3));
                                LST.SubItems.Add(DR.Field<string>(4));
                                LST.SubItems.Add(DR.Field<string>(5));
                                LST.SubItems.Add(DR.Field<string>(6));
                                LST.SubItems.Add(DR.Field<string>(7));
                                LST.SubItems.Add(DR.Field<string>(8));
                                LST.SubItems.Add(DR.Field<string>(9));
                            }
                            LST_GUEST_LIST.Items.Add(LST);
                        }
                        LST_GUEST_LIST.Size = new Size(493, 221);
                        LST_GUEST_LIST.Show();
                    }
                    else
                    {
                        LST_GUEST_LIST.Hide();
                    }
                }
            }
            catch (Exception EX)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), EX.Message, MessageAlertImage.Error());
                mdg.ShowDialog();
            }
}
        private void TXT_NIC_NO_TextChanged(object sender, EventArgs e)
        {
            if(TXT_NIC_NO.Text.Length>1)
            {
                LOAD_GUESTS_TO_LISTVIEW();
            }
            else
            {
                LST_GUEST_LIST.Hide();
            }
        }
        private string GUEST_ID=string.Empty;
        private string GUEST_F_NAME = string.Empty;
        private string GUEST_L_NAME = string.Empty;
        private string MOBILE = string.Empty;
        private string GENDER = string.Empty;
        private string PASSPORT_NO = string.Empty;
        private string ADDRESSS = string.Empty;
        private string EMAIL = string.Empty;
        private string COUNTRY = string.Empty;

        private void LST_GUEST_LIST_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                if (LST_GUEST_LIST.Items[0].Selected == true)
                {
                    TXT_NIC_NO.Focus();
                }
            }
            if (e.KeyCode == Keys.Enter)
            {
             
                GUEST_F_NAME = LST_GUEST_LIST.SelectedItems[0].SubItems[1].Text;
                GUEST_L_NAME = LST_GUEST_LIST.SelectedItems[0].SubItems[2].Text;
                MOBILE = LST_GUEST_LIST.SelectedItems[0].SubItems[3].Text;
                GENDER = LST_GUEST_LIST.SelectedItems[0].SubItems[4].Text;
                PASSPORT_NO = LST_GUEST_LIST.SelectedItems[0].SubItems[5].Text;
                ADDRESSS = LST_GUEST_LIST.SelectedItems[0].SubItems[6].Text;
                EMAIL = LST_GUEST_LIST.SelectedItems[0].SubItems[7].Text;
                COUNTRY = LST_GUEST_LIST.SelectedItems[0].SubItems[8].Text;
                GUEST_ID = LST_GUEST_LIST.SelectedItems[0].SubItems[9].Text;
                TXT_NIC_NO.Text= LST_GUEST_LIST.SelectedItems[0].SubItems[0].Text;
                TXT_F_NAME.Text = GUEST_F_NAME;
                TXT_L_NAME.Text = GUEST_L_NAME;
                TXT_EMAIL.Text = EMAIL;
                TXT_TEL.Text = MOBILE;

                TXT_F_NAME.Enabled = false;
                TXT_L_NAME.Enabled = false;
                TXT_TEL.Enabled = false;
                TXT_EMAIL.Enabled = false;
                LST_GUEST_LIST.Hide();
                TXT_ADULT.Focus();
            }
        }

        private void TXT_NIC_NO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TXT_F_NAME.Focus();
            }
            if (e.KeyCode == Keys.Down)
            {
               if(LST_GUEST_LIST.Items.Count>0)
                {
                    LST_GUEST_LIST.Focus();
                    LST_GUEST_LIST.Items[0].Selected = true;
                }
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void TXT_F_NAME_Enter(object sender, EventArgs e)
        {
            LST_GUEST_LIST.Visible = false;
        }

        private void CMB_TAX_Leave(object sender, EventArgs e)
        {
            if(CMB_TAX.Text.Length==0)
            {
                CMB_TAX.SelectedIndex = 0;
            }
        }

        private void CMB_TAX_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LST_SELECTED_ROOM.Items.Clear();
                LOAD_AVAILABLE_ROOMS();
                calRoomChages();
            }
            catch (Exception)
            {

            }
          
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
