using crypt;
using Kirgaz_Online_Islemleri_Portal_Son_kopya.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kirgaz_Online_Islemleri_Portal_Son_kopya.Controllers
{
    public class PeriyodikMuayeneController : Controller
    {

        
            [HttpGet]
        public ActionResult getAracPlakaLsistesi()
        {
            string connectionString = "", sql;
            string ilkodu =  "71";
           
            //sql = "SELECT DISTINCT (ILCE_ADI) FROM GIS_ILCE WHERE BOLGE_NO ='" + bolgeKodu + "' and IL_KODU ='" + ilkodu + "' ";
            sql = "SELECT DISTINCT (PLAKA) FROM KIRGAZ.ABO_PER_MUA_IS_EMRI WHERE PLAKA IS NOT NULL ";

            if (ilkodu == "71")
            {
                connectionString = ConfigurationManager.ConnectionStrings["kirikkale"].ToString();
            }
            else
            {
                connectionString = ConfigurationManager.ConnectionStrings["kirsehir"].ToString();
            }

            DbBaglanti test = new DbBaglanti();

            OracleConnection bgln = test.OracleConnect(connectionString);

            //OracleConnection bgln = test.OracleConnect(kirikkaleDb);
            DataTable dt = test.GetDataTable(sql, bgln);


            string jsonString = JsonConvert.SerializeObject(dt);
            System.Diagnostics.Debug.Print("######################################  " + jsonString);

            dt.Dispose();

            return this.Content(jsonString, "application/json");
        }
        [HttpGet]
        public ActionResult getBolgeList()
        {
            string connectionString = "", sql;
            string ilkodu = "71";
            // sql = "SELECT DISTINCT (x.BOLGE_KODU), x.BOLGE_ADI FROM ABO_BOLGE x ";
            sql = "select DISTINCT (b.BOLGE_KODU), b.BOLGE_ADI from ABO_BOLGE b, gis_ilce g where g.ilce_kodu = b.ilce";



            if (ilkodu == "71")
            {
                connectionString = ConfigurationManager.ConnectionStrings["kirikkale"].ToString();
            }
            else
            {
                connectionString = ConfigurationManager.ConnectionStrings["kirsehir"].ToString();
            }
           

            DbBaglanti test = new DbBaglanti();

            OracleConnection bgln = test.OracleConnect(connectionString);

            //OracleConnection bgln = test.OracleConnect(kirikkaleDb);
            DataTable dt = test.GetDataTable(sql, bgln);


            string jsonString = JsonConvert.SerializeObject(dt);
            System.Diagnostics.Debug.Print("################################  " + jsonString);

            dt.Dispose();

            return this.Content(jsonString, "application/json");





        }

        [HttpPost]
        public ActionResult getIlceListesi(string bolgeAdi, string ilAdi)
        {
            string connectionString = "", sql;
            string ilkodu = "", bolgeKodu = "";
            System.Diagnostics.Debug.Print("######################################  " + "   " + bolgeAdi + "   " + ilAdi);
            ilkodu = "71";
            if (!string.IsNullOrWhiteSpace(ilAdi))
            {
                if (ilAdi.Equals("Kırıkkale"))
                {
                    ilkodu = "71";
                }
            }
            if (!string.IsNullOrWhiteSpace(bolgeAdi))
            {
                bolgeKodu = bolgeAdi.Split('-')[0];
            }
            //sql = "SELECT DISTINCT (ILCE_ADI) FROM GIS_ILCE WHERE BOLGE_NO ='" + bolgeKodu + "' and IL_KODU ='" + ilkodu + "' ";
            sql = "SELECT DISTINCT (ILCE_ADI) FROM GIS_ILCE WHERE ILCE_KODU in (select DISTINCT (ILCE) from ABO_BOLGE where BOLGE_KODU ='" + bolgeKodu + "') and IL_KODU ='" + ilkodu + "' ";

            if (ilkodu == "71")
            {
                connectionString = ConfigurationManager.ConnectionStrings["kirikkale"].ToString();
            }
            else
            {
                connectionString = ConfigurationManager.ConnectionStrings["kirsehir"].ToString();
            }

            DbBaglanti test = new DbBaglanti();

            OracleConnection bgln = test.OracleConnect(connectionString);

            //OracleConnection bgln = test.OracleConnect(kirikkaleDb);
            DataTable dt = test.GetDataTable(sql, bgln);


            string jsonString = JsonConvert.SerializeObject(dt);
            System.Diagnostics.Debug.Print("######################################  " + jsonString);

            dt.Dispose();

            return this.Content(jsonString, "application/json");
        }

        //

        [HttpPost]
        public ActionResult getMahalleListesi(string bolgeAdi, string ilAdi, string ilceAdi)
        {
            string connectionString = "", sql;
            string ilkodu = "", bolgeKodu = "";
            System.Diagnostics.Debug.Print("######################################  " + "   " + bolgeAdi + "   " + ilceAdi + "   " + ilAdi);
            ilkodu = "71";
            if (!string.IsNullOrWhiteSpace(ilAdi))
            {
                if (ilAdi.Equals("Kırıkkale"))
                {
                    ilkodu = "71";
                }
            }
            if (!string.IsNullOrWhiteSpace(bolgeAdi))
            {
                bolgeKodu = bolgeAdi.Split('-')[0];
            }
            sql = "SELECT DISTINCT (MAHALLE_ADI) FROM GIS_MAHALLE WHERE ILCE_KODU in (select DISTINCT (ILCE) from ABO_BOLGE where BOLGE_KODU ='" + bolgeKodu + "') and IL_KODU ='" + ilkodu + "' and ILCE_ADI= '"+ ilceAdi+ "' order by MAHALLE_ADI ASC";
            if (ilkodu == "71")
            {
                connectionString = ConfigurationManager.ConnectionStrings["kirikkale"].ToString();
            }
            else
            {
                connectionString = ConfigurationManager.ConnectionStrings["kirsehir"].ToString();
            }


            DbBaglanti test = new DbBaglanti();

            OracleConnection bgln = test.OracleConnect(connectionString);

            //OracleConnection bgln = test.OracleConnect(kirikkaleDb);
            DataTable dt = test.GetDataTable(sql, bgln);


            string jsonString = JsonConvert.SerializeObject(dt);
            System.Diagnostics.Debug.Print("######################################  " + jsonString);

            dt.Dispose();

            return this.Content(jsonString, "application/json");
        }


        //SELECT DISTINCT (CADDE_SOKAK_ADI) FROM KIRGAZ.ABO_GIS_BINA x WHERE BOLGE_NO ='01' and ILCE_ADI ='MERKEZ' AND MAHALLE_ADI ='YENISEHIR'
        [HttpPost]
        public ActionResult getSokakListesi(string bolgeAdi, string ilAdi, string ilceAdi, string mahalleAdi)
        {
            string connectionString = "", sql;
            string ilkodu = "", bolgeKodu = "";
            System.Diagnostics.Debug.Print("######################################  " + "   " + bolgeAdi + "   " + ilceAdi + "   " + ilAdi + "   " + mahalleAdi);
            ilkodu = "71";
            if (!string.IsNullOrWhiteSpace(ilAdi))
            {
                if (ilAdi.Equals("Kırıkkale"))
                {
                    ilkodu = "71";
                }
            }
            if (!string.IsNullOrWhiteSpace(bolgeAdi))
            {
                bolgeKodu = bolgeAdi.Split('-')[0];
            }
            sql = "SELECT DISTINCT (CADDE_SOKAK_ADI) FROM GIS_CADDE_SOKAK x WHERE IL_KODU ='" + ilkodu + "' and ILCE_ADI= '" + ilceAdi + "' AND MAHALLE_ADI ='" + mahalleAdi + "' ";
            System.Diagnostics.Debug.Print("######################################  " + "   " + sql);

            if (ilkodu == "71")
            {
                connectionString = ConfigurationManager.ConnectionStrings["kirikkale"].ToString();
            }
            else
            {
                connectionString = ConfigurationManager.ConnectionStrings["kirsehir"].ToString();
            }

            DbBaglanti test = new DbBaglanti();

            OracleConnection bgln = test.OracleConnect(connectionString);

            //OracleConnection bgln = test.OracleConnect(kirikkaleDb);
            DataTable dt = test.GetDataTable(sql, bgln);


            string jsonString = JsonConvert.SerializeObject(dt);
            System.Diagnostics.Debug.Print("######################################  " + jsonString);

            dt.Dispose();

            return this.Content(jsonString, "application/json");
        }


        [HttpPost]
        public ActionResult getPeriyodikIsEmirListesi(string bolgeAdi, string ilAdi, string ilceAdi, string mahalleAdi, string zamanAraligi, string sokakAdiSelector, string binaID, string IsEmriDurumSelector, string AracPlakaSelector, string IsEmriYıl)
        {
            AESCrypt aes = new AESCrypt();
            string connectionString = "", sql;
            string ilkodu = "", bolgeKodu = "";
            System.Diagnostics.Debug.Print("######################################  " + "   " + bolgeAdi + "   " + ilceAdi + "   " + ilAdi + "   " + mahalleAdi);
            ilkodu = "71";
            if (!string.IsNullOrWhiteSpace(ilAdi))
            {
                if (ilAdi.Equals("Kırıkkale"))
                {
                    ilkodu = "71";
                }
            }
            if (!string.IsNullOrWhiteSpace(bolgeAdi))
            {
                bolgeKodu = bolgeAdi.Split('-')[0];
            }
            if (!string.IsNullOrWhiteSpace(bolgeAdi)&& !string.IsNullOrWhiteSpace(ilAdi) && !string.IsNullOrWhiteSpace(ilceAdi) 
                && !string.IsNullOrWhiteSpace(mahalleAdi)) {
                sql = "SELECT ABONE_ID, ABONE_ADI, ABONE_SOYADI, SAYAC_NO, SAYAC_TURU, TAKILAN_SAYAC_NO, ISE_BASLAMA_TARIHI, ISI_BITIRME_TARIHI, EKIP_KODU, EKIP_PERSONEL_1, PLAKA, BILDIRIM_NO, ILCE, MAHALLE, BINA_ID, DURUMU FROM ABO_PER_MUA_IS_EMRI x WHERE BOLGE ='" + bolgeKodu + "' AND MAHALLE = (select DISTINCT(b.MAHALLE_KODU) from GIS_MAHALLE b where MAHALLE_ADI ='" + mahalleAdi + "' and ILCE_KODU = (select DISTINCT(b.ILCE_KODU) from GIS_ILCE b where b.ILCE_ADI  ='" + ilceAdi + "')) AND ILCE = (select DISTINCT(b.ILCE_KODU) from GIS_ILCE b where b.ILCE_ADI  ='" + ilceAdi + "')";
            }
            else
            {
                sql = "SELECT ABONE_ID, ABONE_ADI, ABONE_SOYADI, SAYAC_NO, SAYAC_TURU, TAKILAN_SAYAC_NO, ISE_BASLAMA_TARIHI, ISI_BITIRME_TARIHI, EKIP_KODU, EKIP_PERSONEL_1, PLAKA, BILDIRIM_NO, ILCE, MAHALLE, BINA_ID, DURUMU FROM ABO_PER_MUA_IS_EMRI";
            }
            System.Diagnostics.Debug.Print("######################################  " + "   " + sql);

            //sql = "SELECT * FROM ABO_PER_MUA_IS_EMRI WHERE BOLGE_NO ='" + bolgeKodu + "' and IL_KODU ='" + ilkodu + "' and ILCE_ADI ='" + ilceAdi + "' AND MAHALLE_ADI ='" + mahalleAdi + "'";
            if (ilkodu == "71")
            {
                connectionString = ConfigurationManager.ConnectionStrings["kirikkale"].ToString();
            }
            else
            {
                connectionString = ConfigurationManager.ConnectionStrings["kirsehir"].ToString();
            }


            DbBaglanti test = new DbBaglanti();

            OracleConnection bgln = test.OracleConnect(connectionString);

            //OracleConnection bgln = test.OracleConnect(kirikkaleDb);
            DataTable dt = test.GetDataTable(sql, bgln);
            /*foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    row.SetField(column, new value);
                }
            }
            */

            for (int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
            {
                
                    dt.Rows[rowIndex][1] = aes.decrypt(dt.Rows[rowIndex][1].ToString());
                    dt.Rows[rowIndex][2] = aes.decrypt(dt.Rows[rowIndex][2].ToString());

            }
            string jsonString = JsonConvert.SerializeObject(dt);
            //System.Diagnostics.Debug.Print("######################################  " + jsonString);

            dt.Dispose();

            return this.Content(jsonString, "application/json");
        }

        [HttpGet]
        public ActionResult getFotografBilgisi(string aboneNo)
        {
            string connectionString = "", sql;
            string ilkodu = "";
            System.Diagnostics.Debug.Print("######################################  " + "   " + aboneNo);
            ilkodu = "71";

            sql = "SELECT FOTOGRAF FROM ABO_PER_MUA_IS_EMRI x WHERE ABONE_ID ='" + aboneNo + "'";
            List<ImageModel> images = new List<ImageModel>();
            if (ilkodu == "71")
            {
                connectionString = ConfigurationManager.ConnectionStrings["kirikkale"].ToString();
            }
            else
            {
                connectionString = ConfigurationManager.ConnectionStrings["kirsehir"].ToString();
            }



            DbBaglanti test = new DbBaglanti();

            OracleConnection bgln = test.OracleConnect(connectionString);

            //OracleConnection bgln = test.OracleConnect(kirikkaleDb);
            DataTable dataTable = test.GetDataTable(sql, bgln);
           
            foreach (DataRow dataRow in dataTable.Rows)

            {



                
                byte[] blob = (byte[])dataRow[0];
                





                string imageBase64Data = Convert.ToBase64String(blob);
                string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
                ViewBag.ImageData = imageDataURL;


            }

            return View();


        }




    }
}