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
    public partial class MEAL_PLAN : Form
    {
        public MEAL_PLAN()
        {
            InitializeComponent();
            LOAD_MEAP_PLAN_lIST();


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
                //_MaxButton_Click(sender, e);
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

        private string SAVE_MEAL_PLAN()
        {
            try
            {
                if (TXT_DESCRIPTION.Text == string.Empty)
                    TXT_DESCRIPTION.Text = "N/A";

                CONNECTION.open_connection();
                using (MySqlCommand cmd = new MySqlCommand("INSERT INTO meal_types(type,description,adult_meal_price,child_meal_price) VALUES(@type,@description,@adult_meal_price,@child_meal_price)", CONNECTION.CON))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@type", TXT_MP_NAME.Text);
                    cmd.Parameters.AddWithValue("@description", TXT_DESCRIPTION.Text);
                    cmd.Parameters.AddWithValue("@adult_meal_price", Convert.ToDouble(TXT_ADULT_PRICE.Text));
                    cmd.Parameters.AddWithValue("@child_meal_price", Convert.ToDouble(TXT_CHILD_PRICE.Text));
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        int meal_plan_id = 0;
                        using (MySqlDataAdapter adp = new MySqlDataAdapter("SELECT MAX(meal_type_id) FROM meal_types", CONNECTION.CON))
                        {
                            DataTable tbl = new DataTable();
                            adp.Fill(tbl);
                            if (tbl.Rows.Count > 0)
                            {
                                meal_plan_id = tbl.Rows[0].Field<Int32>(0);
                            }
                        }
                        LBL_MP_ID.Text = meal_plan_id.ToString();

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
            TXT_MP_NAME.Enabled = false;
            TXT_DESCRIPTION.Enabled = false;
            TXT_ADULT_PRICE.Enabled = false;
            TXT_CHILD_PRICE.Enabled = false;
            BTN_SAVE.Enabled = false;
            BTN_NEW.Focus();
        }

        private void EnableControls()
        {
            TXT_MP_NAME.Enabled = true;
            TXT_DESCRIPTION.Enabled = true;
            TXT_ADULT_PRICE.Enabled = true;
            TXT_CHILD_PRICE.Enabled = true;
            BTN_SAVE.Enabled = true;
        }

        private void Clear()
        {
            LBL_MP_ID.Text = "N/A";
            TXT_MP_NAME.Clear();
            TXT_DESCRIPTION.Clear();
            TXT_ADULT_PRICE.Text = "0.00";
            TXT_CHILD_PRICE.Text = "0.00";
            BTN_SAVE.Text = "SAVE";
            TXT_MP_NAME.Focus();
        }

        private void LOAD_MEAP_PLAN_lIST()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter DA = new MySqlDataAdapter("SELECT meal_type_id,`type`,description,adult_meal_price,child_meal_price,CASE (meal_plan_status) WHEN '1' THEN 'ACTIVE' WHEN '0' THEN 'DEACTIVE' END AS meal_plan_status FROM meal_types", CONNECTION.CON))
                {
                    DataTable DT = new DataTable();
                    DA.Fill(DT);
                    if (DT.Rows.Count > 0)
                    {
                        DGV_MP.DataSource = DT;
                        DGV_MP.AutoGenerateColumns = false;
                    }
                    else
                    {
                        DGV_MP .DataSource = null;
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

        private string UPDATE_MEAL_PLAN_STATUS()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlCommand cmd = new MySqlCommand("UPDATE meal_types SET meal_plan_status=@meal_plan_status WHERE meal_type_id=@meal_type_id", CONNECTION.CON))
                {
                    cmd.Parameters.Clear();
                    if (aCTIVEDEACTIVEToolStripMenuItem.Text == "ACTIVE")
                        cmd.Parameters.AddWithValue("@meal_plan_status", "1");
                    else
                        cmd.Parameters.AddWithValue("@meal_plan_status", "0");
                    cmd.Parameters.AddWithValue("@meal_type_id", DGV_MP.SelectedRows[0].Cells["Column1"].Value);
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

        private string UPDATE_MEAL_PLAN()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlCommand cmd = new MySqlCommand("UPDATE meal_types SET type=@type,description=@description,adult_meal_price=@adult_meal_price,child_meal_price=@child_meal_price WHERE meal_type_id=@meal_type_id", CONNECTION.CON))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@meal_type_id", Convert.ToInt32(LBL_MP_ID.Text));
                    cmd.Parameters.AddWithValue("@type", TXT_MP_NAME.Text);
                    cmd.Parameters.AddWithValue("@description", TXT_DESCRIPTION.Text);
                    cmd.Parameters.AddWithValue("@adult_meal_price", Convert.ToDouble(TXT_ADULT_PRICE.Text));
                    cmd.Parameters.AddWithValue("@child_meal_price", Convert.ToDouble(TXT_CHILD_PRICE.Text));
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
            if (TXT_MP_NAME.Text == string.Empty)
            {
                TXT_MP_NAME.Focus();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Warning(), "PLEASE ENTER MEAL PLAN NAME", MessageAlertImage.Warning());
                mdg.ShowDialog();
            }
            else
            {
                if (BTN_SAVE.Text == "SAVE")
                {
                    string result = SAVE_MEAL_PLAN();
                    if (result == "done")
                    {
                        MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "MEAL PLAN ADDED SUCCESSFULLY!", MessageAlertImage.Success());
                        mdg.ShowDialog();
                        DisableControls();
                    }
                }
                else
                {
                    string result = UPDATE_MEAL_PLAN();
                    if (result == "done")
                    {
                        MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "PACKAGE DETAILS UPDATED SUCCESSFULLY!", MessageAlertImage.Success());
                        mdg.ShowDialog();
                    }
                }
                LOAD_MEAP_PLAN_lIST();
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
            if (DGV_MP.SelectedRows.Count > 0)
            {
                aCTIVEDEACTIVEToolStripMenuItem.Enabled = true;
                if (DGV_MP.SelectedRows[0].Cells["Column6"].Value.ToString() == "ACTIVE")
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
            string result = UPDATE_MEAL_PLAN_STATUS();
            if (result == "done")
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "MEAL PLAN STATUS UPDATED SUCCESSFULLY!", MessageAlertImage.Success());
                mdg.ShowDialog();
                LOAD_MEAP_PLAN_lIST();
            }
        }

        private void DGV_MP_DoubleClick(object sender, EventArgs e)
        {
            if (DGV_MP.SelectedRows.Count > 0)
            {
                Clear();
                LBL_MP_ID.Text = DGV_MP.SelectedRows[0].Cells["Column1"].Value.ToString();
                TXT_MP_NAME.Text = DGV_MP.SelectedRows[0].Cells["Column2"].Value.ToString();
                TXT_DESCRIPTION.Text = DGV_MP.SelectedRows[0].Cells["Column3"].Value.ToString();
                TXT_ADULT_PRICE.Text = Convert.ToDouble(DGV_MP.SelectedRows[0].Cells["Column4"].Value.ToString()).ToString("F2");
                TXT_CHILD_PRICE.Text = Convert.ToDouble(DGV_MP.SelectedRows[0].Cells["Column5"].Value.ToString()).ToString("F2");
                BTN_SAVE.Text = "UPDATE";
            }
        }
    }
}
