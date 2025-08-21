using System.Data.SqlClient;
namespace hastaTakipSistemi
{
    internal class frmSqlBaglanti
    {
        string adres = @"Data Source=DESKTOP-138U8DJ;Initial Catalog=HastaTakip;Integrated Security=True;Encrypt=False;";
        public SqlConnection baglan()
        {
            SqlConnection baglanti = new SqlConnection(adres);
            baglanti.Open();
            return baglanti;
        }
    }
}
