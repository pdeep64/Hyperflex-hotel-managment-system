using CustomWindowsForm.FORMS;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomWindowsForm.CLASS
{
    class CLS_METHODS
    {
        public static String GET_MAX_STRING_ID(String QRY)
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter DA = new MySqlDataAdapter(QRY, CONNECTION.CON))
                {
                    DataTable DT = new DataTable();
                    DA.Fill(DT);
                    if (DT.Rows.Count > 0)
                    {
                        return DT.Rows[0].Field<string>(0).ToString();
                    }
                    else
                    {
                        return "0";
                    }

                }

            }
            catch (Exception EX)
            {

                return EX.ToString();
            }
        }
        public static String GET_MAX_INT_ID(String QRY)
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter DA = new MySqlDataAdapter(QRY, CONNECTION.CON))
                {
                    DataTable DT = new DataTable();
                    DA.Fill(DT);
                    if (DT.Rows.Count > 0)
                    {
                        return DT.Rows[0].Field<int>(0).ToString();
                    }
                    else
                    {
                        return "0";
                    }

                }

            }
            catch (Exception EX)
            {

                return EX.ToString();
            }
        }
        public static double GET_DATA(String QRY)
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter DA = new MySqlDataAdapter(QRY, CONNECTION.CON))
                {
                    DataTable DT = new DataTable();
                    DA.Fill(DT);
                    if (DT.Rows.Count > 0)
                    {
                        return DT.Rows[0].Field<double>(0);
                    }
                    else
                    {
                        return 0;
                    }

                }

            }
            catch (Exception EX)
            {

                return 0;
            }
        }
        public static void FILL_COMBOBOX(hyflexComboBox CMB_CATEGORY,string QRY,string DisplayMember,string ValueMember,int SelectedIndex)
        {
            try
            {
                CONNECTION.open_connection();
                using (MySqlDataAdapter DA = new MySqlDataAdapter(QRY, CONNECTION.CON))
                {
                    DataTable DT = new DataTable();
                    DA.Fill(DT);
                    if (DT.Rows.Count > 0)
                    {
                        CMB_CATEGORY.DataSource = DT;
                        CMB_CATEGORY.DisplayMember = DisplayMember;
                        CMB_CATEGORY.ValueMember = ValueMember;
                        CMB_CATEGORY.SelectedIndex = SelectedIndex;
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
