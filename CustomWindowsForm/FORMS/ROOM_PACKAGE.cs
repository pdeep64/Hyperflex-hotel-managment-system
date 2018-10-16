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
    public partial class ROOM_PACKAGE : Form
    {
        double tax_percentage = 0;

        public ROOM_PACKAGE()
        {
            InitializeComponent();
            tax_percentage = CLS_TAX.GetTotalTaxPercentage();
            LOAD_PACKAGE_LIST();
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

        private void DisableControls()
        {
            TXT_PKG_NAME.Enabled = false;
            TXT_DESCRIPTION.Enabled = false;
            CMB_ROOM_TYPE.Enabled = false;
            TXT_PKG_RATE.Enabled = false;
            BTN_SAVE.Enabled = false;
            LBL_COLOR.Enabled = false;
            BTN_NEW.Focus();

        }

        private void EnableControls()
        {
            TXT_PKG_NAME.Enabled = true;
            TXT_DESCRIPTION.Enabled = true;
            CMB_ROOM_TYPE.Enabled = true;
            TXT_PKG_RATE.Enabled = true;
            BTN_SAVE.Enabled = true;
            LBL_COLOR.Enabled = true;
        }

        private void clear()
        {
            LBL_PKG_ID.Text = "N/A";
            TXT_PKG_NAME.Clear();
            TXT_DESCRIPTION.Clear();
            CMB_ROOM_TYPE.SelectedIndex = -1;
            TXT_PKG_RATE.Text = "0.00";
            TXT_PKG_NAME.Focus();
            LBL_COLOR.Text = "SELECT COLOR";
            LBL_COLOR.BackColor = DefaultBackColor;
            BTN_SAVE.Text = "SAVE";
        }


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

        private void SAVE_ROOM_PACKAGE()
        {
            try
            {

                CONNECTION.open_connection();
                using (MySqlCommand cmd = new MySqlCommand("INSERT INTO room_packages(package_name,description,`condition`,room_package_price,package_color) VALUES(@package_name,@description,@condition,@room_package_price,@package_color)", CONNECTION.CON))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@package_name", TXT_PKG_NAME.Text);
                    cmd.Parameters.AddWithValue("@description", TXT_DESCRIPTION.Text);
                    cmd.Parameters.AddWithValue("@condition", CMB_ROOM_TYPE.Text);
                    cmd.Parameters.AddWithValue("@room_package_price", Convert.ToDouble(TXT_PKG_RATE.Text));
                    cmd.Parameters.AddWithValue("@package_color", LBL_COLOR.Text);
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        int room_pkg_no = 0;
                        using(MySqlDataAdapter adp=new MySqlDataAdapter("SELECT MAX(room_package_id) FROM room_packages", CONNECTION.CON))
                        {
                            DataTable tbl = new DataTable();
                            adp.Fill(tbl);
                            if (tbl.Rows.Count > 0)
                            {
                                room_pkg_no = tbl.Rows[0].Field<Int32>(0);
                            }
                        }
                        LBL_PKG_ID.Text = room_pkg_no.ToString();
                        MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "PACKAGE ADDED SUCCESSFULLY!", MessageAlertImage.Success());
                        mdg.ShowDialog();
                        DisableControls();
                    }
                    else
                    {
                        MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), "FAILED TO ADD ROOM PACKAGE!", MessageAlertImage.Alert());
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

        private void LOAD_PACKAGE_LIST()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter DA = new MySqlDataAdapter("SELECT room_package_id,package_name,description,`condition`,room_package_price,package_color,CASE (package_status) WHEN '1' THEN 'ACTIVE' WHEN '0' THEN 'DEACTIVE' END AS package_status FROM room_packages", CONNECTION.CON))
                {
                    DataTable DT = new DataTable();
                    DA.Fill(DT);
                    if (DT.Rows.Count > 0)
                    {
                        DGV_PKG.DataSource = DT;
                        DGV_PKG.AutoGenerateColumns = false;
                    }
                    else
                    {
                        DGV_PKG.DataSource = null;
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

        private string UPDATE_PACKAGE_STATUS()
        {
            try
            {
                CONNECTION.open_connection();
                using(MySqlCommand cmd=new MySqlCommand("UPDATE room_packages SET package_status=@package_status WHERE room_package_id=@room_package_id", CONNECTION.CON))
                {
                    cmd.Parameters.Clear();
                    if (aCTIVEDEACTIVEToolStripMenuItem.Text == "ACTIVE")
                        cmd.Parameters.AddWithValue("@package_status", "1");
                    else
                        cmd.Parameters.AddWithValue("@package_status", "0");
                    cmd.Parameters.AddWithValue("@room_package_id", DGV_PKG.SelectedRows[0].Cells["Column1"].Value);
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
        private string UPDATE_PACKAGE()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlCommand cmd = new MySqlCommand("UPDATE room_packages SET package_name=@package_name,description=@description,`condition`=@condition,room_package_price=@room_package_price,package_color=@package_color WHERE room_package_id=@room_package_id", CONNECTION.CON))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@room_package_id", Convert.ToInt32(LBL_PKG_ID.Text));
                    cmd.Parameters.AddWithValue("@package_name", TXT_PKG_NAME.Text);
                    cmd.Parameters.AddWithValue("@description", TXT_DESCRIPTION.Text);
                    cmd.Parameters.AddWithValue("@condition", CMB_ROOM_TYPE.Text);
                    cmd.Parameters.AddWithValue("@room_package_price", Convert.ToDouble(TXT_PKG_RATE.Text));
                    cmd.Parameters.AddWithValue("@package_color", LBL_COLOR.Text);
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
            if(TXT_PKG_NAME.Text==string.Empty)
            {
                TXT_PKG_NAME.Focus();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Warning(), "PLEASE ENTER PACKAGE NAME", MessageAlertImage.Warning());
                mdg.ShowDialog();
            }
            else if(CMB_ROOM_TYPE.SelectedIndex<0)
            {
                CMB_ROOM_TYPE.Focus();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Warning(), "PLEASE SELECT ROOM TYPE", MessageAlertImage.Warning());
                mdg.ShowDialog();
            }
            else
            {
                if (BTN_SAVE.Text == "SAVE")
                    SAVE_ROOM_PACKAGE();
                else if (BTN_SAVE.Text == "UPDATE")
                {
                    string result= UPDATE_PACKAGE();
                    if (result == "done")
                    {
                        MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "PACKAGE DETAILS UPDATED SUCCESSFULLY!", MessageAlertImage.Success());
                        mdg.ShowDialog();
                        LOAD_PACKAGE_LIST();
                    }
                }
                    

                    LOAD_PACKAGE_LIST();
            }
        }

        private void BTN_NEW_Click(object sender, EventArgs e)
        {
            if (BTN_SAVE.Enabled == false)
                EnableControls();

            clear();
        }

        private void LBL_COLOR_Click(object sender, EventArgs e)
        {
            if (cd_pkg_color.ShowDialog() == DialogResult.OK)
            {
                LBL_COLOR.BackColor = cd_pkg_color.Color;
                LBL_COLOR.Text = "#" + cd_pkg_color.Color.R.ToString("X2") + cd_pkg_color.Color.G.ToString("X2") + cd_pkg_color.Color.B.ToString("X2");
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (DGV_PKG.SelectedRows.Count > 0)
            {
                aCTIVEDEACTIVEToolStripMenuItem.Enabled = true;
                if (DGV_PKG.SelectedRows[0].Cells["Column7"].Value.ToString() == "ACTIVE")
                    aCTIVEDEACTIVEToolStripMenuItem.Text = "DEACTIVE";
                else
                    aCTIVEDEACTIVEToolStripMenuItem.Text = "ACTIVE";
            }
            else
            {
                aCTIVEDEACTIVEToolStripMenuItem.Enabled = false;
            }
           
        }

        private void aCTIVEDEACTIVEToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string result = UPDATE_PACKAGE_STATUS();
            if (result == "done")
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "PACKAGE STATUS UPDATED SUCCESSFULLY!", MessageAlertImage.Success());
                mdg.ShowDialog();
                LOAD_PACKAGE_LIST();
            }
        }

        private void DGV_PKG_DoubleClick(object sender, EventArgs e)
        {
            if (DGV_PKG.SelectedRows.Count > 0)
            {
                clear();
                LBL_PKG_ID.Text = DGV_PKG.SelectedRows[0].Cells["Column1"].Value.ToString();
                TXT_PKG_NAME.Text= DGV_PKG.SelectedRows[0].Cells["Column4"].Value.ToString();
                TXT_DESCRIPTION.Text= DGV_PKG.SelectedRows[0].Cells["Column3"].Value.ToString();
                CMB_ROOM_TYPE.Text = DGV_PKG.SelectedRows[0].Cells["Column8"].Value.ToString();
                TXT_PKG_RATE.Text = Convert.ToDouble(DGV_PKG.SelectedRows[0].Cells["Column2"].Value.ToString()).ToString("F2");
                LBL_COLOR.BackColor = ColorTranslator.FromHtml(DGV_PKG.SelectedRows[0].Cells["Column5"].Value.ToString());
                LBL_COLOR.Text = DGV_PKG.SelectedRows[0].Cells["Column5"].Value.ToString();
                BTN_SAVE.Text = "UPDATE";
            }
        }
    }
}
