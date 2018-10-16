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
    public partial class ITEMS : Form
    {
        public ITEMS()
        {
            InitializeComponent();
            CLS_METHODS.FILL_COMBOBOX(CMB_ITEM_TYPE, "SELECT id,type FROM item_type ORDER BY type", "type", "id", -1);
          
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
        private String GETMAXID()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter DA = new MySqlDataAdapter("SELECT MAX(item_id) FROM item", CONNECTION.CON))
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
        private void UPDATE_ITEMS()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlCommand da = new MySqlCommand("UPDATE item SET barcode=@barcode,item_category=@item_category,item_name=@item_name,item_type_id=@item_type_id WHERE item_id=@item_id", CONNECTION.CON))
                {
                    da.Parameters.Clear();
                    da.Parameters.AddWithValue("@barcode", TXT_BARCODE.Text);
                    da.Parameters.AddWithValue("@item_category", CMB_CATEGORY.SelectedValue);
                    da.Parameters.AddWithValue("@item_name", TXT_ITEMNAME.Text);
                    da.Parameters.AddWithValue("@item_id", LBL_ITEM_ID.Text);
                    da.Parameters.AddWithValue("@item_type_id", CMB_ITEM_TYPE.SelectedValue);
                    da.Parameters.AddWithValue("@item_status", "ENABLE");
                    if (da.ExecuteNonQuery() > 0)
                    {
                        MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "ITEM UPDATED SUCCESFULLY!", MessageAlertImage.Success());
                        mdg.ShowDialog();
                    }
                    else
                    {
                        MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), "ITEM UPDATE FAILED!", MessageAlertImage.Alert());
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
        private void SAVE_ITEMS()
        {
            MySqlCommand cmd = CONNECTION.CON.CreateCommand();
            MySqlTransaction myTrans;
            myTrans = CONNECTION.CON.BeginTransaction();
            cmd.Transaction = myTrans;
            cmd.Connection = CONNECTION.CON;
            try
            {
                CONNECTION.open_connection();

                cmd.CommandText = "INSERT INTO item (barcode,item_category,item_name,item_type_id,item_status,qty_handle) VALUES(@barcode,@item_category,@item_name,@item_type_id,@item_status,@qty_handle)";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@barcode", "N/A");
                cmd.Parameters.AddWithValue("@item_category", CMB_CATEGORY.SelectedValue);
                cmd.Parameters.AddWithValue("@item_name", TXT_ITEMNAME.Text);
                cmd.Parameters.AddWithValue("@item_type_id", CMB_ITEM_TYPE.SelectedValue);
                cmd.Parameters.AddWithValue("@item_status", "ENABLE");
                if(CHK_QTY_HANDEL.Checked==true)
                    cmd.Parameters.AddWithValue("@qty_handle", "0");
                else
                    cmd.Parameters.AddWithValue("@qty_handle", "1");
                cmd.ExecuteNonQuery();

                LBL_ITEM_ID.Text = GETMAXID();
                if (TXT_BARCODE.Text==String.Empty)
                {
                    cmd.CommandText = "UPDATE item SET barcode=@barcode WHERE item_id=@item_id";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@item_id", LBL_ITEM_ID.Text);
                    cmd.Parameters.AddWithValue("@barcode", LBL_ITEM_ID.Text);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    cmd.CommandText = "UPDATE item SET barcode=@barcode WHERE item_id=@item_id";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@item_id", LBL_ITEM_ID.Text);
                    cmd.Parameters.AddWithValue("@barcode", TXT_BARCODE.Text);
                    cmd.ExecuteNonQuery();
                }

                if (CHK_QTY_HANDEL.Checked == true)
                {
                    cmd.CommandText = "INSERT INTO stock(item_code,qty,cost_price,sales_price) VALUES(@item_code,@qty,@cost_price,@sales_price)";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@item_code", LBL_ITEM_ID.Text);
                    cmd.Parameters.AddWithValue("@qty", 0.00);
                    cmd.Parameters.AddWithValue("@cost_price", Convert.ToDouble(TXT_COST_PRICE.Text));
                    cmd.Parameters.AddWithValue("@sales_price", Convert.ToDouble(TXT_SALES_PRICE.Text));
                    cmd.ExecuteNonQuery();
                }

                myTrans.Commit();  
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "ITEM ADDED SUCCESSFULLY!", MessageAlertImage.Success());
                mdg.ShowDialog();

            }
            catch (Exception ex)
            {
                myTrans.Rollback();
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
            LBL_ITEM_ID.Text = "N/A";
            CMB_CATEGORY.SelectedIndex = -1;
            TXT_BARCODE.Clear();
            TXT_ITEMNAME.Clear();
            CHK_SEARCH_ITEMNAME.Checked = false;
            CHK_QTY_HANDEL.Checked = false;
            CMB_ITEM_TYPE.SelectedIndex = -1;
        }
        private void DisableControls()
        {
            TXT_BARCODE.Enabled = false;
            CMB_CATEGORY.Enabled = false;
            TXT_ITEMNAME.Enabled = false;
            CHK_SEARCH_ITEMNAME.Enabled = false;
            CMB_ITEM_TYPE.Enabled = false;
            CHK_QTY_HANDEL.Enabled = false;
            TXT_COST_PRICE.Enabled = false;
            TXT_SALES_PRICE.Enabled = false;
            button1.Enabled = false;
            BTN_SAVE.Enabled = false;
        }
        private void EnableControls()
        {
            TXT_BARCODE.Enabled = true;
            CMB_CATEGORY.Enabled = true;
            TXT_ITEMNAME.Enabled = true;
            CHK_SEARCH_ITEMNAME.Enabled = true;
            CMB_ITEM_TYPE.Enabled = true;
            CHK_QTY_HANDEL.Enabled = true;
            TXT_COST_PRICE.Enabled = true;
            TXT_SALES_PRICE.Enabled = true;
            button1.Enabled = true;
            BTN_SAVE.Enabled = true;
        }
        private void BTN_SAVE_Click(object sender, EventArgs e)
        {
            if (CMB_CATEGORY.SelectedIndex == -1)
            {
                CMB_CATEGORY.Focus();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Warning(), "PLEASE SELECT CATEGORY", MessageAlertImage.Warning());
                mdg.ShowDialog();
            }
            else if (TXT_ITEMNAME.Text == String.Empty)
            {
                TXT_ITEMNAME.Focus();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Warning(), "PLEASE ENTER ITEM NAME", MessageAlertImage.Warning());
                mdg.ShowDialog();
            }
            else if (CMB_ITEM_TYPE.SelectedIndex < 0)
            {
                CMB_ITEM_TYPE.Focus();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Warning(), "PLEASE SELECT ITEM TYPE", MessageAlertImage.Warning());
                mdg.ShowDialog();
            }
            else if(BTN_SAVE.Text=="SAVE")
            {
                SAVE_ITEMS();
                LOAD_ITEM_lIST();
                BTN_SAVE.Text = "SAVE";
                DisableControls();
                BTN_NEW.Focus();
            }
            else if(BTN_SAVE.Text == "UPDATE")
            {
                UPDATE_ITEMS();
                LOAD_ITEM_lIST();
                BTN_NEW.Focus();
            }
        }
        private void LOAD_ITEM_lIST()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter DA = new MySqlDataAdapter("SELECT item.item_id , item.barcode , category.caegory_name , item.item_name , item.item_status, item.item_category,item_type.type,item.item_type_id FROM item INNER JOIN category ON (item.item_category = category.categry_id) INNER JOIN item_type ON (item.item_type_id=item_type.id)", CONNECTION.CON))
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
        private void ITEMS_Load(object sender, EventArgs e)
        {
            CLS_ITEM.LOAD_CATEGORY(CMB_CATEGORY);
            LOAD_ITEM_lIST();
            TXT_BARCODE.Focus();
            this.toolTip1.SetToolTip(this.button1, "Press This to Refresh Category List");
        }
        private void LOAD_ITEMS_lIST_SEARCH_BY_NAME(System.Windows.Forms.TextBox txt_cat_name)
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter DA = new MySqlDataAdapter("SELECT item.item_id , item.barcode , category.caegory_name , item.item_name , item.item_status, item.item_category,item_type.type,item.item_type_id FROM item INNER JOIN category ON (item.item_category = category.categry_id) INNER JOIN item_type ON (item.item_type_id=item_type.id) WHERE item.item_name LIKE @item_name ", CONNECTION.CON))
                {
                    DA.SelectCommand.Parameters.Clear();
                    DA.SelectCommand.Parameters.AddWithValue("@item_name", "%" + TXT_ITEMNAME.Text + "%");
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
        private void BTN_NEW_Click(object sender, EventArgs e)
        {
            if (BTN_SAVE.Enabled == false)
                EnableControls();

            clearAll();
            TXT_BARCODE.Focus();
            CHK_QTY_HANDEL.Enabled = true;
            BTN_SAVE.Text = "SAVE";
        }

        private void DGV_ITEM_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            LBL_ITEM_ID.Text = DGV_ITEM.SelectedRows[0].Cells["Column1"].Value.ToString();
            TXT_BARCODE.Text = DGV_ITEM.SelectedRows[0].Cells["Column4"].Value.ToString();
            TXT_ITEMNAME.Text= DGV_ITEM.SelectedRows[0].Cells["Column2"].Value.ToString();
            CMB_CATEGORY.SelectedValue = Convert.ToInt32(DGV_ITEM.SelectedRows[0].Cells["Column6"].Value);
            CMB_ITEM_TYPE.SelectedValue = Convert.ToInt32(DGV_ITEM.SelectedRows[0].Cells["Column7"].Value);
            CHK_QTY_HANDEL.Checked = false;
            CHK_QTY_HANDEL.Enabled = false;
            BTN_SAVE.Text = "UPDATE";
        }

        private void TXT_BARCODE_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                CMB_CATEGORY.Focus();
            }
        }

        private void CMB_CATEGORY_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TXT_ITEMNAME.Focus();
            }
        }

        private void TXT_ITEMNAME_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CMB_ITEM_TYPE.Focus();
            }
        }
       
        private void TXT_ITEMNAME_TextChanged(object sender, EventArgs e)
        {
            if (CHK_SEARCH_ITEMNAME.Checked == true)
            {
                if (TXT_ITEMNAME.Text.Length > 0)
                {
                    LOAD_ITEMS_lIST_SEARCH_BY_NAME(TXT_ITEMNAME);
                }
                else
                {
                    LOAD_ITEM_lIST();
                }

            }
        }

        private void CHK_SEARCH_ITEMNAME_CheckedChanged(object sender, EventArgs e)
        {

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
            CLS_ITEM.LOAD_CATEGORY(CMB_CATEGORY);
        }

        private void CHK_QTY_HANDEL_CheckedChanged(object sender, EventArgs e)
        {
            if (CHK_QTY_HANDEL.Checked == true)
            {
                TXT_COST_PRICE.Enabled = true;
                TXT_SALES_PRICE.Enabled = true;
            }
            else
            {
                TXT_COST_PRICE.Text = "0.00";
                TXT_SALES_PRICE.Text = "0.00";
                TXT_COST_PRICE.Enabled = false;
                TXT_SALES_PRICE.Enabled = false;
            }
        }

        private void CMB_ITEM_TYPE_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (CHK_QTY_HANDEL.Enabled == true)
                    CHK_QTY_HANDEL.Focus();
                else
                    BTN_SAVE.Focus();
            }
        }

        private void CHK_QTY_HANDEL_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (CHK_QTY_HANDEL.Checked == true)
                    TXT_COST_PRICE.Focus();
                else
                    BTN_SAVE.Focus();
            }
        }

        private void TXT_COST_PRICE_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                TXT_SALES_PRICE.Focus();
            }
        }

        private void TXT_SALES_PRICE_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                BTN_SAVE.Focus();
            }
        }
    }
}
