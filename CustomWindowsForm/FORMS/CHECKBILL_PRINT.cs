using CustomWindowsForm.CLASS;
using HYFLEX_HMS.CLASS;
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
    public partial class CHECKBILL_PRINT : Form
    {
        public CHECKBILL_PRINT()
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

        private void button1_Click(object sender, EventArgs e)
        {
          
        }

        private void LOAD_BILL_LIST_BY_RESERVATION_NO()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter DA = new MySqlDataAdapter("SELECT reservation.reservation_id , guest.guest_id , guest.first_name , guest.last_name , reservation.no_of_rooms , reservation.arrival_date , reservation.depature_date , reservation.no_of_nights , room.room_name FROM reservation INNER JOIN guest ON (reservation.guest_id = guest.guest_id) INNER JOIN recerved_rooms ON (recerved_rooms.reservation_no = reservation.reservation_id) INNER JOIN room ON (recerved_rooms.room_id = room.room_id) WHERE reservation.status='checked out' AND reservation.reservation_id=@reservation_id GROUP BY reservation.reservation_id", CONNECTION.CON))
                {
                    DA.SelectCommand.Parameters.Clear();
                    DA.SelectCommand.Parameters.AddWithValue("@reservation_id",TXT_RESERVATION_NO.Text);
                    DataTable DT = new DataTable();
                    DA.Fill(DT);
                    if (DT.Rows.Count > 0)
                    {
                        DGV_BILL_LIST.DataSource = DT;
                        DGV_BILL_LIST.AutoGenerateColumns = false;
                    }
                    else
                    {
                        DGV_BILL_LIST.DataSource = null;
                    }
                }

            }
            catch (Exception EX)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), EX.ToString(), MessageAlertImage.Error());
                mdg.ShowDialog();
            }
        }
        private void LOAD_RESERVATION_NO()
        {
            AutoCompleteStringCollection sourceName = new AutoCompleteStringCollection();
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter DA = new MySqlDataAdapter("SELECT reservation.reservation_id FROM reservation INNER JOIN guest ON (reservation.guest_id = guest.guest_id) INNER JOIN recerved_rooms ON (recerved_rooms.reservation_no = reservation.reservation_id) INNER JOIN room ON (recerved_rooms.room_id = room.room_id) WHERE reservation.status='checked out' GROUP BY reservation.reservation_id", CONNECTION.CON))
                {
                    DataTable DT = new DataTable();
                    DA.Fill(DT);
                    if (DT.Rows.Count > 0)
                    {
                        foreach (DataRow name in DT.Rows)
                        {
                            sourceName.Add(name.Field<string>(0));
                        }
                        TXT_RESERVATION_NO.AutoCompleteCustomSource = sourceName;
                        TXT_RESERVATION_NO.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                        TXT_RESERVATION_NO.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    }
                   
                }

            }
            catch (Exception EX)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), EX.ToString(), MessageAlertImage.Error());
                mdg.ShowDialog();
            }
        }
        private void LOAD_BILL_LIST_BY_DATE()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter DA = new MySqlDataAdapter("SELECT reservation.reservation_id, guest.guest_id, guest.first_name, guest.last_name, reservation.no_of_rooms, reservation.arrival_date, reservation.depature_date, reservation.no_of_nights, room.room_name FROM reservation INNER JOIN guest ON ( reservation.guest_id = guest.guest_id ) INNER JOIN recerved_rooms ON ( recerved_rooms.reservation_no = reservation.reservation_id ) INNER JOIN room ON ( recerved_rooms.room_id = room.room_id ) WHERE reservation.status = 'checked out' AND reservation.depature_Date BETWEEN @frm_date AND @todate GROUP BY reservation.reservation_id", CONNECTION.CON))
                {
                    DA.SelectCommand.Parameters.Clear();
                    DA.SelectCommand.Parameters.AddWithValue("@frm_date", DTP_FROM_DATE.Value);
                    DA.SelectCommand.Parameters.AddWithValue("@todate", DTP_TO_DATE.Value);
                    DataTable DT = new DataTable();
                    DA.Fill(DT);
                    if (DT.Rows.Count > 0)
                    {
                        DGV_BILL_LIST.DataSource = DT;
                        DGV_BILL_LIST.AutoGenerateColumns = false;
                    }
                    else
                    {
                        DGV_BILL_LIST.DataSource = null;
                    }
                }

            }
            catch (Exception EX)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), EX.ToString(), MessageAlertImage.Error());
                mdg.ShowDialog();
            }
        }
        private void LOAD_BILL_LIST_BY_ROOM()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter DA = new MySqlDataAdapter("SELECT reservation.reservation_id, guest.guest_id, guest.first_name, guest.last_name, reservation.no_of_rooms, reservation.arrival_date, reservation.depature_date, reservation.no_of_nights, room.room_name FROM reservation INNER JOIN guest ON ( reservation.guest_id = guest.guest_id ) INNER JOIN recerved_rooms ON ( recerved_rooms.reservation_no = reservation.reservation_id ) INNER JOIN room ON ( recerved_rooms.room_id = room.room_id ) WHERE reservation.status = 'checked out' AND recerved_rooms.room_id=@room_id GROUP BY reservation.reservation_id", CONNECTION.CON))
                {
                    DA.SelectCommand.Parameters.Clear();
                    DA.SelectCommand.Parameters.AddWithValue("@room_id", CMB_ROOM_NAME.SelectedValue);
                    DataTable DT = new DataTable();
                    DA.Fill(DT);
                    if (DT.Rows.Count > 0)
                    {
                        DGV_BILL_LIST.DataSource = DT;
                        DGV_BILL_LIST.AutoGenerateColumns = false;
                    }
                    else
                    {
                        DGV_BILL_LIST.DataSource = null;
                    }
                }

            }
            catch (Exception EX)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), EX.ToString(), MessageAlertImage.Error());
                mdg.ShowDialog();
            }
        }
        private void TXT_BARCODE_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                CMB_ROOM_NAME.SelectedIndex = -1;
                LOAD_BILL_LIST_BY_RESERVATION_NO();
            }
        }

        private void BTN_LOAD_Click(object sender, EventArgs e)
        {
            LOAD_BILL_LIST_BY_DATE();
        }

        private void CHECKBILL_PRINT_Load(object sender, EventArgs e)
        {
            string QRY3 = "SELECT room_id,room_name FROM room";
            LOAD_RESERVATION_NO();
            CLS_METHODS.FILL_COMBOBOX(CMB_ROOM_NAME, QRY3, "room_name", "room_id", -1);
            DTP_FROM_DATE.Value = DTP_FROM_DATE.Value.AddDays(-7);
        }

        private void CMB_ROOM_NAME_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(CMB_ROOM_NAME.SelectedIndex!=-1)
            {
                TXT_RESERVATION_NO.Clear();
                LOAD_BILL_LIST_BY_ROOM();
            }
            else
            {
                DGV_BILL_LIST.DataSource = null;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            LOAD_RESERVATION_NO();
        }

        private void DGV_BILL_LIST_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(DGV_BILL_LIST.SelectedRows.Count>0)
            {
                REPORT.RESERVATION_NO = DGV_BILL_LIST.SelectedRows[0].Cells[0].Value.ToString();
                REPORT_VIEWER RV = new REPORT_VIEWER();
                RV.ShowDialog();
            }
        }

        private void shapedButton1_Click(object sender, EventArgs e)
        {
            if(DGV_BILL_LIST.SelectedRows.Count>0)
            {
                REPORT.RESERVATION_NO = DGV_BILL_LIST.SelectedRows[0].Cells[0].Value.ToString();
                CrystalDecisions.CrystalReports.Engine.ReportDocument rptDoc =new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                rptDoc.Load(Application.StartupPath + "\\reports\\RECEIPT.rpt");
                rptDoc.PrintToPrinter(1, true, 1, 1);

            }
        }

        private void shapedButton2_Click(object sender, EventArgs e)
        {
            if (DGV_BILL_LIST.SelectedRows.Count > 0)
            {
                REPORT.RESERVATION_NO = DGV_BILL_LIST.SelectedRows[0].Cells[0].Value.ToString();
                INVOICE_SPLITTER SP = new INVOICE_SPLITTER();
                SP.ShowDialog();

            }
        }
    }
}
