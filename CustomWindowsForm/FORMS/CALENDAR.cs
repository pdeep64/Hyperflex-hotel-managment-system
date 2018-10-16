using Braincase.GanttChart;
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
using HYFLEX_HMS;

namespace CustomWindowsForm
{
    public partial class CALENDAR : Form
    {
        public CALENDAR()
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

        private void CALENDAR_Load(object sender, EventArgs e)
        {
            CONNECTION.open_connection();
            ProjectManager _mManager = new ProjectManager();
            using (MySqlDataAdapter da = new MySqlDataAdapter("SELECT room.room_name,room.current_status,guest.first_name,guest.last_name,reservation.arrival_date,reservation.depature_Date,room_packages.package_name FROM reservation,recerved_rooms,room_packages,room,guest WHERE reservation.reservation_id = recerved_rooms.reservation_no AND recerved_rooms.room_id = room.room_id AND room.room_package_id = room_packages.room_package_id AND guest.guest_id = reservation.guest_id AND reservation.depature_Date >= CURDATE()", CONNECTION.CON))
            {
                DataTable getRooms = new DataTable();
                da.Fill(getRooms);
                if (getRooms.Rows.Count > 0)
                {
                    int a = 0;
                    MyTask[] rooms = new MyTask[getRooms.Rows.Count];
                    foreach (DataRow dr in getRooms.Rows)
                    {
                        double duration, start;
                        rooms[a] = new MyTask(_mManager);
                        rooms[a].TaskColor = getRoomColor(dr["current_status"].ToString());
                        rooms[a].Name = "Room "+dr["room_name"].ToString();
                        
                        if (Convert.ToDateTime(dr["arrival_date"].ToString()).Date < DateTime.Now.Date)
                        {
                            duration = (Convert.ToDateTime(dr["depature_Date"].ToString()).Date - DateTime.Today.Date).TotalDays;
                            start = 0;
                        }
                        else
                        {
                            duration = (Convert.ToDateTime(dr["depature_Date"].ToString()).Date - Convert.ToDateTime(dr["arrival_date"].ToString()).Date).TotalDays;
                            start = (Convert.ToDateTime(dr["arrival_date"].ToString()).Date - DateTime.Today.Date).TotalDays;
                        }

                        _mManager.Add(rooms[a]);
                        _mManager.SetStart(rooms[a], (int)start);
                        _mManager.SetDuration(rooms[a], (int)duration);
                        gnattChartRoom.PaintHeader += (s, ee) =>
                        {
                            var headerFormat = new HeaderFormat();
                            headerFormat = ee.Format;
                            headerFormat.GradientLight = Color.Silver;
                            headerFormat.GradientDark = Color.Gray;
                            ee.Format = headerFormat;
                        };
                        gnattChartRoom.PaintTask += (s, ee) =>
                        {
                            MyTask ptask = ee.Task as MyTask;
                            if (ptask != null)
                            {
                                var format = new TaskFormat();
                                format = ee.Format;
                                format.BackFill = new SolidBrush(ptask.TaskColor);
                                format.Border = new Pen(new SolidBrush(ptask.TaskColor));
                                ee.Format = format;
                            }
                        };
                        gnattChartRoom.SetToolTip(rooms[a], dr["first_name"].ToString() + " " + dr["last_name"].ToString() + "\nRoom " + dr["room_name"].ToString() + "\n" + dr["current_status"].ToString());
                        a++;
                    }
                    
                    gnattChartRoom.Init(_mManager);
                    gnattChartRoom.CreateTaskDelegate = delegate () { return new MyTask(_mManager); };
                }
            }
        }

        [Serializable]
        public class MyResource
        {
            public string Name { get; set; }
        }
        /// <summary>
        /// A custom task of your own type deriving from the Task interface (optional)
        /// </summary>
        [Serializable]
        public class MyTask : Braincase.GanttChart.Task
        {
            public MyTask(ProjectManager manager)
                : base()
            {
                Manager = manager;
            }

            private ProjectManager Manager { get; set; }

            public new int Start { get { return base.Start; } set { Manager.SetStart(this, value); } }
            public new int End { get { return base.End; } set { Manager.SetEnd(this, value); } }
            public new int Duration { get { return base.Duration; } set { Manager.SetDuration(this, value); } }
            public new float Complete { get { return base.Complete; } set { Manager.SetComplete(this, value); } }

            public Color TaskColor { get; set; }
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
