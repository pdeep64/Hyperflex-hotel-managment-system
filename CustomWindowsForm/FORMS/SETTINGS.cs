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
    public partial class SETTINGS : Form
    {
        public SETTINGS()
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

        private void SETTINGS_Load(object sender, EventArgs e)
        {
            LOAD_GRID_VIEW();
            TXT_EXCHANGE_RATE.Text = HYFLEX_HMS.Properties.Settings.Default.exchange_rate_lrk.ToString("F2");
        }

        private void LOAD_GRID_VIEW()
        {
            try
            {
                double TOTAL = 0;
                CONNECTION.open_connection();
                using (MySqlDataAdapter DA=new MySqlDataAdapter("SELECT tax_type_id,tax_type,precentage FROM tax_details", CONNECTION.CON))
                {
                    DataTable DT = new DataTable();
                    DA.Fill(DT);
                    if(DT.Rows.Count>0)
                    {
                        DGV_TAV.DataSource = DT;
                        DGV_TAV.AutoGenerateColumns = false;
                        foreach(DataRow DR in DT.Rows)
                        {
                            TOTAL = TOTAL + DR.Field<double>(2);
                        }
                        TXT_TOTAL_TAX.Text = TOTAL.ToString("F2");
                    }
                    else
                    {
                        DGV_TAV.DataSource = null;
                    }
                }
            }
            catch (Exception EX)
            {

            }
        }
        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
        private void UPDATE_RATE()
        {
            try
            {
                double TOTAL = 0;
                CONNECTION.open_connection();
                using (MySqlCommand DA = new MySqlCommand("UPDATE tax_details SET precentage=@precentage WHERE tax_type_id=@tax_type_id", CONNECTION.CON))
                {
                    DA.Parameters.Clear();
                    DA.Parameters.AddWithValue("@precentage",Convert.ToDouble( DGV_TAV.SelectedRows[0].Cells[2].Value));
                    DA.Parameters.AddWithValue("@tax_type_id", Convert.ToInt16(DGV_TAV.SelectedRows[0].Cells[0].Value));
                    if( DA.ExecuteNonQuery()>0)
                    {
                        MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "NEW RATE UPDATED!", MessageAlertImage.Success());
                        mdg.ShowDialog();
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
            if(TXT_TOTAL_TAX.Text.Length>0)
            {
                HYFLEX_HMS.Properties.Settings.Default.tax_percentage = Convert.ToDouble(TXT_TOTAL_TAX.Text);
                HYFLEX_HMS.Properties.Settings.Default.Save();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "NEW RATE SAVED!", MessageAlertImage.Success());
                mdg.ShowDialog();
            }
            else
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "PLEASE SET VAT VALUES", MessageAlertImage.Success());
                mdg.ShowDialog();
            }
           
        }

        private void DGV_TAV_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            UPDATE_RATE();
            LOAD_GRID_VIEW();

        }

        private void shapedButton1_Click(object sender, EventArgs e)
        {
            if(TXT_EXCHANGE_RATE.Text.Length>1)
            {
                HYFLEX_HMS.Properties.Settings.Default.exchange_rate_lrk = Convert.ToDouble(TXT_EXCHANGE_RATE.Text);
                HYFLEX_HMS.Properties.Settings.Default.Save();
                TXT_EXCHANGE_RATE.Text = HYFLEX_HMS.Properties.Settings.Default.exchange_rate_lrk.ToString("F2");

            }
        }
    }
}
