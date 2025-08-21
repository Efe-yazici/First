using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient; 

namespace hastaTakipSistemi
{
    public partial class Form1 : Form
    {
        bool sifreGizli = true;
        public Form1()
        {
            InitializeComponent();
        }
        frmSqlBaglanti bgl = new frmSqlBaglanti();

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // ENTER → Giriş Yap
            if (keyData == Keys.Enter)
            {
                btnGiriş.PerformClick(); // Giriş Yap butonunu tetikler
                return true;
            }
            // ESC → Çıkış
            else if (keyData == Keys.Escape)
            {
                this.Close(); // Formu kapatır
                return true;
            }
            // F2 → Kayıt Ol
            else if (keyData == Keys.F2)
            {
                btnKayıtOl.PerformClick(); // Kayıt Ol butonunu tetikler
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnGiriş_Click(object sender, EventArgs e)
        {
            if (txtKulAd.Text != "" && txtSifre.Text != "")
            {
                SqlCommand giris = new SqlCommand("girisYap", bgl.baglan());
                giris.CommandType = CommandType.StoredProcedure;
                giris.Parameters.AddWithValue("KulAd", txtKulAd.Text);
                giris.Parameters.AddWithValue("Sifre", txtSifre.Text);
                SqlDataReader dr = giris.ExecuteReader();
                if (dr.Read())
                {
                    MessageBox.Show("Giriş işlemi başarılı", "Giriş başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FrmAnaSayfa fr = new FrmAnaSayfa();
                    this.Hide();
                    fr.Show();
                }
                else
                {
                    MessageBox.Show("Giriş işlemi başarısız", "Giriş başarısız", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz !", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnKayıtOl_Click(object sender, EventArgs e)
        {
            frmKayit kayitFormu = new frmKayit();
            kayitFormu.Show();
        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtSifre.PasswordChar = '●'; // Şifre gizli başlasın
            btnGoz.Image = Properties.Resources.eye_open;

            ToolTip tt = new ToolTip();
            tt.SetToolTip(btnGoz, "Şifreyi Göster/Gizle");
            tt.SetToolTip(txtKulAd, "Kullanıcı adınızı girin");

        }

        private void btnGoz_Click(object sender, EventArgs e)
        {
            if (sifreGizli)
            {
                txtSifre.PasswordChar = '\0'; // Şifre görünür 
                btnGoz.Image = Properties.Resources.eye_open;
                sifreGizli = false;
            }
            else
            {
                txtSifre.PasswordChar = '●'; // Şifre gizli
                btnGoz.Image = Properties.Resources.eye_closed;
                sifreGizli = true;
            }

        }
    
    }
}
