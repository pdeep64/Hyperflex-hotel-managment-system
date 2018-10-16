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
    public partial class DAILY_MENU : Form
    {
        bool frm_show = false;
        public DAILY_MENU()
        {
            InitializeComponent();
           
            LOAD_DAY_MENU(DTP_MENU_DATE.Value.ToString("yyyy-MM-dd"));
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

        private void button1_Click(object sender, EventArgs e)
        {
          
        }

        private void BTN_CLEAR_CAT_Click(object sender, EventArgs e)
        {
           
        }

        private void LOAD_DAY_MENU(string menu_date)
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter adp = new MySqlDataAdapter("SELECT id,item_stock_id,i.barcode,meal_type,i.item_name FROM daily_menu dm JOIN stock s ON dm.item_stock_id=s.stock_id JOIN item i ON s.item_code=i.item_id WHERE menu_date=@menu_date", CONNECTION.CON))
                {
                    adp.SelectCommand.Parameters.Clear();
                    adp.SelectCommand.Parameters.AddWithValue("@menu_date", menu_date);
                    DataTable tbl = new DataTable();
                    adp.Fill(tbl);
                    DGV_MENU.DataSource = null;
                    if (tbl.Rows.Count > 0)
                    {
                        DGV_MENU.AutoGenerateColumns = false;
                        DGV_MENU.DataSource = tbl;
                    }
                }
            }
            catch (Exception EX)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), EX.Message, MessageAlertImage.Error());
                mdg.ShowDialog();
            }
        }

        private void LOAD_ITEMS(int category_id)
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter adp = new MySqlDataAdapter("SELECT s.stock_id,i.barcode,i.item_name,s.sales_price,s.item_code FROM stock s JOIN item i ON s.item_code=item_id WHERE i.item_category=@item_category AND i.item_status='ENABLE'", CONNECTION.CON))
                {
                    adp.SelectCommand.Parameters.Clear();
                    adp.SelectCommand.Parameters.AddWithValue("@item_category", category_id);
                    DataTable tbl = new DataTable();
                    adp.Fill(tbl);
                    LST_ITEMS.Items.Clear();
                    if (tbl.Rows.Count > 0)
                    {
                        foreach (DataRow DR in tbl.Rows)
                        {
                            ListViewItem lt = new ListViewItem(DR["barcode"].ToString());
                            lt.SubItems.Add(DR["item_name"].ToString());
                            lt.SubItems.Add(DR["sales_price"].ToString());
                            lt.SubItems.Add(DR["item_code"].ToString());
                            lt.SubItems.Add(DR["stock_id"].ToString());
                            LST_ITEMS.Items.Add(lt);
                        }


                    }
                }
            }
            catch (Exception EX)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), EX.Message, MessageAlertImage.Error());
                mdg.ShowDialog();
            }
        }

        private void CMB_CATEGORY_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CMB_CATEGORY.SelectedIndex != -1)
            {

                if (frm_show==false)
                {
                    LOAD_ITEMS(Convert.ToInt32(CMB_CATEGORY.SelectedValue)); 
                }
            }
        }



        private void BTN_ADD_TO_MENU_Click(object sender, EventArgs e)
        {
            MySqlCommand cmd = CONNECTION.CON.CreateCommand();
            MySqlTransaction myTrans=null;
            try
            {
                CONNECTION.open_connection();
                cmd.Connection = CONNECTION.CON;
                cmd.Transaction = myTrans;
                myTrans = CONNECTION.CON.BeginTransaction();

                if (CMB_MEAL_TYPE.SelectedIndex <= -1)
                {
                    MSGBOX mdg = new MSGBOX(MessageAlertHeder.Warning(), "PLEASE SELECT MEAL TYPE", MessageAlertImage.Warning());
                    mdg.ShowDialog();
                    CMB_MEAL_TYPE.Focus();
                }
                else if(LST_ITEMS.CheckedItems.Count <= 0)
                {
                    MSGBOX mdg = new MSGBOX(MessageAlertHeder.Warning(), "PLEASE SELECT ITEMS TO ADD", MessageAlertImage.Warning());
                    mdg.ShowDialog();
                    LST_ITEMS.Focus();
                }
                else
                {
                    foreach (ListViewItem itm in LST_ITEMS.CheckedItems)
                    {
                        using(MySqlDataAdapter adp=new MySqlDataAdapter("SELECT id FROM daily_menu WHERE menu_date=@menu_date AND item_stock_id=@item_stock_id", CONNECTION.CON))
                        {
                            adp.SelectCommand.Parameters.Clear();
                            adp.SelectCommand.Parameters.AddWithValue("@menu_date", DTP_MENU_DATE.Value.ToString("yyyy-MM-dd"));
                            adp.SelectCommand.Parameters.AddWithValue("@item_stock_id", Convert.ToInt32(itm.SubItems[4].Text));
                            DataTable tbl = new DataTable();
                            adp.Fill(tbl);
                            if (tbl.Rows.Count <= 0)
                            {
                                cmd.CommandText = "INSERT INTO daily_menu(meal_type,item_stock_id,menu_date,added_by) VALUES(@meal_type,@item_stock_id,@menu_date,@added_by)";
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@meal_type", CMB_MEAL_TYPE.Text);
                                cmd.Parameters.AddWithValue("@item_stock_id", Convert.ToInt32(itm.SubItems[4].Text));
                                cmd.Parameters.AddWithValue("@menu_date", DTP_MENU_DATE.Value.ToString("yyyy-MM-dd"));
                                cmd.Parameters.AddWithValue("@added_by", Convert.ToInt32(CLS_CURRENT_LOGGER.LOGGED_IN_USERID));
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    myTrans.Commit();
                    MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "MENU ITEMS ADDED SUCCESSFULLY", MessageAlertImage.Success());
                    mdg.ShowDialog();
                    LOAD_DAY_MENU(DTP_MENU_DATE.Value.ToString("yyyy-MM-dd"));
                    foreach (ListViewItem itm in LST_ITEMS.CheckedItems)
                    {
                        itm.Checked = false;
                    }
                }
            }
            catch (Exception EX)
            {
                myTrans.Rollback();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), EX.Message, MessageAlertImage.Error());
                mdg.ShowDialog();
            }
            finally
            {
                CONNECTION.close_connection();
            }
        }

        private void DTP_MENU_DATE_ValueChanged(object sender, EventArgs e)
        {
            LOAD_DAY_MENU(DTP_MENU_DATE.Value.ToString("yyyy-MM-dd"));
        }

        private void BTN_REMOVE_Click(object sender, EventArgs e)
        {
            if (DGV_MENU.SelectedRows.Count > 0)
            {
                MySqlCommand cmd = CONNECTION.CON.CreateCommand();
                MySqlTransaction myTrans=null;
                try
                {
                    CONNECTION.open_connection();
                    cmd.Connection = CONNECTION.CON;
                    cmd.Transaction = myTrans;
                    myTrans = CONNECTION.CON.BeginTransaction();

                    foreach (DataGridViewRow dr in DGV_MENU.SelectedRows)
                    {
                        cmd.CommandText = "DELETE FROM daily_menu WHERE id=@id";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@id", dr.Cells[0].Value);
                        cmd.ExecuteNonQuery();
                    }
                    myTrans.Commit();
                    MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "SELECTED ITEMS REMOVED SUCCESSFULLY", MessageAlertImage.Success());
                    mdg.ShowDialog();
                    LOAD_DAY_MENU(DTP_MENU_DATE.Value.ToString("yyyy-MM-dd"));
                }
                catch (Exception EX)
                {
                    myTrans.Rollback();
                    MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), EX.Message, MessageAlertImage.Error());
                    mdg.ShowDialog();
                }
                finally
                {
                    CONNECTION.close_connection();
                }
            }
        }

        private void DAILY_MENU_Load(object sender, EventArgs e)
        {
            frm_show = true;
            string query = "SELECT DISTINCT i.item_category,c.caegory_name FROM item i JOIN category c ON i.item_category=c.categry_id WHERE i.item_type_id=1 OR i.item_type_id=2";
            CLS_METHODS.FILL_COMBOBOX(CMB_CATEGORY, query, "caegory_name", "item_category", 0);
            frm_show = false;
        }
    }
}
