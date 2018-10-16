using Hyperflex_HMS_KOT.CLASS;
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

namespace Hyperflex_HMS_KOT.FORMS
{
    public partial class OPEN_FOOD : Form
    {
        double reg_tax_amount = 0;
        public static string ADDED_ITEM_ID = "N/A";
        public OPEN_FOOD()
        {
            InitializeComponent();
            Rectangle rcScreen = Screen.PrimaryScreen.WorkingArea;
            this.Location = new System.Drawing.Point((rcScreen.Left + rcScreen.Right) / 2 - (this.Width / 2), 0);
            reg_tax_amount = CLS_TAX.GetTotalTaxPercentage();
        }

        bool isTopPanelDragged = false;

        Point offset;
        Size _normalWindowSize;
        Point _normalWindowLocation = Point.Empty;

        public static bool SAVE_ORDER=false;
        public static string SP_NOTE = "";

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
        }
        private void _MaxButton_Click(object sender, EventArgs e)
        {
           
        }
        private void TopPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (isTopPanelDragged)
            {
                Point newPoint = TopPanel.PointToScreen(new Point(e.X, e.Y));
                newPoint.Offset(offset);
                this.Location = newPoint;
            }
        }

        private void TopPanel_MouseUp(object sender, MouseEventArgs e)
        {
            isTopPanelDragged = false;
            if (this.Location.Y <= 5)
            {
                    _normalWindowSize = this.Size;
                    _normalWindowLocation = this.Location;

                    Rectangle rect = Screen.PrimaryScreen.WorkingArea;
                    this.Location = new Point(0, 0);
                    this.Size = new System.Drawing.Size(rect.Width, rect.Height);
            }
        }

        private String GETMAXID()
        {
            try
            {
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

        private string ADD_OPEN_FOOD()
        {
            MySqlCommand cmd = CONNECTION.CON.CreateCommand();
            MySqlTransaction myTrans;
            myTrans = CONNECTION.CON.BeginTransaction();
            cmd.Transaction = myTrans;
            cmd.Connection = CONNECTION.CON;
            try
            {
                cmd.CommandText = "INSERT INTO item (barcode,item_category,item_name,item_type_id,item_status,qty_handle) VALUES(@barcode,@item_category,@item_name,@item_type_id,@item_status,@qty_handle)";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@barcode", "N/A");
                cmd.Parameters.AddWithValue("@item_category", 7);
                cmd.Parameters.AddWithValue("@item_name", TXT_ITEM_NAME.Text);
                cmd.Parameters.AddWithValue("@item_type_id", 1);
                cmd.Parameters.AddWithValue("@item_status", "ENABLE");
                cmd.Parameters.AddWithValue("@qty_handle", "0");
                cmd.ExecuteNonQuery();

                ADDED_ITEM_ID= GETMAXID();

                cmd.CommandText = "UPDATE item SET barcode=@barcode WHERE item_id=@item_id";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@item_id", ADDED_ITEM_ID);
                cmd.Parameters.AddWithValue("@barcode", ADDED_ITEM_ID);
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO stock(item_code,qty,cost_price,sales_price) VALUES(@item_code,@qty,@cost_price,@sales_price)";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@item_code", ADDED_ITEM_ID);
                cmd.Parameters.AddWithValue("@qty", 0.00);
                cmd.Parameters.AddWithValue("@cost_price", Convert.ToDouble(TXT_COST.Text));
                cmd.Parameters.AddWithValue("@sales_price", Convert.ToDouble(TXT_SALES_PRICE.Text));
                cmd.ExecuteNonQuery();

                myTrans.Commit();
                return "done";
            }
            catch (Exception EX)
            {
                myTrans.Rollback();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), EX.Message, MessageAlertImage.Error());
                mdg.ShowDialog();
                return "error";
            }
            finally
            {
                CONNECTION.close_connection();
            }
        }

        private void BTN_OK_Click_1(object sender, EventArgs e)
        {
            if (TXT_ITEM_NAME.Text == string.Empty)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Warning(),"PLEASE ENTER ITEM NAME", MessageAlertImage.Warning());
                mdg.ShowDialog();
                TXT_ITEM_NAME.Focus();
            }
            else
            {
                string result = ADD_OPEN_FOOD();
                if (result == "done")
                {
                    MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "OPEN FOOD ADDED SUCCESSFULLY", MessageAlertImage.Success());
                    mdg.ShowDialog();
                    this.Close();
                }
            }
        }

        private void BTN_CANCEL_Click(object sender, EventArgs e)
        {
            ADDED_ITEM_ID = "N/A";
            this.Close();
        }

        private void TXT_SALES_PRICE_TextChanged(object sender, EventArgs e)
        {
            double sp = 0;
            if (TXT_SALES_PRICE.Text != string.Empty)
                sp = Convert.ToDouble(TXT_SALES_PRICE.Text);

            LBL_PRICE_WITH_TAX.Text = (sp + (sp * (reg_tax_amount / 100))).ToString("F2");
        }
    }
}
