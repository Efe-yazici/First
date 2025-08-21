using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace hastaTakipSistemi
{
    public partial class FrmAnaSayfa : Form
    {
        public FrmAnaSayfa()
        {
            InitializeComponent();
        }
        frmSqlBaglanti bgl = new frmSqlBaglanti();
        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("Güncellenecek kayıt seçilmedi! Lütfen listeden bir kayıt seçin.", "Seçim Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (ValidateInputs())
            {
                guncelle();
            }
        }

        private void guncelle()
        {
            try
            {
                // Show loading indicator
                ShowLoadingMessage("Güncelleme işlemi devam ediyor...");
                
                SqlCommand guncelle = new SqlCommand("guncelle", bgl.baglan());
                guncelle.CommandType = CommandType.StoredProcedure;
                guncelle.Parameters.AddWithValue("id", int.Parse(txtID.Text));
                guncelle.Parameters.AddWithValue("ad", txtAD.Text);
                guncelle.Parameters.AddWithValue("soyad", txtSoyad.Text);
                guncelle.Parameters.AddWithValue("tc", txtTC.Text);
                guncelle.Parameters.AddWithValue("tel", txtTelefon.Text);
                guncelle.Parameters.AddWithValue("yas", int.Parse(txtYas.Text.ToString()));
                guncelle.Parameters.AddWithValue("cinsiyet", txtCinsiyet.Text);
                guncelle.Parameters.AddWithValue("sikayet", txtSikayet.Text);
                guncelle.Parameters.AddWithValue("tarih", DateTime.Now);
                guncelle.Parameters.AddWithValue("durum", txtDurum.SelectedValue);
                guncelle.Parameters.AddWithValue("bolum", txtBolum.SelectedValue);
                if (lblEx.Text == "True")
                {
                    guncelle.Parameters.AddWithValue("ex", 1);
                }
                else
                {
                    guncelle.Parameters.AddWithValue("ex", 0);
                }
                guncelle.ExecuteNonQuery();
                
                // Log the action
                AuditLogger.LogPatientAction("Hasta Güncellendi", $"ID: {txtID.Text}, Ad: {txtAD.Text} {txtSoyad.Text}, TC: {txtTC.Text}");
                
                // Hide loading and show success
                HideLoadingMessage();
                ShowSuccessMessage("Güncelleme işlemi başarıyla tamamlandı!", "Güncelleme Başarılı");
                Listele();
                temizle(); // Clear form after successful update
            }
            catch(SqlException ex)
            {
                HideLoadingMessage();
                AuditLogger.LogPatientAction("Hasta Güncelleme Hatası", $"ID: {txtID.Text}, Hata: {ex.Message}");
                ShowErrorMessage($"Veritabanı hatası: {ex.Message}", "Güncelleme Hatası");
            }
            catch(Exception ex)
            {
                HideLoadingMessage();
                ShowErrorMessage($"Beklenmeyen hata: {ex.Message}", "Hata");
            }
        }

        private void temizle()
        {
            txtAD.Text = "";
            txtBolum.Text = "";
            txtCinsiyet.Text = "";
            txtDurum.Text = "";
            txtID.Text = "";
            txtSikayet.Text = "";
            txtSoyad.Text = "";
            txtTarih.Text = "";
            txtTC.Text = "";
            txtTelefon.Text = "";
            txtYas.Text = "";
            rbHayir.Checked = true;
            lblEx.Text = "False";
        }

        private void FrmAnaSayfa_Load(object sender, EventArgs e)
        {
            try
            {
                // Apply modern visual styling first
                ApplyModernTheme();
                
                // Load initial data
                Listele();
                durumDoldur();
                bolumDoldur();
                
                // Set default values
                rbHayir.Checked = true;
                lblEx.Text = "False";
                
                // Add tooltips for better user experience
                AddTooltips();
                
                // Format DataGridView for better appearance
                FormatDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Form yükleme hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddTooltips()
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(txtTC, "11 haneli TC Kimlik Numarası");
            tt.SetToolTip(txtTelefon, "Telefon numarası (örn: 05xxxxxxxxx)");
            tt.SetToolTip(txtYas, "0-150 arası sayı");
            tt.SetToolTip(dataGridView1, "Hasta seçmek için satıra tıklayın");
            
            // Enhanced tooltips with modern information
            tt.IsBalloon = true;
            tt.ToolTipTitle = "Bilgi";
            tt.ToolTipIcon = ToolTipIcon.Info;
        }

        private void FormatDataGridView()
        {
            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.MultiSelect = false;
                dataGridView1.ReadOnly = true;
                
                // Enhanced modern styling for DataGridView
                dataGridView1.BackgroundColor = Color.White;
                dataGridView1.BorderStyle = BorderStyle.None;
                dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(33, 150, 243);
                dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;
                dataGridView1.DefaultCellStyle.BackColor = Color.White;
                dataGridView1.DefaultCellStyle.ForeColor = Color.FromArgb(64, 64, 64);
                dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
                dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
                dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(64, 64, 64);
                dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                dataGridView1.EnableHeadersVisualStyles = false;
                dataGridView1.RowHeadersVisible = false;
                dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
            }
        }

        private void ApplyModernTheme()
        {
            // Set form background to modern color
            this.BackColor = Color.FromArgb(248, 249, 250);
            
            // Apply modern styling to all buttons
            ApplyButtonStyling();
            
            // Apply modern styling to text boxes
            ApplyTextBoxStyling();
            
            // Apply modern styling to labels
            ApplyLabelStyling();
        }

        private void ApplyButtonStyling()
        {
            // Style all buttons with modern appearance
            foreach (Control control in this.Controls)
            {
                StyleControlsRecursively(control);
            }
        }

        private void StyleControlsRecursively(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is Button button)
                {
                    StyleModernButton(button);
                }
                else if (control is TextBox textBox)
                {
                    StyleModernTextBox(textBox);
                }
                else if (control is Label label && !label.Name.Contains("lbl"))
                {
                    StyleModernLabel(label);
                }
                
                // Recursively style nested controls
                if (control.HasChildren)
                {
                    StyleControlsRecursively(control);
                }
            }
        }

        private void StyleModernButton(Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            button.Cursor = Cursors.Hand;
            button.Height = Math.Max(button.Height, 35); // Minimum height for better UX
            
            // Determine button style based on name or text
            if (button.Name.Contains("Kaydet") || button.Text.Contains("Kaydet"))
            {
                button.BackColor = Color.FromArgb(76, 175, 80); // Green for save
                button.ForeColor = Color.White;
                button.MouseEnter += (s, e) => button.BackColor = Color.FromArgb(67, 160, 71);
                button.MouseLeave += (s, e) => button.BackColor = Color.FromArgb(76, 175, 80);
            }
            else if (button.Name.Contains("Sil") || button.Text.Contains("Sil"))
            {
                button.BackColor = Color.FromArgb(244, 67, 54); // Red for delete
                button.ForeColor = Color.White;
                button.MouseEnter += (s, e) => button.BackColor = Color.FromArgb(229, 57, 53);
                button.MouseLeave += (s, e) => button.BackColor = Color.FromArgb(244, 67, 54);
            }
            else if (button.Name.Contains("Guncelle") || button.Text.Contains("Güncelle"))
            {
                button.BackColor = Color.FromArgb(255, 152, 0); // Orange for update
                button.ForeColor = Color.White;
                button.MouseEnter += (s, e) => button.BackColor = Color.FromArgb(245, 124, 0);
                button.MouseLeave += (s, e) => button.BackColor = Color.FromArgb(255, 152, 0);
            }
            else
            {
                button.BackColor = Color.FromArgb(33, 150, 243); // Blue for other actions
                button.ForeColor = Color.White;
                button.MouseEnter += (s, e) => button.BackColor = Color.FromArgb(25, 118, 210);
                button.MouseLeave += (s, e) => button.BackColor = Color.FromArgb(33, 150, 243);
            }
        }

        private void StyleModernTextBox(TextBox textBox)
        {
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.BackColor = Color.White;
            textBox.Font = new Font("Segoe UI", 9F);
            textBox.ForeColor = Color.FromArgb(64, 64, 64);
        }

        private void StyleModernLabel(Label label)
        {
            label.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            label.ForeColor = Color.FromArgb(64, 64, 64);
        }

        private void ApplyTextBoxStyling()
        {
            // This is handled in StyleControlsRecursively
        }

        private void ApplyLabelStyling()
        {
            // This is handled in StyleControlsRecursively
        }


        private void Listele()
        {
            try
            {
                SqlCommand liste = new SqlCommand("listele",bgl.baglan());
                SqlDataAdapter da = new SqlDataAdapter(liste);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Listeleme hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HastaAra(string aramaMetni)
        {
            try
            {
                SqlCommand ara = new SqlCommand("SELECT * FROM HastaBilgi WHERE " +
                    "Ad LIKE @arama OR Soyad LIKE @arama OR TC LIKE @arama OR " +
                    "Telefon LIKE @arama OR Sikayet LIKE @arama", bgl.baglan());
                ara.Parameters.AddWithValue("@arama", "%" + aramaMetni + "%");
                
                SqlDataAdapter da = new SqlDataAdapter(ara);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Arama hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void durumDoldur()
        {
            try
            {
                SqlCommand durum = new SqlCommand("durumDoldur", bgl.baglan());
                SqlDataAdapter da = new SqlDataAdapter(durum);
                DataTable dt = new DataTable();
                da.Fill(dt);
                txtDurum.DataSource = dt;
                txtDurum.DisplayMember = "durumAd";
                txtDurum.ValueMember = "durumID";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Durum bilgileri yüklenemedi: {ex.Message}", "Veri Yükleme Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void bolumDoldur()
        {
            try
            {
                SqlCommand bolum = new SqlCommand("bolumDoldur", bgl.baglan());
                SqlDataAdapter da = new SqlDataAdapter(bolum);
                DataTable dt = new DataTable();
                da.Fill(dt);
                txtBolum.DataSource = dt;
                txtBolum.DisplayMember = "BolumAd";
                txtBolum.ValueMember = "BolumID";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bölüm bilgileri yüklenemedi: {ex.Message}", "Veri Yükleme Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnListele_Click(object sender, EventArgs e)
        {
            Listele();
            durumDoldur();
            bolumDoldur();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && dataGridView1.Rows.Count > 0)
                {
                    int secilen = dataGridView1.SelectedCells[0].RowIndex;
                    
                    // Check if the selected row has data
                    if (dataGridView1.Rows[secilen].Cells[0].Value != null)
                    {
                        txtID.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
                        txtAD.Text = dataGridView1.Rows[secilen].Cells[1].Value?.ToString() ?? "";
                        txtSoyad.Text = dataGridView1.Rows[secilen].Cells[2].Value?.ToString() ?? "";
                        txtTC.Text = dataGridView1.Rows[secilen].Cells[3].Value?.ToString() ?? "";
                        txtTelefon.Text = dataGridView1.Rows[secilen].Cells[4].Value?.ToString() ?? "";
                        txtYas.Text = dataGridView1.Rows[secilen].Cells[5].Value?.ToString() ?? "";
                        txtCinsiyet.Text = dataGridView1.Rows[secilen].Cells[6].Value?.ToString() ?? "";
                        txtSikayet.Text = dataGridView1.Rows[secilen].Cells[7].Value?.ToString() ?? "";
                        txtTarih.Text = dataGridView1.Rows[secilen].Cells[8].Value?.ToString() ?? "";
                        
                        // Safely set combo box selected values
                        if (dataGridView1.Rows[secilen].Cells[9].Value != null)
                            txtDurum.SelectedValue = dataGridView1.Rows[secilen].Cells[9].Value.ToString();
                        
                        if (dataGridView1.Rows[secilen].Cells[10].Value != null)
                            txtBolum.SelectedValue = dataGridView1.Rows[secilen].Cells[10].Value.ToString();
                        
                        lblEx.Text = dataGridView1.Rows[secilen].Cells[11].Value?.ToString() ?? "False";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kayıt seçme hatası: {ex.Message}", "Seçim Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void rbEvet_CheckedChanged(object sender, EventArgs e)
        {
            if (rbEvet.Checked == true)
            {
                lblEx.Text = "true";
            }
            else
            {
                lblEx.Text = "false"; 
            }
        }

        private void lblEx_TextChanged(object sender, EventArgs e)
        {
            if(lblEx.Text == "true")
            {
                rbEvet.Checked = true;
            }
            else
            {
                rbHayir.Checked = true;
            }
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if(ValidateInputs())
            {
                kaydet();
            }
        }

        private bool ValidateInputs()
        {
            // Check for empty fields
            if(string.IsNullOrWhiteSpace(txtAD.Text) || string.IsNullOrWhiteSpace(txtSoyad.Text) || 
               string.IsNullOrWhiteSpace(txtTC.Text) || string.IsNullOrWhiteSpace(txtTelefon.Text) || 
               string.IsNullOrWhiteSpace(txtYas.Text) || string.IsNullOrWhiteSpace(txtCinsiyet.Text) || 
               string.IsNullOrWhiteSpace(txtSikayet.Text) || txtDurum.SelectedValue == null || txtBolum.SelectedValue == null)
            {
                MessageBox.Show("Lütfen ilgili tüm alanları doldurunuz!", "Kayıt Başarısız", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Validate TC (Turkish ID) - should be 11 digits with algorithm validation
            if(!IsValidTurkishID(txtTC.Text))
            {
                MessageBox.Show("TC Kimlik No geçerli değil! Lütfen 11 haneli geçerli bir TC No giriniz.", "Geçersiz TC No", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Validate phone number
            if(txtTelefon.Text.Length < 10 || !txtTelefon.Text.All(c => char.IsDigit(c) || c == ' ' || c == '-' || c == '(' || c == ')'))
            {
                MessageBox.Show("Telefon numarası geçerli formatta olmalıdır!", "Geçersiz Telefon", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Validate age
            if(!int.TryParse(txtYas.Text, out int yas) || yas < 0 || yas > 150)
            {
                MessageBox.Show("Yaş 0-150 arasında geçerli bir sayı olmalıdır!", "Geçersiz Yaş", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Validate name fields (only letters and spaces)
            if(!txtAD.Text.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            {
                MessageBox.Show("Ad sadece harf ve boşluk içermelidir!", "Geçersiz Ad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if(!txtSoyad.Text.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            {
                MessageBox.Show("Soyad sadece harf ve boşluk içermelidir!", "Geçersiz Soyad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void kaydet()
        {
            try
            {
                SqlCommand kaydet = new SqlCommand("Kaydet", bgl.baglan());
                kaydet.CommandType = CommandType.StoredProcedure;
                kaydet.Parameters.AddWithValue("ad",txtAD.Text);
                kaydet.Parameters.AddWithValue("soyad", txtSoyad.Text);
                kaydet.Parameters.AddWithValue("tc", txtTC.Text);
                kaydet.Parameters.AddWithValue("tel", txtTelefon.Text);
                kaydet.Parameters.AddWithValue("yas", int.Parse(txtYas.Text.ToString()));
                kaydet.Parameters.AddWithValue("cinsiyet", txtCinsiyet.Text);
                kaydet.Parameters.AddWithValue("sikayet", txtSikayet.Text);
                kaydet.Parameters.AddWithValue("tarih", DateTime.Now);
                kaydet.Parameters.AddWithValue("durum", txtDurum.SelectedValue);
                kaydet.Parameters.AddWithValue("bolum", txtBolum.SelectedValue);
                if(lblEx.Text == "True")
                {
                    kaydet.Parameters.AddWithValue("ex", 1);
                }
                else
                {
                    kaydet.Parameters.AddWithValue("ex", 0);
                }
                kaydet.ExecuteNonQuery();
                
                // Log the action
                AuditLogger.LogPatientAction("Hasta Eklendi", $"Ad: {txtAD.Text} {txtSoyad.Text}, TC: {txtTC.Text}");
                
                MessageBox.Show("Kayıt başarıyla eklendi","Kayıt Başarılı",MessageBoxButtons.OK, MessageBoxIcon.Information);
                Listele();
                temizle(); // Clear form after successful save
            }
            catch(SqlException ex)
            {
                AuditLogger.LogPatientAction("Hasta Ekleme Hatası", $"TC: {txtTC.Text}, Hata: {ex.Message}");
                MessageBox.Show($"Veritabanı hatası: {ex.Message}", "Kayıt Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Beklenmeyen hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("Silinecek kayıt seçilmedi! Lütfen listeden bir kayıt seçin.", "Seçim Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            sil();
        }

        private void sil()
        {
            try
            {
                DialogResult dr = MessageBox.Show($"{txtID.Text} numaralı kayıt silinecek. Onaylıyor musunuz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    string patientInfo = $"ID: {txtID.Text}, Ad: {txtAD.Text} {txtSoyad.Text}, TC: {txtTC.Text}";
                    
                    SqlCommand sil = new SqlCommand("sil", bgl.baglan());
                    sil.CommandType = CommandType.StoredProcedure;
                    sil.Parameters.AddWithValue("id", int.Parse(txtID.Text));
                    sil.ExecuteNonQuery();
                    
                    // Log the action
                    AuditLogger.LogPatientAction("Hasta Silindi", patientInfo);
                    
                    MessageBox.Show("Kayıt başarıyla silindi", "Kayıt Silme Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Listele();
                    temizle(); // Clear form after successful deletion
                }
            }
            catch(SqlException ex)
            {
                AuditLogger.LogPatientAction("Hasta Silme Hatası", $"ID: {txtID.Text}, Hata: {ex.Message}");
                MessageBox.Show($"Veritabanı hatası: {ex.Message}", "Silme Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Beklenmeyen hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFormuTemizle_Click(object sender, EventArgs e)
        {
            temizle();
        }

        private void btnİstatistik_Click(object sender, EventArgs e)
        {
            frmIstatistik fr = new frmIstatistik();
            fr.Show();
        }

        private void btnRandevu_Click(object sender, EventArgs e)
        {
            frmRandevu fr = new frmRandevu();
            fr.Show();
        }

        private void btnYedekleme_Click(object sender, EventArgs e)
        {
            frmYedeklemevYonetim fr = new frmYedeklemevYonetim();
            fr.Show();
        }

        // New export functionality
        private void ExportToCSV()
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
                saveFileDialog.FileName = $"HastaBilgileri_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    System.Text.StringBuilder csv = new System.Text.StringBuilder();
                    
                    // Add headers
                    for (int i = 0; i < dataGridView1.Columns.Count; i++)
                    {
                        csv.Append(dataGridView1.Columns[i].HeaderText);
                        if (i < dataGridView1.Columns.Count - 1)
                            csv.Append(",");
                    }
                    csv.AppendLine();
                    
                    // Add data
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            for (int i = 0; i < dataGridView1.Columns.Count; i++)
                            {
                                csv.Append(row.Cells[i].Value?.ToString() ?? "");
                                if (i < dataGridView1.Columns.Count - 1)
                                    csv.Append(",");
                            }
                            csv.AppendLine();
                        }
                    }
                    
                    System.IO.File.WriteAllText(saveFileDialog.FileName, csv.ToString(), System.Text.Encoding.UTF8);
                    MessageBox.Show("Veriler başarıyla dışa aktarıldı!", "Dışa Aktarma Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Dışa aktarma hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Quick data validation method
        private bool IsValidTurkishID(string tcNo)
        {
            if (string.IsNullOrEmpty(tcNo) || tcNo.Length != 11 || !tcNo.All(char.IsDigit))
                return false;

            int[] digits = tcNo.Select(c => int.Parse(c.ToString())).ToArray();
            
            // Turkish ID validation algorithm
            int sum1 = 0, sum2 = 0;
            for (int i = 0; i < 10; i++)
            {
                if (i % 2 == 0)
                    sum1 += digits[i];
                else
                    sum2 += digits[i];
            }
            
            return (sum1 * 7 - sum2) % 10 == digits[9] && 
                   (sum1 + sum2 + digits[9]) % 10 == digits[10];
        }

        // Enhanced TC validation for save operations
        private void EnhancedValidateTC()
        {
            if (!string.IsNullOrEmpty(txtTC.Text) && !IsValidTurkishID(txtTC.Text))
            {
                MessageBox.Show("Geçersiz TC Kimlik No! Lütfen kontrol ediniz.", "TC Doğrulama", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Visual feedback methods for better user experience
        private Form loadingForm;
        
        private void ShowLoadingMessage(string message)
        {
            loadingForm = new Form()
            {
                FormBorderStyle = FormBorderStyle.None,
                BackColor = Color.FromArgb(240, 240, 240),
                Size = new Size(300, 100),
                StartPosition = FormStartPosition.CenterParent,
                TopMost = true
            };
            
            Label lblLoading = new Label()
            {
                Text = message,
                ForeColor = Color.FromArgb(64, 64, 64),
                Font = new Font("Segoe UI", 10F),
                AutoSize = false,
                Size = new Size(280, 80),
                Location = new Point(10, 10),
                TextAlign = ContentAlignment.MiddleCenter
            };
            
            loadingForm.Controls.Add(lblLoading);
            loadingForm.Show(this);
            Application.DoEvents();
        }
        
        private void HideLoadingMessage()
        {
            loadingForm?.Close();
            loadingForm?.Dispose();
            loadingForm = null;
        }
        
        private void ShowSuccessMessage(string message, string title)
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
                    BackColor = Color.FromArgb(76, 175, 80),
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
        
        private void ShowErrorMessage(string message, string title)
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
