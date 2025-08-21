using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hastaTakipSistemi
{
    internal static class Program
    {
        /// <summary>
        /// Uygulamanın ana girdi noktası.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Show modern splash screen
            ShowSplashScreen();
            
            Application.Run(new Form1());
        }

        private static void ShowSplashScreen()
        {
            Form splashForm = new Form()
            {
                FormBorderStyle = FormBorderStyle.None,
                BackColor = Color.FromArgb(33, 150, 243),
                Size = new Size(400, 250),
                StartPosition = FormStartPosition.CenterScreen,
                TopMost = true
            };

            Label titleLabel = new Label()
            {
                Text = "Hasta Takip Sistemi",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                AutoSize = false,
                Size = new Size(380, 40),
                Location = new Point(10, 80),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label versionLabel = new Label()
            {
                Text = "Gelişmiş Versiyon v2.0",
                ForeColor = Color.FromArgb(200, 230, 255),
                Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                AutoSize = false,
                Size = new Size(380, 30),
                Location = new Point(10, 130),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label loadingLabel = new Label()
            {
                Text = "Yükleniyor...",
                ForeColor = Color.FromArgb(200, 230, 255),
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                AutoSize = false,
                Size = new Size(380, 30),
                Location = new Point(10, 200),
                TextAlign = ContentAlignment.MiddleCenter
            };

            splashForm.Controls.AddRange(new Control[] { titleLabel, versionLabel, loadingLabel });
            
            splashForm.Show();
            Application.DoEvents();
            
            // Simulate loading time
            System.Threading.Thread.Sleep(2000);
            
            splashForm.Close();
            splashForm.Dispose();
        }
    }
}
