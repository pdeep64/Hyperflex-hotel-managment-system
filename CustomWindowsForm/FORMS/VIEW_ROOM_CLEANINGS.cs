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
    public partial class VIEW_ROOM_CLEANINGS : Form
    {
        public VIEW_ROOM_CLEANINGS()
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
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
          
        }
        private void LOAD_lIST()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter DA = new MySqlDataAdapter("SELECT reservation.reservation_id , room.room_id , room.room_name , room.room_floor , room.room_type,house_keeping FROM reservation INNER JOIN recerved_rooms ON (reservation.reservation_id = recerved_rooms.reservation_no) INNER JOIN room ON (room.room_id = recerved_rooms.room_id) WHERE depature_Date BETWEEN @FRM_DATE AND @TO_DATE", CONNECTION.CON))
                {
                    DA.SelectCommand.Parameters.Clear();
                    DA.SelectCommand.Parameters.AddWithValue("@FRM_DATE", dtp_arrival_date.Value.ToShortDateString());
                    DA.SelectCommand.Parameters.AddWithValue("@TO_DATE", dtp_depature_date.Value.ToShortDateString());
                    DataTable DT = new DataTable();
                    DA.Fill(DT);
                    if (DT.Rows.Count > 0)
                    {
                        DGV_LIST.Rows.Clear();
                        foreach (DataRow DGVR in DT.Rows)
                        {
                           int Y=  DGV_LIST.Rows.Add();
                            DGV_LIST.Rows[Y].Cells[0].Value = DGVR.Field<string>(0);
                            DGV_LIST.Rows[Y].Cells[1].Value = DGVR.Field<int>(1).ToString();
                            DGV_LIST.Rows[Y].Cells[2].Value = DGVR.Field<string>(2);
                            DGV_LIST.Rows[Y].Cells[3].Value = DGVR.Field<string>(3);
                            DGV_LIST.Rows[Y].Cells[4].Value = DGVR.Field<string>(4);
                            
                            if (DGVR.Field<string>(5)=="No")
                            {
                                DGV_LIST.Rows[Y].Cells[5].Value = "PENDING";
                                DGV_LIST.Rows[Y].DefaultCellStyle.BackColor = Color.Yellow;
                            }
                            else
                            {
                                DGV_LIST.Rows[Y].Cells[5].Value = "FINISHED";
                                DGV_LIST.Rows[Y].DefaultCellStyle.BackColor = Color.Green;
                            }
                        }
                    }
                    else
                    {
                        DGV_LIST.Rows.Clear();
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
                LOAD_lIST();

        }

        private void VIEW_RESERVATIONS_Load(object sender, EventArgs e)
        {
            dtp_depature_date.Value= dtp_depature_date.Value.AddDays(1);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        private void UPDATE_CLEAN_SERVICE()
        {
            try
            {
                CONNECTION.open_connection();

                using (MySqlCommand DA = new MySqlCommand("UPDATE room SET current_status=@current_status WHERE room_id=@room_id", CONNECTION.CON))
                {
                    DA.Parameters.Clear();
                    DA.Parameters.AddWithValue("@room_id", DGV_LIST.SelectedRows[0].Cells[1].Value.ToString());
                    DA.Parameters.AddWithValue("@current_status", "AVAILABLE");
                    DA.ExecuteNonQuery();

                }

                using (MySqlCommand DA = new MySqlCommand("UPDATE recerved_rooms SET house_keeping=@house_keeping WHERE reservation_no=@reservation_id AND room_id=@room_id", CONNECTION.CON))
                {
                    DA.Parameters.Clear();
                    DA.Parameters.AddWithValue("@reservation_id", DGV_LIST.SelectedRows[0].Cells[0].Value.ToString());
                    DA.Parameters.AddWithValue("@house_keeping", "Yes");
                    DA.Parameters.AddWithValue("@room_id", DGV_LIST.SelectedRows[0].Cells[1].Value.ToString());
                    DA.ExecuteNonQuery();
                    MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "SELECTED ROOM CLEAN!", MessageAlertImage.Success());
                    mdg.ShowDialog();

                }
                

            }
            catch (Exception EX)
            {

                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), EX.Message, MessageAlertImage.Error());
                mdg.ShowDialog();
            }
        }
        private void DGV_LIST_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex == DGV_LIST.Columns[5].Index)
            {
                UPDATE_CLEAN_SERVICE();
                LOAD_lIST();
            }
            
        }
    }
}
