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
    public partial class ADDITIONAL_SERVICE : Form
    {
        public ADDITIONAL_SERVICE()
        {
            InitializeComponent();
            LOAD_SERVICE_lIST();


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

        private string SAVE_SERVICE()
        {
            try
            {
                if (TXT_DESCRIPTION.Text == string.Empty)
                    TXT_DESCRIPTION.Text = "N/A";

                CONNECTION.open_connection();
                using (MySqlCommand cmd = new MySqlCommand("INSERT INTO additional_service_list(service_name,service_price,description) VALUES(@service_name,@service_price,@description)", CONNECTION.CON))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@service_name", TXT_SV_NAME.Text);
                    cmd.Parameters.AddWithValue("@service_price", Convert.ToDouble(TXT_SV_PRICE.Text));
                    cmd.Parameters.AddWithValue("@description", TXT_DESCRIPTION.Text);
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        int meal_plan_id = 0;
                        using (MySqlDataAdapter adp = new MySqlDataAdapter("SELECT MAX(id) FROM additional_service_list", CONNECTION.CON))
                        {
                            DataTable tbl = new DataTable();
                            adp.Fill(tbl);
                            if (tbl.Rows.Count > 0)
                            {
                                meal_plan_id = tbl.Rows[0].Field<Int32>(0);
                            }
                        }
                        LBL_SV_ID.Text = meal_plan_id.ToString();

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
                    MSGBOX mdg = new MSGBOX(MessageAlertHeder.Warning(), "MEAL PLAN NAME ALREADY EXSIST!", MessageAlertImage.Warning());
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
            TXT_SV_NAME.Enabled = false;
            TXT_DESCRIPTION.Enabled = false;
            TXT_SV_PRICE.Enabled = false;
            BTN_SAVE.Enabled = false;
            BTN_NEW.Focus();
        }

        private void EnableControls()
        {
            TXT_SV_NAME.Enabled = true;
            TXT_DESCRIPTION.Enabled = true;
            TXT_SV_PRICE.Enabled = true;
            BTN_SAVE.Enabled = true;
        }

        private void Clear()
        {
            LBL_SV_ID.Text = "N/A";
            TXT_SV_NAME.Clear();
            TXT_DESCRIPTION.Clear();
            TXT_SV_PRICE.Text = "0.00";
            BTN_SAVE.Text = "SAVE";
            TXT_SV_NAME.Focus();
        }

        private void LOAD_SERVICE_lIST()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter DA = new MySqlDataAdapter("SELECT id,service_name,service_price,description,CASE (service_status) WHEN '1' THEN 'ACTIVE' WHEN '0' THEN 'DEACTIVE' END AS service_status FROM additional_service_list", CONNECTION.CON))
                {
                    DataTable DT = new DataTable();
                    DA.Fill(DT);
                    if (DT.Rows.Count > 0)
                    {
                        DGV_SV.DataSource = DT;
                        DGV_SV.AutoGenerateColumns = false;
                    }
                    else
                    {
                        DGV_SV.DataSource = null;
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

        private string UPDATE_SERVICE_DETAILS()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlCommand cmd = new MySqlCommand("UPDATE additional_service_list SET service_name=@service_name,service_price=@service_price,description=@description WHERE id=@id", CONNECTION.CON))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(LBL_SV_ID.Text));
                    cmd.Parameters.AddWithValue("@service_name", TXT_SV_NAME.Text);
                    cmd.Parameters.AddWithValue("@service_price", Convert.ToDouble(TXT_SV_PRICE.Text));
                    cmd.Parameters.AddWithValue("@description", TXT_DESCRIPTION.Text);
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
            if (TXT_SV_NAME.Text == string.Empty)
            {
                TXT_SV_NAME.Focus();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Warning(), "PLEASE ENTER SERVICE NAME", MessageAlertImage.Warning());
                mdg.ShowDialog();
            }
            else
            {
                if (BTN_SAVE.Text == "SAVE")
                {
                    string result = SAVE_SERVICE();
                    if (result == "done")
                    {
                        MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "ADDITIONAL SERVICE ADDED SUCCESSFULLY!", MessageAlertImage.Success());
                        mdg.ShowDialog();
                        DisableControls();
                    }
                }
                else if (BTN_SAVE.Text == "UPDATE")
                {
                    string result = UPDATE_SERVICE_DETAILS();
                    if (result == "done")
                    {
                        MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "SERVICE DETAILS UPDATED SUCCESSFULLY!", MessageAlertImage.Success());
                        mdg.ShowDialog();
                    }
                }
                LOAD_SERVICE_lIST();
            }
        }

        private string UPDATE_SERVICE_STATUS()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlCommand cmd = new MySqlCommand("UPDATE additional_service_list SET service_status=@service_status WHERE id=@id", CONNECTION.CON))
                {
                    cmd.Parameters.Clear();
                    if (aCTIVEDEACTIVEToolStripMenuItem.Text == "ACTIVE")
                        cmd.Parameters.AddWithValue("@service_status", "1");
                    else
                        cmd.Parameters.AddWithValue("@service_status", "0");
                    cmd.Parameters.AddWithValue("@id", DGV_SV.SelectedRows[0].Cells["Column1"].Value);
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

        private void BTN_NEW_Click(object sender, EventArgs e)
        {
            if (BTN_SAVE.Enabled == false)
                EnableControls();

            Clear();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (DGV_SV.SelectedRows.Count > 0)
            {
                aCTIVEDEACTIVEToolStripMenuItem.Enabled = true;
                if (DGV_SV.SelectedRows[0].Cells["Column6"].Value.ToString() == "ACTIVE")
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
            string result = UPDATE_SERVICE_STATUS();
            if (result == "done")
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "PACKAGE STATUS UPDATED SUCCESSFULLY!", MessageAlertImage.Success());
                mdg.ShowDialog();
                LOAD_SERVICE_lIST();
            }
        }

        private void DGV_SV_DoubleClick(object sender, EventArgs e)
        {
            if (DGV_SV.SelectedRows.Count > 0)
            {
                Clear();
                LBL_SV_ID.Text = DGV_SV.SelectedRows[0].Cells["Column1"].Value.ToString();
                TXT_SV_NAME.Text = DGV_SV.SelectedRows[0].Cells["Column2"].Value.ToString();
                TXT_SV_PRICE.Text = Convert.ToDouble(DGV_SV.SelectedRows[0].Cells["Column4"].Value.ToString()).ToString("F2");
                TXT_DESCRIPTION.Text = DGV_SV.SelectedRows[0].Cells["Column5"].Value.ToString();
                BTN_SAVE.Text = "UPDATE";
            }
        }
    }
}
