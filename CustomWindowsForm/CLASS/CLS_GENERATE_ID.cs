using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomWindowsForm.CLASS
{
    class CLS_GENERATE_ID
    {
        public static string GEN_NEXT_RESERVATION_NO()
        {
            string ACC = "RS-";
            CONNECTION.open_connection();
            using (MySqlDataAdapter adp1 = new MySqlDataAdapter("SELECT reservation_id FROM reservation", CONNECTION.CON))
            {
                DataTable tbl1 = new DataTable();
                adp1.Fill(tbl1);
                if (tbl1.Rows.Count > 0)
                {
                    using (MySqlDataAdapter adp2 = new MySqlDataAdapter("SELECT MAX(SUBSTRING(reservation_id,6,10)) FROM reservation", CONNECTION.CON))
                    {
                        DataTable tbl2 = new DataTable();
                        adp2.Fill(tbl2);
                        if (tbl2.Rows.Count > 0)
                        {
                            int in_no = Convert.ToInt32(tbl2.Rows[0].Field<string>(0));
                            in_no = in_no + 1;
                            if (in_no < 10)
                                ACC = ACC + "000000" + in_no.ToString();
                            else if (in_no < 100)
                                ACC = ACC + "00000" + in_no.ToString();
                            else if (in_no < 1000)
                                ACC = ACC + "0000" + in_no.ToString();
                            else if (in_no < 10000)
                                ACC = ACC + "000" + in_no.ToString();
                            else if (in_no < 100000)
                                ACC = ACC + "00" + in_no.ToString();
                            else if (in_no < 100000)
                                ACC = ACC + "0" + in_no.ToString();
                            else
                                ACC = ACC + in_no.ToString();
                        }
                        else
                            ACC = "RS-0000001";
                    }
                }
                else
                    ACC = "RS-0000001";
            }
            return ACC;
        }
        public static string GEN_NEXT_ACC_NO()
        {
            string ACC = "CR-";
            CONNECTION.open_connection();
            using (MySqlDataAdapter adp1 = new MySqlDataAdapter("SELECT account_no FROM supplier_account", CONNECTION.CON))
            {
                DataTable tbl1 = new DataTable();
                adp1.Fill(tbl1);
                if (tbl1.Rows.Count > 0)
                {
                    using (MySqlDataAdapter adp2 = new MySqlDataAdapter("SELECT MAX(SUBSTRING(account_no,4,10)) FROM supplier_account", CONNECTION.CON))
                    {
                        DataTable tbl2 = new DataTable();
                        adp2.Fill(tbl2);
                        if (tbl2.Rows.Count > 0)
                        {
                            int in_no = Convert.ToInt32(tbl2.Rows[0].Field<string>(0));
                            in_no = in_no + 1;
                            if (in_no < 10)
                                ACC = ACC + "0000" + in_no.ToString();
                            else if (in_no < 100)
                                ACC = ACC + "000" + in_no.ToString();
                            else if (in_no < 1000)
                                ACC = ACC + "00" + in_no.ToString();
                            else if (in_no < 10000)
                                ACC = ACC + "0" + in_no.ToString();
                            else
                                ACC = ACC + in_no.ToString();
                        }
                        else
                            ACC = "CR-00001";
                    }
                }
                else
                    ACC = "CR-00001";
            }
            return ACC;
        }
        public static string GEN_NEXT_GUEST_NO()
        {
            string ACC = "GU-";
            CONNECTION.open_connection();
            using (MySqlDataAdapter adp1 = new MySqlDataAdapter("SELECT guest_id FROM guest", CONNECTION.CON))
            {
                DataTable tbl1 = new DataTable();
                adp1.Fill(tbl1);
                if (tbl1.Rows.Count > 0)
                {
                    using (MySqlDataAdapter adp2 = new MySqlDataAdapter("SELECT MAX(SUBSTRING(guest_id,4,10)) FROM guest", CONNECTION.CON))
                    {
                        DataTable tbl2 = new DataTable();
                        adp2.Fill(tbl2);
                        if (tbl2.Rows.Count > 0)
                        {
                            int in_no = Convert.ToInt32(tbl2.Rows[0].Field<string>(0));
                            in_no = in_no + 1;
                            if (in_no < 10)
                                ACC = ACC + "00000" + in_no.ToString();
                            else if (in_no < 100)
                                ACC = ACC + "0000" + in_no.ToString();
                            else if (in_no < 1000)
                                ACC = ACC + "000" + in_no.ToString();
                            else if (in_no < 10000)
                                ACC = ACC + "00" + in_no.ToString();
                            else if (in_no < 100000)
                                ACC = ACC + "0" + in_no.ToString();
                            else
                                ACC = ACC + in_no.ToString();
                        }
                        else
                            ACC = "GU-000001";
                    }
                }
                else
                    ACC = "GU-000001";
            }
            return ACC;
        }

        public static string GEN_NEXT_GRN_NO()
        {
            string GRN = "GR-";
            CONNECTION.open_connection();
            using (MySqlDataAdapter adp1 = new MySqlDataAdapter("SELECT grn_no FROM grn", CONNECTION.CON))
            {
                DataTable tbl1 = new DataTable();
                adp1.Fill(tbl1);
                if (tbl1.Rows.Count > 0)
                {
                    using (MySqlDataAdapter adp2 = new MySqlDataAdapter("SELECT MAX(SUBSTRING(grn_no,4,10)) FROM grn", CONNECTION.CON))
                    {
                        DataTable tbl2 = new DataTable();
                        adp2.Fill(tbl2);
                        if (tbl2.Rows.Count > 0)
                        {
                            int in_no = Convert.ToInt32(tbl2.Rows[0].Field<string>(0));
                            in_no = in_no + 1;
                            if (in_no < 10)
                                GRN = GRN + "0000" + in_no.ToString();
                            else if (in_no < 100)
                                GRN = GRN + "000" + in_no.ToString();
                            else if (in_no < 1000)
                                GRN = GRN + "00" + in_no.ToString();
                            else if (in_no < 10000)
                                GRN = GRN + "0" + in_no.ToString();
                            else
                                GRN = GRN + in_no.ToString();
                        }
                        else
                            GRN = "GR-00001";
                    }
                }
                else
                    GRN = "GR-00001";
            }
            return GRN;
        }
    }
}
