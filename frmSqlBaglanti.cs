using System.Data.SqlClient;
using System;
using System.Configuration;

namespace hastaTakipSistemi
{
    internal class frmSqlBaglanti
    {
        private string adres = @"Data Source=DESKTOP-138U8DJ;Initial Catalog=HastaTakip;Integrated Security=True;Encrypt=False;";
        
        public SqlConnection baglan()
        {
            try
            {
                SqlConnection baglanti = new SqlConnection(adres);
                if (baglanti.State == System.Data.ConnectionState.Closed)
                {
                    baglanti.Open();
                }
                return baglanti;
            }
            catch (Exception ex)
            {
                throw new Exception($"Veritabanı bağlantısı kurulamadı: {ex.Message}");
            }
        }

        public void baglantiKapat(SqlConnection connection)
        {
            try
            {
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                // Log error if needed
                System.Diagnostics.Debug.WriteLine($"Bağlantı kapatma hatası: {ex.Message}");
            }
        }

        public bool BaglantiTest()
        {
            try
            {
                using (SqlConnection test = baglan())
                {
                    return test.State == System.Data.ConnectionState.Open;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
