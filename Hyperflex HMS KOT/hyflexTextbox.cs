using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hyperflex_HMS_KOT
{
    public partial class hyflexTextbox : TextBox
    {
        public hyflexTextbox()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        #region Textbox Focus Color
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
      

        public enum String_Type
        {
            String,
            Numeric,
            Currency,
            Double_P2,
            Double_P3,
        }

        [Browsable(true)]
        [Description("Select TextBox Value Type")]
        [Category("Hyflex Features")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DisplayName("TextBoxType")]

        public String_Type User_selection
        {
            get;
            set;
        }
        #endregion

        #region
        public enum Resust
        {
            True,
            False,
        }

        [Browsable(true)]
        [Description("Set Null String Check")]
        [Category("Hyflex Features")]
        [DisplayName("IsNullCheck")]

        public Resust User_null_check
        {
            get;
            set;
        }
        #endregion

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            this.Font = new Font("Segoe UI", this.Font.Size);
            this.Font = new Font(this.Font.FontFamily, 10);
            if (User_selection == String_Type.Currency)
            {
                this.Text = "0.00";
            }
            if (User_selection == String_Type.Double_P2)
            {
                this.Text = "0.00";
            }
            if (User_selection == String_Type.Double_P3)
            {
                this.Text = "0.000";
            }
            if (User_selection == String_Type.Numeric)
            {
                this.Text = "0";
            }
        }
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            this.BackColor = focus_clr;
        }
        protected override void OnLeave(EventArgs e)
        {
            base.OnEnter(e);
            this.BackColor = SystemColors.Window;

            //if (User_selection == String_Type.Double_P2)
            //{
            //    this.Text = Convert.ToDouble(this.Text).ToString("f2");
            //}
            //if (User_selection == String_Type.Double_P3)
            //{
            //    this.Text = Convert.ToDouble(this.Text).ToString("f3");
            //}
            //if (User_selection == String_Type.Currency)
            //{
            //    this.Text = Convert.ToDouble(this.Text).ToString("n2");
            //}
            //if (User_selection == String_Type.Numeric)
            //{
            //    if (this.Text == string.Empty)
            //        this.Text = "0";
            //    else
            //        this.Text = Convert.ToDouble(this.Text).ToString() ;
            //}

            if (User_selection == String_Type.String)
            {
                if (this.Text == string.Empty)
                    this.Text = "N/A";
            }
            if (User_selection == String_Type.Numeric)
            {
                if (this.Text == string.Empty)
                    this.Text = "0";
                else
                    this.Text = Convert.ToDouble(this.Text).ToString();
            }
            if (User_selection == String_Type.Double_P3)
            {
                if (this.Text == string.Empty)
                    this.Text = "0.000";
                else
                    this.Text = Convert.ToDouble(this.Text).ToString("F3");
            }
            if (User_selection == String_Type.Double_P2)
            {
                if (this.Text == string.Empty)
                    this.Text = "0.00";
                else
                    this.Text = Convert.ToDouble(this.Text).ToString("F2");
            }
            if (User_selection == String_Type.Currency)
            {
                if (this.Text == string.Empty)
                    this.Text = "0.00";
                else
                    this.Text = Convert.ToDouble(this.Text).ToString("N2");
            }
        }
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (User_selection == String_Type.Numeric)
            {
                base.OnKeyPress(e);
                NumberFormatInfo fi = CultureInfo.CurrentCulture.NumberFormat;
                string c = e.KeyChar.ToString();
                if (char.IsDigit(c, 0))
                    return;
                if ((SelectionStart == 0) && (c.Equals(fi.NegativeSign)))
                    return;
                if ((((int)e.KeyChar == 22) || ((int)e.KeyChar == 3))
                    && ((ModifierKeys & Keys.Control) == Keys.Control))
                    return;
                if (e.KeyChar == '\b')
                    return;
                e.Handled = true;
            }
            else if (User_selection == String_Type.Double_P2)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
                {
                    e.Handled = true;
                }
                if ((e.KeyChar == '.') && (this.Text.IndexOf('.') > -1))
                {
                    e.Handled = true;
                }

            }
            else if (User_selection == String_Type.Double_P3)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
                {
                    e.Handled = true;
                }
                if ((e.KeyChar == '.') && (this.Text.IndexOf('.') > -1))
                {
                    e.Handled = true;
                }

            }
            else if (User_selection == String_Type.Currency)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
                {
                    e.Handled = true;
                }
                if ((e.KeyChar == '.') && (this.Text.IndexOf('.') > -1))
                {
                    e.Handled = true;
                }

            }
            else
            {

            }
        }
    }
}
