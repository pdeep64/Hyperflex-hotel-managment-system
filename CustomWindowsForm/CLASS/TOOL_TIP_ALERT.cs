using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomWindowsForm.CLASS
{
    class TOOL_TIP_ALERT
    {
        public object BUTTON;
        public object TEXTBOX;
        public String LABEL;

        public static void SHOW_TOOL_TIP_ALERT(Control OBJECT,String TITLE_TEXT,String TOOL_TIP)
        {
            ToolTip buttonToolTip = new ToolTip();
            buttonToolTip.ToolTipTitle = TITLE_TEXT;
            buttonToolTip.UseFading = true;
            buttonToolTip.UseAnimation = true;
            buttonToolTip.IsBalloon = true;
            buttonToolTip.ShowAlways = true;
            buttonToolTip.AutoPopDelay = 5000;
            buttonToolTip.InitialDelay = 1000;
            buttonToolTip.ReshowDelay = 500;
            buttonToolTip.SetToolTip(OBJECT, TOOL_TIP);
        }

    }
}
