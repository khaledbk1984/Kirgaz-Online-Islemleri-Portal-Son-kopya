using crypt;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Kirgaz_Online_Islemleri_Portal_Son_kopya.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult smsGonder()

        {
            if ( Request.Form != null && Request.Form.Count > 0)
            {
                string telefonNO = Request.Form["txtCepTEL"];
                string smsMetni = Request.Form["txtSMSMetni"];

                if (String.IsNullOrEmpty(telefonNO))
                {
                    //return Content("HATA", "application/text");
                }

                if (telefonNO.Length == 10)
                {
                    if (telefonNO.Substring(0, 1).Equals("5"))
                    {
                        telefonNO = "90" + telefonNO;
                    }
                }
                else if (telefonNO.Length == 11)
                {
                    if (telefonNO.Substring(0, 2).Equals("05"))
                    {
                        telefonNO = "9" + telefonNO;
                    }
                }
                else
                {
                    //return Content("HATA", "application/text");
                }

                string vbCrLf = "\r\n";

                string StrXml = "";
                string strType = "1";
               /* string user = "3g000776";
                string pass = "147258fe..FE";
                string origin = "KIRGAZ";*/


                // ###############################################
                // * Kırsehir Bilgileri
                string user = "3gk0006";
                string pass = "kirsehir06";
                string origin = "KIRGAZ";

                StrXml = "";
                StrXml = StrXml + ("<?xml version='1.0' encoding='UTF-8'?>") + vbCrLf;
                StrXml = StrXml + ("<MainmsgBody>") + vbCrLf;
                StrXml = StrXml + ("<UserName>" + user + "</UserName>") + vbCrLf;
                StrXml = StrXml + ("<PassWord>" + pass + "</PassWord>") + vbCrLf;
                StrXml = StrXml + ("<CompanyCode></CompanyCode>") + vbCrLf;
                StrXml = StrXml + ("<Type>" + strType + "</Type>") + vbCrLf;
                StrXml = StrXml + ("<Version></Version>") + vbCrLf;
                StrXml = StrXml + ("<Developer></Developer>") + vbCrLf;
                StrXml = StrXml + ("<Originator>" + origin + "</Originator>") + vbCrLf;
                StrXml = StrXml + ("<Numbers>" + telefonNO + "</Numbers>") + vbCrLf;
                StrXml = StrXml + ("<Mesgbody><![CDATA[" + smsMetni + "]]></Mesgbody>") + vbCrLf;
                StrXml = StrXml + ("<SDate></SDate>") + vbCrLf;
                StrXml = StrXml + ("<EDate></EDate>") + vbCrLf;
                StrXml = StrXml + ("</MainmsgBody>") + vbCrLf;

                HttpGonder("https://gateway.3gbilisim.com/SendSmsMany.aspx", StrXml);
            }
            return View();

            // return Content(StrRes, "application/text");
        }

        public void HttpGonder(string u, string strxml)
        {
            try
            {
                WebClient SmsNextWebClient = new WebClient();
                byte[] data = Encoding.UTF8.GetBytes(strxml);
                byte[] response = SmsNextWebClient.UploadData(u, "POST", data);
                string Gelen = Encoding.UTF8.GetString(response).ToString();
                System.Diagnostics.Debug.Print("#############################   " + Gelen);
                // return Gelen;
            }

            catch (Exception e)
            {
                //return "HATA"; //+ ex.Message.ToString();
            }
        }

        public ActionResult Harita()
        {

            ViewBag.IL_Harita = "KIRIKKALE";
            return View();

        }

        public ActionResult UsulsuzGazList()
        {

            string connectionString = "";
            string ilkodu = "71";
            if (ilkodu == "71" || ilkodu == "710")
            {
                ViewBag.IL_Harita = "KIRIKKALE";
                connectionString = ConfigurationManager.ConnectionStrings["kirikkale"].ToString();
            }
            else
            {
                connectionString = ConfigurationManager.ConnectionStrings["kirsehir"].ToString();
            }
            string txtIsEmriTipi = "";
            if ( Request.Form != null && Request.Form.Count > 0)
            {
                txtIsEmriTipi = Request.Form["cbIsEmriUGKTTList"];
            }


            DbBaglanti test = new DbBaglanti();
            string sql = "select ABONE_ID,ABONE_ADI|| ' ' || ABONE_SOYADI as ABONE_AD_SOYAD,SAYAC_NO,TO_CHAR(TO_DATE(KAPATMA_TARIHI, 'DD.MM.YYYY')) as KAPATMA_TARIHI,CREATED_DATE, ILCE_ADI, MAHALLE_ADI, BINA_ID, DURUMU,USULSUZ_KUL_ID from CBSW_USULSUZ_KULLANIM_TBL order by created_date desc";

            if (txtIsEmriTipi == null)
            {
                sql = "select ABONE_ID,ABONE_ADI|| ' ' || ABONE_SOYADI as ABONE_AD_SOYAD,SAYAC_NO,TO_CHAR(TO_DATE(KAPATMA_TARIHI, 'DD.MM.YYYY')) as KAPATMA_TARIHI,CREATED_DATE, ILCE_ADI, MAHALLE_ADI, BINA_ID, DURUMU,USULSUZ_KUL_ID from CBSW_USULSUZ_KULLANIM_TBL  order by created_date desc";
            }
            else if (txtIsEmriTipi == "1")
            {
                sql = "select ABONE_ID,ABONE_ADI|| ' ' || ABONE_SOYADI as ABONE_AD_SOYAD,SAYAC_NO,TO_CHAR(TO_DATE(KAPATMA_TARIHI, 'DD.MM.YYYY')) as KAPATMA_TARIHI,CREATED_DATE, ILCE_ADI, MAHALLE_ADI, BINA_ID, DURUMU,USULSUZ_KUL_ID from CBSW_USULSUZ_KULLANIM_TBL WHERE DURUMU='1' order by created_date desc";
            }
            else if (txtIsEmriTipi == "2")
            {
                sql = "select ABONE_ID,ABONE_ADI|| ' ' || ABONE_SOYADI as ABONE_AD_SOYAD,SAYAC_NO,TO_CHAR(TO_DATE(KAPATMA_TARIHI, 'DD.MM.YYYY')) as KAPATMA_TARIHI,CREATED_DATE, ILCE_ADI, MAHALLE_ADI, BINA_ID, DURUMU,USULSUZ_KUL_ID from CBSW_USULSUZ_KULLANIM_TBL WHERE DURUMU='2' order by created_date desc";
            }
            else if (txtIsEmriTipi == "3")
            {
                sql = "select ABONE_ID,ABONE_ADI|| ' ' || ABONE_SOYADI as ABONE_AD_SOYAD,SAYAC_NO,TO_CHAR(TO_DATE(KAPATMA_TARIHI, 'DD.MM.YYYY')) as KAPATMA_TARIHI,CREATED_DATE, ILCE_ADI, MAHALLE_ADI, BINA_ID, DURUMU,USULSUZ_KUL_ID from CBSW_USULSUZ_KULLANIM_TBL WHERE DURUMU='3' order by created_date desc";
            }
            else if (txtIsEmriTipi == "4")
            {
                sql = "select ABONE_ID,ABONE_ADI|| ' ' || ABONE_SOYADI as ABONE_AD_SOYAD,SAYAC_NO,TO_CHAR(TO_DATE(KAPATMA_TARIHI, 'DD.MM.YYYY')) as KAPATMA_TARIHI,CREATED_DATE, ILCE_ADI, MAHALLE_ADI, BINA_ID, DURUMU,USULSUZ_KUL_ID from CBSW_USULSUZ_KULLANIM_TBL WHERE DURUMU='4' order by created_date desc";
            }
            else if (txtIsEmriTipi == "5")
            {
                sql = "select ABONE_ID,ABONE_ADI|| ' ' || ABONE_SOYADI as ABONE_AD_SOYAD,SAYAC_NO,TO_CHAR(TO_DATE(KAPATMA_TARIHI, 'DD.MM.YYYY')) as KAPATMA_TARIHI,CREATED_DATE, ILCE_ADI, MAHALLE_ADI, BINA_ID, DURUMU,USULSUZ_KUL_ID from CBSW_USULSUZ_KULLANIM_TBL WHERE DURUMU='5' order by created_date desc";
            }
            else if (txtIsEmriTipi == "6")
            {
                sql = "select ABONE_ID,ABONE_ADI|| ' ' || ABONE_SOYADI as ABONE_AD_SOYAD,SAYAC_NO,TO_CHAR(TO_DATE(KAPATMA_TARIHI, 'DD.MM.YYYY')) as KAPATMA_TARIHI,CREATED_DATE, ILCE_ADI, MAHALLE_ADI, BINA_ID, DURUMU,USULSUZ_KUL_ID from CBSW_USULSUZ_KULLANIM_TBL WHERE DURUMU='6' order by created_date desc";
            }
            else if (txtIsEmriTipi == "7")
            {
                sql = "select ABONE_ID,ABONE_ADI|| ' ' || ABONE_SOYADI as ABONE_AD_SOYAD,SAYAC_NO,TO_CHAR(TO_DATE(KAPATMA_TARIHI, 'DD.MM.YYYY')) as KAPATMA_TARIHI,CREATED_DATE, ILCE_ADI, MAHALLE_ADI, BINA_ID, DURUMU,USULSUZ_KUL_ID from CBSW_USULSUZ_KULLANIM_TBL WHERE DURUMU='7' order by created_date desc";
            }
            else if (txtIsEmriTipi == "8")
            {
                sql = "select ABONE_ID,ABONE_ADI|| ' ' || ABONE_SOYADI as ABONE_AD_SOYAD,SAYAC_NO,TO_CHAR(TO_DATE(KAPATMA_TARIHI, 'DD.MM.YYYY')) as KAPATMA_TARIHI,CREATED_DATE, ILCE_ADI, MAHALLE_ADI, BINA_ID, DURUMU,USULSUZ_KUL_ID from CBSW_USULSUZ_KULLANIM_TBL WHERE DURUMU='8' order by created_date desc";
            }
            else
            {
                sql = "select ABONE_ID,ABONE_ADI|| ' ' || ABONE_SOYADI as ABONE_AD_SOYAD,SAYAC_NO,TO_CHAR(TO_DATE(KAPATMA_TARIHI, 'DD.MM.YYYY')) as KAPATMA_TARIHI,CREATED_DATE, ILCE_ADI, MAHALLE_ADI, BINA_ID, DURUMU,USULSUZ_KUL_ID from CBSW_USULSUZ_KULLANIM_TBL order by created_date desc";
            }



            OracleConnection bgln = test.OracleConnect(connectionString);
            DataTable dt = test.GetDataTable(sql, bgln);
            int deger = dt.Rows.Count;
            if (deger > 0)
            {
                //Session["UGKTTKayitSayi"] = deger;
                return View(dt);
            }
            else
            {
                //Session["UGKTTKayitSayi"] = "0";

                DataTable dt2 = new DataTable();
                dt2.Columns.Add("ABONE_ID");
                dt2.Columns.Add("ABONE_AD_SOYAD");
                dt2.Columns.Add("SAYAC_NO");
                dt2.Columns.Add("KAPATMA_TARIHI");
                dt2.Columns.Add("CREATED_DATE");
                dt2.Columns.Add("ILCE_ADI");
                dt2.Columns.Add("MAHALLE_ADI");
                dt2.Columns.Add("BINA_ID");
                dt2.Columns.Add("DURUMU");   // 8
                dt2.Columns.Add("USULSUZ_KUL_ID");

                DataRow dr = dt2.NewRow();
                dr[0] = "----";
                dr[1] = "----";
                dr[2] = "----";
                dr[3] = "----";
                dr[4] = "----";
                dr[5] = "----";
                dr[6] = "----";
                dr[7] = "----";
                dr[8] = 99;
                dr[9] = 0;

                dt2.Rows.Add(dr);

                return View(dt2);
            }

        }
        public ActionResult getCari()

        {
            string connectionString = "";
            string ilkodu = "71";
            if (ilkodu == "71" || ilkodu == "710")
            {
                //connectionString = "DATA SOURCE=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.102.140)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=DOCUART)));USER ID=TEST_KIRGAZ;PASSWORD=TEST_KIRGAZ";
                connectionString = ConfigurationManager.ConnectionStrings["kirikkale"].ToString();
            }
            else
            {
                connectionString = ConfigurationManager.ConnectionStrings["kirsehir"].ToString();
            }

            DbBaglanti test = new DbBaglanti();

            string sql = "select *from CARI_TOPLAMLAR_VIEW";
            if (Request.Form != null && Request.Form.Count > 0)
            {
                string firmaKodu = Request.Form["firmaKodu"];
                string cariMiktari = Request.Form["cariMiktari"];
                if (firmaKodu != "" && cariMiktari == "")
                {
                    System.Diagnostics.Debug.WriteLine("######################################## 1 " + firmaKodu);
                    if (firmaKodu.Contains("%"))
                    {
                        sql = "select *from CARI_TOPLAMLAR_VIEW where FIRMAKODU like '" + firmaKodu + "'";
                    }
                    else
                    {
                        sql = "select *from CARI_TOPLAMLAR_VIEW where FIRMAKODU= '" + firmaKodu + "'";
                    }
                }
                else if (cariMiktari != "" && firmaKodu == "")
                {
                    sql = "select *from CARI_TOPLAMLAR_VIEW where CARI_KALAN> '" + Int32.Parse(cariMiktari) + "'";
                }
                else if (cariMiktari != "" && firmaKodu != "")
                {
                    if (firmaKodu.Contains("%"))
                    {
                        System.Diagnostics.Debug.WriteLine("######################################## 2 " + firmaKodu);
                        sql = "select *from CARI_TOPLAMLAR_VIEW where CARI_KALAN> '" + Int32.Parse(cariMiktari) + "' and FIRMAKODU like '" + firmaKodu + "'";

                    }
                    else
                    {
                        sql = "select *from CARI_TOPLAMLAR_VIEW where CARI_KALAN> '" + Int32.Parse(cariMiktari) + "' and FIRMAKODU= '" + firmaKodu + "'";

                    }
                }

            }
            else
            {

                sql = "select *from CARI_TOPLAMLAR_VIEW";
            }
            //string sql = "select ID,TABLO,DURUMU,IS_EMRI_SAYISI from cbs_is_emri_listesi_toplam2 order by ID,TABLO_ID ";

            OracleConnection bgln = test.OracleConnect(connectionString);
            DataTable dt = test.GetDataTable(sql, bgln);
            return View(dt);
        }


        public ActionResult getPeriyodikIsEmirListesi()

        {
            /* AESCrypt aes = new AESCrypt();
             string connectionString = "", sql;
             string ilkodu  = "71";


             sql = "SELECT * FROM ABO_PER_MUA_IS_EMRI where DURUMU ='TM'";

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

            /* for (int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
             {

                 dt.Rows[rowIndex][23] = aes.decrypt(dt.Rows[rowIndex][23].ToString());
                 dt.Rows[rowIndex][24] = aes.decrypt(dt.Rows[rowIndex][24].ToString());

             }
             string jsonString = JsonConvert.SerializeObject(dt);
             System.Diagnostics.Debug.Print("######################################  " + jsonString);

             dt.Dispose();


               return View(dt);*/
            return View();
        }

        public ActionResult getGonderilenSMSListesi()

        {
            AESCrypt aes = new AESCrypt();
             string connectionString = "", sql="";
             string ilkodu  = "71";

            /*
              <th>ABONE_ID</th>
                        <th>ABONE_ADI</th>
                        <th>ABONE_SOYADI</th>
                        <th>CEP_TEL</th>
                        <th>Fatura SMS</th>
                        <th>Fatura SMS tarihi</th>
                        <th>Fatura SMS ID</th>
                        <th>İhbar SMS</th>
                        <th>İhbar SMS tarihi</th>
                        <th>İhbar SMS ID</th>
             */
            if (Request.Form != null && Request.Form.Count > 0)
            {

                string tarihAraligi = Request.Form["zamanAraligi"];
                string smsTuru = Request.Form["SMSTuruSelector"];

                System.Diagnostics.Debug.Print("%%%%%%%%%%%%%%%%%%%%%%  " + smsTuru);
                string baslamaTarih = tarihAraligi.Split('-')[0].Replace('/', '.');
                string bitirmeTarih = tarihAraligi.Split('-')[1].Replace('/', '.');

                baslamaTarih = new string(baslamaTarih.Where(c => !char.IsWhiteSpace(c)).ToArray());
                StringBuilder s = new StringBuilder();
                s.Append(baslamaTarih.Split('.')[2]).Append(".").Append(baslamaTarih.Split('.')[0]).Append(".").Append(baslamaTarih.Split('.')[1]);
                baslamaTarih = s.ToString();
                System.Diagnostics.Debug.Print("%%%%%%%%%%%%%%%%%%%%%%  " + baslamaTarih);

                bitirmeTarih = new string(bitirmeTarih.Where(c => !char.IsWhiteSpace(c)).ToArray());
                s = new StringBuilder();
                s.Append(bitirmeTarih.Split('.')[2]).Append(".").Append(bitirmeTarih.Split('.')[0]).Append(".").Append(bitirmeTarih.Split('.')[1]);
                bitirmeTarih = s.ToString();
                System.Diagnostics.Debug.Print("%%%%%%%%%%%%%%%%%%%%%%  " + bitirmeTarih);

                if (smsTuru.Equals("")){
                    sql = "SELECT x.ABONE_ID, a.ABONE_ADI , a.ABONE_SOYADI, a.CEP_TEL, x.SMS_BILGI, x.FATURA_SMS_ID, x.FATURA_SMS_GONDERME_ZAMAN, x.SMS_IHBAR, x.IHBAR_SMS_ID, x.IHBAR_SMS_GONDERME_ZAMAN FROM ABO_ENDEKS x, ABO_ABONE_SAHIS a, ABO_ABONE b WHERE x.ABONE_ID = b.ABONE_ID AND a.ABONE_SAHIS_NO = b.ABONE_SAHIS_NO AND x.SMS_BILGI IS NOT NULL AND x.DONEM_AY = (SELECT MAX(DONEM_AY) FROM ABO_DONEM WHERE DONEM_YIL = (SELECT MAX(DONEM_YIL) FROM ABO_DONEM )) AND x.DONEM_YIL = (SELECT max(DONEM_YIL) FROM ABO_DONEM) and FATURA_SMS_GONDERME_ZAMAN BETWEEN to_date('" + baslamaTarih + "%','yyyy-MM-dd HH24:mi:ss') AND  to_date('" + bitirmeTarih + "%','yyyy-MM-dd HH24:mi:ss')";
                }
                else
                {
                    string sorgulanacakSMSTuru = faturaSMSTuruMapping(smsTuru);
                    switch (sorgulanacakSMSTuru)
                    {
                        case "0":
                            sql = "SELECT x.ABONE_ID, a.ABONE_ADI , a.ABONE_SOYADI, a.CEP_TEL, x.SMS_BILGI, x.FATURA_SMS_ID, x.FATURA_SMS_GONDERME_ZAMAN, x.SMS_IHBAR, x.IHBAR_SMS_ID, x.IHBAR_SMS_GONDERME_ZAMAN FROM ABO_ENDEKS x, ABO_ABONE_SAHIS a, ABO_ABONE b WHERE x.ABONE_ID = b.ABONE_ID AND a.ABONE_SAHIS_NO = b.ABONE_SAHIS_NO AND x.SMS_BILGI IS NOT NULL AND x.DONEM_AY = (SELECT MAX(DONEM_AY) FROM ABO_DONEM WHERE DONEM_YIL = (SELECT MAX(DONEM_YIL) FROM ABO_DONEM )) AND x.DONEM_YIL = (SELECT max(DONEM_YIL) FROM ABO_DONEM) and x.SON_OKUMA_TARIHI BETWEEN to_date('" + baslamaTarih + " 00:00:00','yyyy-MM-dd HH24:mi:ss') AND  to_date('" + bitirmeTarih + " 23:59:00','yyyy-MM-dd HH24:mi:ss') and x.SMS_BILGI ='0'";
                            break;
                        case "1":
                            sql = "SELECT x.ABONE_ID, a.ABONE_ADI , a.ABONE_SOYADI, a.CEP_TEL, x.SMS_BILGI, x.FATURA_SMS_ID, x.FATURA_SMS_GONDERME_ZAMAN, x.SMS_IHBAR, x.IHBAR_SMS_ID, x.IHBAR_SMS_GONDERME_ZAMAN FROM ABO_ENDEKS x, ABO_ABONE_SAHIS a, ABO_ABONE b WHERE x.ABONE_ID = b.ABONE_ID AND a.ABONE_SAHIS_NO = b.ABONE_SAHIS_NO AND x.SMS_BILGI IS NOT NULL AND x.DONEM_AY = (SELECT MAX(DONEM_AY) FROM ABO_DONEM WHERE DONEM_YIL = (SELECT MAX(DONEM_YIL) FROM ABO_DONEM )) AND x.DONEM_YIL = (SELECT max(DONEM_YIL) FROM ABO_DONEM) and FATURA_SMS_GONDERME_ZAMAN BETWEEN to_date('" + baslamaTarih + " 00:00:00','yyyy-MM-dd HH24:mi:ss') AND  to_date('" + bitirmeTarih + " 23:59:00','yyyy-MM-dd HH24:mi:ss') and x.SMS_BILGI ='1'";
                            break;
                        case "2":
                            sql = "SELECT x.ABONE_ID, a.ABONE_ADI , a.ABONE_SOYADI, a.CEP_TEL, x.SMS_BILGI, x.FATURA_SMS_ID, x.FATURA_SMS_GONDERME_ZAMAN, x.SMS_IHBAR, x.IHBAR_SMS_ID, x.IHBAR_SMS_GONDERME_ZAMAN FROM ABO_ENDEKS x, ABO_ABONE_SAHIS a, ABO_ABONE b WHERE x.ABONE_ID = b.ABONE_ID AND a.ABONE_SAHIS_NO = b.ABONE_SAHIS_NO AND x.SMS_BILGI IS NOT NULL AND x.DONEM_AY = (SELECT MAX(DONEM_AY) FROM ABO_DONEM WHERE DONEM_YIL = (SELECT MAX(DONEM_YIL) FROM ABO_DONEM )) AND x.DONEM_YIL = (SELECT max(DONEM_YIL) FROM ABO_DONEM) and FATURA_SMS_GONDERME_ZAMAN BETWEEN to_date('" + baslamaTarih + " 00:00:00','yyyy-MM-dd HH24:mi:ss') AND  to_date('" + bitirmeTarih + " 23:59:00','yyyy-MM-dd HH24:mi:ss') and x.SMS_BILGI ='2'";
                            break;
                        case "3":
                            sql = "SELECT x.ABONE_ID, a.ABONE_ADI , a.ABONE_SOYADI, a.CEP_TEL, x.SMS_BILGI, x.FATURA_SMS_ID, x.FATURA_SMS_GONDERME_ZAMAN, x.SMS_IHBAR, x.IHBAR_SMS_ID, x.IHBAR_SMS_GONDERME_ZAMAN FROM ABO_ENDEKS x, ABO_ABONE_SAHIS a, ABO_ABONE b WHERE x.ABONE_ID = b.ABONE_ID AND a.ABONE_SAHIS_NO = b.ABONE_SAHIS_NO AND x.SMS_BILGI IS NOT NULL AND x.DONEM_AY = (SELECT MAX(DONEM_AY) FROM ABO_DONEM WHERE DONEM_YIL = (SELECT MAX(DONEM_YIL) FROM ABO_DONEM )) AND x.DONEM_YIL = (SELECT max(DONEM_YIL) FROM ABO_DONEM) and FATURA_SMS_GONDERME_ZAMAN BETWEEN to_date('" + baslamaTarih + " 00:00:00','yyyy-MM-dd HH24:mi:ss') AND  to_date('" + bitirmeTarih + " 23:59:00','yyyy-MM-dd HH24:mi:ss') and x.SMS_BILGI ='3'";
                            break;
                        case "4":
                            sql = "SELECT x.ABONE_ID, a.ABONE_ADI , a.ABONE_SOYADI, a.CEP_TEL, x.SMS_BILGI, x.FATURA_SMS_ID, x.FATURA_SMS_GONDERME_ZAMAN, x.SMS_IHBAR, x.IHBAR_SMS_ID, x.IHBAR_SMS_GONDERME_ZAMAN FROM ABO_ENDEKS x, ABO_ABONE_SAHIS a, ABO_ABONE b WHERE x.ABONE_ID = b.ABONE_ID AND a.ABONE_SAHIS_NO = b.ABONE_SAHIS_NO AND x.SMS_BILGI IS NOT NULL AND x.DONEM_AY = (SELECT MAX(DONEM_AY) FROM ABO_DONEM WHERE DONEM_YIL = (SELECT MAX(DONEM_YIL) FROM ABO_DONEM )) AND x.DONEM_YIL = (SELECT max(DONEM_YIL) FROM ABO_DONEM) and FATURA_SMS_GONDERME_ZAMAN BETWEEN to_date('" + baslamaTarih + " 00:00:00','yyyy-MM-dd HH24:mi:ss') AND  to_date('" + bitirmeTarih + " 23:59:00','yyyy-MM-dd HH24:mi:ss') and x.SMS_BILGI ='4'";
                            break;
                        case "5":
                            sql = "SELECT x.ABONE_ID, a.ABONE_ADI , a.ABONE_SOYADI, a.CEP_TEL, x.SMS_BILGI, x.FATURA_SMS_ID, x.FATURA_SMS_GONDERME_ZAMAN, x.SMS_IHBAR, x.IHBAR_SMS_ID, x.IHBAR_SMS_GONDERME_ZAMAN FROM ABO_ENDEKS x, ABO_ABONE_SAHIS a, ABO_ABONE b WHERE x.ABONE_ID = b.ABONE_ID AND a.ABONE_SAHIS_NO = b.ABONE_SAHIS_NO AND x.SMS_BILGI IS NOT NULL AND x.DONEM_AY = (SELECT MAX(DONEM_AY) FROM ABO_DONEM WHERE DONEM_YIL = (SELECT MAX(DONEM_YIL) FROM ABO_DONEM )) AND x.DONEM_YIL = (SELECT max(DONEM_YIL) FROM ABO_DONEM) and FATURA_SMS_GONDERME_ZAMAN BETWEEN to_date('" + baslamaTarih + " 00:00:00','yyyy-MM-dd HH24:mi:ss') AND  to_date('" + bitirmeTarih + " 23:59:00','yyyy-MM-dd HH24:mi:ss') and x.SMS_BILGI ='5'";
                            break;
                        case "6":
                            //ihbar sms gonderildigi ise
                            sql = "SELECT x.ABONE_ID, a.ABONE_ADI , a.ABONE_SOYADI, a.CEP_TEL, x.SMS_BILGI, x.FATURA_SMS_ID, x.FATURA_SMS_GONDERME_ZAMAN, x.SMS_IHBAR, x.IHBAR_SMS_ID, x.IHBAR_SMS_GONDERME_ZAMAN FROM ABO_ENDEKS x, ABO_ABONE_SAHIS a, ABO_ABONE b WHERE x.ABONE_ID = b.ABONE_ID AND a.ABONE_SAHIS_NO = b.ABONE_SAHIS_NO AND x.SMS_BILGI IS NOT NULL AND x.DONEM_AY = (SELECT MAX(DONEM_AY) FROM ABO_DONEM WHERE DONEM_YIL = (SELECT MAX(DONEM_YIL) FROM ABO_DONEM )) AND x.DONEM_YIL = (SELECT max(DONEM_YIL) FROM ABO_DONEM) and FATURA_SMS_GONDERME_ZAMAN BETWEEN to_date('" + baslamaTarih + " 00:00:00','yyyy-MM-dd HH24:mi:ss') AND  to_date('" + bitirmeTarih + " 23:59:00','yyyy-MM-dd HH24:mi:ss') and x.SMS_IHBAR ='1'";
                            break;
                        case "7":
                            //İlgili Borç Ödeğindeiğinden İhbar SMS Gönderilmemiştir
                            sql = "SELECT x.ABONE_ID, a.ABONE_ADI , a.ABONE_SOYADI, a.CEP_TEL, x.SMS_BILGI, x.FATURA_SMS_ID, x.FATURA_SMS_GONDERME_ZAMAN, x.SMS_IHBAR, x.IHBAR_SMS_ID, x.IHBAR_SMS_GONDERME_ZAMAN FROM ABO_ENDEKS x, ABO_ABONE_SAHIS a, ABO_ABONE b WHERE x.ABONE_ID = b.ABONE_ID AND a.ABONE_SAHIS_NO = b.ABONE_SAHIS_NO AND x.SMS_BILGI IS NOT NULL AND x.DONEM_AY = (SELECT MAX(DONEM_AY) FROM ABO_DONEM WHERE DONEM_YIL = (SELECT MAX(DONEM_YIL) FROM ABO_DONEM )) AND x.DONEM_YIL = (SELECT max(DONEM_YIL) FROM ABO_DONEM) and FATURA_SMS_GONDERME_ZAMAN BETWEEN to_date('" + baslamaTarih + " 00:00:00','yyyy-MM-dd HH24:mi:ss') AND  to_date('" + bitirmeTarih + " 23:59:00','yyyy-MM-dd HH24:mi:ss') and x.SMS_IHBAR ='6'";
                            break;

                    }

                }

            }
            else
            {
                string FATURA_SMS_GONDERME_ZAMAN = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                sql = "SELECT x.ABONE_ID, a.ABONE_ADI , a.ABONE_SOYADI, a.CEP_TEL, x.SMS_BILGI, x.FATURA_SMS_ID, x.FATURA_SMS_GONDERME_ZAMAN, x.SMS_IHBAR, x.IHBAR_SMS_ID, x.IHBAR_SMS_GONDERME_ZAMAN FROM ABO_ENDEKS x, ABO_ABONE_SAHIS a, ABO_ABONE b WHERE x.ABONE_ID = b.ABONE_ID AND a.ABONE_SAHIS_NO = b.ABONE_SAHIS_NO AND x.SMS_BILGI IS NOT NULL AND x.DONEM_AY = (SELECT MAX(DONEM_AY) FROM ABO_DONEM WHERE DONEM_YIL = (SELECT MAX(DONEM_YIL) FROM ABO_DONEM )) AND x.DONEM_YIL = (SELECT max(DONEM_YIL) FROM ABO_DONEM) and FATURA_SMS_GONDERME_ZAMAN BETWEEN to_date('" + FATURA_SMS_GONDERME_ZAMAN.Split(' ')[0] + " 00:00:00','yyyy-MM-dd HH24:mi:ss') AND  to_date('" + FATURA_SMS_GONDERME_ZAMAN.Split(' ')[0] + " 23:59:00','yyyy-MM-dd HH24:mi:ss')";
            }
            System.Diagnostics.Debug.Print("%%%%%%%%%%%%%%%%%%%%%%  " + sql);


            if (ilkodu == "71")
             {
                 //connectionString = ConfigurationManager.ConnectionStrings["kirikkale"].ToString();
                // connectionString = ConfigurationManager.ConnectionStrings["kirikkaleTest"].ToString();
                connectionString = ConfigurationManager.ConnectionStrings["kirsehir"].ToString();
                //connectionString = ConfigurationManager.ConnectionStrings["KIRSEHIR_TEST"].ToString();





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
                dt.Rows[rowIndex][3] = aes.decrypt(dt.Rows[rowIndex][3].ToString());

            }
            string jsonString = JsonConvert.SerializeObject(dt);
             //System.Diagnostics.Debug.Print("######################################  " + jsonString);

             dt.Dispose();


               return View(dt);
           
        }
        public string faturaSMSTuruMapping(string smsTuru)
        {
            /*<option id="1">Fatura SMS</option>
                    <option id="2">Yetersiz Endeks SMS</option>
                    <option id="3">Adres Kapalı SMS</option>
                    <option id="6">İhbar SMS</option>
                    <option id="4">Telefon No Boş</option>
                    <option id="5">CepTel Formatında Değil</option>
                    İlgili Borç Ödeğindeiğinden İhbar SMS Gönderilmemiştir*/
            switch (smsTuru)
            {
                case "Gönderilmemiş SMSleri":
                    return "0";
                case "Fatura SMS":
                    return "1";
                case "Yetersiz Endeks SMS":
                    return "2";
                case "Adres Kapalı SMS":
                    return "3";
                case "Telefon No Boş":
                    return "4";
                case "CepTel Formatında Değil":
                    return "5";
                case "İhbar SMS":
                    return "6";
                case "İlgili Borç Ödeğindeiğinden İhbar SMS Gönderilmemiştir":
                    return "7";
                default:
                    return "0";
            }
        }

    }
}