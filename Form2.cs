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
            toplamHasta();
            ortalamaYas();
            sayiErkek();
            sayiKadin();
            sayiEx();
        }
        private void toplamHasta()
        {
            SqlCommand toplam = new SqlCommand("SELECT COUNT(*) FROM HastaBilgi", bgl.baglan());
            SqlDataReader dr = toplam.ExecuteReader();
            while (dr.Read())
            {
                lblToplamHasta.Text = dr[0].ToString();
            }
        }

        private void ortalamaYas()
        {
            SqlCommand ortalama = new SqlCommand("SELECT AVG(Yas) FROM HastaBilgi", bgl.baglan());
            SqlDataReader dr = ortalama.ExecuteReader();
            while (dr.Read())
            {
                lblYasOrtalama.Text = dr[0].ToString();
            }
        }

        private void sayiErkek()
        {
            SqlCommand sayiE = new SqlCommand("select count(*) from HastaBilgi where Cinsiyet = 'Erkek' ;", bgl.baglan());
            SqlDataReader dr = sayiE.ExecuteReader();
            while (dr.Read())
            {
                lblErkekSayi.Text = dr[0].ToString();
            }
        }

        private void sayiKadin()
        {
            SqlCommand sayiK = new SqlCommand("select count(*) from HastaBilgi where Cinsiyet = 'Kız' ;", bgl.baglan());
            SqlDataReader dr = sayiK.ExecuteReader();
            while (dr.Read())
            {
                lblKadınSayi.Text = dr[0].ToString();
            }
        }

        private void sayiEx()
        {
            SqlCommand sayiEx = new SqlCommand("select count(*) from HastaBilgi where HastaYasiyorMi = 'False' ;", bgl.baglan());
            SqlDataReader dr = sayiEx.ExecuteReader();
            while (dr.Read())
            {
                lblExSayi.Text = dr[0].ToString();
            }
        }
    }
}
