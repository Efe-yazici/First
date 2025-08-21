using System;
using System.Windows.Forms;

namespace hastaTakipSistemi
{
    public partial class frmStatusSelect : Form
    {
        public string SelectedStatus { get; private set; }

        public frmStatusSelect(string[] options, string currentStatus)
        {
            InitializeComponent();
            foreach (string option in options)
            {
                cmbStatus.Items.Add(option);
            }
            cmbStatus.SelectedItem = currentStatus;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (cmbStatus.SelectedItem != null)
            {
                SelectedStatus = cmbStatus.SelectedItem.ToString();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Lütfen bir durum seçiniz!", "Seçim Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}