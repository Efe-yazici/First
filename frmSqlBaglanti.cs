using System.Data.SqlClient;
using System;
using System.Configuration;
namespace hastaTakipSistemi
{
    internal class frmSqlBaglanti
    {
        string adres;
        
        public frmSqlBaglanti()
        {
            // Try to get connection string from environment variable first, then config, then default
            adres = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") 
                ?? ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString
                ?? @"Data Source=DESKTOP-138U8DJ;Initial Catalog=HastaTakip;Integrated Security=True;Encrypt=False;";
        }
        
        public SqlConnection baglan()
        {
            SqlConnection baglanti = new SqlConnection(adres);
            baglanti.Open();
            return baglanti;
        }
    }
}
