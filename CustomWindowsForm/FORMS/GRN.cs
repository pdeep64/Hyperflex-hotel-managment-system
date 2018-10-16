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
    public partial class GRN : Form
    {
        public GRN()
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

        private void hyflexTextbox5_TextChanged(object sender, EventArgs e)
        {
            if(TXT_ITEMNAME.Text.Length>1)
            {
                LOAD_ITEMS_TO_LISTVIEW();
            }
            else
            {
                LST_ITEM_LIST.Hide();
            }
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void splitter1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void LOAD_ITEMS_TO_LISTVIEW()
        {
            try
            {
                using (MySqlDataAdapter DA=new MySqlDataAdapter("SELECT i.item_id,i.barcode,i.item_category,i.item_name FROM item AS i WHERE (i.barcode LIKE @barcode OR i.item_name LIKE @item_name)", CONNECTION.CON))
                {
                    DA.SelectCommand.Parameters.Clear();
                    DA.SelectCommand.Parameters.AddWithValue("@barcode","%"+TXT_ITEMNAME.Text+"%");
                    DA.SelectCommand.Parameters.AddWithValue("@item_name", "%"+ TXT_ITEMNAME.Text + "%");
                    DataTable DT = new DataTable();
                    DA.Fill(DT);
                    if(DT.Rows.Count>0)
                    {
                        LST_ITEM_LIST.Items.Clear();
                        foreach(DataRow DR in DT.Rows)
                        {
                            ListViewItem LST = new ListViewItem(DR.Field<int>(0).ToString());
                            {
                                LST.SubItems.Add(DR.Field<string>(1));
                                LST.SubItems.Add(DR.Field<string>(3));
                            }
                            LST_ITEM_LIST.Items.Add(LST);
                        }

                        if(LST_ITEM_LIST.Items.Count>0)
                        {
                            LST_ITEM_LIST.Visible = true;
                            LST_ITEM_LIST.Size = new Size(687,335);
                        }
                        else
                        {
                            LST_ITEM_LIST.Hide();
                        }
                    }
                    else
                    {
                        LST_ITEM_LIST.Items.Clear();
                    }
                }
            }
            catch (Exception EX)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), EX.Message, MessageAlertImage.Error());
                mdg.ShowDialog();
            }
        }

        private void TXT_ITEMNAME_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Down)
            {
                if(LST_ITEM_LIST.Items.Count>0)
                {
                    LST_ITEM_LIST.Focus();
                    LST_ITEM_LIST.Items[0].Selected = true;

                }
            }
        }

        private static String ITEM_CODE=String.Empty;
        private static String ITEM_NAME = String.Empty;
        private static String BARCODE = String.Empty;
        private void LST_ITEM_LIST_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Up)
            {
                if (LST_ITEM_LIST.Items[0].Selected==true)
                {
                    TXT_ITEMNAME.Focus();

                }
            }
            if(e.KeyCode==Keys.Enter)
            {
                ITEM_CODE = LST_ITEM_LIST.SelectedItems[0].SubItems[0].Text;
                ITEM_NAME = LST_ITEM_LIST.SelectedItems[0].SubItems[2].Text;
                BARCODE = LST_ITEM_LIST.SelectedItems[0].SubItems[1].Text;
                TXT_ITEMNAME.Text = ITEM_NAME;
                LST_ITEM_LIST.Hide();
                TXT_QTY.Focus();

            }
        }

        private void GRN_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.F1)
            {
                TXT_ITEMNAME.Focus();
            }

            if (e.KeyCode == Keys.Add)
            {
                TXT_CASH.Focus();
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void BTN_NEW_SUPLIER_Click(object sender, EventArgs e)
        {
            SUPPLIER SUP = new SUPPLIER();
            SUP.Show();
        }
        private void FILL_COMBOBOX()
        {
            string QRY1 = "SELECT supplier_id,supplier_name FROM supplier WHERE STATUS='ENABLE'";
            string QRY3 = "SELECT id,bank_name FROM bank";

            CLS_METHODS.FILL_COMBOBOX(CMB_SUPPLIER, QRY1, "supplier_name", "supplier_id", -1);
            CLS_METHODS.FILL_COMBOBOX(CMB_CHEQUE_BANK, QRY3, "bank_name", "id", -1);
        }
        private void GRN_Load(object sender, EventArgs e)
        {
            FILL_COMBOBOX();
            CMB_SUPPLIER.Focus();
        }

        private void TXT_QTY_KeyDown(object sender, KeyEventArgs e)
        {

            if(e.KeyCode==Keys.Enter)
            {
                TXT_COST.Focus();
            }
        }

        private void TXT_COST_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TXT_SALE.Focus();
            }
        }

        private void TXT_SALE_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BTN_ADD.Focus();
            }
        }

        private void BTN_ADD_Click(object sender, EventArgs e)
        {
            if (ITEM_CODE==String.Empty)
            {
                TXT_QTY.Focus();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), "PLEASE SELECT ITEM!", MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
            else if(Convert.ToDouble(TXT_QTY.Text)<=0)
            {
                TXT_QTY.Focus();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), "PLEASE ENTER QTY!", MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
            else if(Convert.ToDouble(TXT_COST.Text) <= 0)
            {
                TXT_COST.Focus();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), "PLEASE ENTER COST PRICE!", MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
            //else if(Convert.ToDouble(TXT_SALE.Text) <= 0)
            //{
            //    TXT_SALE.Focus();
            //    MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), "PLEASE ENTER SALE PRICE!", MessageAlertImage.Alert());
            //    mdg.ShowDialog();
            //}
            //else if(Convert.ToDouble(TXT_SALE.Text) <= Convert.ToDouble(TXT_COST.Text))
            //{
            //    TXT_SALE.Focus();
            //    MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), "ENTERD SALE PRICE IS LESS THAN OR EQUAL COST PRICE!", MessageAlertImage.Alert());
            //    mdg.ShowDialog();
            //}
            else
            {
                ADD_TO_TABLE();
                CLEAR_BOX();
                CAL_TOTAL_COST();
                LBL_TOTAL_PAYABLE.Text = (Convert.ToDouble(LBL_TOT_GRN.Text) + Convert.ToDouble(LBL_PREVIOUS_DUES.Text)).ToString("F2");
                ITEM_CODE = String.Empty;
                ITEM_NAME= String.Empty;
                BARCODE= String.Empty;
                TXT_ITEMNAME.Focus();
            }
        }
        private void CAL_TOTAL_COST()
        {
            Double NO_OF_ITEMS =0;
            Double TOTAL_COST = 0;

            foreach (DataGridViewRow DR in DGV_ITEMS_LIST.Rows)
            {
                NO_OF_ITEMS += 1;
                TOTAL_COST += (Convert.ToDouble(DR.Cells[3].Value)* Convert.ToDouble(DR.Cells[4].Value));
            }
            LBL_NO_OF_ITEMS.Text = DGV_ITEMS_LIST.Rows.Count.ToString();
            LBL_TOT_GRN.Text = TOTAL_COST.ToString("F2");
            TXT_CREDIT.Text= TOTAL_COST.ToString("F2");
        }
        private void CLEAR_BOX()
        {
            TXT_QTY.Text = "0.000";
            TXT_COST.Text = "0.00";
            TXT_SALE.Text = "0.00";
            TXT_ITEMNAME.Clear();
        }
        private void ADD_TO_TABLE()
        {
            Double QTY = Convert.ToDouble(TXT_QTY.Text);
            Double COST = Convert.ToDouble(TXT_COST.Text);
            Double SALE = Convert.ToDouble(TXT_SALE.Text);
            bool ADD_DATA = false;

            foreach (DataGridViewRow DR in DGV_ITEMS_LIST.Rows)
            {
                ////MessageBox.Show(DR.Cells[0].Value.ToString()+" "+ ITEM_CODE);
                ////MessageBox.Show(DR.Cells[4].Value.ToString() + " " + COST);
                //MessageBox.Show(DR.Cells[6].Value.ToString() + " " + CMB_ITEM_TYPE.SelectedValue.ToString());
                if (DR.Cells[0].Value.ToString().Equals(ITEM_CODE) && Convert.ToDouble(DR.Cells[4].Value)==(COST) && Convert.ToDouble(DR.Cells[5].Value)==(SALE))
                {
                    ADD_DATA = true;
                    DR.Cells[3].Value = (Convert.ToDouble(DR.Cells[3].Value) + QTY).ToString("F2");
                    break;
                }
            }

            if(DGV_ITEMS_LIST.Rows.Count==0 ||ADD_DATA==false)
            {
                int Y = DGV_ITEMS_LIST.Rows.Add();
                DGV_ITEMS_LIST.Rows[Y].Cells[0].Value = ITEM_CODE;
                DGV_ITEMS_LIST.Rows[Y].Cells[1].Value = BARCODE;
                DGV_ITEMS_LIST.Rows[Y].Cells[2].Value = ITEM_NAME;
                DGV_ITEMS_LIST.Rows[Y].Cells[3].Value = QTY.ToString("F3"); ;
                DGV_ITEMS_LIST.Rows[Y].Cells[4].Value = COST.ToString("F2"); ;
                DGV_ITEMS_LIST.Rows[Y].Cells[5].Value = SALE.ToString("F2"); ;
            }
        }


        private void CHECK_TEXTBOX()
        {
            if (TXT_CASH.Text == String.Empty)
            {
                TXT_CASH.Text = "0.00";
            }
            if (TXT_CARD.Text == String.Empty)
            {
                TXT_CARD.Text = "0.00";
            }
            if (TXT_CHEQUE.Text == String.Empty)
            {
                TXT_CHEQUE.Text = "0.00";
            }
        }
        private void TXT_CASH_TextChanged(object sender, EventArgs e)
        {
            CHECK_TEXTBOX();
            if ((Convert.ToDouble(TXT_CHEQUE.Text) + Convert.ToDouble(TXT_CARD.Text) + Convert.ToDouble(TXT_CASH.Text)) >= Convert.ToDouble(LBL_TOT_GRN.Text))
                TXT_CREDIT.Text = "0.00";
            else
                TXT_CREDIT.Text = (Convert.ToDouble(LBL_TOT_GRN.Text)-Convert.ToDouble(TXT_CHEQUE.Text) - Convert.ToDouble(TXT_CARD.Text) - Convert.ToDouble(TXT_CASH.Text)).ToString("F2");
        }

        private void TXT_CHEQUE_TextChanged(object sender, EventArgs e)
        {
            CHECK_TEXTBOX();
            if ((Convert.ToDouble(TXT_CHEQUE.Text) + Convert.ToDouble(TXT_CARD.Text) + Convert.ToDouble(TXT_CASH.Text)) >= Convert.ToDouble(LBL_TOT_GRN.Text))
                TXT_CREDIT.Text = "0.00";
            else
                TXT_CREDIT.Text = (Convert.ToDouble(LBL_TOT_GRN.Text) - Convert.ToDouble(TXT_CHEQUE.Text) - Convert.ToDouble(TXT_CARD.Text) - Convert.ToDouble(TXT_CASH.Text)).ToString("F2");
        }

        private void TXT_CARD_TextChanged(object sender, EventArgs e)
        {
            CHECK_TEXTBOX();
            if ((Convert.ToDouble(TXT_CHEQUE.Text) + Convert.ToDouble(TXT_CARD.Text) + Convert.ToDouble(TXT_CASH.Text)) >= Convert.ToDouble(LBL_TOT_GRN.Text))
                TXT_CREDIT.Text = "0.00";
            else
                TXT_CREDIT.Text = (Convert.ToDouble(LBL_TOT_GRN.Text) - Convert.ToDouble(TXT_CHEQUE.Text) - Convert.ToDouble(TXT_CARD.Text) - Convert.ToDouble(TXT_CASH.Text)).ToString("F2");
        }

        private void BTN_PRINT_Click(object sender, EventArgs e)
        {
            MessageBox.Show(LBL_TOT_GRN.Text);
        }

        private void TXT_CASH_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TXT_CHEQUE.Focus();
            }
        }

        private void TXT_CHEQUE_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TXT_CARD.Focus();
            }
        }

        private void TXT_CARD_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CMB_CHEQUE_BANK.Focus();
            }
        }

        private void TXT_CHEQUE_NO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TXT_CARD_NO.Focus();
            }
        }

        private void TXT_CARD_NO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CMB_CHEQUE_BANK.Focus();
            }
        }

        private void CMB_CHEQUE_BANK_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TXT_OWNER.Focus();
            }
        }

        private void TXT_OWNER_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DTP_CHEQUE_DATE.Focus();
            }
        }

        private void DTP_CHEQUE_DATE_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BTN_SAVE.Focus();
            }
        }

        private void splitter2_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }
        MySqlCommand cmd = CONNECTION.CON.CreateCommand();
        MySqlTransaction myTrans;
        private void SAVEGRN()
        {
            try
            {

                Cursor.Current = Cursors.WaitCursor;
                CONNECTION.open_connection();
                myTrans = CONNECTION.CON.BeginTransaction();
                cmd.Connection = CONNECTION.CON;
                cmd.Transaction = myTrans;
                String GRN_NO = CLS_GENERATE_ID.GEN_NEXT_GRN_NO();
                double total_price = Convert.ToDouble(LBL_TOT_GRN.Text);
                double credit = Convert.ToDouble(TXT_CREDIT.Text);
                double cash = Convert.ToDouble(TXT_CASH.Text);
                double cheque = Convert.ToDouble(TXT_CHEQUE.Text);
                double card = Convert.ToDouble(TXT_CARD.Text);

                string type = string.Empty;
                if (cash > 0 && cheque <= 0 && card <= 0)
                {
                    type = "CASH";
                }
                else if (cash <= 0 && cheque > 0 && card <= 0)
                {
                    type = "CHEQUE";
                }
                else if (cash <= 0 && cheque <= 0 && card > 0)
                {
                    type = "CARD";
                }
                else if (cash > 0 && cheque > 0 && card <= 0)
                {
                    type = "CASH/CHEQUE";
                }
                else if (cash > 0 && cheque > 0 && card > 0)
                {
                    type = "CARD/CHEQUE/CARD";
                }
                else if (cash <= 0 && cheque > 0 && card > 0)
                {
                    type = "CHEQUE/CARD";
                }
                else if (cash > 0 && cheque <= 0 && card > 0)
                {
                    type = "CASH/CARD";
                }
                else
                {
                    type = "CREDIT";
                }

                LBL_GRN_NO.Text = GRN_NO;
                cmd.CommandText = "INSERT INTO grn ( grn_no, payment_type, supplier_id, grn_total, added_by, added_date, added_time ) VALUES ( @grn_no, @payment_type, @supplier_id, @grn_total, @added_by, @added_date, @added_time )";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@grn_no", GRN_NO);
                cmd.Parameters.AddWithValue("@payment_type", type);
                cmd.Parameters.AddWithValue("@supplier_id", CMB_SUPPLIER.SelectedValue);
                cmd.Parameters.AddWithValue("@grn_total", Convert.ToDouble(LBL_TOT_GRN.Text));
                cmd.Parameters.AddWithValue("@added_by", CLS_CURRENT_LOGGER.LOGGED_IN_USERID);
                cmd.Parameters.AddWithValue("@added_date", DTP_GRN_DATE.Value.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@added_time", DTP_GRN_DATE.Value.ToString("HH:mm:ss"));
                cmd.ExecuteNonQuery();

                foreach (DataGridViewRow dgvr in DGV_ITEMS_LIST.Rows)
                {
                    cmd.CommandText = "INSERT INTO stock_items ( item_code, qty, cost_price, sales_price, grn_no ) VALUES (@item_code, @qty, @cost_price, @sales_price, @grn_no)";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@item_code", Convert.ToInt32(dgvr.Cells[0].Value));
                    cmd.Parameters.AddWithValue("@qty", Convert.ToDouble(dgvr.Cells[3].Value.ToString()));
                    cmd.Parameters.AddWithValue("@cost_price", Convert.ToDouble(dgvr.Cells[4].Value.ToString()));
                    cmd.Parameters.AddWithValue("@sales_price", Convert.ToDouble(dgvr.Cells[5].Value.ToString()));
                    cmd.Parameters.AddWithValue("@grn_no", GRN_NO);
                    cmd.ExecuteNonQuery();

                    using (MySqlDataAdapter adp = new MySqlDataAdapter("SELECT stock_id,qty FROM stock WHERE item_code=@item_code  AND cost_price=@cost_price AND sales_price=@sales_price", CONNECTION.CON))
                    {
                        adp.SelectCommand.Parameters.Clear();
                        adp.SelectCommand.Parameters.AddWithValue("@item_code", Convert.ToInt32(dgvr.Cells[0].Value.ToString()));
                        adp.SelectCommand.Parameters.AddWithValue("@cost_price", Convert.ToDouble(dgvr.Cells[4].Value.ToString()));
                        adp.SelectCommand.Parameters.AddWithValue("@sales_price", Convert.ToDouble(dgvr.Cells[5].Value.ToString()));
                        DataTable dtb = new DataTable();
                        adp.Fill(dtb);
                        if (dtb.Rows.Count > 0)
                        {
                            cmd.CommandText = "UPDATE stock SET qty=qty+@qty WHERE stock_id=@stock_id";
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@stock_id", Convert.ToInt32(dtb.Rows[0].Field<int>(0)));
                            double total_qty = Convert.ToDouble(dtb.Rows[0].Field<double>(1)) + Convert.ToDouble(dgvr.Cells[3].Value.ToString());
                            cmd.Parameters.AddWithValue("@qty", total_qty);
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            cmd.CommandText = "INSERT INTO stock( item_code, qty, cost_price, sales_price ) VALUES ( @item_code, @qty, @cost_price, @sales_price)";
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@item_code", Convert.ToInt32(dgvr.Cells[0].Value.ToString()));
                            cmd.Parameters.AddWithValue("@qty", Convert.ToDouble(dgvr.Cells[3].Value.ToString()));
                            cmd.Parameters.AddWithValue("@cost_price", Convert.ToDouble(dgvr.Cells[4].Value.ToString()));
                            cmd.Parameters.AddWithValue("@sales_price", Convert.ToDouble(dgvr.Cells[5].Value.ToString()));
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                if (credit > 0)
                {
                    cmd.CommandText = "INSERT INTO supplier_account ( account_no, supplier_id, grn_no, grn_total, paid, due,added_date,added_time,added_by ) VALUES ( @account_no, @supplier_id, @grn_no, @grn_total, @paid, @due ,@added_date,@added_time,@added_by)";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@account_no", CLS_GENERATE_ID.GEN_NEXT_ACC_NO());
                    cmd.Parameters.AddWithValue("@supplier_id", CMB_SUPPLIER.SelectedValue);
                    cmd.Parameters.AddWithValue("@grn_no", GRN_NO);
                    cmd.Parameters.AddWithValue("@grn_total", total_price);
                    cmd.Parameters.AddWithValue("@paid", cash);
                    cmd.Parameters.AddWithValue("@due", credit);
                    cmd.Parameters.AddWithValue("@added_date", DTP_GRN_DATE.Value.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@added_time", DTP_GRN_DATE.Value.ToString("HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@added_by", CLS_CURRENT_LOGGER.LOGGED_IN_USERID);
                    cmd.ExecuteNonQuery();
                }

                if (cheque > 0)
                {
                    cmd.CommandText = "INSERT INTO issue_cheque (grn_no, amount, cheque_date, issued_date, supplier_id, bank_id, cheque_no, issued_time, issued_by ) VALUES ( @grn_no, @amount, @cheque_date, @issued_date, @supplier_id, @bank_id, @cheque_no, @issued_time, @issued_by)";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@grn_no", GRN_NO);
                    cmd.Parameters.AddWithValue("@amount", cheque);
                    cmd.Parameters.AddWithValue("@cheque_date", DTP_CHEQUE_DATE.Value.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@issued_date", DTP_GRN_DATE.Value.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@supplier_id", CMB_SUPPLIER.SelectedValue);
                    cmd.Parameters.AddWithValue("@bank_id", CMB_CHEQUE_BANK.SelectedValue);
                    cmd.Parameters.AddWithValue("@cheque_no", TXT_CHEQUE_NO.Text);
                    cmd.Parameters.AddWithValue("@issued_time", DateTime.Now.ToString("HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@issued_by", CLS_CURRENT_LOGGER.LOGGED_IN_USERID);
                    cmd.ExecuteNonQuery();
                }


                if (cash > 0)
                {
                    cmd.CommandText = "INSERT INTO account(income_type, payment_type, payment, added_date, added_time, note ) VALUES ( @income_type, @payment_type, @payment, @added_date, @added_time, @note)";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@income_type", "EXPENCES");
                    cmd.Parameters.AddWithValue("@payment_type", "CASH");
                    cmd.Parameters.AddWithValue("@payment", cash);
                    cmd.Parameters.AddWithValue("@added_date", DTP_GRN_DATE.Value.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@added_time", DTP_GRN_DATE.Value.ToString("HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@note", "PAYMENT FOR GRN: " + GRN_NO);
                    cmd.ExecuteNonQuery();
                }

                if (cheque > 0)
                {
                    cmd.CommandText = "INSERT INTO account(income_type, payment_type, payment, added_date, added_time, note, cheque_no) VALUES ( @income_type, @payment_type, @payment, @added_date, @added_time, @note, @cheque_no)";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@income_type", "EXPENCES");
                    cmd.Parameters.AddWithValue("@payment_type", "CHEQUE");
                    cmd.Parameters.AddWithValue("@payment", cheque);
                    cmd.Parameters.AddWithValue("@added_date", DTP_GRN_DATE.Value.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@added_time", DTP_GRN_DATE.Value.ToString("HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@note", "PAYMENT FOR GRN: " + GRN_NO);
                    cmd.Parameters.AddWithValue("@cheque_no", TXT_CHEQUE_NO.Text);
                    cmd.ExecuteNonQuery();
                }

                if (card > 0)
                {
                    cmd.CommandText = "INSERT INTO account(income_type, payment_type, payment, added_date, added_time, note , card_details) VALUES ( @income_type, @payment_type, @payment, @added_date, @added_time, @note, @card_details)";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@income_type", "EXPENCES");
                    cmd.Parameters.AddWithValue("@payment_type", "CARD");
                    cmd.Parameters.AddWithValue("@payment", card);
                    cmd.Parameters.AddWithValue("@added_date", DTP_GRN_DATE.Value.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@added_time", DTP_GRN_DATE.Value.ToString("HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@note", "PAYMENT FOR GRN: " + GRN_NO);
                    cmd.Parameters.AddWithValue("@card_details", "CARD NO: "+ TXT_CARD_NO.Text +" OWNER NAME: "+ TXT_OWNER.Text);
                    cmd.ExecuteNonQuery();
                }



                myTrans.Commit();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "GRN DATA ADDED SUCCESSFULLY!", MessageAlertImage.Success());
                mdg.ShowDialog();
                DisableControls();
                BTN_NEW.Focus();

            }
            catch (Exception EX)
            {
                Cursor.Current = Cursors.Default;
                myTrans.Rollback();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), EX.Message, MessageAlertImage.Error());
                mdg.ShowDialog();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                CONNECTION.close_connection();
            }
        }

        private void DisableControls()
        {
            DTP_GRN_DATE.Enabled = false;
            CMB_SUPPLIER.Enabled = false;
            BTN_NEW_SUPLIER.Enabled = false;
            TXT_ITEMNAME.Enabled = false;
            TXT_QTY.Enabled = false;
            TXT_COST.Enabled = false;
            TXT_SALE.Enabled = false;
            BTN_ADD.Enabled = false;
            DGV_ITEMS_LIST.Enabled = false;
            shapedButton1.Enabled = false;
            groupBox4.Enabled = false;
            BTN_SAVE.Enabled = false;
        }

        private void EnableControls()
        {
            DTP_GRN_DATE.Enabled = true;
            CMB_SUPPLIER.Enabled = true;
            BTN_NEW_SUPLIER.Enabled = true;
            TXT_ITEMNAME.Enabled = true;
            TXT_QTY.Enabled = true;
            TXT_COST.Enabled = true;
            TXT_SALE.Enabled = true;
            BTN_ADD.Enabled = true;
            DGV_ITEMS_LIST.Enabled = true;
            shapedButton1.Visible = true;
            groupBox4.Enabled = true;
            BTN_SAVE.Enabled = true;
        }

        private void BTN_SAVE_Click(object sender, EventArgs e)
        {
            if (DGV_ITEMS_LIST.Rows.Count==0)
            {
                TXT_ITEMNAME.Focus();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), "PLEASE ADD ITEMS FIRST!", MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
            else if(CMB_SUPPLIER.SelectedIndex==-1)
            {
                CMB_SUPPLIER.Focus();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), "PLEASE SELECT SUPPLIER!", MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
            else if (Convert.ToDouble(TXT_CARD.Text)<=0 && TXT_CARD_NO.Text==string.Empty)
            {
                TXT_CARD_NO.Focus();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), "PLEASE ENTER CARD NO!", MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
            else if (Convert.ToDouble(TXT_CHEQUE.Text) > 0 && (TXT_CHEQUE_NO.Text == string.Empty || CMB_CHEQUE_BANK.SelectedIndex==-1))
            {
                TXT_CHEQUE_NO.Focus();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), "PLEASE ENTER CHEQUE INFORMATIONS!", MessageAlertImage.Alert());
                mdg.ShowDialog();
            }
            else
            {
                SAVEGRN();
            }
        }
        private void CLER_ALL()
        {
            CLEAR_BOX();
            DGV_ITEMS_LIST.Rows.Clear();
            LBL_GRN_NO.Text = "N/A";
            LBL_NO_OF_ITEMS.Text = "0";
            LBL_PREVIOUS_DUES.Text = "0.00";
            LBL_TOTAL_PAYABLE.Text = "0.00";
            LBL_TOT_GRN.Text = "0.00";
            TXT_CREDIT.Text = "0.00";
            TXT_CARD.Text = "0.00";
            TXT_CASH.Text = "0.00";
            TXT_CHEQUE.Text = "0.00";
            TXT_CHEQUE_NO.Text = "0.00";
            CMB_CHEQUE_BANK.SelectedIndex = -1;
            TXT_OWNER.Text = "0.00";
            TXT_CARD_NO.Text = "0";
            CMB_SUPPLIER.SelectedValue = -1;

        }
        private void BTN_NEW_Click(object sender, EventArgs e)
        {
            if(BTN_SAVE.Enabled==true && DGV_ITEMS_LIST.Rows.Count > 0)
            {
                DialogResult DS = MessageBox.Show("DO YOU WANT TO CLEAR THIS DATA" + Environment.NewLine + "ARE YOU SURE ?", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DS == DialogResult.Yes)
                {
                    CLER_ALL();
                    CMB_SUPPLIER.Focus();

                }
            }
            else
            {
                EnableControls();
                CLER_ALL();
                CMB_SUPPLIER.Focus();
            }
          
        }

        private void CMB_SUPPLIER_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (CMB_SUPPLIER.SelectedIndex >= 0)
            {
                try
                {
                    CONNECTION.open_connection();
                    using(MySqlDataAdapter adp=new MySqlDataAdapter("SELECT IFNULL(SUM(due),0.00) FROM supplier_account WHERE supplier_id=@supplier_id", CONNECTION.CON))
                    {
                        adp.SelectCommand.Parameters.Clear();
                        adp.SelectCommand.Parameters.AddWithValue("@supplier_id", CMB_SUPPLIER.SelectedValue);
                        DataTable tbl = new DataTable();
                        adp.Fill(tbl);
                        if (tbl.Rows.Count > 0)
                        {
                            LBL_PREVIOUS_DUES.Text = tbl.Rows[0].Field<double>(0).ToString("F2");
                        }
                        else
                        {
                            LBL_PREVIOUS_DUES.Text = "0.00";
                        }
                    }
                }
                catch (Exception)
                {
                    LBL_PREVIOUS_DUES.Text = "0.00";
                }
                finally
                {
                    CONNECTION.close_connection();
                }

                LBL_TOTAL_PAYABLE.Text = (Convert.ToDouble(LBL_TOT_GRN.Text) + Convert.ToDouble(LBL_PREVIOUS_DUES.Text)).ToString("F2");
            }

        }

        private void shapedButton1_Click_1(object sender, EventArgs e)
        {
            if (DGV_ITEMS_LIST.Rows.Count > 0)
            {
                DGV_ITEMS_LIST.Rows.RemoveAt(DGV_ITEMS_LIST.SelectedRows[0].Index);
                CAL_TOTAL_COST();
                LBL_TOTAL_PAYABLE.Text = (Convert.ToDouble(LBL_TOT_GRN.Text) + Convert.ToDouble(LBL_PREVIOUS_DUES.Text)).ToString("F2");
            }
        }

        private void BTN_PRINT_Click_1(object sender, EventArgs e)
        {

        }
    }
}
