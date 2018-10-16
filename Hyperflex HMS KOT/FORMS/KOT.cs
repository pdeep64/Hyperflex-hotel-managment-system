using DevExpress.XtraEditors;
using Hyperflex_HMS_KOT.CLASS;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Hyperflex_HMS_KOT.FORMS
{
    public partial class KOT : Form
    {
        public KOT(bool update_order,string order_no)
        {
            InitializeComponent();
          //  FLP_ITEM.Controls.Clear();
            LOAD_RESERVATIONS();
            LOAD_ROOMS();
            LOAD_TABLES();
            LOAD_CATEGORIES();
            reg_tax_amount = CLS_TAX.GetTotalTaxPercentage();
            tax_amount = reg_tax_amount;

            if (update_order == true)
            {
                LBL_ORDER_NO.Text = order_no;
                BTN_ORDER.ButtonText = "UPDATE";
                BTN_ORDER.TextLocation_X =15;
                LOAD_ORDER_DETAILS(order_no);
                BTN_PRINT.Enabled = true;
                BTN_SERVED.Enabled = true;
            }
        }

        private void LOAD_ORDER_DETAILS(string order_no)
        {
            try
            {
                CONNECTION.open_connection();

                using (MySqlDataAdapter adp = new MySqlDataAdapter("SELECT r.tax_status FROM kot_order ko LEFT JOIN reservation r ON ko.reservation_no=r.reservation_id WHERE ko.order_no=@order_no", CONNECTION.CON))
                {
                    adp.SelectCommand.Parameters.Clear();
                    adp.SelectCommand.Parameters.AddWithValue("@order_no", order_no);
                    DataTable tbl = new DataTable();
                    adp.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        if (tbl.Rows[0].Field<string>(0) == "1")
                            tax_amount = reg_tax_amount;
                        else if (tbl.Rows[0].Field<string>(0) == "0")
                            tax_amount = 0;
                        else
                            tax_amount = reg_tax_amount;
                    }
                    else
                        tax_amount = reg_tax_amount;
                }

                

                using (MySqlDataAdapter adp=new MySqlDataAdapter("SELECT IFNULL(ko.reservation_no,'N/A'),IFNULL(r.room_name,'N/A'),IFNULL(ko.room_id,0),IFNULL(rt.table_no,'N/A'),IFNULL(ko.table_id,0),ko.special_note,ko.guest_id,CONCAT_WS(' ',g.first_name,g.last_name) AS guest_name FROM kot_order ko LEFT JOIN room r ON ko.room_id=r.room_id LEFT JOIN resturant_table rt ON ko.table_id=rt.table_id JOIN guest g ON ko.guest_id=g.guest_id WHERE ko.order_no=@order_no", CONNECTION.CON))
                {
                    adp.SelectCommand.Parameters.Clear();
                    adp.SelectCommand.Parameters.AddWithValue("@order_no", order_no);
                    DataTable tbl = new DataTable();
                    adp.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        foreach(DataRow row in tbl.Rows)
                        {
                            LBL_RESERVATION_NO.Text = row[0].ToString();
                            if(LBL_RESERVATION_NO.Text=="N/A")
                            {
                                LBL_GUEST_NAME.Text = row[7].ToString();
                                SELECT_GUEST.SELECTED_GUEST_ID = row[6].ToString();
                            }

                            LBL_ROOM.Text = row[1].ToString();
                            _ROOM_ID = Convert.ToInt32(row[2].ToString());
                            LBL_TABLE.Text = row[3].ToString();
                            _TABLE_ID = Convert.ToInt32(row[4].ToString());
                            SPECIAL_NOTE.SP_NOTE = row[5].ToString();
                        }
                    }
                }

                using (MySqlDataAdapter adp = new MySqlDataAdapter("SELECT koi.item_stock_id,s.item_code,i.barcode,i.item_name,koi.order_qty,koi.unit_price,koi.total_price,i.qty_handle,koi.cost_price FROM kot_order_item koi JOIN stock s ON s.stock_id=koi.item_stock_id JOIN item i ON s.item_code=i.item_id WHERE koi.order_no=@order_no", CONNECTION.CON))
                {
                    adp.SelectCommand.Parameters.Clear();
                    adp.SelectCommand.Parameters.AddWithValue("@order_no", order_no);
                    DataTable tbl = new DataTable();
                    adp.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        foreach (DataRow row in tbl.Rows)
                        {
                            int x = DGV_ORDER_ITEMS.Rows.Add();
                            DGV_ORDER_ITEMS.Rows[x].Cells[0].Value = row.Field<Int32>(0).ToString();
                            DGV_ORDER_ITEMS.Rows[x].Cells[1].Value = row.Field<Int32>(1).ToString();
                            DGV_ORDER_ITEMS.Rows[x].Cells[2].Value = row.Field<string>(2);
                            DGV_ORDER_ITEMS.Rows[x].Cells[3].Value = row.Field<string>(3);
                            DGV_ORDER_ITEMS.Rows[x].Cells[4].Value = row.Field<double>(4).ToString();
                            DGV_ORDER_ITEMS.Rows[x].Cells[5].Value = row.Field<double>(5).ToString("F2");
                            DGV_ORDER_ITEMS.Rows[x].Cells[6].Value = row.Field<double>(6).ToString("F2");
                            DGV_ORDER_ITEMS.Rows[x].Cells[7].Value = row.Field<string>(7);
                            DGV_ORDER_ITEMS.Rows[x].Cells[8].Value = row.Field<double>(8).ToString("F2");
                        }
                        LBL_TOT_PRICE.Text = GET_ORDER_TOTAL_PRICE().ToString("F2");
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

        private void DISABLE_CONTROLS()
        {
            FLP_RESERVATON.Enabled = false;
            FLP_ROOM.Enabled = false;
            PNL_TABLE.Enabled = false;
            FLP_CATEGORY.Enabled = false;
            TXT_BARCODE.Enabled = false;
            TXT_ITEM_NAME.Enabled = false;
            Pnl_SelectedItems.Enabled = false;
            DGV_ORDER_ITEMS.Enabled = false;
            BTN_PLUS_QTY.Enabled = false;
            BTN_MINUS_QTY.Enabled = false;
            BTN_REMOVE.Enabled = false;
            BTN_ORDER.Enabled = false;
            BTN_PRINT.Enabled = true;
            BTN_SERVED.Enabled = true;
            BTN_PRINT.Focus();
        }

        private void ENABLE_CONTROLS()
        {
            FLP_RESERVATON.Enabled = true;
            FLP_ROOM.Enabled = true;
            PNL_TABLE.Enabled = true;
            FLP_CATEGORY.Enabled = true;
            TXT_BARCODE.Enabled = true;
            TXT_ITEM_NAME.Enabled = true;
            Pnl_SelectedItems.Enabled = true;
            DGV_ORDER_ITEMS.Enabled = true;
            BTN_PLUS_QTY.Enabled = true;
            BTN_MINUS_QTY.Enabled = true;
            BTN_REMOVE.Enabled = true;
            BTN_ORDER.Enabled = true;
            BTN_PRINT.Enabled = false;
            BTN_SERVED.Enabled = false;
        }

        private void CLEAR()
        {
            LBL_ORDER_NO.Text = "N/A";
            LBL_RESERVATION_NO.Text = "N/A";
            LBL_ROOM.Text = "N/A";
            LBL_TABLE.Text = "N/A";
            Pnl_SelectedItems.Groups.Clear();
            _ROOM_ID = 0;
            _TABLE_ID = 0;
            Pnl_SelectedItems.Controls.Clear();
            TXT_BARCODE.Clear();
            TXT_ITEM_NAME.Clear();
            DGV_ORDER_ITEMS.Rows.Clear();
            LBL_TOT_PRICE.Text = "0.00";
            BTN_ORDER.ButtonText = "ORDER";
            BTN_ORDER.TextLocation_X = 23;
            SELECT_GUEST.SELECTED_GUEST_ID = "";
            SELECT_GUEST.SELECTED_GUEST_NAME = "";
            LBL_GUEST_NAME.Text = "N/A";
            BTN_SELECT_GUEST.Enabled = true;
        }

        bool isTopPanelDragged = false;
        bool isWindowMaximized = false;
        Point offset;
        Size _normalWindowSize;
        Point _normalWindowLocation = Point.Empty;

        int _ROOM_ID = 0;
        int _TABLE_ID = 0;
        double reg_tax_amount = 0;
        double tax_amount = 0;

        private void SELECT_RESERVATION(object sender, EventArgs e, string reservation_no,string tax_status)
        {
            BTN_SELECT_GUEST.Enabled = false;
            DGV_ORDER_ITEMS.Rows.Clear();
            LBL_RESERVATION_NO.Text = reservation_no;
            LBL_ROOM.Text = "N/A";
            _ROOM_ID = 0;
            LOAD_ROOMS_BY_RES(reservation_no);
            if (tax_status == "0")
                tax_amount = 0;
            else
                tax_amount = reg_tax_amount;

            Pnl_SelectedItems.Controls.Clear();
        }

        private void SELECT_ROOM(object sender, EventArgs e, string room_name,int room_id,string reservation_no)
        {
            BTN_SELECT_GUEST.Enabled = false;
            DGV_ORDER_ITEMS.Rows.Clear();
            LBL_ROOM.Text = room_name;
            _ROOM_ID = room_id;
            LBL_RESERVATION_NO.Text = reservation_no;
        }

        private void SELECT_TABLE(object sender, EventArgs e, string table_no,int table_id)
        {
            LBL_TABLE.Text = table_no;
            _TABLE_ID = table_id;          
        }
        private void SELECT_CATEGORY(object sender, EventArgs e, int category_id)
        {
            LOAD_ITEMS_TO_TILES(category_id);
        }

        private void SELECT_ITEM(object sender, EventArgs e, int stock_id,int item_code,string barcode,string item_name,double qty,double sales_price,string qty_handle)
        {
            bool updated = false;
            foreach(DataGridViewRow row in DGV_ORDER_ITEMS.Rows)
            {
                if(Convert.ToInt32(row.Cells[0].Value.ToString())== stock_id && Convert.ToDouble(row.Cells[5].Value.ToString())== sales_price)
                {
                    double new_qty = Convert.ToDouble(row.Cells[4].Value) + qty;
                    row.Cells[4].Value = new_qty.ToString();
                    double new_price = new_qty * Convert.ToDouble(row.Cells[5].Value);
                    row.Cells[6].Value = new_price.ToString("F2");
                    updated = true;
                    break;
                }
            }
            if (updated == false)
            {
                int i = DGV_ORDER_ITEMS.Rows.Add();
                DGV_ORDER_ITEMS.Rows[i].Cells[0].Value = stock_id.ToString();
                DGV_ORDER_ITEMS.Rows[i].Cells[1].Value = item_code.ToString();
                DGV_ORDER_ITEMS.Rows[i].Cells[2].Value = barcode;
                DGV_ORDER_ITEMS.Rows[i].Cells[3].Value = item_name;
                DGV_ORDER_ITEMS.Rows[i].Cells[4].Value = qty.ToString();
                DGV_ORDER_ITEMS.Rows[i].Cells[5].Value = sales_price.ToString("F2");
                DGV_ORDER_ITEMS.Rows[i].Cells[6].Value = (qty * sales_price).ToString("F2");
                DGV_ORDER_ITEMS.Rows[i].Cells[7].Value = qty_handle;
            }

            LBL_TOT_PRICE.Text= GET_ORDER_TOTAL_PRICE().ToString("F2");

        }

        private void EDIT_PRICE_AND_SELECT_ITEM(object sender, EventArgs e, int stock_id, int item_code, string barcode, string item_name, double qty, double sales_price, string qty_handle,double cost_price)
        {
            if(LBL_RESERVATION_NO.Text=="N/A" && LBL_GUEST_NAME.Text == "N/A")
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Warning(), "Please SELECT RESERVATION OR GUEST TO ADD ITEMS", MessageAlertImage.Error());
                mdg.ShowDialog();
            }
            else
            {
                EDIT_ITEM_PRICE item_price = new EDIT_ITEM_PRICE(item_name, sales_price);
                item_price.ShowDialog();

                if (EDIT_ITEM_PRICE.ADD_ITEM == true)
                {
                    EDIT_ITEM_PRICE.NEW_PRICE = EDIT_ITEM_PRICE.NEW_PRICE + (EDIT_ITEM_PRICE.NEW_PRICE * (tax_amount / 100));
                    bool updated = false;
                    foreach (DataGridViewRow row in DGV_ORDER_ITEMS.Rows)
                    {
                        if (Convert.ToInt32(row.Cells[0].Value.ToString()) == stock_id && Convert.ToDouble(row.Cells[5].Value.ToString()) == EDIT_ITEM_PRICE.NEW_PRICE)
                        {
                            double new_qty = Convert.ToDouble(row.Cells[4].Value) + qty;
                            row.Cells[4].Value = new_qty.ToString();
                            double new_price = new_qty * EDIT_ITEM_PRICE.NEW_PRICE;
                            row.Cells[6].Value = new_price.ToString("F2");
                            updated = true;
                            break;
                        }
                    }
                    if (updated == false)
                    {
                        int i = DGV_ORDER_ITEMS.Rows.Add();
                        DGV_ORDER_ITEMS.Rows[i].Cells[0].Value = stock_id.ToString();
                        DGV_ORDER_ITEMS.Rows[i].Cells[1].Value = item_code.ToString();
                        DGV_ORDER_ITEMS.Rows[i].Cells[2].Value = barcode;
                        DGV_ORDER_ITEMS.Rows[i].Cells[3].Value = item_name;
                        DGV_ORDER_ITEMS.Rows[i].Cells[4].Value = qty.ToString();
                        DGV_ORDER_ITEMS.Rows[i].Cells[5].Value = EDIT_ITEM_PRICE.NEW_PRICE.ToString("F2");
                        DGV_ORDER_ITEMS.Rows[i].Cells[6].Value = (qty * EDIT_ITEM_PRICE.NEW_PRICE).ToString("F2");
                        DGV_ORDER_ITEMS.Rows[i].Cells[7].Value = qty_handle;
                        DGV_ORDER_ITEMS.Rows[i].Cells[8].Value = cost_price.ToString("F2");
                    }

                    LBL_TOT_PRICE.Text = GET_ORDER_TOTAL_PRICE().ToString("F2");
                }
            }
        }

        private double GET_ORDER_TOTAL_PRICE()
        {
            double tot_price = 0;
            foreach(DataGridViewRow row in DGV_ORDER_ITEMS.Rows)
            {
                tot_price += Convert.ToDouble(row.Cells[6].Value);
            }
            return tot_price;
        }

        private void LOAD_RESERVATIONS()
        {
            try
            {
                CONNECTION.open_connection();
                using(MySqlDataAdapter adp=new MySqlDataAdapter("SELECT r.reservation_id,CONCAT_WS(' ',g.first_name,g.last_name) AS guest_name,r.tax_status FROM reservation r JOIN guest g ON r.guest_id=g.guest_id WHERE r.status='CHECKED IN'", CONNECTION.CON))
                {
                    DataTable tbl = new DataTable();
                    adp.Fill(tbl);
                    FLP_RESERVATON.Controls.Clear();
                    if (tbl.Rows.Count > 0)
                    {
                        foreach(DataRow DR in tbl.Rows)
                        {
 
                            TileItem tileItem2 = new TileItem();
                            TileItemFrame logoDXFrame = new TileItemFrame();
                            TileItemElement e1 = new TileItemElement();
                            TileItemElement e2 = new TileItemElement();
                            logoDXFrame.Appearance.Options.UseFont = true;
                            logoDXFrame.Elements.Add(e1);
                            logoDXFrame.Elements.Add(e2);

                            logoDXFrame.Elements[0].AnimateTransition = DevExpress.Utils.DefaultBoolean.True;
                            logoDXFrame.Appearance.Font = new System.Drawing.Font("Consolas", 12F);
                            e1.Text = DR[0].ToString();
                            e2.Text = DR[1].ToString();

                            e2.TextAlignment = TileItemContentAlignment.BottomCenter;
                            e1.TextAlignment = TileItemContentAlignment.TopCenter;
                            logoDXFrame.Appearance.BackColor = System.Drawing.Color.Brown;
                            tileItem2.Frames.Add(logoDXFrame);
                            tileItem2.Appearance.BorderColor = System.Drawing.Color.Cyan;
                            tileItem2.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
                            tileItem2.FrameAnimationInterval = 2500;
                            tileItem2.ItemSize = TileItemSize.Wide;
                            FLP_RESERVATON.Groups.Add(new TileGroup());
                            FLP_RESERVATON.Groups[0].Items.Add(tileItem2);
                            tileItem2.ItemClick += (sender2, e) => SELECT_RESERVATION(sender2, e, DR[0].ToString(), DR[2].ToString());
                            tileItem2.StartAnimation();
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

        private void LOAD_ROOMS()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter adp = new MySqlDataAdapter("SELECT r.reservation_id,rr.room_id,rm.room_name FROM recerved_rooms rr JOIN reservation r ON rr.reservation_no=r.reservation_id JOIN room rm ON rr.room_id=rm.room_id WHERE r.status='CHECKED IN'", CONNECTION.CON))
                {
                    DataTable tbl = new DataTable();
                    adp.Fill(tbl);
                    FLP_ROOM.Controls.Clear();
                    if (tbl.Rows.Count > 0)
                    {
                        FLP_ROOM.Groups.Clear();
                        foreach (DataRow DR in tbl.Rows)
                        {
                            TileItem tileItem2 = new TileItem();
                            TileItemFrame logoDXFrame = new TileItemFrame();
                            TileItemElement e1 = new TileItemElement();
                            TileItemElement e2 = new TileItemElement();
                            logoDXFrame.Appearance.Options.UseFont = true;
                            logoDXFrame.Elements.Add(e1);
                            logoDXFrame.Elements.Add(e2);

                            logoDXFrame.Elements[0].AnimateTransition = DevExpress.Utils.DefaultBoolean.True;
                            logoDXFrame.Appearance.Font = new System.Drawing.Font("Consolas", 12F);
                            e2.Text = DR[0].ToString();
                            e1.Text = DR[2].ToString();

                            e2.TextAlignment = TileItemContentAlignment.TopCenter;
                            e1.TextAlignment = TileItemContentAlignment.BottomCenter;
                            logoDXFrame.Appearance.BackColor = System.Drawing.Color.Brown;
                            tileItem2.Frames.Add(logoDXFrame);
                            tileItem2.Appearance.BorderColor = System.Drawing.Color.Cyan;
                            tileItem2.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
                            tileItem2.FrameAnimationInterval = 2500;
                            tileItem2.ItemSize = TileItemSize.Wide;
                            FLP_ROOM.Groups.Add(new TileGroup());
                            FLP_ROOM.Groups[0].Items.Add(tileItem2);
                            tileItem2.ItemClick += (sender2, e) => SELECT_ROOM(sender2, e, DR[2].ToString(), DR.Field<Int32>(1), DR[0].ToString());
                            tileItem2.StartAnimation();
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

        private void LOAD_ROOMS_BY_RES(string res_no)
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter adp = new MySqlDataAdapter("SELECT r.reservation_id,rr.room_id,rm.room_name FROM recerved_rooms rr JOIN reservation r ON rr.reservation_no=r.reservation_id JOIN room rm ON rr.room_id=rm.room_id WHERE r.status='CHECKED IN' AND r.reservation_id=@reservation_id", CONNECTION.CON))
                {
                    adp.SelectCommand.Parameters.Clear();
                    adp.SelectCommand.Parameters.AddWithValue("@reservation_id", res_no);
                    DataTable tbl = new DataTable();
                    adp.Fill(tbl);
                   
                    if (tbl.Rows.Count > 0)
                    {
                        FLP_ROOM.Groups.Clear();
                        foreach (DataRow DR in tbl.Rows)
                        {
                            TileItem tileItem2 = new TileItem();
                            TileItemFrame logoDXFrame = new TileItemFrame();
                            TileItemElement e1 = new TileItemElement();
                            TileItemElement e2 = new TileItemElement();
                            logoDXFrame.Appearance.Options.UseFont = true;
                            logoDXFrame.Elements.Add(e1);
                            logoDXFrame.Elements.Add(e2);

                            logoDXFrame.Elements[0].AnimateTransition = DevExpress.Utils.DefaultBoolean.True;
                            logoDXFrame.Appearance.Font = new System.Drawing.Font("Consolas", 12F);
                            e1.Text = DR[0].ToString();
                            e2.Text = DR[2].ToString();

                            e2.TextAlignment = TileItemContentAlignment.BottomCenter;
                            e1.TextAlignment = TileItemContentAlignment.TopCenter;
                            logoDXFrame.Appearance.BackColor = System.Drawing.Color.Brown;
                            tileItem2.Frames.Add(logoDXFrame);
                            tileItem2.Appearance.BorderColor = System.Drawing.Color.Cyan;
                            tileItem2.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
                            tileItem2.FrameAnimationInterval = 2500;
                            tileItem2.ItemSize = TileItemSize.Wide;
                            FLP_ROOM.Groups.Add(new TileGroup());
                            FLP_ROOM.Groups[0].Items.Add(tileItem2);
                            tileItem2.ItemClick += (sender2, e) => SELECT_ROOM(sender2, e, DR[2].ToString(), DR.Field<Int32>(1), DR[0].ToString());
                            tileItem2.StartAnimation();

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

        private void LOAD_TABLES()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter adp = new MySqlDataAdapter("SELECT table_id,table_no FROM resturant_table", CONNECTION.CON))
                {
                    DataTable tbl = new DataTable();
                    adp.Fill(tbl);
                    PNL_TABLE.Controls.Clear();
                    if (tbl.Rows.Count > 0)
                    {
                        foreach (DataRow DR in tbl.Rows)
                        {
                            TileItem tileItem2 = new TileItem();
                            TileItemFrame logoDXFrame = new TileItemFrame();
                            TileItemElement e1 = new TileItemElement();
                            logoDXFrame.Appearance.Options.UseFont = true;
                            logoDXFrame.Elements.Add(e1);

                            logoDXFrame.Elements[0].AnimateTransition = DevExpress.Utils.DefaultBoolean.True;
                            logoDXFrame.Appearance.Font = new System.Drawing.Font("Consolas", 15F);
                            e1.Text = DR[1].ToString();
                            e1.TextAlignment = TileItemContentAlignment.TopCenter;
                            logoDXFrame.Appearance.BackColor = System.Drawing.Color.Green;
                            tileItem2.Frames.Add(logoDXFrame);
                            tileItem2.Appearance.BorderColor = System.Drawing.Color.Cyan;
                            tileItem2.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
                            tileItem2.FrameAnimationInterval = 2500;
                            tileItem2.ItemSize = TileItemSize.Small;
                            PNL_TABLE.Groups.Add(new TileGroup());
                            PNL_TABLE.Groups[0].Items.Add(tileItem2);
                            tileItem2.ItemClick += (sender2, e) => SELECT_TABLE(sender2, e, DR[1].ToString(), DR.Field<Int32>(0));
                            tileItem2.StartAnimation();

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

        private void LOAD_CATEGORIES()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter adp = new MySqlDataAdapter("SELECT DISTINCT i.item_category,c.caegory_name FROM item i JOIN category c ON i.item_category=c.categry_id WHERE i.item_type_id=1 OR i.item_type_id=2", CONNECTION.CON))
                {
                    DataTable tbl = new DataTable();
                    
                    adp.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        FLP_CATEGORY.Groups.Clear();
                        foreach (DataRow DR in tbl.Rows)
                        {

                            TileItem tileItem2 = new TileItem();
                            TileItemFrame logoDXFrame = new TileItemFrame();
                            TileItemElement e2 = new TileItemElement();
                            logoDXFrame.Appearance.Options.UseFont = true;
                            logoDXFrame.Elements.Add(e2);
                            logoDXFrame.Elements[0].AnimateTransition = DevExpress.Utils.DefaultBoolean.True;
                            logoDXFrame.Appearance.Font = new System.Drawing.Font("Consolas", 15F);
                            e2.Text = DR[1].ToString();
                            e2.TextAlignment = TileItemContentAlignment.TopCenter;
                            logoDXFrame.Appearance.BackColor = System.Drawing.Color.Purple;
                            tileItem2.Frames.Add(logoDXFrame);
                            tileItem2.Appearance.BorderColor = System.Drawing.Color.LightYellow;
                            tileItem2.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
                            tileItem2.FrameAnimationInterval = 2500;
                            tileItem2.ItemSize = TileItemSize.Wide;
                            FLP_CATEGORY.Groups.Add(new TileGroup());
                            FLP_CATEGORY.Groups[0].Items.Add(tileItem2);
                            tileItem2.ItemClick += (sender2, e) => SELECT_CATEGORY(sender2, e, DR.Field<Int32>(0));
                            tileItem2.StartAnimation();
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

        private void LOAD_CATEGORIES_BY_MENU(string meal_type)
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter adp = new MySqlDataAdapter("SELECT DISTINCT c.categry_id,c.caegory_name FROM daily_menu dm JOIN stock s ON dm.item_stock_id=s.stock_id JOIN item i ON s.item_code=i.item_id JOIN category c ON i.item_category=c.categry_id WHERE dm.menu_date=CURDATE() AND dm.meal_type=@meal_type", CONNECTION.CON))
                {
                    adp.SelectCommand.Parameters.Clear();
                    adp.SelectCommand.Parameters.AddWithValue("@meal_type", meal_type);
                    DataTable tbl = new DataTable();
                    adp.Fill(tbl);
                    FLP_CATEGORY.Groups.Clear();
                    if (tbl.Rows.Count > 0)
                    {
                        foreach (DataRow DR in tbl.Rows)
                        {
                            //Panel BTN_CATEGORY = new Panel();
                            //BTN_CATEGORY.Name = DR[0].ToString();
                            //BTN_CATEGORY.Click += (sender2, e2) => SELECT_CATEGORY(sender2, e2, DR.Field<Int32>(0));
                            //BTN_CATEGORY.Size = new Size(173, 78);
                            //BTN_CATEGORY.BackColor = Color.RoyalBlue;
                            //BTN_CATEGORY.BorderStyle = BorderStyle.FixedSingle;

                            //////RESERVATION NO
                            //Label LBL_CATEGORY_NAME = new Label();
                            ////LBL_GUEST_NAME.BackColor = Color.Maroon;
                            //LBL_CATEGORY_NAME.Dock = DockStyle.Bottom;
                            //LBL_CATEGORY_NAME.Font = new Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                            //LBL_CATEGORY_NAME.ForeColor = Color.White;
                            //LBL_CATEGORY_NAME.Location = new Point(0, 0);
                            //LBL_CATEGORY_NAME.Size = new Size(171, 76);
                            //LBL_CATEGORY_NAME.Text = DR[1].ToString();
                            //LBL_CATEGORY_NAME.TextAlign = ContentAlignment.MiddleCenter;
                            //LBL_CATEGORY_NAME.MouseClick += (sender2, e2) => SELECT_CATEGORY(sender2, e2, DR.Field<Int32>(0));

                            //BTN_CATEGORY.Controls.Add(LBL_CATEGORY_NAME);

                            //FLP_CATEGORY.Controls.Add(BTN_CATEGORY);

                            TileItem tileItem2 = new TileItem();
                            TileItemFrame logoDXFrame = new TileItemFrame();
                            TileItemElement e2 = new TileItemElement();
                            logoDXFrame.Appearance.Options.UseFont = true;
                            logoDXFrame.Elements.Add(e2);
                            logoDXFrame.Elements[0].AnimateTransition = DevExpress.Utils.DefaultBoolean.True;
                            logoDXFrame.Appearance.Font = new System.Drawing.Font("Consolas", 15F);
                            e2.Text = DR[1].ToString();
                            e2.TextAlignment = TileItemContentAlignment.TopCenter;
                            logoDXFrame.Appearance.BackColor = System.Drawing.Color.Purple;
                            tileItem2.Frames.Add(logoDXFrame);
                            tileItem2.Appearance.BorderColor = System.Drawing.Color.LightYellow;
                            tileItem2.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
                            tileItem2.FrameAnimationInterval = 2500;
                            tileItem2.ItemSize = TileItemSize.Wide;
                            FLP_CATEGORY.Groups.Add(new TileGroup());
                            FLP_CATEGORY.Groups[0].Items.Add(tileItem2);
                            tileItem2.ItemClick += (sender2, e) => SELECT_CATEGORY(sender2, e, DR.Field<Int32>(0));
                            tileItem2.StartAnimation();

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

        private void LOAD_ITEMS_TO_TILES(int category_id)
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter adp = new MySqlDataAdapter("SELECT s.stock_id,i.barcode,i.item_name,s.qty,s.cost_price,s.sales_price,s.item_code,i.qty_handle FROM stock s JOIN item i ON s.item_code=item_id WHERE i.item_category=@item_category AND i.item_status='ENABLE'", CONNECTION.CON))
                {
                    adp.SelectCommand.Parameters.Clear();
                    adp.SelectCommand.Parameters.AddWithValue("@item_category", category_id);
                    DataTable tbl = new DataTable();
                    adp.Fill(tbl);
                    Pnl_SelectedItems.Groups.Clear();
                    if (tbl.Rows.Count > 0)
                    {
                        tileGroup1.Items.Clear();
                        foreach (DataRow DR in tbl.Rows)
                        {

                            TileItem tileItem2 = new TileItem();
                            TileItemFrame logoDXFrame = new TileItemFrame();
                            TileItemElement e1 = new TileItemElement();
                            TileItemElement e2 = new TileItemElement();
                            TileItemElement e3 = new TileItemElement();
                            TileItemElement e4 = new TileItemElement();
                            TileItemElement e5 = new TileItemElement();
                            TileItemElement e6 = new TileItemElement();
                            TileItemElement e7 = new TileItemElement();
                            TileItemElement e8 = new TileItemElement();

                            logoDXFrame.Appearance.Options.UseFont = true;
                            logoDXFrame.Elements.Add(e2);
                            logoDXFrame.Elements.Add(e3);
                            logoDXFrame.Elements.Add(e4);

                            logoDXFrame.Elements[0].AnimateTransition = DevExpress.Utils.DefaultBoolean.True;
                            logoDXFrame.Appearance.Font = new System.Drawing.Font("Consolas", 11F);

                            e2.Text = "QTY :"+ DR.Field<double>(3).ToString();
                            double sp = DR.Field<double>(5);
                            double sp_with_tax = sp + (sp * (tax_amount / 100));
                            e3.Text = "S.P. :" + sp_with_tax.ToString("F2");
                            e4.Text = DR[2].ToString();

                            e2.TextAlignment = TileItemContentAlignment.TopRight;
                            e3.TextAlignment = TileItemContentAlignment.BottomLeft;
                            e4.TextAlignment = TileItemContentAlignment.MiddleCenter;

                            logoDXFrame.Appearance.BackColor = System.Drawing.Color.Purple;
                            tileItem2.Frames.Add(logoDXFrame);
                            tileItem2.Appearance.BorderColor = System.Drawing.Color.LightYellow;
                            tileItem2.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
                            tileItem2.FrameAnimationInterval = 2500;
                            tileItem2.ItemSize = TileItemSize.Wide;
                            Pnl_SelectedItems.Groups.Add(new TileGroup());
                            Pnl_SelectedItems.Groups[0].Items.Add(tileItem2);
                            tileItem2.ItemClick += (sender2, e) => EDIT_PRICE_AND_SELECT_ITEM(sender2, e, DR.Field<Int32>(0), DR.Field<Int32>(6), DR.Field<string>(1), DR.Field<string>(2), 1, DR.Field<double>(5), DR[7].ToString(), DR.Field<double>(4));
                            tileItem2.StartAnimation();
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

        private void LOAD_ITEMS(int category_id)
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter adp = new MySqlDataAdapter("SELECT s.stock_id,i.barcode,i.item_name,s.qty,s.cost_price,s.sales_price,s.item_code,i.qty_handle FROM stock s JOIN item i ON s.item_code=item_id WHERE i.item_category=@item_category AND i.item_status='ENABLE'", CONNECTION.CON))
                {
                    adp.SelectCommand.Parameters.Clear();
                    adp.SelectCommand.Parameters.AddWithValue("@item_category", category_id);
                    DataTable tbl = new DataTable();
                    adp.Fill(tbl);
                    Pnl_SelectedItems.Controls.Clear();
                    if (tbl.Rows.Count > 0)
                    {
                        foreach (DataRow DR in tbl.Rows) 
                        {
                            Panel BTN_ITEM = new Panel();
                            BTN_ITEM.Name = DR[0].ToString();
                           // BTN_ITEM.Click += (sender2, e2) => SELECT_ITEM(sender2, e2, DR.Field<Int32>(0), DR.Field<Int32>(6), DR.Field<string>(1), DR.Field<string>(2), 1, DR.Field<double>(5), DR[7].ToString());
                            BTN_ITEM.Click += (sender2, e2) => EDIT_PRICE_AND_SELECT_ITEM(sender2, e2, DR.Field<Int32>(0), DR.Field<Int32>(6), DR.Field<string>(1), DR.Field<string>(2), 1, DR.Field<double>(5), DR[7].ToString(), DR.Field<double>(4));
                            BTN_ITEM.Size = new Size(170, 77);
                            BTN_ITEM.BackColor = Color.Indigo;
                            BTN_ITEM.BorderStyle = BorderStyle.FixedSingle;

                            Label LBL_QTY_TEXT = new Label();
                            //LBL_QTY_TEXT.Click += (sender2, e2) => SELECT_ITEM(sender2, e2, DR.Field<Int32>(0), DR.Field<Int32>(6), DR.Field<string>(1), DR.Field<string>(2), 1, DR.Field<double>(5), DR[7].ToString());
                            LBL_QTY_TEXT.Click += (sender2, e2) => EDIT_PRICE_AND_SELECT_ITEM(sender2, e2, DR.Field<Int32>(0), DR.Field<Int32>(6), DR.Field<string>(1), DR.Field<string>(2), 1, DR.Field<double>(5), DR[7].ToString(), DR.Field<double>(4));
                            LBL_QTY_TEXT.BackColor = Color.Black;
                            LBL_QTY_TEXT.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                            LBL_QTY_TEXT.ForeColor = Color.White;
                            LBL_QTY_TEXT.Location = new Point(-3, 0);
                            LBL_QTY_TEXT.Size = new Size(39, 23);
                            LBL_QTY_TEXT.Text = "QTY :";

                            Label LBL_QTY = new Label();
                            //LBL_QTY.Click += (sender2, e2) => SELECT_ITEM(sender2, e2, DR.Field<Int32>(0), DR.Field<Int32>(6), DR.Field<string>(1), DR.Field<string>(2), 1, DR.Field<double>(5), DR[7].ToString());
                            LBL_QTY.Click += (sender2, e2) => EDIT_PRICE_AND_SELECT_ITEM(sender2, e2, DR.Field<Int32>(0), DR.Field<Int32>(6), DR.Field<string>(1), DR.Field<string>(2), 1, DR.Field<double>(5), DR[7].ToString(), DR.Field<double>(4));
                            LBL_QTY.BackColor = Color.Black;
                            LBL_QTY.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                            LBL_QTY.ForeColor = Color.White;
                            LBL_QTY.Location = new System.Drawing.Point(36, 0);
                            LBL_QTY.Size = new System.Drawing.Size(39, 23);
                            LBL_QTY.Text = DR.Field<double>(3).ToString();

                            Label LBL_SP_TEXT = new Label();
                            //LBL_SP_TEXT.Click += (sender2, e2) => SELECT_ITEM(sender2, e2, DR.Field<Int32>(0), DR.Field<Int32>(6), DR.Field<string>(1), DR.Field<string>(2), 1, DR.Field<double>(5), DR[7].ToString());
                            LBL_SP_TEXT.Click += (sender2, e2) => EDIT_PRICE_AND_SELECT_ITEM(sender2, e2, DR.Field<Int32>(0), DR.Field<Int32>(6), DR.Field<string>(1), DR.Field<string>(2), 1, DR.Field<double>(5), DR[7].ToString(), DR.Field<double>(4));
                            LBL_SP_TEXT.BackColor = Color.Black;
                            LBL_SP_TEXT.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                            LBL_SP_TEXT.ForeColor = Color.White;
                            LBL_SP_TEXT.Location = new Point(75, -1);
                            LBL_SP_TEXT.Size = new Size(39, 23);
                            LBL_SP_TEXT.Text = "S.P. :";

                            Label LBL_SP = new Label();
                            //LBL_SP.Click += (sender2, e2) => SELECT_ITEM(sender2, e2, DR.Field<Int32>(0), DR.Field<Int32>(6), DR.Field<string>(1), DR.Field<string>(2), 1, DR.Field<double>(5), DR[7].ToString());
                            LBL_SP.Click += (sender2, e2) => EDIT_PRICE_AND_SELECT_ITEM(sender2, e2, DR.Field<Int32>(0), DR.Field<Int32>(6), DR.Field<string>(1), DR.Field<string>(2), 1, DR.Field<double>(5), DR[7].ToString(), DR.Field<double>(4));
                            LBL_SP.BackColor = Color.Black;
                            LBL_SP.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                            LBL_SP.ForeColor = Color.White;
                            LBL_SP.Location = new Point(108, -1);
                            LBL_SP.Size = new System.Drawing.Size(60, 23);
                            double sp = DR.Field<double>(5);
                            double sp_with_tax = sp + (sp * (tax_amount / 100));
                            LBL_SP.Text = sp_with_tax.ToString("F2");

                            Label LBL_ITEM_NAME = new Label();
                            //LBL_ITEM_NAME.Click += (sender2, e2) => SELECT_ITEM(sender2, e2, DR.Field<Int32>(0), DR.Field<Int32>(6), DR.Field<string>(1), DR.Field<string>(2), 1, DR.Field<double>(5), DR[7].ToString());
                            LBL_ITEM_NAME.Click += (sender2, e2) => EDIT_PRICE_AND_SELECT_ITEM(sender2, e2, DR.Field<Int32>(0), DR.Field<Int32>(6), DR.Field<string>(1), DR.Field<string>(2), 1, DR.Field<double>(5), DR[7].ToString(), DR.Field<double>(4));
                            LBL_ITEM_NAME.BackColor = Color.Indigo;
                            LBL_ITEM_NAME.Dock = DockStyle.Bottom;
                            LBL_ITEM_NAME.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                            LBL_ITEM_NAME.ForeColor = Color.White;
                            LBL_ITEM_NAME.Location = new Point(0, 17);
                            LBL_ITEM_NAME.Size = new Size(168, 58);
                            LBL_ITEM_NAME.TabIndex = 1;
                            LBL_ITEM_NAME.Text = DR[2].ToString();
                            LBL_ITEM_NAME.TextAlign = ContentAlignment.MiddleCenter;

                            Label LBL_STK_ID = new Label();
                            LBL_STK_ID.BackColor = Color.Maroon;
                            LBL_STK_ID.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                            LBL_STK_ID.ForeColor = Color.White;
                            LBL_STK_ID.Location = new Point(2, 8);
                            LBL_STK_ID.Size = new Size(34, 16);
                            LBL_STK_ID.Text = DR[0].ToString();
                            LBL_STK_ID.TextAlign = ContentAlignment.MiddleCenter;
                            LBL_STK_ID.Visible = false;

                            Label LBL_ITEM_CODE = new Label();
                            LBL_ITEM_CODE.BackColor = Color.Maroon;
                            LBL_ITEM_CODE.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                            LBL_ITEM_CODE.ForeColor = Color.White;
                            LBL_ITEM_CODE.Location = new Point(2, 8);
                            LBL_ITEM_CODE.Size = new Size(34, 16);
                            LBL_ITEM_CODE.Text = DR[6].ToString();
                            LBL_ITEM_CODE.TextAlign = ContentAlignment.MiddleCenter;
                            LBL_ITEM_CODE.Visible = false;

                            Label LBL_BARCODE = new Label();
                            LBL_BARCODE.BackColor = Color.Maroon;
                            LBL_BARCODE.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                            LBL_BARCODE.ForeColor = Color.White;
                            LBL_BARCODE.Location = new Point(2, 8);
                            LBL_BARCODE.Size = new Size(34, 16);
                            LBL_BARCODE.Text = DR[1].ToString();
                            LBL_BARCODE.TextAlign = ContentAlignment.MiddleCenter;
                            LBL_BARCODE.Visible = false;

                            Label LBL_QTY_HANDEL = new Label();
                            LBL_QTY_HANDEL.BackColor = Color.Maroon;
                            LBL_QTY_HANDEL.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                            LBL_QTY_HANDEL.ForeColor = Color.White;
                            LBL_QTY_HANDEL.Location = new Point(2, 8);
                            LBL_QTY_HANDEL.Size = new Size(34, 16);
                            LBL_QTY_HANDEL.Text = DR[7].ToString();
                            LBL_QTY_HANDEL.TextAlign = ContentAlignment.MiddleCenter;
                            LBL_QTY_HANDEL.Visible = false;

                            BTN_ITEM.Controls.Add(LBL_ITEM_NAME);
                            BTN_ITEM.Controls.Add(LBL_QTY);
                            BTN_ITEM.Controls.Add(LBL_QTY_TEXT);
                            BTN_ITEM.Controls.Add(LBL_SP);
                            BTN_ITEM.Controls.Add(LBL_SP_TEXT);                           

                            Pnl_SelectedItems.Controls.Add(BTN_ITEM);
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

        private void LOAD_ITEMS_BY_MENU(string meal_type)
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter adp = new MySqlDataAdapter("SELECT dm.item_stock_id,i.barcode,i.item_name,s.qty,s.cost_price,s.sales_price,s.item_code,i.qty_handle FROM daily_menu dm JOIN stock s ON dm.item_stock_id=s.stock_id JOIN item i ON s.item_code=item_id WHERE dm.menu_date=CURDATE() AND dm.meal_type=@meal_type AND i.item_status='ENABLE'", CONNECTION.CON))
                {
                    adp.SelectCommand.Parameters.Clear();
                    adp.SelectCommand.Parameters.AddWithValue("@meal_type", meal_type);
                    DataTable tbl = new DataTable();
                    adp.Fill(tbl);
                    Pnl_SelectedItems.Groups.Clear();
                    if (tbl.Rows.Count > 0)
                    {
                        foreach (DataRow DR in tbl.Rows)
                        {
                            //Panel BTN_ITEM = new Panel();
                            //BTN_ITEM.Name = DR[0].ToString();
                            ////BTN_ITEM.Click += (sender2, e2) => SELECT_ITEM(sender2, e2, DR.Field<Int32>(0), DR.Field<Int32>(6), DR.Field<string>(1), DR.Field<string>(2), 1, DR.Field<double>(5), DR[7].ToString());
                            //BTN_ITEM.Click += (sender2, e2) => EDIT_PRICE_AND_SELECT_ITEM(sender2, e2, DR.Field<Int32>(0), DR.Field<Int32>(6), DR.Field<string>(1), DR.Field<string>(2), 1, DR.Field<double>(5), DR[7].ToString(), DR.Field<double>(4));
                            //BTN_ITEM.Size = new Size(170, 77);
                            //BTN_ITEM.BackColor = Color.Indigo;
                            //BTN_ITEM.BorderStyle = BorderStyle.FixedSingle;

                            //Label LBL_QTY_TEXT = new Label();
                            ////LBL_QTY_TEXT.Click += (sender2, e2) => SELECT_ITEM(sender2, e2, DR.Field<Int32>(0), DR.Field<Int32>(6), DR.Field<string>(1), DR.Field<string>(2), 1, DR.Field<double>(5), DR[7].ToString());
                            //LBL_QTY_TEXT.Click += (sender2, e2) => EDIT_PRICE_AND_SELECT_ITEM(sender2, e2, DR.Field<Int32>(0), DR.Field<Int32>(6), DR.Field<string>(1), DR.Field<string>(2), 1, DR.Field<double>(5), DR[7].ToString(), DR.Field<double>(4));
                            //LBL_QTY_TEXT.BackColor = Color.Black;
                            //LBL_QTY_TEXT.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                            //LBL_QTY_TEXT.ForeColor = Color.White;
                            //LBL_QTY_TEXT.Location = new Point(-3, 0);
                            //LBL_QTY_TEXT.Size = new Size(39, 23);
                            //LBL_QTY_TEXT.Text = "QTY :";

                            //Label LBL_QTY = new Label();
                            ////LBL_QTY.Click += (sender2, e2) => SELECT_ITEM(sender2, e2, DR.Field<Int32>(0), DR.Field<Int32>(6), DR.Field<string>(1), DR.Field<string>(2), 1, DR.Field<double>(5), DR[7].ToString());
                            //LBL_QTY.Click += (sender2, e2) => EDIT_PRICE_AND_SELECT_ITEM(sender2, e2, DR.Field<Int32>(0), DR.Field<Int32>(6), DR.Field<string>(1), DR.Field<string>(2), 1, DR.Field<double>(5), DR[7].ToString(), DR.Field<double>(4));
                            //LBL_QTY.BackColor = Color.Black;
                            //LBL_QTY.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                            //LBL_QTY.ForeColor = Color.White;
                            //LBL_QTY.Location = new System.Drawing.Point(36, 0);
                            //LBL_QTY.Size = new System.Drawing.Size(39, 23);
                            //LBL_QTY.Text = DR.Field<double>(3).ToString();

                            //Label LBL_SP_TEXT = new Label();
                            ////LBL_SP_TEXT.Click += (sender2, e2) => SELECT_ITEM(sender2, e2, DR.Field<Int32>(0), DR.Field<Int32>(6), DR.Field<string>(1), DR.Field<string>(2), 1, DR.Field<double>(5), DR[7].ToString());
                            //LBL_SP_TEXT.Click += (sender2, e2) => EDIT_PRICE_AND_SELECT_ITEM(sender2, e2, DR.Field<Int32>(0), DR.Field<Int32>(6), DR.Field<string>(1), DR.Field<string>(2), 1, DR.Field<double>(5), DR[7].ToString(), DR.Field<double>(4));
                            //LBL_SP_TEXT.BackColor = Color.Black;
                            //LBL_SP_TEXT.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                            //LBL_SP_TEXT.ForeColor = Color.White;
                            //LBL_SP_TEXT.Location = new Point(75, -1);
                            //LBL_SP_TEXT.Size = new Size(39, 23);
                            //LBL_SP_TEXT.Text = "S.P. :";

                            //Label LBL_SP = new Label();
                            ////LBL_SP.Click += (sender2, e2) => SELECT_ITEM(sender2, e2, DR.Field<Int32>(0), DR.Field<Int32>(6), DR.Field<string>(1), DR.Field<string>(2), 1, DR.Field<double>(5), DR[7].ToString());
                            //LBL_SP.Click += (sender2, e2) => EDIT_PRICE_AND_SELECT_ITEM(sender2, e2, DR.Field<Int32>(0), DR.Field<Int32>(6), DR.Field<string>(1), DR.Field<string>(2), 1, DR.Field<double>(5), DR[7].ToString(), DR.Field<double>(4));
                            //LBL_SP.BackColor = Color.Black;
                            //LBL_SP.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                            //LBL_SP.ForeColor = Color.White;
                            //LBL_SP.Location = new Point(108, -1);
                            //LBL_SP.Size = new System.Drawing.Size(60, 23);
                            //double sp = DR.Field<double>(5);
                            //double sp_with_tax = sp + (sp * (tax_amount / 100));
                            //LBL_SP.Text = sp_with_tax.ToString("F2");

                            //Label LBL_ITEM_NAME = new Label();
                            ////LBL_ITEM_NAME.Click += (sender2, e2) => SELECT_ITEM(sender2, e2, DR.Field<Int32>(0), DR.Field<Int32>(6), DR.Field<string>(1), DR.Field<string>(2), 1, DR.Field<double>(5), DR[7].ToString());
                            //LBL_ITEM_NAME.Click += (sender2, e2) => EDIT_PRICE_AND_SELECT_ITEM(sender2, e2, DR.Field<Int32>(0), DR.Field<Int32>(6), DR.Field<string>(1), DR.Field<string>(2), 1, DR.Field<double>(5), DR[7].ToString(), DR.Field<double>(4));
                            //LBL_ITEM_NAME.BackColor = Color.Indigo;
                            //LBL_ITEM_NAME.Dock = DockStyle.Bottom;
                            //LBL_ITEM_NAME.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                            //LBL_ITEM_NAME.ForeColor = Color.White;
                            //LBL_ITEM_NAME.Location = new Point(0, 17);
                            //LBL_ITEM_NAME.Size = new Size(168, 58);
                            //LBL_ITEM_NAME.TabIndex = 1;
                            //LBL_ITEM_NAME.Text = DR[2].ToString();
                            //LBL_ITEM_NAME.TextAlign = ContentAlignment.MiddleCenter;

                            //Label LBL_STK_ID = new Label();
                            //LBL_STK_ID.BackColor = Color.Maroon;
                            //LBL_STK_ID.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                            //LBL_STK_ID.ForeColor = Color.White;
                            //LBL_STK_ID.Location = new Point(2, 8);
                            //LBL_STK_ID.Size = new Size(34, 16);
                            //LBL_STK_ID.Text = DR[0].ToString();
                            //LBL_STK_ID.TextAlign = ContentAlignment.MiddleCenter;
                            //LBL_STK_ID.Visible = false;

                            //Label LBL_ITEM_CODE = new Label();
                            //LBL_ITEM_CODE.BackColor = Color.Maroon;
                            //LBL_ITEM_CODE.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                            //LBL_ITEM_CODE.ForeColor = Color.White;
                            //LBL_ITEM_CODE.Location = new Point(2, 8);
                            //LBL_ITEM_CODE.Size = new Size(34, 16);
                            //LBL_ITEM_CODE.Text = DR[6].ToString();
                            //LBL_ITEM_CODE.TextAlign = ContentAlignment.MiddleCenter;
                            //LBL_ITEM_CODE.Visible = false;

                            //Label LBL_BARCODE = new Label();
                            //LBL_BARCODE.BackColor = Color.Maroon;
                            //LBL_BARCODE.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                            //LBL_BARCODE.ForeColor = Color.White;
                            //LBL_BARCODE.Location = new Point(2, 8);
                            //LBL_BARCODE.Size = new Size(34, 16);
                            //LBL_BARCODE.Text = DR[1].ToString();
                            //LBL_BARCODE.TextAlign = ContentAlignment.MiddleCenter;
                            //LBL_BARCODE.Visible = false;

                            //Label LBL_QTY_HANDEL = new Label();
                            //LBL_QTY_HANDEL.BackColor = Color.Maroon;
                            //LBL_QTY_HANDEL.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                            //LBL_QTY_HANDEL.ForeColor = Color.White;
                            //LBL_QTY_HANDEL.Location = new Point(2, 8);
                            //LBL_QTY_HANDEL.Size = new Size(34, 16);
                            //LBL_QTY_HANDEL.Text = DR[7].ToString();
                            //LBL_QTY_HANDEL.TextAlign = ContentAlignment.MiddleCenter;
                            //LBL_QTY_HANDEL.Visible = false;

                            //BTN_ITEM.Controls.Add(LBL_ITEM_NAME);
                            //BTN_ITEM.Controls.Add(LBL_QTY);
                            //BTN_ITEM.Controls.Add(LBL_QTY_TEXT);
                            //BTN_ITEM.Controls.Add(LBL_SP);
                            //BTN_ITEM.Controls.Add(LBL_SP_TEXT);

                            //Pnl_SelectedItems.Controls.Add(BTN_ITEM);

                            TileItem tileItem2 = new TileItem();
                            TileItemFrame logoDXFrame = new TileItemFrame();
                            TileItemElement e1 = new TileItemElement();
                            TileItemElement e2 = new TileItemElement();
                            TileItemElement e3 = new TileItemElement();
                            TileItemElement e4 = new TileItemElement();
                            TileItemElement e5 = new TileItemElement();
                            TileItemElement e6 = new TileItemElement();
                            TileItemElement e7 = new TileItemElement();
                            TileItemElement e8 = new TileItemElement();

                            logoDXFrame.Appearance.Options.UseFont = true;
                            logoDXFrame.Elements.Add(e2);
                            logoDXFrame.Elements.Add(e3);
                            logoDXFrame.Elements.Add(e4);

                            logoDXFrame.Elements[0].AnimateTransition = DevExpress.Utils.DefaultBoolean.True;
                            logoDXFrame.Appearance.Font = new System.Drawing.Font("Consolas", 11F);

                            e2.Text = "QTY :" + DR.Field<double>(3).ToString();
                            double sp = DR.Field<double>(5);
                            double sp_with_tax = sp + (sp * (tax_amount / 100));
                            e3.Text = "S.P. :" + sp_with_tax.ToString("F2");
                            e4.Text = DR[2].ToString();

                            e2.TextAlignment = TileItemContentAlignment.TopRight;
                            e3.TextAlignment = TileItemContentAlignment.BottomLeft;
                            e4.TextAlignment = TileItemContentAlignment.MiddleCenter;

                            logoDXFrame.Appearance.BackColor = System.Drawing.Color.Purple;
                            tileItem2.Frames.Add(logoDXFrame);
                            tileItem2.Appearance.BorderColor = System.Drawing.Color.LightYellow;
                            tileItem2.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
                            tileItem2.FrameAnimationInterval = 2500;
                            tileItem2.ItemSize = TileItemSize.Wide;
                            Pnl_SelectedItems.Groups.Add(new TileGroup());
                            Pnl_SelectedItems.Groups[0].Items.Add(tileItem2);
                            tileItem2.ItemClick += (sender2, e) => EDIT_PRICE_AND_SELECT_ITEM(sender2, e, DR.Field<Int32>(0), DR.Field<Int32>(6), DR.Field<string>(1), DR.Field<string>(2), 1, DR.Field<double>(5), DR[7].ToString(), DR.Field<double>(4));
                            tileItem2.StartAnimation();
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

        private void LOAD_ITEMS_BY_BARCODE(string barcode)
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter adp = new MySqlDataAdapter("SELECT s.stock_id,i.barcode,i.item_name,s.qty,s.cost_price,s.sales_price,s.item_code,i.qty_handle FROM stock s JOIN item i ON s.item_code=item_id WHERE i.barcode=@barcode AND item_status='ENABLE'", CONNECTION.CON))
                {
                    adp.SelectCommand.Parameters.Clear();
                    adp.SelectCommand.Parameters.AddWithValue("@barcode", barcode);
                    DataTable tbl = new DataTable();
                    adp.Fill(tbl);
                   
                    if (tbl.Rows.Count > 0)
                    {
                        Pnl_SelectedItems.Groups.Clear();
                        foreach (DataRow DR in tbl.Rows)
                        {
                            TileItem tileItem2 = new TileItem();
                            TileItemFrame logoDXFrame = new TileItemFrame();
                            TileItemElement e1 = new TileItemElement();
                            TileItemElement e2 = new TileItemElement();
                            TileItemElement e3 = new TileItemElement();
                            TileItemElement e4 = new TileItemElement();
                            TileItemElement e5 = new TileItemElement();
                            TileItemElement e6 = new TileItemElement();
                            TileItemElement e7 = new TileItemElement();
                            TileItemElement e8 = new TileItemElement();

                            logoDXFrame.Appearance.Options.UseFont = true;
                            logoDXFrame.Elements.Add(e2);
                            logoDXFrame.Elements.Add(e3);
                            logoDXFrame.Elements.Add(e4);

                            logoDXFrame.Elements[0].AnimateTransition = DevExpress.Utils.DefaultBoolean.True;
                            logoDXFrame.Appearance.Font = new System.Drawing.Font("Consolas", 11F);

                            e2.Text = "QTY :" + DR.Field<double>(3).ToString();
                            double sp = DR.Field<double>(5);
                            double sp_with_tax = sp + (sp * (tax_amount / 100));
                            e3.Text = "S.P. :" + sp_with_tax.ToString("F2");
                            e4.Text = DR[2].ToString();

                            e2.TextAlignment = TileItemContentAlignment.TopRight;
                            e3.TextAlignment = TileItemContentAlignment.BottomLeft;
                            e4.TextAlignment = TileItemContentAlignment.MiddleCenter;

                            logoDXFrame.Appearance.BackColor = System.Drawing.Color.Purple;
                            tileItem2.Frames.Add(logoDXFrame);
                            tileItem2.Appearance.BorderColor = System.Drawing.Color.LightYellow;
                            tileItem2.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
                            tileItem2.FrameAnimationInterval = 2500;
                            tileItem2.ItemSize = TileItemSize.Wide;
                            Pnl_SelectedItems.Groups.Add(new TileGroup());
                            Pnl_SelectedItems.Groups[0].Items.Add(tileItem2);
                            tileItem2.ItemClick += (sender2, e) => EDIT_PRICE_AND_SELECT_ITEM(sender2, e, DR.Field<Int32>(0), DR.Field<Int32>(6), DR.Field<string>(1), DR.Field<string>(2), 1, DR.Field<double>(5), DR[7].ToString(), DR.Field<double>(4));
                            tileItem2.StartAnimation();
                        }


                    }
                    else
                    {
                        Pnl_SelectedItems.Groups.Clear();
                    }
                }
            }
            catch (Exception EX)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), EX.Message, MessageAlertImage.Error());
                mdg.ShowDialog();
            }
        }

        private void LOAD_ITEMS_BY_NAME(string item_name)
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter adp = new MySqlDataAdapter("SELECT s.stock_id,i.barcode,i.item_name,s.qty,s.cost_price,s.sales_price,s.item_code,i.qty_handle FROM stock s JOIN item i ON s.item_code=item_id WHERE i.item_name LIKE @item_name AND item_status='ENABLE'", CONNECTION.CON))
                {
                    adp.SelectCommand.Parameters.Clear();
                    adp.SelectCommand.Parameters.AddWithValue("@item_name", "%" + item_name + "%");
                    DataTable tbl = new DataTable();
                    adp.Fill(tbl);
                    Pnl_SelectedItems.Groups.Clear();
                    if (tbl.Rows.Count > 0)
                    {
                        foreach (DataRow DR in tbl.Rows)
                        {

                            TileItem tileItem2 = new TileItem();
                            TileItemFrame logoDXFrame = new TileItemFrame();
                            TileItemElement e1 = new TileItemElement();
                            TileItemElement e2 = new TileItemElement();
                            TileItemElement e3 = new TileItemElement();
                            TileItemElement e4 = new TileItemElement();
                            TileItemElement e5 = new TileItemElement();
                            TileItemElement e6 = new TileItemElement();
                            TileItemElement e7 = new TileItemElement();
                            TileItemElement e8 = new TileItemElement();

                            logoDXFrame.Appearance.Options.UseFont = true;
                            logoDXFrame.Elements.Add(e2);
                            logoDXFrame.Elements.Add(e3);
                            logoDXFrame.Elements.Add(e4);

                            logoDXFrame.Elements[0].AnimateTransition = DevExpress.Utils.DefaultBoolean.True;
                            logoDXFrame.Appearance.Font = new System.Drawing.Font("Consolas", 11F);

                            e2.Text = "QTY :" + DR.Field<double>(3).ToString();
                            double sp = DR.Field<double>(5);
                            double sp_with_tax = sp + (sp * (tax_amount / 100));
                            e3.Text = "S.P. :" + sp_with_tax.ToString("F2");
                            e4.Text = DR[2].ToString();

                            e2.TextAlignment = TileItemContentAlignment.TopRight;
                            e3.TextAlignment = TileItemContentAlignment.BottomLeft;
                            e4.TextAlignment = TileItemContentAlignment.MiddleCenter;

                            logoDXFrame.Appearance.BackColor = System.Drawing.Color.Purple;
                            tileItem2.Frames.Add(logoDXFrame);
                            tileItem2.Appearance.BorderColor = System.Drawing.Color.LightYellow;
                            tileItem2.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
                            tileItem2.FrameAnimationInterval = 2500;
                            tileItem2.ItemSize = TileItemSize.Wide;
                            Pnl_SelectedItems.Groups.Add(new TileGroup());
                            Pnl_SelectedItems.Groups[0].Items.Add(tileItem2);
                            tileItem2.ItemClick += (sender2, e) => EDIT_PRICE_AND_SELECT_ITEM(sender2, e, DR.Field<Int32>(0), DR.Field<Int32>(6), DR.Field<string>(1), DR.Field<string>(2), 1, DR.Field<double>(5), DR[7].ToString(), DR.Field<double>(4));
                            tileItem2.StartAnimation();
                        }


                    }
                    else
                    {

                       Pnl_SelectedItems.Groups.Clear();
                        
                    }
                }
            }
            catch (Exception EX)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), EX.Message, MessageAlertImage.Error());
                mdg.ShowDialog();
            }
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
                _MaxButton_Click(sender, e);
            }
        }
        private void _MaxButton_Click(object sender, EventArgs e)
        {
            if (isWindowMaximized)
            {
                this.Location = _normalWindowLocation;
                this.Size = _normalWindowSize;
                isWindowMaximized = false;
            }
            else
            {
                _normalWindowSize = this.Size;
                _normalWindowLocation = this.Location;

                Rectangle rect = Screen.PrimaryScreen.WorkingArea;
                this.Location = new Point(0, 0);
                this.Size = new System.Drawing.Size(rect.Width, rect.Height);
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
                    isWindowMaximized = true;
                }
            }
        }

        private void KOT_Load(object sender, EventArgs e)
        {
            foreach (Control ctrl in FLP_CATEGORY.Controls)
                if (ctrl.GetType() == typeof(ScrollBars))
                    ctrl.Width = 200;
        }

        private void KOT_Shown(object sender, EventArgs e)
        {

        }

        private void dataGridView1_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string header_text = "";
            header_text = DGV_ORDER_ITEMS.Columns[e.ColumnIndex].HeaderText;
        }

        private void BTN_PLUS_QTY_Click(object sender, EventArgs e)
        {
            if (DGV_ORDER_ITEMS.SelectedRows.Count > 0)
            {
                double new_qty = Convert.ToDouble(DGV_ORDER_ITEMS.SelectedRows[0].Cells[4].Value) + 1;
                DGV_ORDER_ITEMS.SelectedRows[0].Cells[4].Value = new_qty.ToString();
                double new_price = new_qty * Convert.ToDouble(DGV_ORDER_ITEMS.SelectedRows[0].Cells[5].Value);
                DGV_ORDER_ITEMS.SelectedRows[0].Cells[6].Value = new_price.ToString("F2");

                LBL_TOT_PRICE.Text = GET_ORDER_TOTAL_PRICE().ToString("F2");
            }
        }

        private void ADD_OPEN_FOOD_TO_LIST(string open_food_id)
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter adp=new MySqlDataAdapter("SELECT s.stock_id,i.barcode,i.item_name,s.sales_price,i.qty_handle,s.cost_price FROM stock s JOIN item i ON s.item_code=i.item_id WHERE s.item_code=@item_code", CONNECTION.CON))
                {
                    adp.SelectCommand.Parameters.Clear();
                    adp.SelectCommand.Parameters.AddWithValue("@item_code", open_food_id);
                    DataTable tbl = new DataTable();
                    adp.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        int i = DGV_ORDER_ITEMS.Rows.Add();
                        DGV_ORDER_ITEMS.Rows[i].Cells[0].Value = tbl.Rows[0].Field<Int32>(0);
                        DGV_ORDER_ITEMS.Rows[i].Cells[1].Value = open_food_id;
                        DGV_ORDER_ITEMS.Rows[i].Cells[2].Value = tbl.Rows[0].Field<string>(1);
                        DGV_ORDER_ITEMS.Rows[i].Cells[3].Value = tbl.Rows[0].Field<string>(2);
                        DGV_ORDER_ITEMS.Rows[i].Cells[4].Value = "1";
                        DGV_ORDER_ITEMS.Rows[i].Cells[5].Value = tbl.Rows[0].Field<double>(3).ToString("F2");
                        DGV_ORDER_ITEMS.Rows[i].Cells[6].Value = tbl.Rows[0].Field<double>(3).ToString("F2");
                        DGV_ORDER_ITEMS.Rows[i].Cells[7].Value = tbl.Rows[0].Field<string>(4);
                        DGV_ORDER_ITEMS.Rows[i].Cells[8].Value = tbl.Rows[0].Field<double>(5).ToString("F2");

                        LBL_TOT_PRICE.Text = GET_ORDER_TOTAL_PRICE().ToString("F2");
                    }
                }
            }
            catch (Exception EX)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), EX.Message, MessageAlertImage.Error());
                mdg.ShowDialog();
            }
        }

        private void BTN_OPEN_FOOD_Click(object sender, EventArgs e)
        {
            OPEN_FOOD of = new OPEN_FOOD();
            of.ShowDialog();

            if (OPEN_FOOD.ADDED_ITEM_ID != "N/A")
            {
                ADD_OPEN_FOOD_TO_LIST(OPEN_FOOD.ADDED_ITEM_ID);
            }
        }

        private void BTN_MINUS_QTY_Click(object sender, EventArgs e)
        {
            if (DGV_ORDER_ITEMS.SelectedRows.Count > 0)
            {
                if (Convert.ToDouble(DGV_ORDER_ITEMS.SelectedRows[0].Cells[4].Value) > 0)
                {
                    double new_qty = Convert.ToDouble(DGV_ORDER_ITEMS.SelectedRows[0].Cells[4].Value) - 1;
                    DGV_ORDER_ITEMS.SelectedRows[0].Cells[4].Value = new_qty.ToString();
                    double new_price = new_qty * Convert.ToDouble(DGV_ORDER_ITEMS.SelectedRows[0].Cells[5].Value);
                    DGV_ORDER_ITEMS.SelectedRows[0].Cells[6].Value = new_price.ToString("F2");
                }
                
                LBL_TOT_PRICE.Text = GET_ORDER_TOTAL_PRICE().ToString("F2");
            }
        }

        private void BTN_REMOVE_Click(object sender, EventArgs e)
        {
            try
            {
                DGV_ORDER_ITEMS.Rows.RemoveAt(DGV_ORDER_ITEMS.SelectedRows[0].Index);
                LBL_TOT_PRICE.Text = GET_ORDER_TOTAL_PRICE().ToString("F2");
            }
            catch (Exception)
            {
            }
               
           
        }

        private string PLACE_ORDER(string sp_note)
        {
            MySqlCommand cmd = CONNECTION.CON.CreateCommand();
            MySqlTransaction myTrans=null;

            try
            {
                CONNECTION.open_connection();
                cmd.Connection = CONNECTION.CON;
                cmd.Transaction = myTrans;
                myTrans = CONNECTION.CON.BeginTransaction();

                string KOT_NO =CLS_GENERATE_ID.GEN_NEXT_KOT_NO();

                string reservation_guest_id = "";
                if (LBL_RESERVATION_NO.Text != "N/A")
                {
                    using (MySqlCommand cmdGuest = new MySqlCommand("SELECT guest_id FROM reservation WHERE reservation_id=@reservation_id", CONNECTION.CON))
                    {
                        cmdGuest.Parameters.Clear();
                        cmdGuest.Parameters.AddWithValue("@reservation_id", LBL_RESERVATION_NO.Text);
                        reservation_guest_id = cmdGuest.ExecuteScalar().ToString();
                    }
                }

                cmd.CommandText = "INSERT INTO kot_order(order_no,reservation_no,room_id,table_id,tax_percentage,total_price,added_by,added_date,added_time,special_note,guest_id) VALUES(@order_no,@reservation_no,@room_id,@table_id,@tax_percentage,@total_price,@added_by,@added_date,@added_time,@special_note,@guest_id)";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@order_no", KOT_NO);
                if(LBL_RESERVATION_NO.Text!="N/A")
                    cmd.Parameters.AddWithValue("@reservation_no", LBL_RESERVATION_NO.Text);
                else
                    cmd.Parameters.AddWithValue("@reservation_no", DBNull.Value);
                if (LBL_ROOM.Text != "N/A")
                    cmd.Parameters.AddWithValue("@room_id", _ROOM_ID);
                else
                    cmd.Parameters.AddWithValue("@room_id", DBNull.Value);
                if (LBL_TABLE.Text != "N/A")
                    cmd.Parameters.AddWithValue("@table_id", _TABLE_ID);
                else
                    cmd.Parameters.AddWithValue("@table_id", DBNull.Value);
                cmd.Parameters.AddWithValue("@tax_percentage",tax_amount);
                cmd.Parameters.AddWithValue("@total_price", Convert.ToDouble(LBL_TOT_PRICE.Text));
                cmd.Parameters.AddWithValue("@added_by", CLS_CURRENT_LOGGER.LOGGED_IN_USERID);
                cmd.Parameters.AddWithValue("@added_date", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@added_time", DateTime.Now.ToString("HH:mm:ss"));
                cmd.Parameters.AddWithValue("@special_note", sp_note);
                if(SELECT_GUEST.SELECTED_GUEST_ID=="")
                    cmd.Parameters.AddWithValue("@guest_id", reservation_guest_id);
                else
                    cmd.Parameters.AddWithValue("@guest_id", SELECT_GUEST.SELECTED_GUEST_ID);
                cmd.ExecuteNonQuery();

                foreach(DataGridViewRow row in DGV_ORDER_ITEMS.Rows)
                {
                    cmd.CommandText = "INSERT INTO kot_order_item(item_stock_id,order_qty,unit_price,total_price,order_no,cost_price) VALUES(@item_stock_id,@order_qty,@unit_price,@total_price,@order_no,@cost_price)";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@item_stock_id", row.Cells[0].Value);
                    cmd.Parameters.AddWithValue("@order_qty",Convert.ToDouble(row.Cells[4].Value));
                    cmd.Parameters.AddWithValue("@unit_price", Convert.ToDouble(row.Cells[5].Value));
                    cmd.Parameters.AddWithValue("@total_price", Convert.ToDouble(row.Cells[6].Value));
                    cmd.Parameters.AddWithValue("@order_no", KOT_NO);
                    cmd.Parameters.AddWithValue("@cost_price", Convert.ToDouble(row.Cells[8].Value));
                    cmd.ExecuteNonQuery();

                    if (row.Cells[7].Value.ToString() == "1")
                    {
                        cmd.CommandText = "UPDATE stock SET qty=qty-@order_qty WHERE stock_id=@stock_id";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@order_qty", Convert.ToDouble(row.Cells[4].Value));
                        cmd.Parameters.AddWithValue("@stock_id", row.Cells[0].Value);
                        cmd.ExecuteNonQuery();
                    }
                }
                myTrans.Commit();
                LBL_ORDER_NO.Text = KOT_NO;
                return "done";
            }
            catch (Exception ex)
            {
                myTrans.Rollback();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), ex.Message, MessageAlertImage.Error());
                mdg.ShowDialog();
                return "error";
            }
        }

        private string UPDATE_ORDER()
        {
            MySqlCommand cmd = new MySqlCommand();
            MySqlTransaction myTrans = null;

            try
            {               
                CONNECTION.open_connection();
                cmd.Connection = CONNECTION.CON;
                myTrans = CONNECTION.CON.BeginTransaction(IsolationLevel.ReadCommitted);
                cmd.Transaction = myTrans;

                string reservation_guest_id = "";
                if (LBL_RESERVATION_NO.Text != "N/A")
                {
                    using (MySqlCommand cmdGuest = new MySqlCommand("SELECT guest_id FROM reservation WHERE reservation_id=@reservation_id", CONNECTION.CON))
                    {
                        cmdGuest.Parameters.Clear();
                        cmdGuest.Parameters.AddWithValue("@reservation_id", LBL_RESERVATION_NO.Text);
                        reservation_guest_id = cmdGuest.ExecuteScalar().ToString();
                    }
                }

                cmd.CommandText = "UPDATE kot_order SET reservation_no=@reservation_no,room_id=@room_id,table_id=@table_id,tax_percentage=@tax_percentage,total_price=@total_price,guest_id=@guest_id WHERE order_no=@order_no";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@order_no", LBL_ORDER_NO.Text);
                if (LBL_RESERVATION_NO.Text != "N/A")
                    cmd.Parameters.AddWithValue("@reservation_no", LBL_RESERVATION_NO.Text);
                else
                    cmd.Parameters.AddWithValue("@reservation_no", DBNull.Value);
                if (LBL_ROOM.Text != "N/A")
                    cmd.Parameters.AddWithValue("@room_id", _ROOM_ID);
                else
                    cmd.Parameters.AddWithValue("@room_id", DBNull.Value);
                if (LBL_TABLE.Text != "N/A")
                    cmd.Parameters.AddWithValue("@table_id", _TABLE_ID);
                else
                    cmd.Parameters.AddWithValue("@table_id", DBNull.Value);
                cmd.Parameters.AddWithValue("@tax_percentage", tax_amount);
                cmd.Parameters.AddWithValue("@total_price", Convert.ToDouble(LBL_TOT_PRICE.Text));
                if (LBL_RESERVATION_NO.Text != "N/A")
                    cmd.Parameters.AddWithValue("@guest_id", reservation_guest_id);
                else
                    cmd.Parameters.AddWithValue("@guest_id", SELECT_GUEST.SELECTED_GUEST_ID);
                cmd.ExecuteNonQuery();

                using(MySqlDataAdapter adp=new MySqlDataAdapter("SELECT koi.item_stock_id,koi.order_qty,koi.unit_price,koi.total_price,i.qty_handle FROM kot_order_item koi JOIN stock s ON s.stock_id=koi.item_stock_id JOIN item i ON s.item_code=i.item_id WHERE koi.order_no=@order_no", CONNECTION.CON))
                {
                    adp.SelectCommand.Parameters.Clear();
                    adp.SelectCommand.Parameters.AddWithValue("@order_no", LBL_ORDER_NO.Text);
                    DataTable tbl = new DataTable();
                    adp.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        foreach(DataRow row in tbl.Rows)
                        {
                            if (row.Field<string>(4) == "1")
                            {
                                cmd.CommandText = "UPDATE stock SET qty=qty+@order_qty WHERE stock_id=@stock_id";
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@order_qty", row.Field<double>(1));
                                cmd.Parameters.AddWithValue("@stock_id", row.Field<Int32>(0));
                                cmd.ExecuteNonQuery();
                            }
                        }

                        cmd.CommandText = "DELETE FROM kot_order_item WHERE order_no=@order_no";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@order_no",LBL_ORDER_NO.Text);
                        cmd.ExecuteNonQuery();
                    }
                }


                foreach (DataGridViewRow row in DGV_ORDER_ITEMS.Rows)
                {
                    cmd.CommandText = "INSERT INTO kot_order_item(item_stock_id,order_qty,unit_price,total_price,order_no,cost_price) VALUES(@item_stock_id,@order_qty,@unit_price,@total_price,@order_no,@cost_price)";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@item_stock_id", row.Cells[0].Value);
                    cmd.Parameters.AddWithValue("@order_qty", Convert.ToDouble(row.Cells[4].Value));
                    cmd.Parameters.AddWithValue("@unit_price", Convert.ToDouble(row.Cells[5].Value));
                    cmd.Parameters.AddWithValue("@total_price", Convert.ToDouble(row.Cells[6].Value));
                    cmd.Parameters.AddWithValue("@order_no", LBL_ORDER_NO.Text);
                    cmd.Parameters.AddWithValue("@cost_price", Convert.ToDouble(row.Cells[8].Value));
                    cmd.ExecuteNonQuery();

                    if (row.Cells[7].Value.ToString() == "1")
                    {
                        cmd.CommandText = "UPDATE stock SET qty=qty-@order_qty WHERE stock_id=@stock_id";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@order_qty", Convert.ToDouble(row.Cells[4].Value));
                        cmd.Parameters.AddWithValue("@stock_id", row.Cells[0].Value);
                        cmd.ExecuteNonQuery();
                    }
                }

                cmd.CommandText = "INSERT INTO kot_order_update_log(updated_by,updated_date,updated_time) VALUES(@updated_by,@updated_date,@updated_time)";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@updated_by", CLS_CURRENT_LOGGER.LOGGED_IN_USERID);
                cmd.Parameters.AddWithValue("@updated_date", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@updated_time", DateTime.Now.ToString("HH:mm:ss"));
                cmd.ExecuteNonQuery();

                myTrans.Commit();
                return "done";
            }
            catch (Exception ex)
            {
                myTrans.Rollback();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), ex.Message, MessageAlertImage.Error());
                mdg.ShowDialog();
                return "error";
            }
        }


        private void BTN_ORDER_Click(object sender, EventArgs e)
        {

            if (LBL_RESERVATION_NO.Text == "N/A" && SELECT_GUEST.SELECTED_GUEST_ID == "")
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Warning(), "PLEASE SELECT RESEVATION OR GUEST", MessageAlertImage.Warning());
                mdg.ShowDialog();
            }
            else if (DGV_ORDER_ITEMS.Rows.Count <= 0)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Warning(), "PLEASE ADD ORDER ITEMS", MessageAlertImage.Warning());
                mdg.ShowDialog();
            }
            else
            {
                if (BTN_ORDER.ButtonText == "ORDER")
                {
                    SPECIAL_NOTE sp = new SPECIAL_NOTE(false,string.Empty,string.Empty);
                    sp.ShowDialog();

                    if(SPECIAL_NOTE.SAVE_ORDER == true)
                    {
                        string result = PLACE_ORDER(SPECIAL_NOTE.SP_NOTE);
                        if (result == "done")
                        {
                            MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "ORDER PLACED SUCCESSFULLY", MessageAlertImage.Success());
                            mdg.ShowDialog();
                            DISABLE_CONTROLS();
                        }
                    }
                }
                else
                {
                    string result = UPDATE_ORDER();
                    if (result == "done")
                    {
                        MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "ORDER UPDATED SUCCESSFULLY", MessageAlertImage.Success());
                        mdg.ShowDialog();
                    }
                }
            }
        }

        private string MARK_ORDER_AS_SERVED()
        {
            try
            {
                CONNECTION.open_connection();
                using(MySqlCommand cmd=new MySqlCommand("UPDATE kot_order SET order_status='Served' WHERE order_no=@order_no", CONNECTION.CON))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@order_no",LBL_ORDER_NO.Text);
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

        private string MARK_ORDER_IN_PREPERATION()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlCommand cmd = new MySqlCommand("UPDATE kot_order SET order_status='In preparation' WHERE order_no=@order_no", CONNECTION.CON))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@order_no", LBL_ORDER_NO.Text);
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

        private void BTN_SERVED_Click(object sender, EventArgs e)
        {
            string result= MARK_ORDER_AS_SERVED();
            if (result == "done")
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "ORDER STATUS UPDATED AS SERVED", MessageAlertImage.Success());
                mdg.ShowDialog();
            }
        }

        private void BTN_NEW_Click(object sender, EventArgs e)
        {
            if (BTN_ORDER.Enabled == false)
                ENABLE_CONTROLS();
            CLEAR();
        }

        private void PD_KOT_BILL_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Far;
            stringFormat.LineAlignment = StringAlignment.Near;
            string bill_font = "Consolas";
            Graphics graphics = e.Graphics;
            Font font = new Font("Consolas", 11);
            float fontHeight = font.GetHeight();
            //int startX = 10;
            //int startY = 0;
            int Offset = 0;                                     //=
            //String underLine = "=====================================";                                    //-
            string underLineSingle = "----------------------------------------------------------------------------";

            float xx = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString("KITCHEN ORDER TICKET", new Font(bill_font, 12, FontStyle.Bold)).Width / 2);

            graphics.DrawString("KITCHEN ORDER TICKET", new Font(bill_font, 12, FontStyle.Bold), new SolidBrush(Color.Black), xx, 10 + Offset);
            Offset = Offset + 12;

            graphics.DrawString("ORDER NO : " + LBL_ORDER_NO.Text, new Font(bill_font, 10, FontStyle.Regular), new SolidBrush(Color.Black), 1, 30 + Offset);
            Offset = Offset + 12;

            graphics.DrawString("RESERVATION NO : " + LBL_RESERVATION_NO.Text, new Font(bill_font, 10, FontStyle.Regular), new SolidBrush(Color.Black), 1, 30 + Offset);
            Offset = Offset + 12;

            if (LBL_ROOM.Text != "N/A")
            {
                graphics.DrawString("ROOM NO : " + LBL_ROOM.Text, new Font(bill_font, 10, FontStyle.Regular), new SolidBrush(Color.Black), 1, 30 + Offset);
                Offset = Offset + 12;
            }

            if (LBL_TABLE.Text != "N/A")
            {
                graphics.DrawString("TABLE NO : " + LBL_TABLE.Text, new Font(bill_font, 10, FontStyle.Regular), new SolidBrush(Color.Black), 1, 30 + Offset);
                Offset = Offset + 12;
            }

            graphics.DrawString("ORDER DATE : " + DateTime.Now.ToString(), new Font(bill_font, 10, FontStyle.Regular), new SolidBrush(Color.Black), 1, 30 + Offset);
            Offset = Offset + 12;

            graphics.DrawString("ORDER ADDED BY : " + CLS_CURRENT_LOGGER.LOGGED_IN_USER_NAME, new Font(bill_font, 10, FontStyle.Regular), new SolidBrush(Color.Black), 1, 30 + Offset);
            Offset = Offset + 12;

            graphics.DrawString(underLineSingle, new Font(bill_font, 7), new SolidBrush(Color.Black), 1, 35 + Offset);
            Offset = Offset + 6;

            graphics.DrawString("ITEM NAME", new Font(bill_font, 10, System.Drawing.FontStyle.Regular), new SolidBrush(Color.Black), 1, 37 + Offset);
            graphics.DrawString("AMOUNT", new Font(bill_font, 10, System.Drawing.FontStyle.Regular), new SolidBrush(Color.Black), 205, 37 + Offset);//220
            Offset = Offset + 6;

            graphics.DrawString(underLineSingle, new Font(bill_font, 7), new SolidBrush(Color.Black), 1, 39 + Offset);
            Offset = Offset + 10;

            foreach(DataGridViewRow row in DGV_ORDER_ITEMS.Rows)
            {
                string item_name = row.Cells[3].Value.ToString();
                double price = Convert.ToDouble(row.Cells[6].Value);
                double unitPrice = Convert.ToDouble(row.Cells[5].Value);
                string qty = Convert.ToDouble(row.Cells[4].Value).ToString();

                if (item_name.Length > 40)
                    item_name = item_name.Substring(0, 39) + "...";
                else
                    item_name =row.Cells[3].Value.ToString();

                graphics.DrawString(item_name, new Font(bill_font, 10, FontStyle.Regular), new SolidBrush(Color.Black), 1, 41 + Offset);
                Offset = Offset + 10;

                graphics.DrawString("(" + qty + " " + " x " + "   " + (unitPrice).ToString("F2") + ")", new Font(bill_font, 8, FontStyle.Regular), new SolidBrush(Color.Black), 110, 43 + Offset, stringFormat);
                graphics.DrawString(price.ToString("F2"), new Font(bill_font, 10, FontStyle.Regular), new SolidBrush(Color.Black), 248, 43 + Offset, stringFormat);
                Offset = Offset + 13;
            }

            graphics.DrawString(underLineSingle, new Font(bill_font, 7), new SolidBrush(Color.Black), 1, 56 + Offset);
            Offset = Offset + 10;
            graphics.DrawString("NO OF ITEMS: " + DGV_ORDER_ITEMS.Rows.Count.ToString(), new Font(bill_font, 10, FontStyle.Regular), new SolidBrush(Color.Black), 1, 61 + Offset);
            Offset = Offset + 12;

            graphics.DrawString("TOTAL PRICE ", new Font(bill_font, 12, FontStyle.Bold), new SolidBrush(Color.Black), 1, 71 + Offset);
            graphics.DrawString(LBL_TOT_PRICE.Text, new Font(bill_font, 10, FontStyle.Regular), new SolidBrush(Color.Black), 248, 71 + Offset, stringFormat);
            Offset = Offset + 17;

            if (SPECIAL_NOTE.SP_NOTE != "-")
            {
                graphics.DrawString("SPECIAL NOTE:", new Font(bill_font, 10, FontStyle.Bold), new SolidBrush(Color.Black), 1, 75 + Offset);
                Offset = Offset + 12;

                SizeF sf = graphics.MeasureString(SPECIAL_NOTE.SP_NOTE, new Font(bill_font, 8, FontStyle.Regular), 280);
                graphics.DrawString(SPECIAL_NOTE.SP_NOTE, new Font(bill_font, 8, FontStyle.Regular), Brushes.Black, new RectangleF(new PointF(1, 80 + Offset), sf), StringFormat.GenericTypographic);

            }

        }

        private void BTN_PRINT_Click(object sender, EventArgs e)
        {
            string result = MARK_ORDER_IN_PREPERATION();
            if (result == "done")
            {
                PD_KOT_BILL.PrintPage += new PrintPageEventHandler(this.PD_KOT_BILL_PrintPage);

                //PrintDialog printdlg = new PrintDialog();
                //PrintPreviewDialog printPrvDlg = new PrintPreviewDialog();

                //printPrvDlg.Document = PD_KOT_BILL;
                //printPrvDlg.ShowDialog(); 

                try
                {
                    PD_KOT_BILL.Print();
                }
                catch (Exception ex)
                {
                    MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), ex.Message, MessageAlertImage.Error());
                    mdg.ShowDialog();
                }
            }
        }

        private void TXT_BARCODE_TextChanged(object sender, EventArgs e)
        {

        }

        private void TXT_BARCODE_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                LOAD_ITEMS_BY_BARCODE(TXT_BARCODE.Text);
            }
        }

        private void TXT_BARCODE_Enter(object sender, EventArgs e)
        {
            
        }

        private void TXT_ITEM_NAME_TextChanged(object sender, EventArgs e)
        {
            if (TXT_ITEM_NAME.Text != string.Empty)
                LOAD_ITEMS_BY_NAME(TXT_ITEM_NAME.Text);
            else
                Pnl_SelectedItems.Groups.Clear();
        }

        private void BTN_CLEAR_RESERVATION_Click(object sender, EventArgs e)
        {
            LBL_RESERVATION_NO.Text = "N/A";
            BTN_SELECT_GUEST.Enabled = true;
            LOAD_RESERVATIONS();
        }

        private void BTN_CLEAR_ROOM_Click(object sender, EventArgs e)
        {
            LBL_ROOM.Text = "N/A";
            _ROOM_ID = 0;
            LBL_RESERVATION_NO.Text = "N/A";
            BTN_SELECT_GUEST.Enabled = true;
            LOAD_ROOMS();
        }

        private void BTN_CLEAR_TABLE_Click(object sender, EventArgs e)
        {
            LBL_TABLE.Text = "N/A";
            _TABLE_ID = 0;
        }

        private void BTN_CLEAR_GUEST_Click(object sender, EventArgs e)
        {
            SELECT_GUEST.SELECTED_GUEST_ID = "";
            SELECT_GUEST.SELECTED_GUEST_NAME = "";
            LBL_GUEST_NAME.Text = "N/A";
        }

        private void BTN_CLEAR_CAT_FILTER_Click(object sender, EventArgs e)
        {
            LOAD_CATEGORIES();
        }

        private void BTN_BF_Click(object sender, EventArgs e)
        {
            LOAD_CATEGORIES_BY_MENU("BREAKFAST");
            LOAD_ITEMS_BY_MENU("BREAKFAST");
        }

        private void BTN_LUNCH_Click(object sender, EventArgs e)
        {
            LOAD_CATEGORIES_BY_MENU("LUNCH");
            LOAD_ITEMS_BY_MENU("LUNCH");
        }

        private void BTN_DINNER_Click(object sender, EventArgs e)
        {
            LOAD_CATEGORIES_BY_MENU("DINNER");
            LOAD_ITEMS_BY_MENU("DINNER");
        }

        private void _MinButton_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void BTN_SELECT_GUEST_Click(object sender, EventArgs e)
        {
            SELECT_GUEST sg = new SELECT_GUEST(false,"");
            sg.ShowDialog();
            if (SELECT_GUEST.SELECTED_GUEST_ID != "")
            {
                DGV_ORDER_ITEMS.Rows.Clear();
                Pnl_SelectedItems.Controls.Clear();
                LBL_GUEST_NAME.Text = SELECT_GUEST.SELECTED_GUEST_NAME;
                tax_amount = reg_tax_amount;
            }
        }

        private const int VerticalStep = 40;

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }
    }
}
