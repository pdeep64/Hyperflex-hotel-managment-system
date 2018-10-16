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
    public partial class ROOMS : Form
    {
        public ROOMS()
        {
            InitializeComponent();
            LOAD_ROOM_TYPES();
            LOAD_FLOOR_LIST();
            LOAD_ROOM_lIST();
        }

        int room_id = 0;

        private void LOAD_ROOM_TYPES()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter adp = new MySqlDataAdapter("SELECT room_package_id,package_name FROM room_packages WHERE package_status='1'", CONNECTION.CON))
                {
                    DataTable tbl = new DataTable();
                    adp.Fill(tbl);
                    CMB_ROOM_PKG.DataSource = null;
                    if (tbl.Rows.Count > 0)
                    {
                        CMB_ROOM_PKG.DataSource = tbl;
                        CMB_ROOM_PKG.DisplayMember = "package_name";
                        CMB_ROOM_PKG.ValueMember = "room_package_id";
                        CMB_ROOM_PKG.SelectedIndex = -1;
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

        private void LOAD_FLOOR_LIST()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter adp = new MySqlDataAdapter("SELECT id,floor_no FROM floor", CONNECTION.CON))
                {
                    DataTable tbl = new DataTable();
                    adp.Fill(tbl);
                    CMB_FLOOR.DataSource = null;
                    if (tbl.Rows.Count > 0)
                    {
                        CMB_FLOOR.DataSource = tbl;
                        CMB_FLOOR.DisplayMember = "floor_no";
                        CMB_FLOOR.ValueMember = "id";
                        CMB_FLOOR.SelectedIndex = -1;
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

        private void BTN_NEW_FLOOR_Click(object sender, EventArgs e)
        {
            if (PNL_ADD_FLOOR.Visible == false)
            {
                PNL_ADD_FLOOR.Visible = true;
                PNL_ADD_FLOOR.Size = new Size(288, 80);
                TXT_FLOOR_NAME.Focus();
            }
            else
            {
                PNL_ADD_FLOOR.Visible = false;
                PNL_ADD_FLOOR.Size = new Size(0, 0);
            }
        }

        private string SAVE_ROOM()
        {
            try
            {
                if (TXT_DESCRIPTION.Text == string.Empty)
                    TXT_DESCRIPTION.Text = "N/A";

                CONNECTION.open_connection();
                using (MySqlCommand cmd = new MySqlCommand("INSERT INTO room(room_name,room_package_id,room_floor,description,room_type) VALUES(@room_name,@room_package_id,@room_floor,@description,@room_type)", CONNECTION.CON))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@room_name", TXT_ROOM_NO.Text);
                    cmd.Parameters.AddWithValue("@room_package_id", CMB_ROOM_PKG.SelectedValue);
                    cmd.Parameters.AddWithValue("@room_floor", CMB_FLOOR.Text);
                    cmd.Parameters.AddWithValue("@description", TXT_DESCRIPTION.Text);
                    cmd.Parameters.AddWithValue("@room_type", CMB_ROOM_TYPE.Text);
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        return "done";
                    }
                    else
                    {
                        return "error";
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Duplicate entry"))
                {
                    MSGBOX mdg = new MSGBOX(MessageAlertHeder.Warning(), "ROOM NAME ALREADY EXSIST!", MessageAlertImage.Warning());
                    mdg.ShowDialog();
                }
                else
                {
                    MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), ex.Message, MessageAlertImage.Error());
                    mdg.ShowDialog();
                }
                return "error";
            }
            finally
            {
                CONNECTION.close_connection();
            }
        }

        private string ADD_FLOOR()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlCommand cmd = new MySqlCommand("INSERT INTO floor(floor_no) VALUES(@floor_no)", CONNECTION.CON))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@floor_no", TXT_FLOOR_NAME.Text);
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        return "done";
                    }
                    else
                    {
                        return "error";
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Duplicate entry"))
                {
                    MSGBOX mdg = new MSGBOX(MessageAlertHeder.Warning(), "fLOOR NAME ALREADY EXSIST!", MessageAlertImage.Warning());
                    mdg.ShowDialog();
                }
                else
                {
                    MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), ex.Message, MessageAlertImage.Error());
                    mdg.ShowDialog();
                }
                return "error";
            }
            finally
            {
                CONNECTION.close_connection();
            }
        }

        private void DisableControls()
        {
            TXT_ROOM_NO.Enabled = false;
            CMB_ROOM_PKG.Enabled = false;
            CMB_ROOM_TYPE.Enabled = false;
            CMB_FLOOR.Enabled = false;
            TXT_DESCRIPTION.Enabled = false;
            BTN_SAVE.Enabled = false;
            BTN_NEW.Focus();
        }
        private void EnableControls()
        {
            TXT_ROOM_NO.Enabled = true;
            CMB_ROOM_PKG.Enabled = true;
            CMB_ROOM_TYPE.Enabled = true;
            CMB_FLOOR.Enabled = true;
            TXT_DESCRIPTION.Enabled = true;
            BTN_SAVE.Enabled = true;
        }

        private void Clear()
        {
            TXT_ROOM_NO.Clear();
            CMB_ROOM_PKG.SelectedIndex = -1;
            CMB_ROOM_TYPE.SelectedIndex = -1;
            CMB_FLOOR.SelectedIndex = -1;
            TXT_DESCRIPTION.Clear();room_id = 0;
            BTN_SAVE.Text = "SAVE";
            TXT_ROOM_NO.Focus();
        }

        private void LOAD_ROOM_lIST()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter DA = new MySqlDataAdapter("SELECT r.room_id,r.room_name,r.room_package_id,rp.package_name,r.room_floor,r.description,r.room_type,r.current_status FROM room  r JOIN room_packages rp ON r.room_package_id=rp.room_package_id", CONNECTION.CON))
                {
                    DataTable DT = new DataTable();
                    DA.Fill(DT);
                    if (DT.Rows.Count > 0)
                    {
                        DGV__ROOMS.DataSource = DT;
                        DGV__ROOMS.AutoGenerateColumns = false;
                    }
                    else
                    {
                        DGV__ROOMS.DataSource = null;
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

        private string UPDATE_ROOM_STATUS()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlCommand cmd = new MySqlCommand("UPDATE room SET current_status=@current_status WHERE room_id=@room_id", CONNECTION.CON))
                {
                    cmd.Parameters.Clear();
                    if (aCTIVEDEACTIVEToolStripMenuItem.Text == "MAINTENANCE")
                        cmd.Parameters.AddWithValue("@current_status", "MAINTANANCE");
                    else
                        cmd.Parameters.AddWithValue("@current_status", "AVAILABLE");
                    cmd.Parameters.AddWithValue("@room_id", DGV__ROOMS.SelectedRows[0].Cells["Column1"].Value);
                    if (cmd.ExecuteNonQuery() > 0)
                        return "done";
                    else
                        return "error";
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

        private string UPDATE_ROOM_DETAILS()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlCommand cmd = new MySqlCommand("UPDATE room SET room_name=@room_name,room_package_id=@room_package_id,room_floor=@room_floor,description=@description,room_type=@room_type WHERE room_id=@room_id", CONNECTION.CON))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@room_name", TXT_ROOM_NO.Text);
                    cmd.Parameters.AddWithValue("@room_package_id", CMB_ROOM_PKG.SelectedValue);
                    cmd.Parameters.AddWithValue("@room_floor", CMB_FLOOR.Text);
                    cmd.Parameters.AddWithValue("@description", TXT_DESCRIPTION.Text);
                    cmd.Parameters.AddWithValue("@room_type", CMB_ROOM_TYPE.Text);
                    cmd.Parameters.AddWithValue("@room_id", room_id);
                    if (cmd.ExecuteNonQuery() > 0)
                        return "done";
                    else
                        return "error";
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

        private void BTN_SAVE_Click(object sender, EventArgs e)
        {
            if (TXT_ROOM_NO.Text == string.Empty)
            {
                TXT_ROOM_NO.Focus();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Warning(), "PLEASE ENTER ROOM NO/NAME", MessageAlertImage.Warning());
                mdg.ShowDialog();
            }
            else if (CMB_ROOM_PKG.SelectedIndex < 0)
            {
                CMB_ROOM_PKG.Focus();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Warning(), "PLEASE SELECT ROOM PACKAGE", MessageAlertImage.Warning());
                mdg.ShowDialog();
            }
            else if (CMB_ROOM_TYPE.SelectedIndex < 0)
            {
                CMB_ROOM_TYPE.Focus();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Warning(), "PLEASE SELECT ROOM TYPE", MessageAlertImage.Warning());
                mdg.ShowDialog();
            }
            else if (CMB_FLOOR.SelectedIndex < 0)
            {
                CMB_FLOOR.Focus();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Warning(), "PLEASE SELECT THE FLOOR", MessageAlertImage.Warning());
                mdg.ShowDialog();
            }
            else
            {
                if (BTN_SAVE.Text == "SAVE")
                {
                    string result = SAVE_ROOM();
                    if (result == "done")
                    {
                        MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "ROOM ADDED SUCCESSFULLY!", MessageAlertImage.Success());
                        mdg.ShowDialog();
                        DisableControls();
                    }
                }
                else if (BTN_SAVE.Text == "UPDATE")
                {
                    string result = UPDATE_ROOM_DETAILS();
                    if (result == "done")
                    {
                        MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "ROOM DETAILS UPDATED SUCCESSFULLY!", MessageAlertImage.Success());
                        mdg.ShowDialog();
                    }

                }
                LOAD_ROOM_lIST();
            }
        }

        private void BTN_NEW_Click(object sender, EventArgs e)
        {
            if (BTN_SAVE.Enabled == false)
                EnableControls();

            Clear();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (DGV__ROOMS.SelectedRows.Count > 0)
            {
                aCTIVEDEACTIVEToolStripMenuItem.Enabled = true;
                if (DGV__ROOMS.SelectedRows[0].Cells["Column7"].Value.ToString() != "MAINTANANCE")
                    aCTIVEDEACTIVEToolStripMenuItem.Text = "MAINTENANCE";
                else
                    aCTIVEDEACTIVEToolStripMenuItem.Text = "AVAILABLE";
            }
            else
            {
                aCTIVEDEACTIVEToolStripMenuItem.Enabled = false;
            }
        }

        private void aCTIVEDEACTIVEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string result = UPDATE_ROOM_STATUS();
            if (result == "done")
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "PACKAGE STATUS UPDATED SUCCESSFULLY!", MessageAlertImage.Success());
                mdg.ShowDialog();
                LOAD_ROOM_lIST();
            }
        }

        private void DGV__ROOMS_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DGV__ROOMS_DoubleClick(object sender, EventArgs e)
        {
            if (DGV__ROOMS.SelectedRows.Count > 0)
            {
                Clear();
                room_id =Convert.ToInt32(DGV__ROOMS.SelectedRows[0].Cells["Column1"].Value);
                TXT_ROOM_NO.Text = DGV__ROOMS.SelectedRows[0].Cells["Column4"].Value.ToString();
                CMB_ROOM_PKG.SelectedValue = DGV__ROOMS.SelectedRows[0].Cells["Column8"].Value;
                CMB_ROOM_TYPE.Text = DGV__ROOMS.SelectedRows[0].Cells["Column2"].Value.ToString();
                CMB_FLOOR.Text = DGV__ROOMS.SelectedRows[0].Cells["Column5"].Value.ToString();
                TXT_DESCRIPTION.Text = DGV__ROOMS.SelectedRows[0].Cells["Column6"].Value.ToString();
                BTN_SAVE.Text = "UPDATE";
            }
        }

        private void shapedButton1_Click(object sender, EventArgs e)
        {
            string result = ADD_FLOOR();
            if (result == "done")
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "FLOOR ADDED SUCCESSFULLY!", MessageAlertImage.Success());
                mdg.ShowDialog();
                TXT_FLOOR_NAME.Clear();
                TXT_FLOOR_NAME.Focus();
                LOAD_FLOOR_LIST();
            }
        }
    }
}
