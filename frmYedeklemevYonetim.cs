using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hastaTakipSistemi
{
    public partial class frmYedeklemevYonetim : Form
    {
        public frmYedeklemevYonetim()
        {
            InitializeComponent();
        }

        frmSqlBaglanti bgl = new frmSqlBaglanti();

        private void frmYedeklemevYonetim_Load(object sender, EventArgs e)
        {
            try
            {
                // Set default backup location
                txtBackupPath.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "HastaTakipYedek");
                
                // Load backup history
                LoadBackupHistory();
                
                // Show database connection status
                UpdateConnectionStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Form yükleme hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadBackupHistory()
        {
            try
            {
                listBoxBackups.Items.Clear();
                string backupFolder = Path.GetDirectoryName(txtBackupPath.Text);
                
                if (Directory.Exists(backupFolder))
                {
                    string[] backupFiles = Directory.GetFiles(backupFolder, "*.sql")
                        .OrderByDescending(f => File.GetCreationTime(f))
                        .ToArray();
                    
                    foreach (string file in backupFiles)
                    {
                        FileInfo fi = new FileInfo(file);
                        string displayText = $"{fi.Name} - {fi.CreationTime:dd/MM/yyyy HH:mm} ({GetFileSizeString(fi.Length)})";
                        listBoxBackups.Items.Add(displayText);
                        listBoxBackups.Tag = file; // Store full path
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Yedek geçmişi yüklenirken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private string GetFileSizeString(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }

        private void UpdateConnectionStatus()
        {
            try
            {
                if (bgl.BaglantiTest())
                {
                    lblConnectionStatus.Text = "Bağlantı: Başarılı ✓";
                    lblConnectionStatus.ForeColor = Color.Green;
                }
                else
                {
                    lblConnectionStatus.Text = "Bağlantı: Başarısız ✗";
                    lblConnectionStatus.ForeColor = Color.Red;
                }
            }
            catch
            {
                lblConnectionStatus.Text = "Bağlantı: Bilinmiyor";
                lblConnectionStatus.ForeColor = Color.Orange;
            }
        }

        private void btnSelectPath_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Yedek dosyası konumunu seçin";
                fbd.SelectedPath = Path.GetDirectoryName(txtBackupPath.Text);
                
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtBackupPath.Text = Path.Combine(fbd.SelectedPath, $"HastaTakip_Yedek_{DateTime.Now:yyyyMMdd_HHmmss}.sql");
                    LoadBackupHistory();
                }
            }
        }

        private void btnCreateBackup_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBackupPath.Text))
            {
                MessageBox.Show("Lütfen yedek dosyası konumunu belirtin!", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                CreateDatabaseBackup();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Yedekleme hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateDatabaseBackup()
        {
            try
            {
                // Ensure directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(txtBackupPath.Text));
                
                // Create a simple SQL script backup
                StringBuilder backup = new StringBuilder();
                backup.AppendLine("-- Hasta Takip Sistemi Yedek Dosyası");
                backup.AppendLine($"-- Oluşturulma Tarihi: {DateTime.Now}");
                backup.AppendLine("-- Bu dosya manuel olarak SQL Server'da çalıştırılmalıdır");
                backup.AppendLine();
                
                // Backup user accounts
                backup.AppendLine("-- Kullanıcı Hesapları");
                backup.AppendLine("DELETE FROM Kullanicilar;");
                BackupTable("Kullanicilar", backup);
                
                // Backup patient data
                backup.AppendLine("-- Hasta Bilgileri");
                backup.AppendLine("DELETE FROM HastaBilgi;");
                BackupTable("HastaBilgi", backup);
                
                // Backup appointments if table exists
                try
                {
                    backup.AppendLine("-- Randevu Bilgileri");
                    backup.AppendLine("DELETE FROM Randevu;");
                    BackupTable("Randevu", backup);
                }
                catch
                {
                    backup.AppendLine("-- Randevu tablosu bulunamadı");
                }
                
                File.WriteAllText(txtBackupPath.Text, backup.ToString(), Encoding.UTF8);
                
                MessageBox.Show($"Yedekleme başarıyla tamamlandı!\nDosya: {txtBackupPath.Text}", 
                    "Yedekleme Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                LoadBackupHistory();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Yedekleme sırasında hata: {ex.Message}", "Yedekleme Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BackupTable(string tableName, StringBuilder backup)
        {
            try
            {
                SqlCommand cmd = new SqlCommand($"SELECT * FROM {tableName}", bgl.baglan());
                SqlDataReader reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
                    backup.Append($"INSERT INTO {tableName} VALUES (");
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        if (i > 0) backup.Append(", ");
                        
                        object value = reader[i];
                        if (value == null || value == DBNull.Value)
                        {
                            backup.Append("NULL");
                        }
                        else if (value is string || value is DateTime)
                        {
                            backup.Append($"'{value.ToString().Replace("'", "''")}'");
                        }
                        else if (value is bool)
                        {
                            backup.Append(((bool)value) ? "1" : "0");
                        }
                        else
                        {
                            backup.Append(value.ToString());
                        }
                    }
                    backup.AppendLine(");");
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                backup.AppendLine($"-- Tablo {tableName} yedeklenirken hata: {ex.Message}");
            }
        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                UpdateConnectionStatus();
                
                if (bgl.BaglantiTest())
                {
                    // Get database info
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM HastaBilgi", bgl.baglan());
                    int patientCount = (int)cmd.ExecuteScalar();
                    
                    cmd = new SqlCommand("SELECT COUNT(*) FROM Kullanicilar", bgl.baglan());
                    int userCount = (int)cmd.ExecuteScalar();
                    
                    MessageBox.Show($"Bağlantı başarılı!\n\nVeri Özeti:\n- Hasta Sayısı: {patientCount}\n- Kullanıcı Sayısı: {userCount}", 
                        "Bağlantı Testi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Veritabanı bağlantısı kurulamadı!", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bağlantı testi hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void btnCleanOldBackups_Click(object sender, EventArgs e)
        {
            try
            {
                string backupFolder = Path.GetDirectoryName(txtBackupPath.Text);
                if (!Directory.Exists(backupFolder))
                {
                    MessageBox.Show("Yedek klasörü bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                DateTime cutoffDate = DateTime.Now.AddDays(-30); // Keep backups for 30 days
                string[] oldFiles = Directory.GetFiles(backupFolder, "*.sql")
                    .Where(f => File.GetCreationTime(f) < cutoffDate)
                    .ToArray();
                
                if (oldFiles.Length == 0)
                {
                    MessageBox.Show("Silinecek eski yedek dosyası bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                DialogResult result = MessageBox.Show($"{oldFiles.Length} adet 30 günden eski yedek dosyası bulundu. Silmek istiyor musunuz?", 
                    "Eski Dosyaları Temizle", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    int deletedCount = 0;
                    foreach (string file in oldFiles)
                    {
                        try
                        {
                            File.Delete(file);
                            deletedCount++;
                        }
                        catch
                        {
                            // Ignore individual file deletion errors
                        }
                    }
                    
                    MessageBox.Show($"{deletedCount} dosya başarıyla silindi.", "Temizlik Tamamlandı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadBackupHistory();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Dosya temizleme hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOpenBackupFolder_Click(object sender, EventArgs e)
        {
            try
            {
                string folder = Path.GetDirectoryName(txtBackupPath.Text);
                if (Directory.Exists(folder))
                {
                    System.Diagnostics.Process.Start("explorer.exe", folder);
                }
                else
                {
                    MessageBox.Show("Yedek klasörü bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Klasör açma hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}