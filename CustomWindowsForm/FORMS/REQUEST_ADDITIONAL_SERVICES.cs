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
    public partial class REQUEST_ADDITIONAL_SERVICES : Form
    {
        String Reservation_id = "", Room_id;
        public REQUEST_ADDITIONAL_SERVICES()
        {
            InitializeComponent();
            CLS_METHODS.FILL_COMBOBOX(CMB_SERVICES, "SELECT id,service_name FROM additional_service_list WHERE service_status='1'", "service_name", "id", -1);
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
              //  _MaxButton_Click(sender, e);
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


        private void REQUEST_ADDITIONAL_SERVICES_Load(object sender, EventArgs e)
        {

        }

        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            CONNECTION.open_connection();
            try
            {
                if (txt_search.Text.Length > 1)
                {

                    using (MySqlDataAdapter da = new MySqlDataAdapter("SELECT reservation.status,reservation.reservation_id, guest.first_name, guest.last_name, reservation.arrival_date, reservation.depature_Date, room.room_name,room.room_id FROM reservation, guest, recerved_rooms, room WHERE reservation.guest_id = guest.guest_id AND recerved_rooms.reservation_no = reservation.reservation_id AND room.room_id = recerved_rooms.room_id AND STATUS = 'CHECKED IN' AND(reservation_id LIKE @search OR guest.first_name LIKE @search OR guest.last_name LIKE @search OR room.room_name LIKE @search)", CONNECTION.CON))
                    {
                        da.SelectCommand.Parameters.AddWithValue("@search", "%" + txt_search.Text + "%");
                        DataTable getreserv = new DataTable();
                        da.Fill(getreserv);
                        lst_load_reserv.Items.Clear();
                        if (getreserv.Rows.Count > 0)
                        {
                            foreach (DataRow dr in getreserv.Rows)
                            {
                                ListViewItem lt = new ListViewItem(dr["reservation_id"].ToString());
                                lt.SubItems.Add(dr["room_name"].ToString());
                                lt.SubItems.Add(dr["first_name"].ToString());
                                lt.SubItems.Add(dr["last_name"].ToString());
                                lt.SubItems.Add(Convert.ToDateTime(dr["arrival_date"].ToString()).ToString("yyyy-MM-dd"));
                                lt.SubItems.Add(Convert.ToDateTime(dr["depature_Date"].ToString()).ToString("yyyy-MM-dd"));
                                lt.SubItems.Add(dr["status"].ToString());
                                lt.SubItems.Add(dr["room_id"].ToString());
                                lst_load_reserv.Items.Add(lt);
                            }
                            lst_load_reserv.Height = 140;
                            lst_load_reserv.Visible = true;
                        }
                        else
                        {
                            lst_load_reserv.Visible = false;
                            lst_load_reserv.Height = 0;
                        }
                    }
                }
                else
                {
                    lst_load_reserv.Visible = false;
                    lst_load_reserv.Height = 0;
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

        private void lst_load_reserv_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                if (lst_load_reserv.Items[0].Selected == true)
                {
                    lst_load_reserv.Focus();
                }
            }
            if (e.KeyCode == Keys.Enter)
            {
                foreach (ListViewItem lt in lst_load_reserv.SelectedItems)
                {
                    LBL_GUEST_NAME.Text = lt.SubItems[2].Text + " " + lt.SubItems[3].Text;
                    LBL_ARRIVAL_DATE.Text = Convert.ToDateTime(lt.SubItems[4].Text).ToString("yyyy-MM-dd");
                    LBL_DEPARTURE_DATE.Text = Convert.ToDateTime(lt.SubItems[5].Text).ToString("yyyy-MM-dd");
                    LBL_STATUS.Text = lt.SubItems[6].Text;
                    Room_id = lt.SubItems[7].Text;
                    Reservation_id = lt.SubItems[0].Text;
                    DISPLAY_ADDED_SERVICES(lt.SubItems[0].Text, Room_id);
                }
                lst_load_reserv.Visible = false;
                groupBox2.Enabled = true;
                CMB_SERVICES.Focus();
            }
        }

        private void txt_search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (lst_load_reserv.Items.Count > 0)
                {
                    lst_load_reserv.Focus();
                    lst_load_reserv.Items[0].Selected = true;
                }
            }
        }

        private void CMB_SERVICES_SelectedIndexChanged(object sender, EventArgs e)
        {
            CONNECTION.open_connection();
            try
            {
                using (MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM additional_service_list WHERE id=@id", CONNECTION.CON))
                {
                    da.SelectCommand.Parameters.AddWithValue("@id", CMB_SERVICES.SelectedValue);
                    DataTable getservices = new DataTable();
                    da.Fill(getservices);
                    if (getservices.Rows.Count > 0)
                    {
                        double price = ((Convert.ToDouble(getservices.Rows[0]["service_price"].ToString()) * CLS_TAX.GetTotalTaxPercentage()) / 100.0) + Convert.ToDouble(getservices.Rows[0]["service_price"].ToString());
                        double subtotal = Convert.ToDouble(TXT_QTY.Text) * price;
                        TXT_SERVICE_CHARGE.Text = string.Format("{0:#,##0.00}", price);
                        TXT_SUBTOTAL.Text = string.Format("{0:#,##0.00}", subtotal);
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

        private void DISPLAY_ADDED_SERVICES(string reserv_id, string room_id)
        {
            CONNECTION.open_connection();
            try
            {
                using (MySqlDataAdapter da = new MySqlDataAdapter("SELECT additional_service.id,additional_service.additional_serivice_id, additional_service_list.service_name, additional_service.additional_serivice_qty, additional_service.additional_serivice_price FROM additional_service, additional_service_list WHERE additional_service_list.id = additional_service.additional_serivice_id AND reservation_id = @rid AND additional_service.room_id = @roomid", CONNECTION.CON))
                {
                    da.SelectCommand.Parameters.AddWithValue("@rid", reserv_id);
                    da.SelectCommand.Parameters.AddWithValue("@roomid", Room_id);
                    DataTable get_added_services = new DataTable();
                    da.Fill(get_added_services);
                    DGV_ADD_SERVICE.Rows.Clear();
                    if (get_added_services.Rows.Count > 0)
                    {
                        double total = 0;
                        foreach (DataRow dr in get_added_services.Rows)
                        {
                            DGV_ADD_SERVICE.Rows.Add(dr["id"].ToString(), dr["additional_serivice_id"].ToString(), dr["service_name"].ToString(), dr["additional_serivice_qty"].ToString(), dr["additional_serivice_price"].ToString());
                            total += Convert.ToDouble(dr["additional_serivice_price"].ToString());
                        }
                        LBL_TOTAL.Text = string.Format("{0:#,##0.00}", total);
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

        private void TXT_QTY_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(TXT_QTY.Text))
                {
                    double subtotal = Convert.ToDouble(TXT_QTY.Text) * Convert.ToDouble(TXT_SERVICE_CHARGE.Text);
                    TXT_SUBTOTAL.Text = string.Format("{0:#,##0.00}", subtotal);
                }
            }
            catch (Exception ex)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), ex.Message, MessageAlertImage.Error());
                mdg.ShowDialog();
            }
        }

        private void BTN_REMOVE_Click(object sender, EventArgs e)
        {
            CONNECTION.open_connection();
            try
            {
                foreach (DataGridViewRow dr in DGV_ADD_SERVICE.SelectedRows)
                {
                    using (MySqlCommand deleteservice = new MySqlCommand("DELETE FROM additional_service WHERE id=@id", CONNECTION.CON))
                    {
                        deleteservice.Parameters.AddWithValue("@id", dr.Cells[0].Value.ToString());
                        int n1 = deleteservice.ExecuteNonQuery();
                        if (n1 == 1)
                        {
                            DISPLAY_ADDED_SERVICES(Reservation_id, Room_id);
                            MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "Removed Successfully", MessageAlertImage.Success());
                            mdg.ShowDialog();
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

        private void BTN_ADD_SERVICE_Click(object sender, EventArgs e)
        {
            CONNECTION.open_connection();
            try
            {
                if (CMB_SERVICES.SelectedIndex == -1)
                {
                    MSGBOX mdg = new MSGBOX(MessageAlertHeder.Warning(), "Select a Service", MessageAlertImage.Warning());
                    mdg.ShowDialog();
                    CMB_SERVICES.DroppedDown = true;
                }
                else if (TXT_QTY.Text == "0.00")
                {
                    MSGBOX mdg = new MSGBOX(MessageAlertHeder.Warning(), "Enter Quantity", MessageAlertImage.Warning());
                    mdg.ShowDialog();
                    TXT_QTY.Focus();
                }
                else
                {
                    using (MySqlCommand addservice = new MySqlCommand("INSERT INTO additional_service(reservation_id,additional_serivice_id,additional_serivice_qty,additional_serivice_price,room_id) VALUES(@rid,@serviceid,@qty,@price,@roomid)", CONNECTION.CON))
                    {
                        addservice.Parameters.AddWithValue("@rid", Reservation_id);
                        addservice.Parameters.AddWithValue("@serviceid", CMB_SERVICES.SelectedValue);
                        addservice.Parameters.AddWithValue("@qty", TXT_QTY.Text);
                        addservice.Parameters.AddWithValue("@price", Convert.ToDouble(TXT_SUBTOTAL.Text));
                        addservice.Parameters.AddWithValue("@roomid", Room_id);
                        int n1 = addservice.ExecuteNonQuery();
                        if (n1 == 1)
                        {
                            DISPLAY_ADDED_SERVICES(Reservation_id, Room_id);
                            MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "Service Added Successfully", MessageAlertImage.Success());
                            mdg.ShowDialog();
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
    }
}
