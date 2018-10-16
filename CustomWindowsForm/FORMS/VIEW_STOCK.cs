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
    public partial class VIEW_STOCK : Form
    {
        public VIEW_STOCK()
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
        private void LOAD_ALL_ITEMS()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter DA=new MySqlDataAdapter("SELECT stock.stock_id AS STOCKID , item.item_id AS ITEMID , item.barcode AS BARCODE , item.item_name AS ITEM_NAME , item.item_status AS ITEM_STATUS , category.categry_id AS CATEGORYID, category.caegory_name AS CATEGORY_NAME, stock.qty AS QTY , stock.cost_price AS COST_PRICE , stock.sales_price AS SALE_PRICE , item_type.id AS TYPE_ID, item_type.type AS ITEM_TYPE FROM stock INNER JOIN item ON (stock.item_code = item.item_id) INNER JOIN item_type ON (item.item_type_id = item_type.id) INNER JOIN category ON (item.item_category = category.categry_id) WHERE item.item_status='ENABLE'", CONNECTION.CON))
                {
                    DataTable DT = new DataTable();
                    DA.Fill(DT);
                    if(DT.Rows.Count>0)
                    {
                        dataGridView1.DataSource = DT;
                        dataGridView1.AutoGenerateColumns = false;
                        dataGridView1.Columns[0].Visible = false;
                        dataGridView1.Columns[1].Visible = false;
                        dataGridView1.Columns[5].Visible = false;
                        dataGridView1.Columns[10].Visible = false;
                        dataGridView1.Columns[3].Width = 200;
                        dataGridView1.Columns[7].DefaultCellStyle.Format = "F3";
                        dataGridView1.Columns[8].DefaultCellStyle.Format = "F2";
                        dataGridView1.Columns[9].DefaultCellStyle.Format = "F2";
                        LBL_COUNT.Text = DT.Rows.Count.ToString();
                    }
                    else
                    {
                        dataGridView1.DataSource = null;
                        LBL_COUNT.Text = "0";
                    }
                }
            }
            catch (Exception EX)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), EX.Message, MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
            finally
            {
                CONNECTION.close_connection();
            }
        }
        private void LOAD_ALL_ITEMS_BY_ITEM_NAME()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter DA = new MySqlDataAdapter("SELECT stock.stock_id AS STOCKID , item.item_id AS ITEMID , item.barcode AS BARCODE , item.item_name AS ITEM_NAME , item.item_status AS ITEM_STATUS , category.categry_id AS CATEGORYID, category.caegory_name AS CATEGORY_NAME, stock.qty AS QTY , stock.cost_price AS COST_PRICE , stock.sales_price AS SALE_PRICE , item_type.id AS TYPE_ID, item_type.type AS ITEM_TYPE FROM stock INNER JOIN item ON (stock.item_code = item.item_id) INNER JOIN item_type ON (item.item_type_id = item_type.id) INNER JOIN category ON (item.item_category = category.categry_id) WHERE item.item_status='ENABLE' AND item.item_name LIKE @item_name", CONNECTION.CON))
                {
                    DA.SelectCommand.Parameters.Clear();
                    DA.SelectCommand.Parameters.AddWithValue("@item_name","%"+TXT_ITEM_NAME.Text+"%");
                    DataTable DT = new DataTable();
                    DA.Fill(DT);
                    if (DT.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = DT;
                        dataGridView1.AutoGenerateColumns = false;
                        dataGridView1.Columns[0].Visible = false;
                        dataGridView1.Columns[1].Visible = false;
                        dataGridView1.Columns[5].Visible = false;
                        dataGridView1.Columns[10].Visible = false;
                        dataGridView1.Columns[3].Width = 200;
                        dataGridView1.Columns[7].DefaultCellStyle.Format = "F3";
                        dataGridView1.Columns[8].DefaultCellStyle.Format = "F2";
                        dataGridView1.Columns[9].DefaultCellStyle.Format = "F2";
                        LBL_COUNT.Text = DT.Rows.Count.ToString();
                    }
                    else
                    {
                        dataGridView1.DataSource = null;
                        LBL_COUNT.Text = "0";
                    }
                }
            }
            catch (Exception EX)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), EX.Message, MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
            finally
            {
                CONNECTION.close_connection();
            }
        }
        private void LOAD_ALL_ITEMS_BY_ITEM_NAME_ALL()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter DA = new MySqlDataAdapter("SELECT stock.stock_id AS STOCKID , item.item_id AS ITEMID , item.barcode AS BARCODE , item.item_name AS ITEM_NAME , item.item_status AS ITEM_STATUS , category.categry_id AS CATEGORYID, category.caegory_name AS CATEGORY_NAME, stock.qty AS QTY , stock.cost_price AS COST_PRICE , stock.sales_price AS SALE_PRICE , item_type.id AS TYPE_ID, item_type.type AS ITEM_TYPE FROM stock INNER JOIN item ON (stock.item_code = item.item_id) INNER JOIN item_type ON (item.item_type_id = item_type.id) INNER JOIN category ON (item.item_category = category.categry_id) WHERE item.item_status='ENABLE' AND (item.item_name LIKE @item_name || item.item_category=@item_category || item.item_type_id=@item_type_id)", CONNECTION.CON))
                {
                    DA.SelectCommand.Parameters.Clear();
                    DA.SelectCommand.Parameters.AddWithValue("@item_name", "%" + TXT_ITEM_NAME.Text + "%");
                    DA.SelectCommand.Parameters.AddWithValue("@item_category", CMB_CATEGORY.SelectedValue);
                    DA.SelectCommand.Parameters.AddWithValue("@item_type_id", CMB_ITEM_TYPE.SelectedValue);
                    DataTable DT = new DataTable();
                    DA.Fill(DT);
                    if (DT.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = DT;
                        dataGridView1.AutoGenerateColumns = false;
                        dataGridView1.Columns[0].Visible = false;
                        dataGridView1.Columns[1].Visible = false;
                        dataGridView1.Columns[5].Visible = false;
                        dataGridView1.Columns[10].Visible = false;
                        dataGridView1.Columns[3].Width = 200;
                        dataGridView1.Columns[7].DefaultCellStyle.Format = "F3";
                        dataGridView1.Columns[8].DefaultCellStyle.Format = "F2";
                        dataGridView1.Columns[9].DefaultCellStyle.Format = "F2";
                        LBL_COUNT.Text = DT.Rows.Count.ToString();
                    }
                    else
                    {
                        dataGridView1.DataSource = null;
                        LBL_COUNT.Text = "0";
                    }
                }
            }
            catch (Exception EX)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), EX.Message, MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
            finally
            {
                CONNECTION.close_connection();
            }
        }
        private void LOAD_ALL_ITEMS_BY_CATEGORY()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter DA = new MySqlDataAdapter("SELECT stock.stock_id AS STOCKID , item.item_id AS ITEMID , item.barcode AS BARCODE , item.item_name AS ITEM_NAME , item.item_status AS ITEM_STATUS , category.categry_id AS CATEGORYID, category.caegory_name AS CATEGORY_NAME, stock.qty AS QTY , stock.cost_price AS COST_PRICE , stock.sales_price AS SALE_PRICE , item_type.id AS TYPE_ID, item_type.type AS ITEM_TYPE FROM stock INNER JOIN item ON (item.item_id = stock.item_code) INNER JOIN item_type ON (item.item_type_id = item_type.id) INNER JOIN category ON (item.item_category = category.categry_id) WHERE item.item_status='ENABLE' AND  item.item_category=@item_category", CONNECTION.CON))
                {
                    DA.SelectCommand.Parameters.Clear();
                    DA.SelectCommand.Parameters.AddWithValue("@item_category", CMB_CATEGORY.SelectedValue);
                    DataTable DT = new DataTable();
                    DA.Fill(DT);
                    if (DT.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = DT;
                        dataGridView1.AutoGenerateColumns = false;
                        dataGridView1.Columns[0].Visible = false;
                        dataGridView1.Columns[1].Visible = false;
                        dataGridView1.Columns[5].Visible = false;
                        dataGridView1.Columns[10].Visible = false;
                        dataGridView1.Columns[3].Width = 200;
                        dataGridView1.Columns[7].DefaultCellStyle.Format = "F3";
                        dataGridView1.Columns[8].DefaultCellStyle.Format = "F2";
                        dataGridView1.Columns[9].DefaultCellStyle.Format = "F2";
                        LBL_COUNT.Text = DT.Rows.Count.ToString();
                    }
                    else
                    {
                        dataGridView1.DataSource = null;
                        LBL_COUNT.Text = "0";
                    }
                }
            }
            catch (Exception EX)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), EX.Message, MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
            finally
            {
                CONNECTION.close_connection();
            }
        }
        private void LOAD_ALL_ITEMS_BY_ITEM_TYPE()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter DA = new MySqlDataAdapter("SELECT stock.stock_id AS STOCKID , item.item_id AS ITEMID , item.barcode AS BARCODE , item.item_name AS ITEM_NAME , item.item_status AS ITEM_STATUS , category.categry_id AS CATEGORYID, category.caegory_name AS CATEGORY_NAME, stock.qty AS QTY , stock.cost_price AS COST_PRICE , stock.sales_price AS SALE_PRICE , item_type.id AS TYPE_ID, item_type.type AS ITEM_TYPE FROM stock INNER JOIN item ON (stock.item_code = item.item_id) INNER JOIN item_type ON (item.item_type_id = item_type.id) INNER JOIN category ON (item.item_category = category.categry_id) WHERE item.item_status='ENABLE' AND  item.item_type_id=@item_type_id", CONNECTION.CON))
                {
                    DA.SelectCommand.Parameters.Clear();
                    DA.SelectCommand.Parameters.AddWithValue("@item_type_id", CMB_ITEM_TYPE.SelectedValue);
                    DataTable DT = new DataTable();
                    DA.Fill(DT);
                    if (DT.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = DT;
                        dataGridView1.AutoGenerateColumns = false;
                        dataGridView1.Columns[0].Visible = false;
                        dataGridView1.Columns[1].Visible = false;
                        dataGridView1.Columns[5].Visible = false;
                        dataGridView1.Columns[10].Visible = false;
                        dataGridView1.Columns[3].Width = 200;
                        dataGridView1.Columns[7].DefaultCellStyle.Format = "F3";
                        dataGridView1.Columns[8].DefaultCellStyle.Format = "F2";
                        dataGridView1.Columns[9].DefaultCellStyle.Format = "F2";
                        LBL_COUNT.Text = DT.Rows.Count.ToString();
                    }
                    else
                    {
                        dataGridView1.DataSource = null;
                        LBL_COUNT.Text = "0";
                    }
                }
            }
            catch (Exception EX)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), EX.Message, MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
            finally
            {
                CONNECTION.close_connection();
            }
        }
        private void BTN_NEW_Click(object sender, EventArgs e)
        {
            LOAD_ALL_ITEMS();
        }

        private void VIEW_STOCK_Load(object sender, EventArgs e)
        {
            LOAD_ALL_ITEMS();
            String QRY1 = "SELECT categry_id, caegory_name FROM category";
            String QRY2 = "SELECT id, TYPE FROM item_type";
            CLS_METHODS.FILL_COMBOBOX(CMB_CATEGORY, QRY1, "caegory_name", "categry_id", -1);
            CLS_METHODS.FILL_COMBOBOX(CMB_ITEM_TYPE, QRY2, "TYPE", "id", -1);
        }

        private void TXT_QTY_TextChanged(object sender, EventArgs e)
        {
            if(TXT_ITEM_NAME.Text.Length>1 && CMB_CATEGORY.SelectedIndex==-1 && CMB_ITEM_TYPE.SelectedIndex==-1)
            {
                LOAD_ALL_ITEMS_BY_ITEM_NAME();
            }
            else if (TXT_ITEM_NAME.Text.Length > 1 && (CMB_CATEGORY.SelectedIndex != -1 || CMB_ITEM_TYPE.SelectedIndex != -1))
            {
                CMB_CATEGORY.SelectedIndex = -1;
                CMB_ITEM_TYPE.SelectedIndex = -1;
                LOAD_ALL_ITEMS_BY_ITEM_NAME_ALL();
            }
            else
            {
                LOAD_ALL_ITEMS();
            }
        }

        private void CMB_CATEGORY_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(CMB_CATEGORY.SelectedIndex != -1)
            {
                CMB_ITEM_TYPE.SelectedIndex = -1;
                LOAD_ALL_ITEMS_BY_CATEGORY();
            }
        }

        private void CMB_ITEM_TYPE_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CMB_ITEM_TYPE.SelectedIndex != -1)
            {
                CMB_CATEGORY.SelectedIndex = -1;
                LOAD_ALL_ITEMS_BY_ITEM_TYPE();

            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
           
        }

        private void hyflexTextbox1_TextChanged(object sender, EventArgs e)
        {
            if(hyflexTextbox1.Text.Length>0)
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Format("ITEM_NAME LIKE '%{0}%'", hyflexTextbox1.Text);
            }
            else
            {
                LOAD_ALL_ITEMS();
            }
          
        }
    }
}
