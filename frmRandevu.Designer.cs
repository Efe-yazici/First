namespace hastaTakipSistemi
{
    partial class frmRandevu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnRandevuKaydet = new System.Windows.Forms.Button();
            this.txtNotlar = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpRandevuSaati = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpRandevuTarihi = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbDoktor = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbHasta = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnBugunkuRandevular = new System.Windows.Forms.Button();
            this.btnRandevuGuncelle = new System.Windows.Forms.Button();
            this.btnRandevuSil = new System.Windows.Forms.Button();
            this.dgvRandevular = new System.Windows.Forms.DataGridView();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRandevular)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnRandevuKaydet);
            this.groupBox1.Controls.Add(this.txtNotlar);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.dtpRandevuSaati);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.dtpRandevuTarihi);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cmbDoktor);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmbHasta);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(400, 300);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Yeni Randevu";
            // 
            // btnRandevuKaydet
            // 
            this.btnRandevuKaydet.Location = new System.Drawing.Point(150, 260);
            this.btnRandevuKaydet.Name = "btnRandevuKaydet";
            this.btnRandevuKaydet.Size = new System.Drawing.Size(100, 30);
            this.btnRandevuKaydet.TabIndex = 10;
            this.btnRandevuKaydet.Text = "Kaydet";
            this.btnRandevuKaydet.UseVisualStyleBackColor = true;
            this.btnRandevuKaydet.Click += new System.EventHandler(this.btnRandevuKaydet_Click);
            // 
            // txtNotlar
            // 
            this.txtNotlar.Location = new System.Drawing.Point(100, 180);
            this.txtNotlar.Multiline = true;
            this.txtNotlar.Name = "txtNotlar";
            this.txtNotlar.Size = new System.Drawing.Size(280, 70);
            this.txtNotlar.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 180);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Notlar:";
            // 
            // dtpRandevuSaati
            // 
            this.dtpRandevuSaati.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpRandevuSaati.Location = new System.Drawing.Point(100, 140);
            this.dtpRandevuSaati.Name = "dtpRandevuSaati";
            this.dtpRandevuSaati.ShowUpDown = true;
            this.dtpRandevuSaati.Size = new System.Drawing.Size(280, 20);
            this.dtpRandevuSaati.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 140);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Saat:";
            // 
            // dtpRandevuTarihi
            // 
            this.dtpRandevuTarihi.Location = new System.Drawing.Point(100, 100);
            this.dtpRandevuTarihi.Name = "dtpRandevuTarihi";
            this.dtpRandevuTarihi.Size = new System.Drawing.Size(280, 20);
            this.dtpRandevuTarihi.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Tarih:";
            // 
            // cmbDoktor
            // 
            this.cmbDoktor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDoktor.FormattingEnabled = true;
            this.cmbDoktor.Location = new System.Drawing.Point(100, 60);
            this.cmbDoktor.Name = "cmbDoktor";
            this.cmbDoktor.Size = new System.Drawing.Size(280, 21);
            this.cmbDoktor.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Bölüm:";
            // 
            // cmbHasta
            // 
            this.cmbHasta.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbHasta.FormattingEnabled = true;
            this.cmbHasta.Location = new System.Drawing.Point(100, 20);
            this.cmbHasta.Name = "cmbHasta";
            this.cmbHasta.Size = new System.Drawing.Size(280, 21);
            this.cmbHasta.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Hasta:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnBugunkuRandevular);
            this.groupBox2.Controls.Add(this.btnRandevuGuncelle);
            this.groupBox2.Controls.Add(this.btnRandevuSil);
            this.groupBox2.Controls.Add(this.dgvRandevular);
            this.groupBox2.Location = new System.Drawing.Point(430, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(650, 500);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Randevu Listesi";
            // 
            // btnBugunkuRandevular
            // 
            this.btnBugunkuRandevular.Location = new System.Drawing.Point(20, 460);
            this.btnBugunkuRandevular.Name = "btnBugunkuRandevular";
            this.btnBugunkuRandevular.Size = new System.Drawing.Size(120, 30);
            this.btnBugunkuRandevular.TabIndex = 3;
            this.btnBugunkuRandevular.Text = "Bugünkü Randevular";
            this.btnBugunkuRandevular.UseVisualStyleBackColor = true;
            this.btnBugunkuRandevular.Click += new System.EventHandler(this.btnBugunkuRandevular_Click);
            // 
            // btnRandevuGuncelle
            // 
            this.btnRandevuGuncelle.Location = new System.Drawing.Point(540, 460);
            this.btnRandevuGuncelle.Name = "btnRandevuGuncelle";
            this.btnRandevuGuncelle.Size = new System.Drawing.Size(100, 30);
            this.btnRandevuGuncelle.TabIndex = 2;
            this.btnRandevuGuncelle.Text = "Durum Güncelle";
            this.btnRandevuGuncelle.UseVisualStyleBackColor = true;
            this.btnRandevuGuncelle.Click += new System.EventHandler(this.btnRandevuGuncelle_Click);
            // 
            // btnRandevuSil
            // 
            this.btnRandevuSil.Location = new System.Drawing.Point(430, 460);
            this.btnRandevuSil.Name = "btnRandevuSil";
            this.btnRandevuSil.Size = new System.Drawing.Size(100, 30);
            this.btnRandevuSil.TabIndex = 1;
            this.btnRandevuSil.Text = "Sil";
            this.btnRandevuSil.UseVisualStyleBackColor = true;
            this.btnRandevuSil.Click += new System.EventHandler(this.btnRandevuSil_Click);
            // 
            // dgvRandevular
            // 
            this.dgvRandevular.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRandevular.Location = new System.Drawing.Point(20, 20);
            this.dgvRandevular.Name = "dgvRandevular";
            this.dgvRandevular.Size = new System.Drawing.Size(620, 430);
            this.dgvRandevular.TabIndex = 0;
            // 
            // frmRandevu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 530);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmRandevu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Randevu Yönetimi";
            this.Load += new System.EventHandler(this.frmRandevu_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRandevular)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnRandevuKaydet;
        private System.Windows.Forms.TextBox txtNotlar;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtpRandevuSaati;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpRandevuTarihi;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbDoktor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbHasta;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnBugunkuRandevular;
        private System.Windows.Forms.Button btnRandevuGuncelle;
        private System.Windows.Forms.Button btnRandevuSil;
        private System.Windows.Forms.DataGridView dgvRandevular;
    }
}