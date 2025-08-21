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
            if (string.IsNullOrWhiteSpace(txtKulAd.Text) || string.IsNullOrWhiteSpace(txtSifre.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz !", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                SqlCommand giris = new SqlCommand("girisYap", bgl.baglan());
                giris.CommandType = CommandType.StoredProcedure;
                giris.Parameters.AddWithValue("KulAd", txtKulAd.Text);
                giris.Parameters.AddWithValue("Sifre", txtSifre.Text);
                SqlDataReader dr = giris.ExecuteReader();
                if (dr.Read())
                {
                    string kullaniciAdi = txtKulAd.Text;
                    AuditLogger.LogUserAction("Başarılı Giriş", $"Kullanıcı: {kullaniciAdi}");
                    
                    MessageBox.Show("Giriş işlemi başarılı", "Giriş başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FrmAnaSayfa fr = new FrmAnaSayfa();
                    this.Hide();
                    fr.Show();
                }
                else
                {
                    AuditLogger.LogUserAction("Başarısız Giriş Denemesi", $"Kullanıcı Adı: {txtKulAd.Text}");
                    MessageBox.Show("Kullanıcı adı veya şifre hatalı!", "Giriş başarısız", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSifre.Clear(); // Clear password on failed login
                    txtKulAd.Focus(); // Focus back to username
                }
            }
            catch(SqlException ex)
            {
                MessageBox.Show($"Veritabanı bağlantı hatası: {ex.Message}", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Beklenmeyen hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            tt.SetToolTip(txtSifre, "Şifrenizi girin");

            // Apply modern visual improvements
            ApplyModernStyling();
        }

        private void ApplyModernStyling()
        {
            // Set modern form styling
            this.BackColor = Color.FromArgb(240, 244, 248); // Light modern background
            
            // Style text boxes with modern appearance
            StyleTextBox(txtKulAd);
            StyleTextBox(txtSifre);
            
            // Style buttons with gradient and hover effects
            StylePrimaryButton(btnGiriş);
            StyleSecondaryButton(btnKayıtOl);
            
            // Style the eye button
            btnGoz.FlatStyle = FlatStyle.Flat;
            btnGoz.FlatAppearance.BorderSize = 0;
            btnGoz.BackColor = Color.Transparent;
            btnGoz.Cursor = Cursors.Hand;
        }

        private void StyleTextBox(TextBox textBox)
        {
            textBox.BorderStyle = BorderStyle.None;
            textBox.BackColor = Color.White;
            textBox.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
            textBox.ForeColor = Color.FromArgb(64, 64, 64);
            
            // Add padding appearance by using a panel wrapper if needed
            if (textBox.Parent is Panel panel)
            {
                panel.BackColor = Color.White;
                panel.BorderStyle = BorderStyle.FixedSingle;
                panel.Padding = new Padding(8);
            }
        }

        private void StylePrimaryButton(Button button)
        {
            button.BackColor = Color.FromArgb(33, 150, 243); // Modern blue
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            button.Cursor = Cursors.Hand;
            
            // Add hover effects
            button.MouseEnter += (s, e) => button.BackColor = Color.FromArgb(25, 118, 210);
            button.MouseLeave += (s, e) => button.BackColor = Color.FromArgb(33, 150, 243);
        }

        private void StyleSecondaryButton(Button button)
        {
            button.BackColor = Color.FromArgb(96, 125, 139); // Modern grey
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            button.Cursor = Cursors.Hand;
            
            // Add hover effects
            button.MouseEnter += (s, e) => button.BackColor = Color.FromArgb(69, 90, 100);
            button.MouseLeave += (s, e) => button.BackColor = Color.FromArgb(96, 125, 139);
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
