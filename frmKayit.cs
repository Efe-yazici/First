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
            // Check for empty fields
            if (string.IsNullOrWhiteSpace(txtKulAd.Text) || string.IsNullOrWhiteSpace(txtSifre.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz !", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Username validation
            if (txtKulAd.Text.Length < 3)
            {
                MessageBox.Show("Kullanıcı adı en az 3 karakter olmalıdır!", "Geçersiz Kullanıcı Adı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sifre = txtSifre.Text;

            // Enhanced password validation
            if (sifre.Length < 8 ||
                !sifre.Any(char.IsUpper) ||
                !sifre.Any(char.IsDigit) ||
                !sifre.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                MessageBox.Show("Şifre en az 8 karakterli olmalı, en az 1 büyük harf, 1 rakam ve 1 özel karakter içermelidir!", "Geçersiz Şifre", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                SqlCommand kayit = new SqlCommand("kayitOl", bgl.baglan());
                kayit.CommandType = CommandType.StoredProcedure;
                kayit.Parameters.AddWithValue("KulAd", txtKulAd.Text);
                kayit.Parameters.AddWithValue("Sifre", txtSifre.Text);
                kayit.ExecuteNonQuery();
                
                // Log the registration
                AuditLogger.LogUserAction("Yeni Kullanıcı Kaydı", $"Kullanıcı Adı: {txtKulAd.Text}");
                
                MessageBox.Show("Kayıt işlemi başarılı! Giriş yapabilirsiniz.", "Kayıt başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); // Close registration form after successful registration
            }
            catch(SqlException ex)
            {
                if(ex.Message.Contains("duplicate") || ex.Message.Contains("PRIMARY KEY"))
                {
                    MessageBox.Show("Bu kullanıcı adı zaten kullanımda! Farklı bir kullanıcı adı seçiniz.", "Kullanıcı Adı Mevcut", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show($"Veritabanı hatası: {ex.Message}", "Kayıt Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Beklenmeyen hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
