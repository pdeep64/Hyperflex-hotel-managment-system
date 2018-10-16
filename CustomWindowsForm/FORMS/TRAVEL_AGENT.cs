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
    public partial class TRAVEL_AGENT : Form
    {
        public TRAVEL_AGENT()
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

        private void TRAVEL_AGENT_Load(object sender, EventArgs e)
        {
            LoadGrid();
        }

        private void LoadGrid()
        {
            CONNECTION.open_connection();
            try
            {
                using (MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM agent", CONNECTION.CON))
                {
                    DataTable getagents = new DataTable();
                    da.Fill(getagents);
                    if (getagents.Rows.Count > 0)
                    {
                        DGV_AGENT_LIST.Rows.Clear();
                        foreach (DataRow dr in getagents.Rows)
                        {
                            DGV_AGENT_LIST.Rows.Add(
                                dr["agent_id"].ToString(),
                                dr["agent_name"].ToString(),
                                dr["address"].ToString(),
                                dr["company_name"].ToString(),
                                dr["contact_no"].ToString(),
                                dr["company_contact_no"].ToString(),
                                dr["description"].ToString(),
                                dr["commison_rate"].ToString(),
                                dr["added_by"].ToString(),
                                Convert.ToDateTime(dr["added_date"].ToString()).ToString("yyyy-MM-dd"),
                                Convert.ToDateTime(dr["added_time"].ToString()).ToString("HH:mm:ss"),
                                dr["agent_status"].ToString()
                                );
                        }
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

        private void BTN_SAVE_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(TXT_AGENT_NAME.Text))
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Warning(), "Enter Agent Name", MessageAlertImage.Warning());
                mdg.ShowDialog();
                TXT_AGENT_NAME.Focus();
            }
            else if (String.IsNullOrEmpty(TXT_ADDRESS.Text))
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Warning(), "Enter Agent Address", MessageAlertImage.Warning());
                mdg.ShowDialog();
                TXT_ADDRESS.Focus();
            }
            else if (String.IsNullOrEmpty(TXT_COMPANY_NAME.Text))
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Warning(), "Enter Company Name", MessageAlertImage.Warning());
                mdg.ShowDialog();
                TXT_COMPANY_NAME.Focus();
            }
            else if (String.IsNullOrEmpty(TXT_CONTACT_NO.Text))
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Warning(), "Enter Contact Number", MessageAlertImage.Warning());
                mdg.ShowDialog();
                TXT_CONTACT_NO.Focus();
            }
            else if (String.IsNullOrEmpty(TXT_COMPANY_CONTACT_NO.Text))
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Warning(), "Enter Company Contact Number", MessageAlertImage.Warning());
                mdg.ShowDialog();
                TXT_COMPANY_CONTACT_NO.Focus();
            }
            else if (String.IsNullOrEmpty(TXT_COM_RATE.Text))
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Warning(), "Enter Commision Rate", MessageAlertImage.Warning());
                mdg.ShowDialog();
                TXT_COM_RATE.Focus();
            }
            else
            {
                if (BTN_SAVE.Text == "SAVE")
                {
                    AddAgent();
                }
                else if (BTN_SAVE.Text == "UPDATE")
                {
                    UpdateAgent();
                }
            }
        }

        private void AddAgent()
        {
            CONNECTION.open_connection();
            try
            {
                using (MySqlCommand addagent = new MySqlCommand("INSERT INTO agent(agent_name,address,contact_no,company_name,company_contact_no,commison_rate,description,added_by,added_date,added_time,agent_status) VALUES(@name,@address,@contact_no,@company_name,@copmany_contact_no,@commison_rate,@description,@added_by,@added_date,@added_time,@agent_status)", CONNECTION.CON))
                {
                    addagent.Parameters.AddWithValue("@name", TXT_AGENT_NAME.Text);
                    addagent.Parameters.AddWithValue("@address", TXT_ADDRESS.Text);
                    addagent.Parameters.AddWithValue("@contact_no", TXT_CONTACT_NO.Text);
                    addagent.Parameters.AddWithValue("@company_name", TXT_COMPANY_NAME.Text);
                    addagent.Parameters.AddWithValue("@copmany_contact_no", TXT_COMPANY_CONTACT_NO.Text);
                    addagent.Parameters.AddWithValue("@commison_rate", TXT_COM_RATE.Text);
                    addagent.Parameters.AddWithValue("@description", TXT_DESCRIPTION.Text);
                    addagent.Parameters.AddWithValue("@added_by", CLS_CURRENT_LOGGER.LOGGED_IN_USERID);
                    addagent.Parameters.AddWithValue("@added_date", DateTime.Now.ToString("yyyy-MM-dd"));
                    addagent.Parameters.AddWithValue("@added_time", DateTime.Now.ToString("HH:mm:ss"));
                    addagent.Parameters.AddWithValue("@agent_status", "1");
                    int n1 = addagent.ExecuteNonQuery();
                    if (n1 == 1)
                    {
                        MySqlDataAdapter da = new MySqlDataAdapter("SELECT LAST_INSERT_ID()", CONNECTION.CON);
                        DataTable getlastid = new DataTable();
                        da.Fill(getlastid);
                        if (getlastid.Rows.Count > 0)
                        {
                            LBL_AGENT_ID.Text = getlastid.Rows[0][0].ToString();
                        }
                        LoadGrid();
                        MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "Saved Successfully", MessageAlertImage.Success());
                        mdg.ShowDialog();
                        EnableControls(false);

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

        private void UpdateAgent()
        {
            CONNECTION.open_connection();
            try
            {
                using (MySqlCommand updateagent = new MySqlCommand("UPDATE agent SET agent_name=@name,address=@address,contact_no=@contact_no,company_name=@company_name,company_contact_no=@company_contact_no,commison_rate=@commison_rate,description=@description WHERE agent_id=@id", CONNECTION.CON))
                {
                    updateagent.Parameters.AddWithValue("@id", LBL_AGENT_ID.Text);
                    updateagent.Parameters.AddWithValue("@name", TXT_AGENT_NAME.Text);
                    updateagent.Parameters.AddWithValue("@address", TXT_ADDRESS.Text);
                    updateagent.Parameters.AddWithValue("@contact_no", TXT_CONTACT_NO.Text);
                    updateagent.Parameters.AddWithValue("@company_name", TXT_COMPANY_NAME.Text);
                    updateagent.Parameters.AddWithValue("@company_contact_no", TXT_COMPANY_CONTACT_NO.Text);
                    updateagent.Parameters.AddWithValue("@commison_rate", TXT_COM_RATE.Text);
                    updateagent.Parameters.AddWithValue("@description", TXT_DESCRIPTION.Text);
                    int n1 = updateagent.ExecuteNonQuery();
                    if (n1 == 1)
                    {
                        LoadGrid();
                        ClearAll();
                        MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "Updated Successfully", MessageAlertImage.Success());
                        mdg.ShowDialog();
                        BTN_SAVE.Text = "SAVE";
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

        private void EnableControls(bool status)
        {
            TXT_ADDRESS.Enabled = status;
            TXT_AGENT_NAME.Enabled = status;
            TXT_COMPANY_CONTACT_NO.Enabled = status;
            TXT_COMPANY_NAME.Enabled = status;
            TXT_COM_RATE.Enabled = status;
            TXT_CONTACT_NO.Enabled = status;
            TXT_DESCRIPTION.Enabled = status;
            BTN_SAVE.Enabled = status;
        }
        private void ClearAll()
        {
            TXT_ADDRESS.Clear();
            TXT_AGENT_NAME.Clear();
            TXT_COMPANY_CONTACT_NO.Clear();
            TXT_COMPANY_NAME.Clear();
            TXT_COM_RATE.Clear();
            TXT_CONTACT_NO.Clear();
            TXT_DESCRIPTION.Clear();
            LBL_AGENT_ID.Text = "N/A";
            BTN_SAVE.Text = "SAVE";
        }

        private void BTN_NEW_Click(object sender, EventArgs e)
        {
            ClearAll();
            EnableControls(true);
        }

        private void DGV_AGENT_LIST_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (DGV_AGENT_LIST.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow dgvr in DGV_AGENT_LIST.SelectedRows)
                {
                    LBL_AGENT_ID.Text = dgvr.Cells[0].Value.ToString();
                    TXT_AGENT_NAME.Text = dgvr.Cells[1].Value.ToString();
                    TXT_ADDRESS.Text = dgvr.Cells[2].Value.ToString();
                    TXT_COMPANY_NAME.Text = dgvr.Cells[3].Value.ToString();
                    TXT_CONTACT_NO.Text = dgvr.Cells[4].Value.ToString();
                    TXT_COMPANY_CONTACT_NO.Text = dgvr.Cells[5].Value.ToString();
                    TXT_DESCRIPTION.Text = dgvr.Cells[6].Value.ToString();
                    TXT_COM_RATE.Text = dgvr.Cells[7].Value.ToString();
                }
                EnableControls(true);
                BTN_SAVE.Text = "UPDATE";
            }
            else
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Warning(), "No Rows Selected", MessageAlertImage.Warning());
                mdg.ShowDialog();
            }
        }
    }
}


