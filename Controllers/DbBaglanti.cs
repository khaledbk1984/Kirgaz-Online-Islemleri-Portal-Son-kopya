using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Kirgaz_Online_Islemleri_Portal_Son_kopya.Controllers
{
    public class DbBaglanti
    {
        private OracleConnection con;
        //private OracleCommand cmd;
        private OracleDataAdapter sda;
        private DataTable dt;

        public OracleConnection OracleConnect(String connectionString)
        {
            try
            {
                con = new OracleConnection(connectionString);
                con.Open();
                return con;
            }
            catch (OracleException e)
            {
                throw e;
            }
        }

        //public int Command(string ConStr, OracleConnection conn)
        //{
        //    int adet = 0;

        //    try
        //    {
        //        cmd = new OracleCommand(ConStr, conn);
        //        adet = cmd.ExecuteNonQuery();
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //    finally
        //    {
        //        cmd.Dispose();
        //        conn.Close();
        //        conn.Dispose();
        //    }
        //    return adet;
        //}

        //public string Command22(string ConStr, OracleConnection conn)
        //{
        //    int adet = 0;
        //    string sonuc = "";

        //    try
        //    {
        //        cmd = new OracleCommand(ConStr, conn);
        //        adet = cmd.ExecuteNonQuery();
        //    }
        //    catch (Exception e)
        //    {
        //        sonuc = e.Message;
        //        throw e;
        //    }
        //    finally
        //    {
        //        cmd.Dispose();
        //        conn.Close();
        //        conn.Dispose();
        //    }
        //    return sonuc;
        //}

        public DataTable GetDataTable(string ConStr, OracleConnection conn)
        {
            try
            {
                dt = new DataTable();
                sda = new OracleDataAdapter(ConStr, conn);
                sda.Fill(dt);

                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                sda.Dispose();
                conn.Close();
                conn.Dispose();
            }
        }

        public DataRow GetDataRow(string ConStr, OracleConnection conn)
        {
            dt = GetDataTable(ConStr, conn);
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return dt.Rows[0];
            }
        }

        public string GetDataCell(string ConStr, OracleConnection conn)
        {
            dt = GetDataTable(ConStr, conn);
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return dt.Rows[0][0].ToString();
            }
        }

        public string GetDataCell_0(string ConStr, OracleConnection conn)
        {
            dt = GetDataTable(ConStr, conn);
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return dt.Rows[0][0].ToString();
            }
        }

    }
}
