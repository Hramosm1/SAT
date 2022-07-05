using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsultaRTU
{
    public partial class SAT : Form
    {
        public SAT()
        {
            InitializeComponent();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnConsultarNit_Click(object sender, EventArgs e)
        {
            AbrirFormulariosEnWrapper(new FrmDPINIT());
        }
        private Form formActivo = null;
        private void AbrirFormulariosEnWrapper(Form FormHijo)
        {
            if (formActivo != null)
                formActivo.Close();
            formActivo = FormHijo;
            FormHijo.TopLevel = false;
            FormHijo.Dock = DockStyle.Fill;
            Wrapper.Controls.Add(FormHijo);
            Wrapper.Tag = FormHijo;
            FormHijo.BringToFront();
            FormHijo.Show();

        }

        private void btnConsultarRTU_Click(object sender, EventArgs e)
        {
            AbrirFormulariosEnWrapper(new FrmRTU());
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            AbrirFormulariosEnWrapper(new FrmRTUConsulta());
        }
    }
}
