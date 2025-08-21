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

namespace hastaTakipSistemi
{
    public partial class frmRandevu : Form
    {
        public frmRandevu()
        {
            InitializeComponent();
        }

        frmSqlBaglanti bgl = new frmSqlBaglanti();

        private void frmRandevu_Load(object sender, EventArgs e)
        {
            try
            {
                LoadPatients();
                LoadDoctors();
                SetDefaultDateTime();
                LoadAppointments();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Form yükleme hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadPatients()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT HastaID, (Ad + ' ' + Soyad + ' - ' + TC) as HastaInfo FROM HastaBilgi WHERE HastaYasiyorMi = 'True'", bgl.baglan());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmbHasta.DataSource = dt;
                cmbHasta.DisplayMember = "HastaInfo";
                cmbHasta.ValueMember = "HastaID";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hasta listesi yüklenemedi: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadDoctors()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT BolumID, BolumAd FROM Bolum", bgl.baglan());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmbDoktor.DataSource = dt;
                cmbDoktor.DisplayMember = "BolumAd";
                cmbDoktor.ValueMember = "BolumID";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Doktor listesi yüklenemedi: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SetDefaultDateTime()
        {
            dtpRandevuTarihi.Value = DateTime.Now.AddDays(1); // Default to tomorrow
            dtpRandevuSaati.Value = DateTime.Today.AddHours(9); // Default to 9 AM
        }

        private void LoadAppointments()
        {
            try
            {
                string query = @"
                    SELECT 
                        r.RandevuID,
                        (h.Ad + ' ' + h.Soyad) as Hasta,
                        h.TC,
                        b.BolumAd as Bolum,
                        r.RandevuTarihi,
                        r.RandevuSaati,
                        r.Durum,
                        r.Notlar
                    FROM Randevu r
                    INNER JOIN HastaBilgi h ON r.HastaID = h.HastaID
                    INNER JOIN Bolum b ON r.BolumID = b.BolumID
                    ORDER BY r.RandevuTarihi, r.RandevuSaati";
                
                SqlCommand cmd = new SqlCommand(query, bgl.baglan());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvRandevular.DataSource = dt;
                
                FormatAppointmentGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Randevu listesi yüklenemedi: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FormatAppointmentGrid()
        {
            if (dgvRandevular.Columns.Count > 0)
            {
                dgvRandevular.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvRandevular.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvRandevular.MultiSelect = false;
                dgvRandevular.ReadOnly = true;
                
                // Format date time columns
                if (dgvRandevular.Columns["RandevuTarihi"] != null)
                    dgvRandevular.Columns["RandevuTarihi"].DefaultCellStyle.Format = "dd/MM/yyyy";
                
                if (dgvRandevular.Columns["RandevuSaati"] != null)
                    dgvRandevular.Columns["RandevuSaati"].DefaultCellStyle.Format = "HH:mm";
            }
        }

        private void btnRandevuKaydet_Click(object sender, EventArgs e)
        {
            if (ValidateAppointmentInput())
            {
                SaveAppointment();
            }
        }

        private bool ValidateAppointmentInput()
        {
            if (cmbHasta.SelectedValue == null)
            {
                MessageBox.Show("Lütfen bir hasta seçiniz!", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbDoktor.SelectedValue == null)
            {
                MessageBox.Show("Lütfen bir bölüm seçiniz!", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (dtpRandevuTarihi.Value.Date < DateTime.Today)
            {
                MessageBox.Show("Randevu tarihi bugünden önce olamaz!", "Geçersiz Tarih", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Check if appointment time is within working hours (8 AM - 6 PM)
            TimeSpan appointmentTime = dtpRandevuSaati.Value.TimeOfDay;
            if (appointmentTime < TimeSpan.FromHours(8) || appointmentTime > TimeSpan.FromHours(18))
            {
                MessageBox.Show("Randevu saati 08:00 - 18:00 arasında olmalıdır!", "Geçersiz Saat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void SaveAppointment()
        {
            try
            {
                string query = @"
                    INSERT INTO Randevu (HastaID, BolumID, RandevuTarihi, RandevuSaati, Durum, Notlar, OlusturmaTarihi)
                    VALUES (@HastaID, @BolumID, @RandevuTarihi, @RandevuSaati, @Durum, @Notlar, @OlusturmaTarihi)";

                SqlCommand cmd = new SqlCommand(query, bgl.baglan());
                cmd.Parameters.AddWithValue("@HastaID", cmbHasta.SelectedValue);
                cmd.Parameters.AddWithValue("@BolumID", cmbDoktor.SelectedValue);
                cmd.Parameters.AddWithValue("@RandevuTarihi", dtpRandevuTarihi.Value.Date);
                cmd.Parameters.AddWithValue("@RandevuSaati", dtpRandevuSaati.Value.TimeOfDay);
                cmd.Parameters.AddWithValue("@Durum", "Bekliyor");
                cmd.Parameters.AddWithValue("@Notlar", txtNotlar.Text);
                cmd.Parameters.AddWithValue("@OlusturmaTarihi", DateTime.Now);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Randevu başarıyla kaydedildi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                ClearForm();
                LoadAppointments();
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("duplicate") || ex.Message.Contains("UNIQUE"))
                {
                    MessageBox.Show("Bu tarih ve saatte zaten bir randevu bulunmaktadır!", "Çakışan Randevu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show($"Veritabanı hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Beklenmeyen hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearForm()
        {
            cmbHasta.SelectedIndex = -1;
            cmbDoktor.SelectedIndex = -1;
            txtNotlar.Clear();
            SetDefaultDateTime();
        }

        private void btnRandevuSil_Click(object sender, EventArgs e)
        {
            if (dgvRandevular.SelectedRows.Count > 0)
            {
                DeleteAppointment();
            }
            else
            {
                MessageBox.Show("Lütfen silinecek randevuyu seçiniz!", "Seçim Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DeleteAppointment()
        {
            try
            {
                int randevuID = Convert.ToInt32(dgvRandevular.SelectedRows[0].Cells["RandevuID"].Value);
                
                DialogResult result = MessageBox.Show("Seçili randevuyu silmek istediğinizden emin misiniz?", 
                    "Randevu Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM Randevu WHERE RandevuID = @RandevuID", bgl.baglan());
                    cmd.Parameters.AddWithValue("@RandevuID", randevuID);
                    cmd.ExecuteNonQuery();
                    
                    MessageBox.Show("Randevu başarıyla silindi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadAppointments();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Randevu silme hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRandevuGuncelle_Click(object sender, EventArgs e)
        {
            if (dgvRandevular.SelectedRows.Count > 0)
            {
                UpdateAppointmentStatus();
            }
            else
            {
                MessageBox.Show("Lütfen güncellenecek randevuyu seçiniz!", "Seçim Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void UpdateAppointmentStatus()
        {
            try
            {
                int randevuID = Convert.ToInt32(dgvRandevular.SelectedRows[0].Cells["RandevuID"].Value);
                string currentStatus = dgvRandevular.SelectedRows[0].Cells["Durum"].Value.ToString();
                
                // Create a simple status selection dialog
                string[] statusOptions = { "Bekliyor", "Tamamlandı", "İptal" };
                
                // Use a simple input dialog
                frmStatusSelect statusForm = new frmStatusSelect(statusOptions, currentStatus);
                if (statusForm.ShowDialog() == DialogResult.OK)
                {
                    string newStatus = statusForm.SelectedStatus;
                    
                    SqlCommand cmd = new SqlCommand("UPDATE Randevu SET Durum = @Durum WHERE RandevuID = @RandevuID", bgl.baglan());
                    cmd.Parameters.AddWithValue("@Durum", newStatus);
                    cmd.Parameters.AddWithValue("@RandevuID", randevuID);
                    cmd.ExecuteNonQuery();
                    
                    MessageBox.Show("Randevu durumu başarıyla güncellendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadAppointments();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Randevu güncelleme hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBugunkuRandevular_Click(object sender, EventArgs e)
        {
            LoadTodaysAppointments();
        }

        private void LoadTodaysAppointments()
        {
            try
            {
                string query = @"
                    SELECT 
                        r.RandevuID,
                        (h.Ad + ' ' + h.Soyad) as Hasta,
                        h.TC,
                        b.BolumAd as Bolum,
                        r.RandevuTarihi,
                        r.RandevuSaati,
                        r.Durum,
                        r.Notlar
                    FROM Randevu r
                    INNER JOIN HastaBilgi h ON r.HastaID = h.HastaID
                    INNER JOIN Bolum b ON r.BolumID = b.BolumID
                    WHERE CAST(r.RandevuTarihi AS DATE) = CAST(GETDATE() AS DATE)
                    ORDER BY r.RandevuSaati";
                
                SqlCommand cmd = new SqlCommand(query, bgl.baglan());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvRandevular.DataSource = dt;
                
                FormatAppointmentGrid();
                MessageBox.Show($"Bugün için {dt.Rows.Count} randevu bulundu.", "Bugünkü Randevular", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bugünkü randevular yüklenemedi: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}