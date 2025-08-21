using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
namespace hastaTakipSistemi
{
    public partial class frmKayit : Form
    {
        public frmKayit()
        {
            InitializeComponent();
        }
        frmSqlBaglanti bgl = new frmSqlBaglanti();


        private void btnKayıt_Click(object sender, EventArgs e)
        {
            string sifre = txtSifre.Text;

            // Şifre kurallarını kontrol et
            if (sifre.Length < 8 ||
                !sifre.Any(char.IsUpper) ||
                !sifre.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                MessageBox.Show("Şifre en az 8 karakterli olmalı, en az 1 büyük harf ve 1 özel karakter içermelidir!", "Geçersiz Şifre", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
                if (txtKulAd.Text != "" && txtSifre.Text != "")
            {
                SqlCommand kayit = new SqlCommand("kayitOl", bgl.baglan());
                kayit.CommandType = CommandType.StoredProcedure;
                kayit.Parameters.AddWithValue("KulAd", txtKulAd.Text);
                kayit.Parameters.AddWithValue("Sifre", txtSifre.Text);
                kayit.ExecuteNonQuery();
                MessageBox.Show("Kayıt işlemi başarılı", "Kayıt başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz !", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
