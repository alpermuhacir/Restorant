﻿using System;
using System.Data;
using System.Data.SqlClient;

namespace EserRestorant
{
    class clsOdeme
    {
        clsGenel gnl = new clsGenel();

        #region Fields
        private int _OdemeID;
        private int _AdisyonID;
        private int _OdemeTurId;
        private decimal _AraToplam;
        private decimal _Indirim;
        private decimal _Kdvtutari;
        private decimal _GenelToplam;
        private DateTime _Tarih;
        private int _MusteriId;
        #endregion

        #region Properties
        public int OdemeID { get => _OdemeID; set => _OdemeID = value; }
        public int AdisyonID { get => _AdisyonID; set => _AdisyonID = value; }
        public int OdemeTurId { get => _OdemeTurId; set => _OdemeTurId = value; }
        public decimal AraToplam { get => _AraToplam; set => _AraToplam = value; }
        public decimal Indirim { get => _Indirim; set => _Indirim = value; }
        public decimal Kdvtutari { get => _Kdvtutari; set => _Kdvtutari = value; }
        public decimal GenelToplam { get => _GenelToplam; set => _GenelToplam = value; }
        public DateTime Tarih { get => _Tarih; set => _Tarih = value; }
        public int MusteriId { get => _MusteriId; set => _MusteriId = value; }
        #endregion

        //müşterinin masa hesabını kapatıyoruz
        public bool billClose(clsOdeme bill)
        {
            bool result = false;

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd = new SqlCommand("insert into HESAPODEMELERI(ADISYONID,ODEMETURID,MUSTERIID,ARATOPLAM,KDVTUTARI,TOPLAMTUTAR,INDIRIM)" +
                                            "VALUES (@ADISYONID,@ODEMETURID,@MUSTERIID,@ARATOPLAM,@KDVTUTARI,@TOPLAMTUTAR,@INDIRIM);", con);
            //SqlCommand cmd = new SqlCommand("insert into HESAPODEMELERI(ADISYONID, ODEMETURID, MUSTERIID, ARATOPLAM, KDVTUTARI, INDIRIM, TOPLAMTUTAR) VALUES(@ADISYONID, @ODEMETURID, @MUSTERIID, @ARATOPLAM, @KDVTUTARI, @INDIRIM, @TOPLAMTUTAR);", con);

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                cmd.Parameters.Add("@ADISYONID", SqlDbType.Int).Value = bill._AdisyonID;
                cmd.Parameters.Add("@ODEMETURID", SqlDbType.Int).Value = bill._OdemeTurId;
                cmd.Parameters.Add("@MUSTERIID", SqlDbType.Int).Value = bill._MusteriId;
                cmd.Parameters.Add("@ARATOPLAM", SqlDbType.Money).Value = bill._AraToplam;
                cmd.Parameters.Add("@KDVTUTARI", SqlDbType.Money).Value = bill._Kdvtutari;
                //cmd.Parameters.Add("@INDIRIM", SqlDbType.Money).Value = bill._Indirim;
                cmd.Parameters.Add("@TOPLAMTUTAR", SqlDbType.Money).Value = bill._GenelToplam;
                cmd.Parameters.Add("@INDIRIM", SqlDbType.Money).Value = bill._Indirim;

                result = Convert.ToBoolean(cmd.ExecuteNonQuery());
            }
            catch (SqlException ex)
            {
                string hata = ex.Message;
                throw;
            }
            finally
            {
                con.Dispose();
                con.Close();
            }

            return result;
        }

        //müşterinin toplam harcamasını buluyoruz
        public decimal sumTotalforClientId(int clientId)
        {
            decimal total = 0;

            SqlConnection con = new SqlConnection(gnl.conString);
            SqlCommand cmd =
                new SqlCommand("SELECT sum(TOPLAMTUTAR) as total from HESAPODEMELERI WHERE MUSTERIID=@clientId", con);

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                cmd.Parameters.Add("clientId", SqlDbType.Int).Value = clientId;
                total = Convert.ToDecimal(cmd.ExecuteScalar());
            }
            catch (SqlException ex)
            {
                string hata = ex.Message;
                throw;
            }
            finally
            {
                con.Dispose();
                con.Close();
            }

            return total;
        }
    }
}
