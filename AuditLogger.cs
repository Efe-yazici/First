using System;
using System.Data.SqlClient;

namespace hastaTakipSistemi
{
    public static class AuditLogger
    {
        private static frmSqlBaglanti bgl = new frmSqlBaglanti();

        public static void LogAction(string kullaniciAdi, string islem, string detay, string tabloAdi = "")
        {
            try
            {
                // Create audit table if it doesn't exist
                CreateAuditTableIfNotExists();

                string query = @"
                    INSERT INTO AuditLog (KullaniciAdi, Islem, Detay, TabloAdi, Tarih)
                    VALUES (@KullaniciAdi, @Islem, @Detay, @TabloAdi, @Tarih)";

                SqlCommand cmd = new SqlCommand(query, bgl.baglan());
                cmd.Parameters.AddWithValue("@KullaniciAdi", kullaniciAdi ?? "Bilinmiyor");
                cmd.Parameters.AddWithValue("@Islem", islem);
                cmd.Parameters.AddWithValue("@Detay", detay);
                cmd.Parameters.AddWithValue("@TabloAdi", tabloAdi ?? "");
                cmd.Parameters.AddWithValue("@Tarih", DateTime.Now);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // Log error to system debug if audit logging fails
                System.Diagnostics.Debug.WriteLine($"Audit logging failed: {ex.Message}");
            }
        }

        private static void CreateAuditTableIfNotExists()
        {
            try
            {
                string createTableQuery = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='AuditLog' AND xtype='U')
                    CREATE TABLE AuditLog (
                        LogID int IDENTITY(1,1) PRIMARY KEY,
                        KullaniciAdi nvarchar(50),
                        Islem nvarchar(100),
                        Detay nvarchar(max),
                        TabloAdi nvarchar(50),
                        Tarih datetime
                    )";

                SqlCommand cmd = new SqlCommand(createTableQuery, bgl.baglan());
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Audit table creation failed: {ex.Message}");
            }
        }

        public static void LogPatientAction(string action, string patientInfo)
        {
            LogAction("Sistem", action, patientInfo, "HastaBilgi");
        }

        public static void LogUserAction(string action, string userInfo)
        {
            LogAction("Sistem", action, userInfo, "Kullanicilar");
        }

        public static void LogAppointmentAction(string action, string appointmentInfo)
        {
            LogAction("Sistem", action, appointmentInfo, "Randevu");
        }

        public static void LogSystemAction(string action, string details)
        {
            LogAction("Sistem", action, details, "Sistem");
        }
    }
}