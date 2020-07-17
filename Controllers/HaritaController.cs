using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kirgaz_Online_Islemleri_Portal_Son_kopya.Controllers
{
    public class HaritaController : Controller
    {
       


        //  public string kirikkaleDb = ConfigurationManager.ConnectionStrings["Oracle.LOCAL"].ToString();
        // public string kirikkaleDb = ConfigurationManager.ConnectionStrings["Oracle.KIRIKKALE"].ToString();
        public static string kirikkaleDb = ConfigurationManager.ConnectionStrings["Oracle.KIRSEHIR"].ToString();
        public string il_kodum = "71";

        // GET: Harita
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult bbox(string tablo, string field, string idkolonu)
        {


            //string connectionString = ConfigurationManager.ConnectionStrings["Oracle.KIRGAZ"].ToString();
            //OracleConnection conn = new OracleConnection(connectionString);
            OracleConnection conn = new OracleConnection(kirikkaleDb);
            conn.Open();

            var sqlbbox =
                "SELECT min(o.x) as minx, max(o.x) as maxx, min(o.y) as miny, max(o.y) as maxy FROM dotsis." + tablo + " t, TABLE(sdo_util.GetVertices(sdo_geom.sdo_mbr(t.geoloc))) o WHERE t." + field + " ='" + idkolonu + "' AND OZEL_DURUM !='DEST' ";

            //var sqlbbox = "select XCOOR as minx,YCOOR as maxx,XCOOR as miny,YCOOR  as maxy from gis_bina WHERE t." + field + " ='" + idkolonu + "'";

            OracleCommand cmd = new OracleCommand()
            {
                Connection = conn,
                CommandText = sqlbbox,
                CommandType = CommandType.Text
            };
            OracleDataReader dr = cmd.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[4] {
                new DataColumn("MINX", typeof(string)),
                new DataColumn("MAXX", typeof(string)),
                new DataColumn("MINY", typeof(string)),
                new DataColumn("MAXY", typeof(string))
            });

            while (dr.Read())
            {
                dt.Rows.Add(dr[0], dr[1], dr[2], dr[3]);
            }

            string jsonString = JsonConvert.SerializeObject(dt);

            return this.Content(jsonString, "application/json");

        }

        public ActionResult bnbox(string tablo, string field, string idkolonu)
        {
            //if (Session["IL_ADI"] == null)
            //    return RedirectToAction("Index");



            string dbBaglan;

            if (il_kodum == "71")
            {
                dbBaglan = ConfigurationManager.ConnectionStrings["kirikkale"].ToString();
            }
            else
            {
                dbBaglan = ConfigurationManager.ConnectionStrings["kirsehir"].ToString();
            }
            //if (Session["IL_ADI"].ToString() == "KIRIKKALE")
            //    dbBaglan = ConfigurationManager.ConnectionStrings["Oracle.KIRIKKALE"].ToString();
            //else
            //    dbBaglan = ConfigurationManager.ConnectionStrings["Oracle.KIRSEHIR"].ToString();

            OracleConnection conn = new OracleConnection(dbBaglan);
            //OracleConnection conn = new OracleConnection(kirikkaleDb);
            conn.Open();

            var sqlbbox = "select XCOOR as minx,XCOOR as maxx,YCOOR as miny,YCOOR  as maxy from dotsis.gis_bina t WHERE t." + field + " ='" + idkolonu + "' AND OZEL_DURUM !='DEST' ";

            OracleCommand cmd = new OracleCommand()
            {
                Connection = conn,
                CommandText = sqlbbox,
                CommandType = CommandType.Text
            };
            OracleDataReader dr = cmd.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[4] {
                new DataColumn("MINX", typeof(string)),
                new DataColumn("MAXX", typeof(string)),
                new DataColumn("MINY", typeof(string)),
                new DataColumn("MAXY", typeof(string))
            });

            while (dr.Read())
            {
                dt.Rows.Add(dr[0], dr[1], dr[2], dr[3]);
            }

            string jsonString = JsonConvert.SerializeObject(dt);

            return this.Content(jsonString, "application/json");

        }


        public ActionResult abnbox(string tablo, string field, string idkolonu)
        {
            //if (Session["IL_ADI"] == null)
            //    return RedirectToAction("Index");



            DbBaglanti db = new DbBaglanti();
            string sql = "SELECT BINA_ID,ABONE_NO from CBS_ABN_IHBAR_V t WHERE t." + field + " ='" + idkolonu + "' AND ABONELIK_DURUMU='NA' ";

            string dbBaglan;

            if (il_kodum == "71")
            {
                dbBaglan = ConfigurationManager.ConnectionStrings["kirikkale"].ToString();
            }
            else
            {
                dbBaglan = ConfigurationManager.ConnectionStrings["kirsehir"].ToString();
            }
            //if (Session["IL_ADI"].ToString() == "KIRIKKALE")
            //    dbBaglan = ConfigurationManager.ConnectionStrings["Oracle.KIRIKKALE"].ToString();
            //else
            //    dbBaglan = ConfigurationManager.ConnectionStrings["Oracle.KIRSEHIR"].ToString();

            OracleConnection bgln = db.OracleConnect(dbBaglan);
            //OracleConnection bgln = db.OracleConnect(kirikkaleDb);
            DataTable dt2 = db.GetDataTable(sql, bgln);

            int ksay = dt2.Rows.Count;

            if (ksay > 0)
            {


                string my_BINA_ID = dt2.Rows[0]["BINA_ID"].ToString();


                OracleConnection conn = new OracleConnection(dbBaglan);
                //OracleConnection conn = new OracleConnection(kirikkaleDb);
                conn.Open();

                //var sqlbbox = "SELECT BINA_ID,ABONE_NO from CBS_ABN_IHBAR_V t WHERE t." + field + " ='" + idkolonu + "'";
                var sqlbbox = "select XCOOR as minx,XCOOR as maxx,YCOOR as miny,YCOOR  as maxy from dotsis.gis_bina t WHERE t.BINA_ID ='" + my_BINA_ID + "'";
                // SELECT BINA_ID,ABONE_NO from CBS_ABN_IHBAR_V WHERE ABONE_NO = '71013962'

                OracleCommand cmd = new OracleCommand()
                {
                    Connection = conn,
                    CommandText = sqlbbox,
                    CommandType = CommandType.Text
                };
                OracleDataReader dr = cmd.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[4] {
                new DataColumn("MINX", typeof(string)),
                new DataColumn("MAXX", typeof(string)),
                new DataColumn("MINY", typeof(string)),
                new DataColumn("MAXY", typeof(string))
            });

                while (dr.Read())
                {
                    dt.Rows.Add(dr[0], dr[1], dr[2], dr[3]);
                }

                string jsonString = JsonConvert.SerializeObject(dt);

                return this.Content(jsonString, "application/json");
            }
            else
            {
                return null;

            }
        }

        public ActionResult cepbox(string tablo, string field, string idkolonu)
        {



            DbBaglanti db = new DbBaglanti();
            string sql = "SELECT BINA_ID,ABONE_NO from CBS_ABN_IHBAR_V t WHERE (t." + field + " ='" + idkolonu + "' or CEP_TEL ='" + idkolonu + "') AND ABONELIK_DURUMU='NA' ";

            string dbBaglan;

            if (il_kodum == "71")
            {
                dbBaglan = ConfigurationManager.ConnectionStrings["kirikkale"].ToString();
            }
            else
            {
                dbBaglan = ConfigurationManager.ConnectionStrings["kirsehir"].ToString();
            }

            //if (Session["IL_ADI"].ToString() == "KIRIKKALE")
            //    dbBaglan = ConfigurationManager.ConnectionStrings["Oracle.KIRIKKALE"].ToString();
            //else
            //    dbBaglan = ConfigurationManager.ConnectionStrings["Oracle.KIRSEHIR"].ToString();

            OracleConnection bgln = db.OracleConnect(dbBaglan);
            //OracleConnection bgln = db.OracleConnect(kirikkaleDb);
            DataTable dt2 = db.GetDataTable(sql, bgln);

            int ksay = dt2.Rows.Count;

            if (ksay > 0)
            {
                string my_BINA_ID = dt2.Rows[0]["BINA_ID"].ToString();

                OracleConnection conn = new OracleConnection(dbBaglan);
                //OracleConnection conn = new OracleConnection(kirikkaleDb);
                conn.Open();

                //var sqlbbox = "SELECT BINA_ID,ABONE_NO from CBS_ABN_IHBAR_V t WHERE t." + field + " ='" + idkolonu + "'";
                var sqlbbox = "select XCOOR as minx,XCOOR as maxx,YCOOR as miny,YCOOR  as maxy from dotsis.gis_bina t WHERE t.BINA_ID ='" + my_BINA_ID + "'";
                // SELECT BINA_ID,ABONE_NO from CBS_ABN_IHBAR_V WHERE ABONE_NO = '71013962'

                OracleCommand cmd = new OracleCommand()
                {
                    Connection = conn,
                    CommandText = sqlbbox,
                    CommandType = CommandType.Text
                };
                OracleDataReader dr = cmd.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[4] {
                new DataColumn("MINX", typeof(string)),
                new DataColumn("MAXX", typeof(string)),
                new DataColumn("MINY", typeof(string)),
                new DataColumn("MAXY", typeof(string))
            });

                while (dr.Read())
                {
                    dt.Rows.Add(dr[0], dr[1], dr[2], dr[3]);
                }

                string jsonString = JsonConvert.SerializeObject(dt);

                return this.Content(jsonString, "application/json");
            }
            else
            {
                return null;

            }
        }

        public ActionResult vanabox(string vana_no) // vana_no= Konum_no
        {

            string dbBaglan;

            if (il_kodum == "71")
            {
                dbBaglan = ConfigurationManager.ConnectionStrings["kirikkale"].ToString();
            }
            else
            {
                dbBaglan = ConfigurationManager.ConnectionStrings["kirsehir"].ToString();
            }
            OracleConnection conn = new OracleConnection(dbBaglan);
            //OracleConnection conn = new OracleConnection(kirikkaleDb);
            conn.Open();

            var sqlbbox = "select XX as minx,XX as maxx,YY as miny,YY  as maxy from dotsis.gis_vanalar_nok t WHERE t.KONUM_NO ='" + vana_no + "'";
            // SELECT BINA_ID,ABONE_NO from CBS_ABN_IHBAR_V WHERE ABONE_NO = '71013962'

            OracleCommand cmd = new OracleCommand()
            {
                Connection = conn,
                CommandText = sqlbbox,
                CommandType = CommandType.Text
            };
            OracleDataReader dr = cmd.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[4] {
                new DataColumn("MINX", typeof(string)),
                new DataColumn("MAXX", typeof(string)),
                new DataColumn("MINY", typeof(string)),
                new DataColumn("MAXY", typeof(string))
            });

            while (dr.Read())
            {
                dt.Rows.Add(dr[0], dr[1], dr[2], dr[3]);
            }

            string jsonString = JsonConvert.SerializeObject(dt);

            return this.Content(jsonString, "application/json");

        }

        public ActionResult abonebbox(string tablo, string field, string idkolonu)
        {

            string connectionString = ConfigurationManager.ConnectionStrings["Oracle.KIRIKKALE"].ToString();
            // string connectionString = ConfigurationManager.ConnectionStrings["Oracle.KIRGAZ"].ToString(); // 240418
            OracleConnection conn = new OracleConnection(connectionString);
            conn.Open();

            var sqlbbox =
                "SELECT min(o.x) as minx, max(o.x) as maxx, min(o.y) as miny, max(o.y) as maxy FROM dotsis." + tablo + " t, TABLE(sdo_util.GetVertices(sdo_geom.sdo_mbr(t.geometry))) o WHERE t." + field + " ='" + idkolonu + "'";

            OracleCommand cmd = new OracleCommand()
            {
                Connection = conn,
                CommandText = sqlbbox,
                CommandType = CommandType.Text
            };
            OracleDataReader dr = cmd.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[4] {
                new DataColumn("MINX", typeof(string)),
                new DataColumn("MAXX", typeof(string)),
                new DataColumn("MINY", typeof(string)),
                new DataColumn("MAXY", typeof(string))
            });

            while (dr.Read())
            {
                dt.Rows.Add(dr[0], dr[1], dr[2], dr[3]);
            }

            string jsonString = JsonConvert.SerializeObject(dt);

            return this.Content(jsonString, "application/json");

        }

        public ActionResult adabox(string adano)
        {
            //if (Session["IL_ADI"] == null)
            //    return RedirectToAction("Index");



            //string sql = "select ADA_NO,ILCE_ADI,ILCE_ADI_CBS,MAHALLE_ADI,X,Y from dotsis.kirikkale_imar_ada WHERE ADA_NO= '" + adano + "' ";

            string dbBaglan, sql;
            if (il_kodum == "71")
            {
                dbBaglan = ConfigurationManager.ConnectionStrings["kirikkale"].ToString();
                sql = "select ADA_NO,ILCE_ADI,ILCE_ADI_CBS,MAHALLE_ADI,X,Y from dotsis.kirikkale_imar_ada WHERE ADA_NO= '" + adano + "' ";

            }
            else
            {
                dbBaglan = ConfigurationManager.ConnectionStrings["kirsehir"].ToString();
                sql = "select ADA_NO,ILCE_ADI,ILCE_ADI_CBS,MAHALLE_ADI,X,Y from dotsis.kirsehir_imar_ada WHERE ADA_NO= '" + adano + "' ";

            }
           



            //if (Session["IL_ADI"].ToString() == "KIRIKKALE")
            //{
            //    sql = "select ADA_NO,ILCE_ADI,ILCE_ADI_CBS,MAHALLE_ADI,X,Y from dotsis.kirikkale_imar_ada WHERE ADA_NO= '" + adano + "' ";
            //    dbBaglan = ConfigurationManager.ConnectionStrings["Oracle.KIRIKKALE"].ToString();
            //}
            //else
            //{
            //    sql = "select ADA_NO,ILCE_ADI,ILCE_ADI_CBS,MAHALLE_ADI,X,Y from dotsis.kirsehir_imar_ada WHERE ADA_NO= '" + adano + "' ";
            //    dbBaglan = ConfigurationManager.ConnectionStrings["Oracle.KIRSEHIR"].ToString();
            //}
            DbBaglanti test = new DbBaglanti();

            OracleConnection bgln = test.OracleConnect(dbBaglan);

            //OracleConnection bgln = test.OracleConnect(kirikkaleDb);
            DataTable dt = test.GetDataTable(sql, bgln);


            string jsonString = JsonConvert.SerializeObject(dt);

            dt.Dispose();

            return this.Content(jsonString, "application/json");

            //OracleConnection conn = new OracleConnection(kirikkaleDb);
            //conn.Open();

            //var sqlbbox = "select MINX,MINY,MAXX,MAXY from dotsis.kirikkale_imar_ada t WHERE ada_no= '" + adano + "' ";

            //OracleCommand cmd = new OracleCommand()
            //{
            //    Connection = conn,
            //    CommandText = sqlbbox,
            //    CommandType = CommandType.Text
            //};
            //OracleDataReader dr = cmd.ExecuteReader();

            //DataTable dt = new DataTable();
            //dt.Columns.AddRange(new DataColumn[4] {
            //    new DataColumn("MINX", typeof(string)),
            //    new DataColumn("MAXX", typeof(string)),
            //    new DataColumn("MINY", typeof(string)),
            //    new DataColumn("MAXY", typeof(string))
            //});

            //while (dr.Read())
            //{
            //    dt.Rows.Add(dr[0], dr[1], dr[2], dr[3]);
            //}

            //string jsonString = JsonConvert.SerializeObject(dt);

            //int KSayi = dt.Rows.Count;

            //return this.Content(jsonString, "application/json");

        }

        public ActionResult yolbox(string yoladi)
        {
            //if (Session["IL_ADI"] == null)
            //    return RedirectToAction("Index");



            DbBaglanti test = new DbBaglanti();
            //string sql = "select ADA_NO,ILCE_ADI,ILCE_ADI_CBS,MAHALLE_ADI,X,Y from dotsis.kirikkale_imar_ada WHERE ADA_NO= '" + adano + "' ";

            string dbBaglan, sql;
            if (il_kodum == "71")
            {
                dbBaglan = ConfigurationManager.ConnectionStrings["kirikkale"].ToString();
                sql = " select t.cadde_sokak_kodu, t.cadde_sokak_adi, t.mahalle_adi,k.ilce_adi,k.xx as xcoor,k.yy as ycoor " +
                                                " from dotsis.cbsw_yol_grup t , dotsis.gis_cadde_sokak_koor k where t.id = k.mi_prinx and k.cadde_sokak_adi like '%" + yoladi + "%' ";
            }
            else
            {
                dbBaglan = ConfigurationManager.ConnectionStrings["kirsehir"].ToString();
                sql = " select t.cadde_sokak_kodu, t.cadde_sokak_adi, t.mahalle_adi,k.ilce_adi,k.xcoor,k.ycoor" +
                                                   " from dotsis.cbsw_yol_grup t , dotsis.gis_cadde_sokak k where t.id = k.mi_prinx and k.cadde_sokak_adi like '%" + yoladi + "%' ";
            }



           

            /* if (il_kodum == "71")
             {
                 //ViewBag.IL_Harita = "KIRIKKALE";
                 //sql = "select t.cadde_sokak_adi,t.mahalle_adi,t.cadde_sokak_kodu,k.xx as xcoor,k.yy as ycoor" +
                 //    " from dotsis.gis_cadde_sokak t ,  dotsis.gis_cadde_sokak_koor k " +
                 //    " where t.cadde_sokak_kodu=k.cadde_sokak_kodu and t.mahalle_kodu=k.mahalle_kodu and t.cadde_sokak_adi like '%" + yoladi + "%' " +
                 //    " group by t.cadde_sokak_adi,t.mahalle_adi,t.cadde_sokak_kodu,k.xx,k.yy order by t.cadde_sokak_adi,t.mahalle_adi ";

                 sql = " select t.cadde_sokak_kodu, t.cadde_sokak_adi, t.mahalle_adi,k.ilce_adi,k.xx as xcoor,k.yy as ycoor " +
                 " from dotsis.cbsw_yol_grup t , dotsis.gis_cadde_sokak_koor k where t.id = k.mi_prinx and k.cadde_sokak_adi like '%" + yoladi + "%' ";

                 dbBaglan = ConfigurationManager.ConnectionStrings["Oracle.KIRIKKALE"].ToString();
             }
             else
             {
                 //ViewBag.IL_Harita = "KIRSEHIR";
                 sql = " select t.cadde_sokak_kodu, t.cadde_sokak_adi, t.mahalle_adi,k.ilce_adi,k.xcoor,k.ycoor" +
                     " from dotsis.cbsw_yol_grup t , dotsis.gis_cadde_sokak k where t.id = k.mi_prinx and k.cadde_sokak_adi like '%" + yoladi + "%' ";

                 //sql = "select cadde_sokak_adi,mahalle_adi,cadde_sokak_kodu,xcoor,ycoor from gis_cadde_sokak t where cadde_sokak_adi like '%" + yoladi + "%' " +
                 //    " group by cadde_sokak_adi,mahalle_adi,cadde_sokak_kodu,xcoor,ycoor  order by cadde_sokak_adi,mahalle_adi ";

                 dbBaglan = ConfigurationManager.ConnectionStrings["Oracle.KIRSEHIR"].ToString();
             }
             */
            OracleConnection bgln = test.OracleConnect(dbBaglan);

            //OracleConnection bgln = test.OracleConnect(kirikkaleDb);
            DataTable dt = test.GetDataTable(sql, bgln);

            string jsonString = JsonConvert.SerializeObject(dt);

            dt.Dispose();

            return this.Content(jsonString, "application/json");
        }

        public ActionResult harita_zoom_gelen(string is_id)
        {
            //if (Session["IL_ADI"] == null)
            //    return RedirectToAction("Index");



            //string url2 = Request.Url.Query;   // ?is_id=1523

            //string my_BINA_ID = "2513";

            string dbBaglan;
            if (il_kodum == "71")
            {
                dbBaglan = ConfigurationManager.ConnectionStrings["kirikkale"].ToString();
               
            }
            else
            {
                dbBaglan = ConfigurationManager.ConnectionStrings["kirsehir"].ToString();
               
            }

           

            //if (Session["IL_ADI"].ToString() == "KIRIKKALE")
            //    dbBaglan = ConfigurationManager.ConnectionStrings["Oracle.KIRIKKALE"].ToString();
            //else
            //    dbBaglan = ConfigurationManager.ConnectionStrings["Oracle.KIRSEHIR"].ToString();

            OracleConnection conn = new OracleConnection(dbBaglan);
            //OracleConnection conn = new OracleConnection(kirikkaleDb);
            conn.Open();

            //var sqlbbox = "SELECT BINA_ID,ABONE_NO from CBS_ABN_IHBAR_V t WHERE t." + field + " ='" + idkolonu + "'";
            var sqlbbox = "select XCOOR as minx,XCOOR as maxx,YCOOR as miny,YCOOR  as maxy from dotsis.gis_bina t WHERE t.BINA_ID ='" + is_id + "'";
            // SELECT BINA_ID,ABONE_NO from CBS_ABN_IHBAR_V WHERE ABONE_NO = '71013962'

            OracleCommand cmd = new OracleCommand()
            {
                Connection = conn,
                CommandText = sqlbbox,
                CommandType = CommandType.Text
            };
            OracleDataReader dr = cmd.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[4] {
                new DataColumn("MINX", typeof(string)),
                new DataColumn("MAXX", typeof(string)),
                new DataColumn("MINY", typeof(string)),
                new DataColumn("MAXY", typeof(string))
            });

            while (dr.Read())
            {
                dt.Rows.Add(dr[0], dr[1], dr[2], dr[3]);
            }

            string jsonString = JsonConvert.SerializeObject(dt);

            return this.Content(jsonString, "application/json");
        }

        public ActionResult muracaat_result(string bn_id)
        {
            //if (Session["IL_ADI"] == null)
            //    return RedirectToAction("Index");



            DbBaglanti test = new DbBaglanti();
            string sql = "select BINA_ID,ABONE_NO,ABONE_ADI,ABONE_SOYADI,TELEFON_NO,DAIRE_NO,SAYAC_NO,ABONELIK_DURUMU_A from CBS_ABN_IHBAR_V WHERE BINA_ID = " + bn_id + " ORDER BY ABONELIK_DURUMU_A DESC";
            // select BINA_ID,ABONE_NO,ABONE_ADI,ABONE_SOYADI,TELEFON_NO,DAIRE_NO,SAYAC_NO from CBS_ABN_IHBAR_V WHERE BINA_ID 

            string dbBaglan;
            if (il_kodum == "71")
            {
                dbBaglan = ConfigurationManager.ConnectionStrings["kirikkale"].ToString();

            }
            else
            {
                dbBaglan = ConfigurationManager.ConnectionStrings["kirsehir"].ToString();

            }



            //if (Session["IL_ADI"].ToString() == "KIRIKKALE")
            //    dbBaglan = ConfigurationManager.ConnectionStrings["Oracle.KIRIKKALE"].ToString();
            //else
            //    dbBaglan = ConfigurationManager.ConnectionStrings["Oracle.KIRSEHIR"].ToString();

            OracleConnection bgln = test.OracleConnect(dbBaglan);
            //OracleConnection bgln = test.OracleConnect(kirikkaleDb);
            DataTable dt = test.GetDataTable(sql, bgln);


            string jsonString = JsonConvert.SerializeObject(dt);

            dt.Dispose();

            return this.Content(jsonString, "application/json");
        }

        public ActionResult muracaat_result_tum(string bn_id, string il_kod)
        {
            //if (Session["IL_ADI"] == null)
            //    return RedirectToAction("Index");



            string dbBaglan;
            if (il_kodum == "71")
            {
                dbBaglan = ConfigurationManager.ConnectionStrings["kirikkale"].ToString();

            }
            else
            {
                dbBaglan = ConfigurationManager.ConnectionStrings["kirsehir"].ToString();

            }


            //if (Session["IL_ADI"].ToString() == "KIRIKKALE")
            //    dbBaglan = ConfigurationManager.ConnectionStrings["Oracle.KIRIKKALE"].ToString();
            //else
            //    dbBaglan = ConfigurationManager.ConnectionStrings["Oracle.KIRSEHIR"].ToString();

            DbBaglanti test = new DbBaglanti();
            string sql = "select BINA_ID,ABONE_NO,ABONE_ADI,ABONE_SOYADI,TELEFON_NO,DAIRE_NO,SAYAC_NO,ABONELIK_DURUMU_A from CBS_ABN_IHBAR_V WHERE BINA_ID = " + bn_id + " ORDER BY ABONELIK_DURUMU_A DESC";

            OracleConnection bgln = test.OracleConnect(dbBaglan);
            //OracleConnection bgln = test.OracleConnect(kirikkaleDb7);
            DataTable dt = test.GetDataTable(sql, bgln);


            string jsonString = JsonConvert.SerializeObject(dt);

            dt.Dispose();

            return this.Content(jsonString, "application/json");
        }

        public ActionResult muracaat_result_gercek(string bn_id)
        {


            DbBaglanti test = new DbBaglanti();
            string sql = "select BINA_ID,ABONE_NO,YER_SAHIBI,KULLANIM_ALANI,DABH_NO,TELEFON_NO,DAIRE_NO,SAYAC_NO from dotsis.gis_abo_muracaat_adr WHERE BINA_ID = " + bn_id + " ";  // ORDER BY ABONELIK_DURUMU_A DESC
                                                                                                                                                                                    // select DABH_NO,ABONE_NO,KULLANIM_ALANI,ABONE_YER_DURUMU,YER_SAHIBI,AKTIF_ABONE_ID,DAIRE_NO,SAYAC_TURU,SAYAC_NO,BINA_ID,ABONE_SAHIS_NO,ABONE_ADI,ABONE_SOYADI,TELEFON_NO,KIMLIK_NO,BABA_ADI,VATANDASLIK_NO,CEP_TEL,IL_ADI,ILCE_ADI,MAHALLE_ADI,CADDE_SOKAK_ADI,BINA_NO  from gis_abo_muracaat_adr

            string dbBaglan;

            if (il_kodum == "71")
            {
                dbBaglan = ConfigurationManager.ConnectionStrings["kirikkale"].ToString();

            }
            else
            {
                dbBaglan = ConfigurationManager.ConnectionStrings["kirsehir"].ToString();

            }
            //if (Session["IL_ADI"].ToString() == "KIRIKKALE")
            //    dbBaglan = ConfigurationManager.ConnectionStrings["Oracle.KIRIKKALE"].ToString();
            //else
            //    dbBaglan = ConfigurationManager.ConnectionStrings["Oracle.KIRSEHIR"].ToString();

            OracleConnection bgln = test.OracleConnect(dbBaglan);
            //OracleConnection bgln = test.OracleConnect(kirikkaleDb);
            DataTable dt = test.GetDataTable(sql, bgln);


            string jsonString = JsonConvert.SerializeObject(dt);

            dt.Dispose();

            return this.Content(jsonString, "application/json");
        }

        public ActionResult dropdown_result(string tablo, string alan, string where, string query)
        {
            //dropdown_result?alan=BINA_ADI&tablo=GIS_BINA_4326&where=BINA_NO&query=34
            //if (Session["IL_ADI"] == null)
            //    return RedirectToAction("Index");



            DbBaglanti test = new DbBaglanti();
            string sql = "SELECT " + alan + " FROM " + tablo;
            //string sql = "SELECT " + alan + " FROM " + tablo;
            //string sql = "SELECT DISTINCT(" + alan + ") FROM " + tablo + " WHERE " + where + " LIKE '%" + query + "%'";

            string dbBaglan = "";


            string ilkodu = "71";
            if (il_kodum == "71")
            {
                dbBaglan = ConfigurationManager.ConnectionStrings["kirikkale"].ToString();

            }
            else
            {
                dbBaglan = ConfigurationManager.ConnectionStrings["kirsehir"].ToString();

            }



            //if (Session["IL_ADI"].ToString() == "KIRIKKALE")
            //    dbBaglan = ConfigurationManager.ConnectionStrings["Oracle.KIRIKKALE"].ToString();
            //else
            //    dbBaglan = ConfigurationManager.ConnectionStrings["Oracle.KIRSEHIR"].ToString();

            OracleConnection bgln = test.OracleConnect(dbBaglan);
            //OracleConnection bgln = test.OracleConnect(kirikkaleDb);
            DataTable dt = test.GetDataTable(sql, bgln);


            string jsonString = JsonConvert.SerializeObject(dt);

            dt.Dispose();

            return this.Content(jsonString, "application/json");

        }

        public ActionResult dropdown_result_tkgm(string tablo, string alan, string where, string query)
        {
            //if (Session["IL_ADI"] == null)
            //    return RedirectToAction("Index");



            DbBaglanti test = new DbBaglanti();
            string sql = "SELECT " + alan + " FROM " + tablo + " GROUP BY ILCE_ADI,ILCE_KODU";

            string dbBaglan;

            if (il_kodum == "71")
            {
                dbBaglan = ConfigurationManager.ConnectionStrings["kirikkale"].ToString();

            }
            else
            {
                dbBaglan = ConfigurationManager.ConnectionStrings["kirsehir"].ToString();

            }
            //if (Session["IL_ADI"].ToString() == "KIRIKKALE")
            //    dbBaglan = ConfigurationManager.ConnectionStrings["Oracle.KIRIKKALE"].ToString();
            //else
            //    dbBaglan = ConfigurationManager.ConnectionStrings["Oracle.KIRSEHIR"].ToString();

            OracleConnection bgln = test.OracleConnect(dbBaglan);
            //OracleConnection bgln = test.OracleConnect(kirikkaleDb);
            DataTable dt = test.GetDataTable(sql, bgln);

            string jsonString = JsonConvert.SerializeObject(dt);

            dt.Dispose();

            return this.Content(jsonString, "application/json");

        }

        public ActionResult dropdown_result_ilce(string tablo, string alan, string where, string query)
        {
            //if (Session["IL_ADI"] == null)
            //    return RedirectToAction("Index");



            DbBaglanti test = new DbBaglanti();
            string sql = "SELECT " + alan + " FROM dotsis." + tablo + " WHERE " + where + " = '" + query + "'";

            string dbBaglan;

            if (il_kodum == "71")
            {
                dbBaglan = ConfigurationManager.ConnectionStrings["kirikkale"].ToString();

            }
            else
            {
                dbBaglan = ConfigurationManager.ConnectionStrings["kirsehir"].ToString();

            }

            //if (Session["IL_ADI"].ToString() == "KIRIKKALE")
            //    dbBaglan = ConfigurationManager.ConnectionStrings["Oracle.KIRIKKALE"].ToString();
            //else
            //    dbBaglan = ConfigurationManager.ConnectionStrings["Oracle.KIRSEHIR"].ToString();

            OracleConnection bgln = test.OracleConnect(dbBaglan);
            //OracleConnection bgln = test.OracleConnect(kirikkaleDb);
            DataTable dt = test.GetDataTable(sql, bgln);

            string jsonString = JsonConvert.SerializeObject(dt);
            dt.Dispose();
            return this.Content(jsonString, "application/json");
        }

        public ActionResult dropdown_result_ilce_tkgm(string tablo, string alan, string where, string query)
        {
            //if (Session["IL_ADI"] == null)
            //    return RedirectToAction("Index");



            DbBaglanti test = new DbBaglanti();
            string sql = "SELECT " + alan + " FROM " + tablo + " WHERE " + where + " = '" + query + "'";

            string dbBaglan;

            if (il_kodum == "71")
            {
                dbBaglan = ConfigurationManager.ConnectionStrings["kirikkale"].ToString();

            }
            else
            {
                dbBaglan = ConfigurationManager.ConnectionStrings["kirsehir"].ToString();

            }

            //if (Session["IL_ADI"].ToString() == "KIRIKKALE")
            //    dbBaglan = ConfigurationManager.ConnectionStrings["Oracle.KIRIKKALE"].ToString();
            //else
            //    dbBaglan = ConfigurationManager.ConnectionStrings["Oracle.KIRSEHIR"].ToString();

            OracleConnection bgln = test.OracleConnect(dbBaglan);

            //OracleConnection bgln = test.OracleConnect(kirikkaleDb);
            DataTable dt = test.GetDataTable(sql, bgln);

            string jsonString = JsonConvert.SerializeObject(dt);
            dt.Dispose();
            return this.Content(jsonString, "application/json");
        }

        public ActionResult dropdown_result_mah(string tablo, string alan, string where, string query1, string query2)
        {
            //if (Session["IL_ADI"] == null)
            //    return RedirectToAction("Index");



            DbBaglanti test = new DbBaglanti();
            string sql = "SELECT " + alan + " FROM dotsis." + tablo + " WHERE ILCE_KODU = '" + query1 + "' AND MAHALLE_KODU = '" + query2 + "' GROUP BY  CADDE_SOKAK_ADI,CADDE_SOKAK_KODU ORDER BY CADDE_SOKAK_ADI";

            string dbBaglan;

            if (il_kodum == "71")
            {
                dbBaglan = ConfigurationManager.ConnectionStrings["kirikkale"].ToString();

            }
            else
            {
                dbBaglan = ConfigurationManager.ConnectionStrings["kirsehir"].ToString();

            }

            //if (Session["IL_ADI"].ToString() == "KIRIKKALE")
            //    dbBaglan = ConfigurationManager.ConnectionStrings["Oracle.KIRIKKALE"].ToString();
            //else
            //    dbBaglan = ConfigurationManager.ConnectionStrings["Oracle.KIRSEHIR"].ToString();

            OracleConnection bgln = test.OracleConnect(dbBaglan);
            //OracleConnection bgln = test.OracleConnect(kirikkaleDb);
            DataTable dt = test.GetDataTable(sql, bgln);

            string jsonString = JsonConvert.SerializeObject(dt);
            dt.Dispose();
            return this.Content(jsonString, "application/json");
        }

        public ActionResult dropdown_result_yol(string tablo, string alan, string where, string query1, string query2, string query3)
        {
            //if (Session["IL_ADI"] == null)
            //    return RedirectToAction("Index");


            DbBaglanti test = new DbBaglanti();
            // string sql = "SELECT " + alan + " FROM dotsis." + tablo + " WHERE ILCE_KODU = '" + query1 + "' AND CADDE_SOKAK_KODU = '" + query3 + "' GROUP BY  BINA_NO,BINA_ID ORDER BY BINA_NO";
            string sql = "SELECT " + alan + " FROM dotsis." + tablo + " WHERE ILCE_KODU = '" + query1 + "' AND MAHALLE_KODU = '" + query2 + "' AND CADDE_SOKAK_KODU = '" + query3 + "' GROUP BY  BINA_NO,BINA_ID ORDER BY BINA_NO";

            string dbBaglan;

            if (il_kodum == "71")
            {
                dbBaglan = ConfigurationManager.ConnectionStrings["kirikkale"].ToString();

            }
            else
            {
                dbBaglan = ConfigurationManager.ConnectionStrings["kirsehir"].ToString();

            }
            //if (Session["IL_ADI"].ToString() == "KIRIKKALE")
            //    dbBaglan = ConfigurationManager.ConnectionStrings["Oracle.KIRIKKALE"].ToString();
            //else
            //    dbBaglan = ConfigurationManager.ConnectionStrings["Oracle.KIRSEHIR"].ToString();

            OracleConnection bgln = test.OracleConnect(dbBaglan);
            //OracleConnection bgln = test.OracleConnect(kirikkaleDb);
            DataTable dt = test.GetDataTable(sql, bgln);

            string jsonString = JsonConvert.SerializeObject(dt);
            dt.Dispose();
            return this.Content(jsonString, "application/json");
        }

        public ActionResult zoom_result_mah(string tablo, string alan, string where, string ilce_kod, string mah_kod)
        {
            //if (Session["IL_ADI"] == null)
            //    return RedirectToAction("Index");



            DbBaglanti test = new DbBaglanti();
            string sql = "SELECT " + alan + " FROM dotsis." + tablo + " WHERE ILCE_KODU = '" + ilce_kod + "' AND MAHALLE_KODU = '" + mah_kod + "'";

            string dbBaglan;


            if (il_kodum == "71")
            {
                dbBaglan = ConfigurationManager.ConnectionStrings["kirikkale"].ToString();

            }
            else
            {
                dbBaglan = ConfigurationManager.ConnectionStrings["kirsehir"].ToString();

            }

            //if (Session["IL_ADI"].ToString() == "KIRIKKALE")
            //    dbBaglan = ConfigurationManager.ConnectionStrings["Oracle.KIRIKKALE"].ToString();
            //else
            //    dbBaglan = ConfigurationManager.ConnectionStrings["Oracle.KIRSEHIR"].ToString();

            OracleConnection bgln = test.OracleConnect(dbBaglan);
            //OracleConnection bgln = test.OracleConnect(kirikkaleDb);
            DataTable dt = test.GetDataTable(sql, bgln);

            string jsonString = JsonConvert.SerializeObject(dt);
            dt.Dispose();
            return this.Content(jsonString, "application/json");
        }

        public ActionResult zoom_result_cd(string tablo, string alan, string where, string ilce_kod, string mah_kod, string cd_kod)
        {
            //if (Session["IL_ADI"] == null)
            //    return RedirectToAction("Index");



            DbBaglanti test = new DbBaglanti();
            string sql = "SELECT " + alan + " FROM dotsis." + tablo + " WHERE ILCE_KODU = '" + ilce_kod + "' AND CADDE_SOKAK_KODU = '" + cd_kod + "' and rownum=1 ";
            //string sql = "SELECT " + alan + " FROM dotsis." + tablo + " WHERE ILCE_KODU = '" + ilce_kod + "' AND MAHALLE_KODU = '" + mah_kod + "' AND CADDE_SOKAK_KODU = '" + cd_kod + "' ";

            string dbBaglan;

            if (il_kodum == "71")
            {
                dbBaglan = ConfigurationManager.ConnectionStrings["kirikkale"].ToString();

            }
            else
            {
                dbBaglan = ConfigurationManager.ConnectionStrings["kirsehir"].ToString();

            }
            //if (Session["IL_ADI"].ToString() == "KIRIKKALE")
            //    dbBaglan = ConfigurationManager.ConnectionStrings["Oracle.KIRIKKALE"].ToString();
            //else
            //    dbBaglan = ConfigurationManager.ConnectionStrings["Oracle.KIRSEHIR"].ToString();

            OracleConnection bgln = test.OracleConnect(dbBaglan);
            //OracleConnection bgln = test.OracleConnect(kirikkaleDb);
            DataTable dt = test.GetDataTable(sql, bgln);

            string jsonString = JsonConvert.SerializeObject(dt);
            dt.Dispose();
            return this.Content(jsonString, "application/json");
        }

    }
}