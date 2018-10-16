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
using System.Globalization;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CustomWindowsForm.FORMS
{
    public partial class ROOMS_OVERVIEW : Form
    {
        public ROOMS_OVERVIEW()
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

        private void ROOMS_OVERVIEW_Load(object sender, EventArgs e)
        {
            loadingCircleRoomOverview.InnerCircleRadius = 11;
            loadingCircleRoomOverview.OuterCircleRadius = 12;
            loadingCircleRoomOverview.NumberSpoke = 10;
            loadingCircleRoomOverview.SpokeThickness = 5;
        }

        private void ROOMS_OVERVIEW_Shown(object sender, EventArgs e)
        {
            flpRoomsOverview.Controls.Clear();
            Thread threadLoadOverview = new Thread(LoadOverview);
            threadLoadOverview.IsBackground = true;
            threadLoadOverview.Start();
        }

        private void LoadOverview()
        {
            CONNECTION.open_connection();
            try
            {
                loadingCircleRoomOverview.Invoke(new Action(() =>
                {
                    loadingCircleRoomOverview.Visible = true;
                    loadingCircleRoomOverview.Active = true;
                }));
                using (MySqlDataAdapter da = new MySqlDataAdapter("SELECT DISTINCT room_packages.package_name,room_packages.room_package_id FROM room,room_packages WHERE room.room_package_id = room_packages.room_package_id", CONNECTION.CON))
                {
                    DataTable getpackgenames = new DataTable();
                    da.Fill(getpackgenames);
                    if (getpackgenames.Rows.Count > 0)
                    {
                        int a = 0;
                        Panel[] panelPackageName = new Panel[getpackgenames.Rows.Count];
                        Label[] lblPanelName = new Label[getpackgenames.Rows.Count];

                        foreach (DataRow dr in getpackgenames.Rows)
                        {
                            MySqlDataAdapter da2 = new MySqlDataAdapter("SELECT room.room_name,room.current_status,guest.first_name,guest.last_name,reservation.arrival_date,reservation.depature_Date,room_packages.package_name FROM reservation,recerved_rooms,room_packages,room,guest WHERE reservation.reservation_id = recerved_rooms.reservation_no AND recerved_rooms.room_id = room.room_id AND room.room_package_id = room_packages.room_package_id AND guest.guest_id = reservation.guest_id AND reservation.arrival_date <= CURDATE() AND reservation.depature_Date >= CURDATE() AND room_packages.room_package_id = @packageid UNION SELECT room.room_name,room.current_status,'Free' AS first_name,'Room' AS last_name,CURDATE() AS arrival_date,CURDATE() AS depature_Date,room_packages.package_name FROM room LEFT JOIN recerved_rooms ON room.room_id = recerved_rooms.room_id INNER JOIN room_packages ON room.room_package_id = room_packages.room_package_id WHERE recerved_rooms.room_id IS NULL AND room_packages.room_package_id = @packageid", CONNECTION.CON);
                            da2.SelectCommand.Parameters.AddWithValue("@packageid", dr["room_package_id"].ToString());
                            DataTable getroomdetails = new DataTable();
                            da2.Fill(getroomdetails);

                            panelPackageName[a] = new Panel();
                            panelPackageName[a].Size = new Size(1035, 35);
                            panelPackageName[a].BackColor = Color.FromArgb(60,60,60);
                            lblPanelName[a] = new Label();
                            lblPanelName[a].AutoSize = false;
                            lblPanelName[a].Dock = DockStyle.Fill;
                            lblPanelName[a].TextAlign = ContentAlignment.MiddleLeft;
                            lblPanelName[a].Text = dr["package_name"].ToString();
                            lblPanelName[a].Font = new Font("Segoe UI Semibold", 12);
                            lblPanelName[a].ForeColor = Color.White;
                            panelPackageName[a].Controls.Add(lblPanelName[a]);
                            flpRoomsOverview.Invoke(new Action(() =>
                            {
                                flpRoomsOverview.Controls.Add(panelPackageName[a]);
                            }));
                            
                            if (getroomdetails.Rows.Count > 0)
                            {
                                int b = 0;
                                Panel[] panelRoomParent = new Panel[getroomdetails.Rows.Count];
                                Label[] lblParentText = new Label[getroomdetails.Rows.Count];
                                Label[] lblRoomNumber = new Label[getroomdetails.Rows.Count];
                                Label[] lblRoomStatus = new Label[getroomdetails.Rows.Count];
                                Label[] lblNumberofDays = new Label[getroomdetails.Rows.Count];

                                foreach (DataRow dr2 in getroomdetails.Rows)
                                {
                                    panelRoomParent[b] = new Panel();
                                    panelRoomParent[b].Size = new Size(220, 90);
                                    panelRoomParent[b].BackColor = getRoomColor(dr2["current_status"].ToString());
                                    //panelRoomParent[b].Padding = new Padding(10);
                                    panelRoomParent[b].BorderStyle = BorderStyle.FixedSingle;


                                    //GetsDays
                                    double days = (Convert.ToDateTime(dr2["depature_Date"].ToString()) - Convert.ToDateTime(dr2["arrival_date"].ToString())).TotalDays;

                                    lblNumberofDays[b] = new Label();
                                    lblNumberofDays[b].Dock = DockStyle.Bottom;
                                    lblNumberofDays[b].Size = new Size(220, 20);
                                    lblNumberofDays[b].Font = new Font("Segoe UI Semibold", 9, FontStyle.Bold);
                                    lblNumberofDays[b].ForeColor = Color.Black;
                                    lblNumberofDays[b].Text = days.ToString() + " days";
                                    lblNumberofDays[b].TextAlign = ContentAlignment.MiddleCenter;
                                    lblNumberofDays[b].BackColor = Color.FromName("ControlDarkDark");
                                    panelRoomParent[b].Controls.Add(lblNumberofDays[b]);

                                    lblRoomNumber[b] = new Label();
                                    lblRoomNumber[b].Location = new Point(0, 0);
                                    lblRoomNumber[b].Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold);
                                    lblRoomNumber[b].ForeColor = Color.Black;
                                    lblRoomNumber[b].Text = "Room " + dr2["room_name"].ToString();
                                    lblRoomNumber[b].BackColor = Color.Transparent;
                                    panelRoomParent[b].Controls.Add(lblRoomNumber[b]);

                                    lblRoomStatus[b] = new Label();
                                    lblRoomStatus[b].AutoSize = false;
                                    lblRoomStatus[b].RightToLeft = RightToLeft.Yes;
                                    lblRoomStatus[b].Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold);
                                    lblRoomStatus[b].ForeColor = Color.Black;
                                    lblRoomStatus[b].Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dr2["current_status"].ToString().ToLower());
                                    lblRoomStatus[b].BackColor = Color.Transparent;
                                    lblRoomStatus[b].Width = panelRoomParent[b].Width;
                                    panelRoomParent[b].Controls.Add(lblRoomStatus[b]);

                                    lblParentText[b] = new Label();
                                    lblParentText[b].AutoSize = false;
                                    lblParentText[b].Dock = DockStyle.Fill;
                                    lblParentText[b].TextAlign = ContentAlignment.MiddleCenter;
                                    lblParentText[b].Font = new Font("Segoe UI Semibold", 13, FontStyle.Bold);
                                    lblParentText[b].ForeColor = Color.Black;
                                    lblParentText[b].Text = dr2["first_name"].ToString() + " " + dr2["last_name"].ToString();
                                    lblParentText[b].BackColor = Color.Transparent;
                                    panelRoomParent[b].Controls.Add(lblParentText[b]);

                                    flpRoomsOverview.Invoke(new Action(() =>
                                    {
                                        flpRoomsOverview.Controls.Add(panelRoomParent[b]);
                                    }));
                                    b++;
                                }
                            }
                            a++;
                            getroomdetails.Clear();
                        }
                    }
                    getpackgenames.Clear();
                }
                Panel pnl = new Panel();
                pnl.Size = new Size(1035, 35);
                pnl.BackColor = Color.FromArgb(60, 60, 60);
                flpRoomsOverview.Invoke(new Action(() =>
                {
                    flpRoomsOverview.Controls.Add(pnl);
                }));
                loadingCircleRoomOverview.Invoke(new Action(() =>
                {
                    loadingCircleRoomOverview.Visible = false;
                    loadingCircleRoomOverview.Active = false;
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "HyperFlex Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                CONNECTION.close_connection();
            }
        }

        private Color getRoomColor(String Type)
        {
            Color color;
            switch (Type)
            {
                case "AVAILABLE": color = Color.Green; break;
                case "MAINTANANCE": color = Color.SaddleBrown; break;
                case "CHECKED IN": color = Color.Firebrick; break;
                case "CHECKED OUT": color = Color.DodgerBlue; break;
                case "RESERVED": color = Color.DodgerBlue; break;
                case "HOUSE KEEPING": color = Color.DarkOrange; break;
                default: color = Color.LightSlateGray; break;
            }
            return color;
        }
    }
}
