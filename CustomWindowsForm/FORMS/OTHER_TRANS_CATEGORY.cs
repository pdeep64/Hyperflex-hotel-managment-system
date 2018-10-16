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
    public partial class OTHER_TRANS_CATEGORY : Form
    {
        int CATEGORY_ID = 0;

        public OTHER_TRANS_CATEGORY()
        {
            InitializeComponent();
            LOAD_OTHER_CATEGORY_LIST();
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

        private string SAVE_TRANSACTION_CATEGORY()
        {
            try
            {
                CONNECTION.open_connection();
                using(MySqlCommand cmd=new MySqlCommand("INSERT INTO other_transaction_category(trans_category_name,trans_category_type,desctiption) VALUES(@trans_category_name,@trans_category_type,@desctiption)", CONNECTION.CON))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@trans_category_name", TXT_TRANS_CAT.Text);
                    cmd.Parameters.AddWithValue("@trans_category_type", CMB_TRANS_TYPE.Text);
                    cmd.Parameters.AddWithValue("@desctiption", TXT_DESCRIPTION.Text);
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

        private string UPDATE_TRANSACTION_CATEGORY()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlCommand cmd = new MySqlCommand("UPDATE other_transaction_category SET trans_category_name=@trans_category_name,trans_category_type=@trans_category_type,desctiption=@desctiption WHERE id=@id", CONNECTION.CON))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@trans_category_name", TXT_TRANS_CAT.Text);
                    cmd.Parameters.AddWithValue("@trans_category_type", CMB_TRANS_TYPE.Text);
                    cmd.Parameters.AddWithValue("@desctiption", TXT_DESCRIPTION.Text);
                    cmd.Parameters.AddWithValue("@id", CATEGORY_ID);
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

        private void LOAD_OTHER_CATEGORY_LIST()
        {
            try
            {
                CONNECTION.open_connection();
                using(MySqlDataAdapter adp=new MySqlDataAdapter("SELECT id,trans_category_name,trans_category_type,desctiption,category_status FROM other_transaction_category", CONNECTION.CON))
                {
                    DataTable tbl = new DataTable();
                    adp.Fill(tbl);
                    DGV_TRANS_CAT.DataSource = null;
                    if (tbl.Rows.Count > 0)
                    {
                        DGV_TRANS_CAT.AutoGenerateColumns = false;
                        DGV_TRANS_CAT.DataSource = tbl;
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


        private void CLEAR()
        {
            CMB_TRANS_TYPE.SelectedIndex = -1;
            TXT_TRANS_CAT.Clear();
            TXT_DESCRIPTION.Clear();
            BTN_SAVE.Text = "SAVE";
            CATEGORY_ID = 0;
            CMB_TRANS_TYPE.Focus();
        }


        private void BTN_SAVE_Click(object sender, EventArgs e)
        {
            if (CMB_TRANS_TYPE.SelectedIndex < 0)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Warning(), "PLEASE SELECT TRANSACTION CATEGORY TYPE!", MessageAlertImage.Warning());
                mdg.ShowDialog();
                CMB_TRANS_TYPE.Focus();
            }
            else if (TXT_TRANS_CAT.Text == string.Empty)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Warning(), "PLEASE ENTER TRANSACTION CATEGORY NAME!", MessageAlertImage.Warning());
                mdg.ShowDialog();
                CMB_TRANS_TYPE.Focus();
            }
            else
            {
                if (BTN_SAVE.Text == "SAVE")
                {
                    string result=  SAVE_TRANSACTION_CATEGORY();
                    if (result == "done")
                    {
                        MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "TRANSACTION CATEGORY SAVED SUCCESSFULLY", MessageAlertImage.Success());
                        mdg.ShowDialog();
                        CLEAR();
                        LOAD_OTHER_CATEGORY_LIST();
                    }
                }
                else
                {
                    string result = UPDATE_TRANSACTION_CATEGORY();
                    if (result == "done")
                    {
                        MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "TRANSACTION CATEGORY UPDATED SUCCESSFULLY", MessageAlertImage.Success());
                        mdg.ShowDialog();
                        LOAD_OTHER_CATEGORY_LIST();
                    }
                }
            }
        }

        private void BTN_CLEAR_Click(object sender, EventArgs e)
        {
            CLEAR();
        }

        private void DGV_TRANS_CAT_DoubleClick(object sender, EventArgs e)
        {
            if (DGV_TRANS_CAT.SelectedRows.Count > 0)
            {
                CATEGORY_ID = Convert.ToInt32(DGV_TRANS_CAT.SelectedRows[0].Cells["Column1"].Value);
                CMB_TRANS_TYPE.Text = DGV_TRANS_CAT.SelectedRows[0].Cells["Column3"].Value.ToString();
                TXT_TRANS_CAT.Text= DGV_TRANS_CAT.SelectedRows[0].Cells["Column2"].Value.ToString();
                TXT_DESCRIPTION.Text= DGV_TRANS_CAT.SelectedRows[0].Cells["Column4"].Value.ToString() ;
                BTN_SAVE.Text = "UPDATE";
            }
        }
    }
}
