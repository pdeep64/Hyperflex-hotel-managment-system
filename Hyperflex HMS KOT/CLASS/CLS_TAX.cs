using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using Hyperflex_HMS_KOT.FORMS;

namespace Hyperflex_HMS_KOT.CLASS
{
    class CLS_TAX
    {
        public static double GetTotalTaxPercentage()
        {
            try
            {
                using(MySqlDataAdapter adp=new MySqlDataAdapter("SELECT IFNULL(SUM(precentage),0.00) FROM tax_details", CONNECTION.CON))
                {
                    DataTable tbl = new DataTable();
                    adp.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                        return tbl.Rows[0].Field<double>(0);
                    else
                        return 0;
                }
            }
            catch (Exception ex)
            {
                MSGBOX mdg = new MSGBOX(MessageAlertHeder.Error(), ex.Message, MessageAlertImage.Error());
                mdg.ShowDialog();
                return 0;
            }
        }
    }
}
