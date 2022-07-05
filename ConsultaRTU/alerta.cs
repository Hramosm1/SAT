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
    public partial class alerta : Form
    {
        
        public alerta( string mensaje)
        {
            InitializeComponent();
            lbMensaje.Text = mensaje;
        }
        public static void ErrorMensaje(string mensaje)
        {
            alerta frm = new alerta(mensaje);
            frm.ShowDialog();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }
    }
}
