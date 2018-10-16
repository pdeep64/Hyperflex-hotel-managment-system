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
    public partial class SUPPLIER : Form
    {
        public SUPPLIER()
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
        private String GETMAXID()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter DA = new MySqlDataAdapter("SELECT MAX(supplier_id) FROM supplier", CONNECTION.CON))
                {
                    DataTable DT = new DataTable();
                    DA.Fill(DT);
                    if (DT.Rows.Count > 0)
                    {
                        return DT.Rows[0].Field<int>(0).ToString();
                    }
                    else
                    {
                        return "0";
                    }

                }

            }
            catch (Exception EX)
            {

                return EX.ToString();
            }
        }
        private void BTN_SAVE_Click(object sender, EventArgs e)
        {
            if(TXT_SUPPLIER_NAME.Text.Length==0)
            {
                TXT_SUPPLIER_NAME.Focus();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Warning(), "PLEASE ENTER SUPPLIER NAME", MessageAlertImage.Warning());
                mdg.ShowDialog();
            }
            else if (BTN_SAVE.Text == "SAVE")
            {
                SAVE_SUPPLIER();
                LOAD_SUPPLIER_lIST();
                BTN_SAVE.Text = "SAVE";
                BTN_NEW.Focus();
                clearAll();
            }
            else if (BTN_SAVE.Text == "UPDATE")
            {
                UPDATE_SUPPLIERS_DATA();
                LOAD_SUPPLIER_lIST();
                BTN_NEW.Focus();
                BTN_SAVE.Text = "SAVE";
                clearAll();
            }

        }
        private void LOAD_SUPPLIER_lIST()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter DA = new MySqlDataAdapter("SELECT supplier_id, supplier_name, company_name, address, contact1, contact2, note FROM supplier ", CONNECTION.CON))
                {
                    DataTable DT = new DataTable();
                    DA.Fill(DT);
                    if (DT.Rows.Count > 0)
                    {
                        DGV_ITEM.DataSource = DT;
                        DGV_ITEM.AutoGenerateColumns = false;
                    }
                    else
                    {
                        DGV_ITEM.DataSource = null;
                    }
                }

            }
            catch (Exception EX)
            {

                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), EX.Message, MessageAlertImage.Error());
                mdg.ShowDialog();
            }
        }
        private void UPDATE_SUPPLIERS_DATA()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlCommand da = new MySqlCommand("UPDATE supplier SET supplier_id = @supplier_id, supplier_name = @supplier_name, company_name = @company_name, address = @address, contact1 = @contact1, contact2 = @contact2, note = @note WHERE supplier_id = @supplier_id", CONNECTION.CON))
                {
                    da.Parameters.Clear();
                    da.Parameters.AddWithValue("@supplier_id", LBL_SUPPLIER_ID.Text);
                    da.Parameters.AddWithValue("@supplier_name", TXT_SUPPLIER_NAME.Text);
                    da.Parameters.AddWithValue("@company_name", TXT_COMPANY_NAME.Text);
                    da.Parameters.AddWithValue("@address", TXT_ADDRESS.Text);
                    da.Parameters.AddWithValue("@contact1", TXT_CONTACT_01.Text);
                    da.Parameters.AddWithValue("@contact2", TXT_CONTACT_02.Text);
                    da.Parameters.AddWithValue("@note", TXT_NOTE.Text);
                    if (da.ExecuteNonQuery() > 0)
                    {
                        MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "SUPPLIER DETAILS UPDATED SUCCESFULLY!", MessageAlertImage.Success());
                        mdg.ShowDialog();
                        clearAll();
                    }
                    else
                    {
                        MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), "SUPPLIER DETAILS UPDATE FAILED!", MessageAlertImage.Alert());
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
        private void SAVE_SUPPLIER()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlCommand da = new MySqlCommand("INSERT INTO supplier (supplier_name, company_name, address, contact1, contact2, note ) VALUES ( @supplier_name, @company_name, @address, @contact1, @contact2, @note )", CONNECTION.CON))
                {
                    da.Parameters.Clear();
                    da.Parameters.AddWithValue("@supplier_name", TXT_SUPPLIER_NAME.Text);
                    da.Parameters.AddWithValue("@company_name", TXT_COMPANY_NAME.Text);
                    da.Parameters.AddWithValue("@address", TXT_ADDRESS.Text);
                    da.Parameters.AddWithValue("@contact1", TXT_CONTACT_01.Text);
                    da.Parameters.AddWithValue("@contact2", TXT_CONTACT_02.Text);
                    da.Parameters.AddWithValue("@note", TXT_NOTE.Text);
                    if (da.ExecuteNonQuery() > 0)
                    {
                        LBL_SUPPLIER_ID.Text = GETMAXID();
                        MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "SUPPLIER ADDED SUCCESFULLY!", MessageAlertImage.Success());
                        mdg.ShowDialog();

                    }
                    else
                    {
                        MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), "SUPPLIER ADDED FAILED!", MessageAlertImage.Alert());
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
        private void clearAll()
        {
            LBL_SUPPLIER_ID.Text = "N/A";
            TXT_SUPPLIER_NAME.Clear();
            TXT_COMPANY_NAME.Clear();
            TXT_ADDRESS.Clear();
            TXT_CONTACT_01.Clear();
            TXT_CONTACT_02.Clear();
            TXT_NOTE.Clear();

        }
        private void SUPPLIER_Load(object sender, EventArgs e)
        {
            LOAD_SUPPLIER_lIST();
            TXT_SUPPLIER_NAME.Focus();
        }

        private void DGV_ITEM_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            LBL_SUPPLIER_ID.Text = DGV_ITEM.SelectedRows[0].Cells[0].Value.ToString();
            TXT_SUPPLIER_NAME.Text = DGV_ITEM.SelectedRows[0].Cells[1].Value.ToString();
            TXT_COMPANY_NAME.Text = DGV_ITEM.SelectedRows[0].Cells[2].Value.ToString();
            TXT_ADDRESS.Text = DGV_ITEM.SelectedRows[0].Cells[3].Value.ToString();
            TXT_CONTACT_01.Text = DGV_ITEM.SelectedRows[0].Cells[4].Value.ToString();
            TXT_CONTACT_02.Text = DGV_ITEM.SelectedRows[0].Cells[5].Value.ToString();
            TXT_NOTE.Text = DGV_ITEM.SelectedRows[0].Cells[6].Value.ToString();
            BTN_SAVE.Text = "UPDATE";
        }

        private void BTN_NEW_Click(object sender, EventArgs e)
        {
            clearAll();
            TXT_SUPPLIER_NAME.Focus();
            BTN_SAVE.Text = "SAVE";
        }

        private void TXT_SUPPLIER_NAME_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                TXT_COMPANY_NAME.Focus();
            }
        }

        private void TXT_COMPANY_NAME_KeyDown(object sender, KeyEventArgs e)
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
                TXT_CONTACT_01.Focus();
            }
        }

        private void TXT_CONTACT_01_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TXT_CONTACT_02.Focus();
            }
        }

        private void TXT_CONTACT_02_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TXT_NOTE.Focus();
            }
        }

        private void TXT_NOTE_KeyDow(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BTN_SAVE.Focus();
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}
