using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace Hyperflex_HMS_KOT.CLASS
{
    class CLS_GENERATE_ID
    {
        public static string GEN_NEXT_KOT_NO()
        {
            string KOT_NO = "KO-";
            CONNECTION.open_connection();
            using (MySqlDataAdapter adp1 = new MySqlDataAdapter("SELECT order_no FROM kot_order", CONNECTION.CON))
            {
                DataTable tbl1 = new DataTable();
                adp1.Fill(tbl1);
                if (tbl1.Rows.Count > 0)
                {
                    using (MySqlDataAdapter adp2 = new MySqlDataAdapter("SELECT MAX(SUBSTRING(order_no,4,10)) FROM kot_order", CONNECTION.CON))
                    {
                        DataTable tbl2 = new DataTable();
                        adp2.Fill(tbl2);
                        if (tbl2.Rows.Count > 0)
                        {
                            int in_no = Convert.ToInt32(tbl2.Rows[0].Field<string>(0));
                            in_no = in_no + 1;
                            if (in_no < 10)
                                KOT_NO = KOT_NO + "0000" + in_no.ToString();
                            else if (in_no < 100)
                                KOT_NO = KOT_NO + "000" + in_no.ToString();
                            else if (in_no < 1000)
                                KOT_NO = KOT_NO + "00" + in_no.ToString();
                            else if (in_no < 10000)
                                KOT_NO = KOT_NO + "0" + in_no.ToString();
                            else
                                KOT_NO = KOT_NO + in_no.ToString();
                        }
                        else
                            KOT_NO = "KO-00001";
                    }
                }
                else
                    KOT_NO = "KO-00001";
            }
            return KOT_NO;
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
    }
}
