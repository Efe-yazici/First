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
    public partial class frmIstatistik : Form
    {
        public frmIstatistik()
        {
            InitializeComponent();
        }
        frmSqlBaglanti bgl = new frmSqlBaglanti();
        private void frmİstatistik_Load(object sender, EventArgs e)
        {
            try
            {
                toplamHasta();
                ortalamaYas();
                sayiErkek();
                sayiKadin();
                sayiEx();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"İstatistik yükleme hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void toplamHasta()
        {
            try
            {
                SqlCommand toplam = new SqlCommand("SELECT COUNT(*) FROM HastaBilgi", bgl.baglan());
                SqlDataReader dr = toplam.ExecuteReader();
                while (dr.Read())
                {
                    lblToplamHasta.Text = dr[0].ToString();
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                lblToplamHasta.Text = "Hata";
                System.Diagnostics.Debug.WriteLine($"Toplam hasta hatası: {ex.Message}");
            }
        }

        private void ortalamaYas()
        {
            try
            {
                SqlCommand ortalama = new SqlCommand("SELECT CAST(AVG(CAST(Yas AS FLOAT)) AS DECIMAL(5,2)) FROM HastaBilgi", bgl.baglan());
                SqlDataReader dr = ortalama.ExecuteReader();
                while (dr.Read())
                {
                    lblYasOrtalama.Text = dr[0].ToString();
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                lblYasOrtalama.Text = "Hata";
                System.Diagnostics.Debug.WriteLine($"Ortalama yaş hatası: {ex.Message}");
            }
        }

        private void sayiErkek()
        {
            try
            {
                SqlCommand sayiE = new SqlCommand("select count(*) from HastaBilgi where Cinsiyet = 'Erkek' ;", bgl.baglan());
                SqlDataReader dr = sayiE.ExecuteReader();
                while (dr.Read())
                {
                    lblErkekSayi.Text = dr[0].ToString();
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                lblErkekSayi.Text = "Hata";
                System.Diagnostics.Debug.WriteLine($"Erkek sayısı hatası: {ex.Message}");
            }
        }

        private void sayiKadin()
        {
            try
            {
                SqlCommand sayiK = new SqlCommand("select count(*) from HastaBilgi where Cinsiyet = 'Kız' ;", bgl.baglan());
                SqlDataReader dr = sayiK.ExecuteReader();
                while (dr.Read())
                {
                    lblKadınSayi.Text = dr[0].ToString();
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                lblKadınSayi.Text = "Hata";
                System.Diagnostics.Debug.WriteLine($"Kadın sayısı hatası: {ex.Message}");
            }
        }

        private void sayiEx()
        {
            try
            {
                SqlCommand sayiEx = new SqlCommand("select count(*) from HastaBilgi where HastaYasiyorMi = 'False' ;", bgl.baglan());
                SqlDataReader dr = sayiEx.ExecuteReader();
                while (dr.Read())
                {
                    lblExSayi.Text = dr[0].ToString();
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                lblExSayi.Text = "Hata";
                System.Diagnostics.Debug.WriteLine($"Ex hasta sayısı hatası: {ex.Message}");
            }
        }
    }
}
