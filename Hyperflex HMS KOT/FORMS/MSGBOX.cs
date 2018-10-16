using Hyperflex_HMS_KOT.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hyperflex_HMS_KOT.FORMS
{
    public partial class MSGBOX : Form
    {
        public MSGBOX(string lbl_headr,string lbl_message,string Image)
        {
            InitializeComponent();
            lbl_heder.Text = lbl_headr;
            txt_message.Text = lbl_message;
            Bitmap IMG;
            if (Image == "Warning")
            {
                pb_image.Image = Resources.warning;
                IMG= Resources.warning;
            } 
            else if (Image == "Alert")
            {
                pb_image.Image = Resources.alert;
                IMG = Resources.alert;
            }
            else if (Image == "Success")
            {
                pb_image.Image = Resources.success;
                IMG = Resources.success;
            }
            else if (Image == "Error")
            {
                pb_image.Image = Resources.error;
                IMG = Resources.error;
            }
            else if (Image == "Notification")
            {
                pb_image.Image = Resources.notification;
                IMG = Resources.notification;
            }
            else
            {
                pb_image.Image = Resources.warning;
                IMG = Resources.warning;
            }
               

            CenterPictureBox(pb_image,IMG);
        }
        private void CenterPictureBox(PictureBox picBox, Bitmap picImage)
        {
            if (picImage!=null)
            {
                picBox.Image = picImage;
                picBox.Location = new Point((picBox.Parent.ClientSize.Width / 2) - (picImage.Width / 2), 35);
                picBox.Refresh(); 
            }
        }
        private void CenterButton(Button btn_ok)
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

        private void MSGBOX_Load(object sender, EventArgs e)
        {

        }

        private void txt_message_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
