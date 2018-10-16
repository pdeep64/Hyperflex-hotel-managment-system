using Hyperflex_HMS_KOT.CLASS;
using Hyperflex_HMS_KOT.FORMS;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperflex_HMS_KOT
{
    class CLS_ITEM
    {
        public static void LOAD_CATEGORY(hyflexComboBox CMB_CATEGORY)
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter DA = new MySqlDataAdapter("SELECT categry_id,caegory_name FROM category", CONNECTION.CON))
                {
                    DataTable DT = new DataTable();
                    DA.Fill(DT);
                    if (DT.Rows.Count > 0)
                    {
                        CMB_CATEGORY.DataSource = DT;
                        CMB_CATEGORY.DisplayMember = "caegory_name";
                        CMB_CATEGORY.ValueMember = "categry_id";
                        CMB_CATEGORY.SelectedIndex = -1;
                    }
                    else
                    {
                        CMB_CATEGORY.DataSource = null;
                    }
                }
            }
            catch (Exception EX)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), EX.Message, MessageAlertImage.Error());
                mdg.ShowDialog();
            }
        }
    }
}
