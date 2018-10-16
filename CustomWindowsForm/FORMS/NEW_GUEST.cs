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
    public partial class NEW_GUEST : Form
    {
        public NEW_GUEST()
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
        private void SAVE_GUEST()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlCommand da = new MySqlCommand("INSERT INTO guest ( guest_id, id_no, first_name, last_name, mobile_no, gender, passport_no, address, email, country_id ) VALUES ( @guest_id, @id_no, @first_name, @last_name, @mobile_no, @gender, @passport_no, @address, @email, @country_id )", CONNECTION.CON))
                {
                    da.Parameters.Clear();
                    da.Parameters.AddWithValue("@guest_id", CLS_GENERATE_ID.GEN_NEXT_GUEST_NO());
                    da.Parameters.AddWithValue("@id_no", TXT_ID_NUMBER.Text);
                    da.Parameters.AddWithValue("@first_name", TXT_FNAME.Text);
                    da.Parameters.AddWithValue("@last_name", TXT_LNAME.Text);
                    da.Parameters.AddWithValue("@mobile_no", TXT_MOBILENO.Text);
                    da.Parameters.AddWithValue("@gender", CMB_GENDER.Text);
                    da.Parameters.AddWithValue("@passport_no", TXT_PASSPORT.Text);
                    da.Parameters.AddWithValue("@address", TXT_ADDRESS.Text);
                    da.Parameters.AddWithValue("@email", TXT_EMAIL.Text);
                    da.Parameters.AddWithValue("@country_id", CMB_COUNTRY.SelectedValue);
                    if (da.ExecuteNonQuery() > 0)
                    {
                        LBL_GUEST.Text = CLS_METHODS.GET_MAX_STRING_ID("SELECT MAX(guest_id) FROM guest");
                        MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "GUEST ADDED SUCCESFULLY!", MessageAlertImage.Success());
                        mdg.ShowDialog();

                    }
                    else
                    {
                        MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), "GUEST ADDED FAILED!", MessageAlertImage.Alert());
                        mdg.ShowDialog();
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
        private void CLEAR_ALL()
        {
            LBL_GUEST.Text = "N/A";
            TXT_ID_NUMBER.Clear();
            TXT_FNAME.Clear();
            TXT_LNAME.Clear();
            CMB_GENDER.SelectedIndex = -1;
            CMB_COUNTRY.SelectedIndex = -1;
            TXT_MOBILENO.Text = "0";
            TXT_PASSPORT.Clear();
            TXT_ADDRESS.Clear();
            TXT_EMAIL.Clear();
        }
        private void BTN_NEW_Click(object sender, EventArgs e)
        {
           DialogResult DS= MessageBox.Show("DO YOU WANT TO CLEAR THIS DATA"+Environment.NewLine+"AYE YOU SURE","ALERT",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if(DS==DialogResult.Yes)
            {
                CLEAR_ALL();
            }
           
        }
        
        private void BTN_SAVE_Click(object sender, EventArgs e)
        {

             if (TXT_ID_NUMBER.Text.Length == 0)
            {
                TXT_ID_NUMBER.Focus();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), "PLEASE ENTER ID NO!", MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
            else if(TXT_FNAME.Text.Length==0)
            {
                TXT_FNAME.Focus();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), "PLEASE ENTER FIRST NAME!", MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
            else if (TXT_LNAME.Text.Length == 0)
            {
                TXT_LNAME.Focus();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), "PLEASE ENTER LAST NAME!", MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
            else if (CMB_GENDER.SelectedIndex == -1)
            {
                CMB_GENDER.Focus();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), "PLEASE SELECT GENDER TYPE!", MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
            else if (CMB_COUNTRY.SelectedIndex == -1)
            {
                CMB_COUNTRY.Focus();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), "PLEASE SELECT COUNTRY!", MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
            else
            {
                groupBox2.Enabled = false;
                SAVE_GUEST();
                groupBox2.Enabled = true;
                BTN_NEW.Focus();
            }
        }

        private void NEW_GUEST_Load(object sender, EventArgs e)
        {
            String QRY = "SELECT id,country_name FROM country";
            CLS_METHODS.FILL_COMBOBOX(CMB_COUNTRY,QRY, "country_name", "id",-1);
            TXT_ID_NUMBER.Focus();
        }

        private void TXT_ID_NUMBER_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                TXT_FNAME.Focus();
                CHECK_ID_IS_AVAILABLE();
            }
        }

        private void TXT_FNAME_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TXT_LNAME.Focus();
            }
        }

        private void TXT_LNAME_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CMB_GENDER.Focus();
            }
        }

        private void CMB_GENDER_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CMB_COUNTRY.Focus();
            }
        }

        private void CMB_COUNTRY_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TXT_MOBILENO.Focus();
            }
        }

        private void TXT_MOBILENO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TXT_PASSPORT.Focus();
            }
        }

        private void TXT_PASSPORT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TXT_ADDRESS.Focus();
            }
        }

        private void TXT_ADDRESS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TXT_EMAIL.Focus();
            }
        }

        private void TXT_EMAIL_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BTN_SAVE.Focus();
            }
        }
        private void CHECK_ID_IS_AVAILABLE()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter da = new MySqlDataAdapter("SELECT guest_id, id_no, first_name, last_name, mobile_no, gender, passport_no, address, email, country_id FROM guest WHERE id_no=@id_no", CONNECTION.CON))
                {
                    da.SelectCommand.Parameters.Clear();
                    da.SelectCommand.Parameters.AddWithValue("@id_no", TXT_ID_NUMBER.Text);
                    DataTable DT = new DataTable();
                    da.Fill(DT);
                    if(DT.Rows.Count>0)
                    {
                        String guest_id = DT.Rows[0][0].ToString();
                        String id_no = DT.Rows[0][1].ToString();
                        String first_name = DT.Rows[0][2].ToString();
                        String last_name = DT.Rows[0][3].ToString();
                        String mobile_no = DT.Rows[0][4].ToString();
                        String gender = DT.Rows[0][5].ToString();
                        String passport_no = DT.Rows[0][6].ToString();
                        MessageBox.Show("THIS GUEST ALREADY REGISTERED"+Environment.NewLine+"GUEST ID: "+guest_id + Environment.NewLine +"NIC: "+id_no+Environment.NewLine +"FIRST NAME: "+first_name+Environment.NewLine +"LAST NAME: " + last_name + Environment.NewLine +"MOBILE NO: "+mobile_no, "ALERT",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), ex.Message, MessageAlertImage.Error());
                mdg.ShowDialog();
            }
        }
        private void TXT_ID_NUMBER_Leave(object sender, EventArgs e)
        {
           
        }
    }
}
