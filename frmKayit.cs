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

        private void frmKayit_Load(object sender, EventArgs e)
        {
            // Apply modern theme to registration form
            ApplyModernRegistrationTheme();
            
            // Add helpful tooltips
            AddRegistrationTooltips();
        }

        private void ApplyModernRegistrationTheme()
        {
            // Set modern background color
            this.BackColor = Color.FromArgb(245, 248, 255); // Light blue background
            
            // Apply registration-specific styling
            ApplyRegistrationStyling();
        }

        private void ApplyRegistrationStyling()
        {
            foreach (Control control in this.Controls)
            {
                StyleRegistrationControlsRecursively(control);
            }
        }

        private void StyleRegistrationControlsRecursively(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is Button button)
                {
                    StyleRegistrationButton(button);
                }
                else if (control is TextBox textBox)
                {
                    StyleRegistrationTextBox(textBox);
                }
                else if (control is Label label)
                {
                    StyleRegistrationLabel(label);
                }
                
                // Recursively style nested controls
                if (control.HasChildren)
                {
                    StyleRegistrationControlsRecursively(control);
                }
            }
        }

        private void StyleRegistrationButton(Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            button.Cursor = Cursors.Hand;
            button.Height = Math.Max(button.Height, 40);
            
            // Registration button styling
            button.BackColor = Color.FromArgb(63, 81, 181); // Indigo
            button.ForeColor = Color.White;
            button.MouseEnter += (s, e) => button.BackColor = Color.FromArgb(48, 63, 159);
            button.MouseLeave += (s, e) => button.BackColor = Color.FromArgb(63, 81, 181);
        }

        private void StyleRegistrationTextBox(TextBox textBox)
        {
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.BackColor = Color.White;
            textBox.Font = new Font("Segoe UI", 10F);
            textBox.ForeColor = Color.FromArgb(64, 64, 64);
            textBox.Height = Math.Max(textBox.Height, 30);
        }

        private void StyleRegistrationLabel(Label label)
        {
            label.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            label.ForeColor = Color.FromArgb(64, 64, 64);
        }

        private void AddRegistrationTooltips()
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(txtKulAd, "En az 3 karakter, alfanümerik karakterler kullanabilirsiniz");
            tt.SetToolTip(txtSifre, "En az 8 karakter, 1 büyük harf, 1 rakam ve 1 özel karakter içermelidir");
        }


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

        // Visual feedback methods for registration form
        private Form registrationLoadingForm;

        private void ShowRegistrationLoadingMessage(string message)
        {
            registrationLoadingForm = new Form()
            {
                FormBorderStyle = FormBorderStyle.None,
                BackColor = Color.FromArgb(240, 248, 255),
                Size = new Size(300, 100),
                StartPosition = FormStartPosition.CenterParent,
                TopMost = true
            };

            Label lblLoading = new Label()
            {
                Text = message,
                ForeColor = Color.FromArgb(63, 81, 181),
                Font = new Font("Segoe UI", 10F),
                AutoSize = false,
                Size = new Size(280, 80),
                Location = new Point(10, 10),
                TextAlign = ContentAlignment.MiddleCenter
            };

            registrationLoadingForm.Controls.Add(lblLoading);
            registrationLoadingForm.Show(this);
            Application.DoEvents();
        }

        private void HideRegistrationLoadingMessage()
        {
            registrationLoadingForm?.Close();
            registrationLoadingForm?.Dispose();
            registrationLoadingForm = null;
        }

        private void ShowRegistrationSuccessMessage(string message, string title)
        {
            using (Form successForm = new Form())
            {
                successForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                successForm.BackColor = Color.FromArgb(232, 245, 233);
                successForm.Size = new Size(350, 150);
                successForm.StartPosition = FormStartPosition.CenterParent;
                successForm.Text = title;
                successForm.MaximizeBox = false;
                successForm.MinimizeBox = false;

                Label lblMessage = new Label()
                {
                    Text = message,
                    ForeColor = Color.FromArgb(46, 125, 50),
                    Font = new Font("Segoe UI", 10F),
                    AutoSize = false,
                    Size = new Size(310, 60),
                    Location = new Point(20, 20),
                    TextAlign = ContentAlignment.MiddleCenter
                };

                Button btnOK = new Button()
                {
                    Text = "Tamam",
                    BackColor = Color.FromArgb(63, 81, 181),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Size = new Size(80, 30),
                    Location = new Point(135, 90),
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold)
                };
                btnOK.FlatAppearance.BorderSize = 0;
                btnOK.Click += (s, e) => successForm.Close();

                successForm.Controls.AddRange(new Control[] { lblMessage, btnOK });
                successForm.ShowDialog(this);
            }
        }

        private void ShowRegistrationErrorMessage(string message, string title)
        {
            using (Form errorForm = new Form())
            {
                errorForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                errorForm.BackColor = Color.FromArgb(255, 235, 238);
                errorForm.Size = new Size(350, 150);
                errorForm.StartPosition = FormStartPosition.CenterParent;
                errorForm.Text = title;
                errorForm.MaximizeBox = false;
                errorForm.MinimizeBox = false;

                Label lblMessage = new Label()
                {
                    Text = message,
                    ForeColor = Color.FromArgb(183, 28, 28),
                    Font = new Font("Segoe UI", 10F),
                    AutoSize = false,
                    Size = new Size(310, 60),
                    Location = new Point(20, 20),
                    TextAlign = ContentAlignment.MiddleCenter
                };

                Button btnOK = new Button()
                {
                    Text = "Tamam",
                    BackColor = Color.FromArgb(244, 67, 54),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Size = new Size(80, 30),
                    Location = new Point(135, 90),
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold)
                };
                btnOK.FlatAppearance.BorderSize = 0;
                btnOK.Click += (s, e) => errorForm.Close();

                errorForm.Controls.AddRange(new Control[] { lblMessage, btnOK });
                errorForm.ShowDialog(this);
            }
        }
    }
}
