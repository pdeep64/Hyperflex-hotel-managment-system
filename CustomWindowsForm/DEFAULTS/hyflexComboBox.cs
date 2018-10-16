using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomWindowsForm
{
    public partial class hyflexComboBox : ComboBox
    {
        public hyflexComboBox()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
        Color Fcus_color = Color.LightGreen;
        public static Color focus_clr;
        [Browsable(true)]
        [Category("Hyflex Features")]
        [Description("Sets Focusing Color")]
        [DisplayName("FocusingColor")]
        public Color F_color
        {
            get
            {
                return this.Fcus_color;
            }
            set
            {
                this.Fcus_color = value;
                focus_clr = value;
            }
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            this.Font = new Font(this.Font.FontFamily, 10);
            this.Font = new Font("Segoe UI", this.Font.Size);

        }
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            this.BackColor = focus_clr;
        }
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            this.BackColor = SystemColors.Window;
        }
    }
}
