using CustomWindowsForm.CLASS;
using CustomWindowsForm.FORMS;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace CustomWindowsForm
{
    public partial class CATEGORY : Form
    {
        public CATEGORY()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;

        }
       // private fmProgress m_fmProgress = null;
        private BackgroundWorker m_AsyncWorker = new BackgroundWorker();
        private  void BlackForm_LoadAsync(object sender, EventArgs e)
        {
            LOADCATEGORYlIST();
            txt_category_name.Focus();
          
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



        private void TopBorderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isTopBorderPanelDragged = true;
            }
            else
            {
                isTopBorderPanelDragged = false;
            }
        }


        private void TopBorderPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Y < this.Location.Y)
            {
                if (isTopBorderPanelDragged)
                {
                    if (this.Height < 50)
                    {
                        this.Height = 50;
                        isTopBorderPanelDragged = false;
                    }
                    else
                    {
                        this.Location = new Point(this.Location.X, this.Location.Y + e.Y);
                        this.Height = this.Height - e.Y;
                    }
                }
            }
        }


        private void TopBorderPanel_MouseUp(object sender, MouseEventArgs e)
        {
            isTopBorderPanelDragged = false;
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



        private void LeftPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.Location.X <= 0 || e.X < 0)
            {
                isLeftPanelDragged = false;
                this.Location = new Point(10, this.Location.Y);
            }
            else
            {
                if (e.Button == MouseButtons.Left)
                {
                    isLeftPanelDragged = true;
                }
                else
                {
                    isLeftPanelDragged = false;
                }
            }
        }

        private void LeftPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.X < this.Location.X)
            {
                if (isLeftPanelDragged)
                {
                    if (this.Width < 100)
                    {
                        this.Width = 100;
                        isLeftPanelDragged = false;
                    }
                    else
                    {
                        this.Location = new Point(this.Location.X + e.X, this.Location.Y);
                        this.Width = this.Width - e.X;
                    }
                }
            }
        }

        private void LeftPanel_MouseUp(object sender, MouseEventArgs e)
        {
            isLeftPanelDragged = false;
        }



        private void RightPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isRightPanelDragged = true;
            }
            else
            {
                isRightPanelDragged = false;
            }
        }

        private void RightPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (isRightPanelDragged)
            {
                if (this.Width < 100)
                {
                    this.Width = 100;
                    isRightPanelDragged = false;
                }
                else
                {
                    this.Width = this.Width + e.X;
                }
            }
        }

        private void RightPanel_MouseUp(object sender, MouseEventArgs e)
        {
            isRightPanelDragged = false;
        }



        private void BottomPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isBottomPanelDragged = true;
            }
            else
            {
                isBottomPanelDragged = false;
            }
        }

        private void BottomPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (isBottomPanelDragged)
            {
                if (this.Height < 50)
                {
                    this.Height = 50;
                    isBottomPanelDragged = false;
                }
                else
                {
                    this.Height = this.Height + e.Y;
                }
            }
        }

        private void BottomPanel_MouseUp(object sender, MouseEventArgs e)
        {
            isBottomPanelDragged = false;
        }


        private void _MinButton_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
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

        private void _CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }




        private void RightBottomPanel_1_MouseDown(object sender, MouseEventArgs e)
        {
            isRightBottomPanelDragged = true;
        }

        private void RightBottomPanel_1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isRightBottomPanelDragged)
            {
                if (this.Width < 100 || this.Height < 50)
                {
                    this.Width = 100;
                    this.Height = 50;
                    isRightBottomPanelDragged = false;
                }
                else
                {
                    this.Width = this.Width + e.X;
                    this.Height = this.Height + e.Y;
                }
            }
        }


        private void RightBottomPanel_1_MouseUp(object sender, MouseEventArgs e)
        {
            isRightBottomPanelDragged = false;
        }

        private void RightBottomPanel_2_MouseDown(object sender, MouseEventArgs e)
        {
            RightBottomPanel_1_MouseDown(sender, e);
        }

        private void RightBottomPanel_2_MouseMove(object sender, MouseEventArgs e)
        {
            RightBottomPanel_1_MouseMove(sender, e);
        }

        private void RightBottomPanel_2_MouseUp(object sender, MouseEventArgs e)
        {
            RightBottomPanel_1_MouseUp(sender, e);
        }



        private void LeftBottomPanel_1_MouseDown(object sender, MouseEventArgs e)
        {
            isLeftBottomPanelDragged = true;
        }

        private void LeftBottomPanel_1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.X < this.Location.X)
            {
                if (isLeftBottomPanelDragged || this.Height < 50)
                {
                    if (this.Width < 100)
                    {
                        this.Width = 100;
                        this.Height = 50;
                        isLeftBottomPanelDragged = false;
                    }
                    else
                    {
                        this.Location = new Point(this.Location.X + e.X, this.Location.Y);
                        this.Width = this.Width - e.X;
                        this.Height = this.Height + e.Y;
                    }
                }
            }
        }

        private void LeftBottomPanel_1_MouseUp(object sender, MouseEventArgs e)
        {
            isLeftBottomPanelDragged = false;
        }

        private void LeftBottomPanel_2_MouseDown(object sender, MouseEventArgs e)
        {
            LeftBottomPanel_1_MouseDown(sender, e);
        }

        private void LeftBottomPanel_2_MouseMove(object sender, MouseEventArgs e)
        {
            LeftBottomPanel_1_MouseMove(sender, e);
        }

        private void LeftBottomPanel_2_MouseUp(object sender, MouseEventArgs e)
        {
            LeftBottomPanel_1_MouseUp(sender, e);
        }




        private void RightTopPanel_1_MouseDown(object sender, MouseEventArgs e)
        {
            isRightTopPanelDragged = true;
        }

        private void RightTopPanel_1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Y < this.Location.Y || e.X < this.Location.X)
            {
                if (isRightTopPanelDragged)
                {
                    if (this.Height < 50 || this.Width < 100)
                    {
                        this.Height = 50;
                        this.Width = 100;
                        isRightTopPanelDragged = false;
                    }
                    else
                    {
                        this.Location = new Point(this.Location.X, this.Location.Y + e.Y);
                        this.Height = this.Height - e.Y;
                        this.Width = this.Width + e.X;
                    }
                }
            }
        }

        private void RightTopPanel_1_MouseUp(object sender, MouseEventArgs e)
        {
            isRightTopPanelDragged = false;
        }

        private void RightTopPanel_2_MouseDown(object sender, MouseEventArgs e)
        {
            RightTopPanel_1_MouseDown(sender, e);
        }

        private void RightTopPanel_2_MouseMove(object sender, MouseEventArgs e)
        {
            RightTopPanel_1_MouseMove(sender, e);
        }

        private void RightTopPanel_2_MouseUp(object sender, MouseEventArgs e)
        {
            RightTopPanel_1_MouseUp(sender, e);
        }





        private void LeftTopPanel_1_MouseDown(object sender, MouseEventArgs e)
        {
            isLeftTopPanelDragged = true;
        }

        private void LeftTopPanel_1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.X < this.Location.X || e.Y < this.Location.Y)
            {
                if (isLeftTopPanelDragged)
                {
                    if (this.Width < 100 || this.Height < 50)
                    {
                        this.Width = 100;
                        this.Height = 100;
                        isLeftTopPanelDragged = false;
                    }
                    else
                    {
                        this.Location = new Point(this.Location.X + e.X, this.Location.Y);
                        this.Width = this.Width - e.X;
                        this.Location = new Point(this.Location.X, this.Location.Y + e.Y);
                        this.Height = this.Height - e.Y;
                    }
                }
            }

        }

        private void LeftTopPanel_1_MouseUp(object sender, MouseEventArgs e)
        {
            isLeftTopPanelDragged = false;
        }

        private void LeftTopPanel_2_MouseDown(object sender, MouseEventArgs e)
        {
            LeftTopPanel_1_MouseDown(sender, e);
        }

        private void LeftTopPanel_2_MouseMove(object sender, MouseEventArgs e)
        {
            LeftTopPanel_1_MouseMove(sender, e);
        }

        private void LeftTopPanel_2_MouseUp(object sender, MouseEventArgs e)
        {
            LeftTopPanel_1_MouseUp(sender, e);
        }
       
        private void WindowTextLabel_MouseDown(object sender, MouseEventArgs e)
        {
            TopPanel_MouseDown(sender, e);
        }

        private void WindowTextLabel_MouseMove(object sender, MouseEventArgs e)
        {
            TopPanel_MouseMove(sender, e);
        }

        private void WindowTextLabel_MouseUp(object sender, MouseEventArgs e)
        {
            TopPanel_MouseUp(sender, e);
        }
        String res = String.Empty;
        //private async Task lOADgRIDVIEW()
        //{

        //    loadingCircle1.Visible = true;
        //    loadingCircle1.Size = new Size(75, 75);
        //    loadingCircle1.Active = true;
        //    await Task.Run(() => {
        //         LOADCATEGORYlIST();
        //    });
        //    loadingCircle1.Active = false;
        //    loadingCircle1.Visible = false;
        //    loadingCircle1.Size = new Size(1, 1);

           
        //}
        //private async Task SAVECATEGORY()
        //{
            
        //    loadingCircle1.Visible = true;
        //    loadingCircle1.Size = new Size(75, 75);
        //    loadingCircle1.Active = true;
        //    btn_save.Enabled = false;
        //    await Task.Run(() => {
        //        res = saveCategory();
        //    });
        //    loadingCircle1.Active = false;
        //    loadingCircle1.Visible = false;
        //    loadingCircle1.Size = new Size(1, 1);
        //    btn_save.Enabled = true;
        //    BTN_NEW.Focus();
        //    txt_category_name.Clear();
        //    if (res == "1")
        //    {
        //        lbl_category.Text = GETMAXID();
        //        MSGBOX mdg = new MSGBOX(MessageAlertHeder.Warning(), "CATEGORY ADDED SUCCESFULLY!", MessageAlertImage.Warning());
        //        mdg.ShowDialog();
        //    }
        //    else if (res == "0")
        //    {
        //        MSGBOX mdg = new MSGBOX(MessageAlertHeder.Warning(), "CATEGORY ADDED FAILED!", MessageAlertImage.Warning());
        //        mdg.ShowDialog();
        //    }
        //    else
        //    {
        //        MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), saveCategory(), MessageAlertImage.Alert());
        //        mdg.ShowDialog();
        //    }
          
        //}
        //private async Task UPDATE_CATEGORY_NAME()
        //{

        //    loadingCircle1.Visible = true;
        //    loadingCircle1.Size = new Size(75, 75);
        //    loadingCircle1.Active = true;
        //    btn_save.Enabled = false;
        //    await Task.Run(() => {
        //        res = updatecategory();
        //        // LOADCATEGORYlIST();
        //    });
        //    loadingCircle1.Active = false;
        //    loadingCircle1.Visible = false;
        //    loadingCircle1.Size = new Size(1, 1);
        //    btn_save.Enabled = true;
        //    BTN_NEW.Focus();
        //    txt_category_name.Clear();
        //    lbl_category.Text = "N/A";
        //    btn_save.Text = "SAVE";
        //    if (res == "1")
        //    {
        //        lbl_category.Text = GETMAXID();
        //        MSGBOX mdg = new MSGBOX(MessageAlertHeder.Warning(), "CATEGORY UPDATED SUCCESFULLY!", MessageAlertImage.Warning());
        //        mdg.ShowDialog();
        //    }
        //    else if (res == "0")
        //    {
        //        MSGBOX mdg = new MSGBOX(MessageAlertHeder.Warning(), "CATEGORY UPDATED FAILED!", MessageAlertImage.Warning());
        //        mdg.ShowDialog();
        //    }
        //    else
        //    {
        //        MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), saveCategory(), MessageAlertImage.Alert());
        //        mdg.ShowDialog();
        //    }

        //}
        private void saveCategory()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlCommand da =new MySqlCommand("INSERT INTO category(caegory_name) VALUES(@caegory_name)", CONNECTION.CON))
                {
                    da.Parameters.Clear();
                    da.Parameters.AddWithValue("@caegory_name",txt_category_name.Text);
                    if( da.ExecuteNonQuery()>0)
                    {

                        lbl_category.Text = GETMAXID();
                        MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "CATEGORY ADDED SUCCESFULLY!", MessageAlertImage.Success());
                        mdg.ShowDialog();

                    }
                    else
                    {
                        MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), "CATEGORY ADDED FAILED!", MessageAlertImage.Alert());
                        mdg.ShowDialog();
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
        private void Updatecategory()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlCommand da = new MySqlCommand("UPDATE category SET caegory_name=@caegory_name WHERE categry_id=@categry_id", CONNECTION.CON))
                {
                    da.Parameters.Clear();
                    da.Parameters.AddWithValue("@categry_id", lbl_category.Text);
                    da.Parameters.AddWithValue("@caegory_name", txt_category_name.Text);
                    if (da.ExecuteNonQuery() > 0)
                    {
                        MSGBOX mdg = new MSGBOX(MessageAlertHeder.Success(), "CATEGORY UPDATED SUCCESFULLY!", MessageAlertImage.Success());
                        mdg.ShowDialog();
                    }
                    else
                    {
                        MSGBOX mdg = new MSGBOX(MessageAlertHeder.Alert(), "CATEGORY UPDATED SUCCESFULLY!", MessageAlertImage.Alert());
                        mdg.ShowDialog();
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
        private  void btn_save_ClickAsync(object sender, EventArgs e)
        {
            if (txt_category_name.Text.Length == 0)
            {
                txt_category_name.Focus();
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Warning(), "PLEASE ENTER CATEGORY NAME", MessageAlertImage.Warning());
                mdg.ShowDialog();
            }
            else if (btn_save.Text == "SAVE")
            {
                Cursor.Current = Cursors.WaitCursor;
                saveCategory();
                LOADCATEGORYlIST();
                BTN_NEW.PerformClick();
                if (DGV_CATEGORY.Rows.Count>0)
                DGV_CATEGORY.FirstDisplayedScrollingRowIndex = DGV_CATEGORY.RowCount - 1;
                Cursor.Current = Cursors.Default;
            }
            else if (btn_save.Text == "UPDATE")
            {
                Cursor.Current = Cursors.WaitCursor;
                Updatecategory();
                LOADCATEGORYlIST();
                BTN_NEW.PerformClick();
                Cursor.Current = Cursors.Default;
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void txt_category_name_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                btn_save.Focus();
            }
        }

        private void LOADCATEGORYlIST()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter DA=new MySqlDataAdapter("SELECT categry_id,caegory_name FROM category", CONNECTION.CON))
                {
                    DataTable DT = new DataTable();
                    DA.Fill(DT);
                    if(DT.Rows.Count>0)
                    {
                        DGV_CATEGORY.DataSource = DT;
                        DGV_CATEGORY.AutoGenerateColumns = false;
                    }
                    else
                    {
                        DGV_CATEGORY.DataSource = null;
                    }
                }

            }
            catch (Exception EX)
            {

                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), EX.Message, MessageAlertImage.Error());
                mdg.ShowDialog();
            }
        }
        private void LOADCATEGORYlIST_SEARCH_BY_NAME(TextBox txt_cat_name)
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter DA = new MySqlDataAdapter("SELECT categry_id,caegory_name FROM category where caegory_name LIKE @caegory_name", CONNECTION.CON))
                {
                    DA.SelectCommand.Parameters.Clear();
                    DA.SelectCommand.Parameters.AddWithValue("@caegory_name","%"+txt_category_name.Text+"%");
                    DataTable DT = new DataTable();
                    DA.Fill(DT);
                    if (DT.Rows.Count > 0)
                    {
                        DGV_CATEGORY.DataSource = DT;
                        DGV_CATEGORY.AutoGenerateColumns = false;
                    }
                    else
                    {
                        DGV_CATEGORY.DataSource = null;
                    }
                }

            }
            catch (Exception EX)
            {

                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), EX.Message, MessageAlertImage.Error());
                mdg.ShowDialog();
            }
        }
        private String  GETMAXID()
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter DA = new MySqlDataAdapter("SELECT MAX(categry_id) FROM category", CONNECTION.CON))
                {
                    DataTable DT = new DataTable();
                    DA.Fill(DT);
                    if (DT.Rows.Count > 0)
                    {
                        return DT.Rows[0].Field<int>(0).ToString();
                    }
                    else
                    {
                        return  "0";
                    }

                }

            }
            catch (Exception EX)
            {

                return EX.ToString();
            }
        }
        private void BTN_NEW_Click(object sender, EventArgs e)
        {
            txt_category_name.Clear();
            txt_category_name.Focus();
            lbl_category.Text = "N/A";
            btn_save.Text = "SAVE";
        }

      

        private void DGV_CATEGORY_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            lbl_category.Text = DGV_CATEGORY.SelectedRows[0].Cells[0].Value.ToString();
            txt_category_name.Text = DGV_CATEGORY.SelectedRows[0].Cells[1].Value.ToString();
            btn_save.Text = "UPDATE";
        }

        private void txt_category_name_TextChanged(object sender, EventArgs e)
        {
            if(chk_search_category.Checked==true)
            {
                if(txt_category_name.Text.Length>0)
                {
                    LOADCATEGORYlIST_SEARCH_BY_NAME(txt_category_name);
                }
               
            }
           
        }
    }
}
