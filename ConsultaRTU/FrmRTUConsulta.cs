using ConsultaRTU.BaseDatos;
using ConsultaRTU.Entidades;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebDriverManager.DriverConfigs.Impl;

namespace ConsultaRTU
{
    public partial class FrmRTUConsulta : Form
    {
        List<E_PERSONAS_RTU> lst = new List<E_PERSONAS_RTU>();
        List<e_caracteristicas> cara = new List<e_caracteristicas>();
        List<e_afiliacion> IVA = new List<e_afiliacion>();
        List<e_isr> ISR = new List<e_isr>();
        DBConection conexion = new DBConection();
        public FrmRTUConsulta()
        {
            InitializeComponent();
        }

        private void btnConsultaLocal_Click(object sender, EventArgs e)
        {
            try
            {
                lst.Clear();
                dataGrid.DataSource = null;


                OpenFileDialog abrir = new OpenFileDialog();
                abrir.Filter = "Excel Files |*.xlsx";
                abrir.Title = "Seleccione el archivo de Excel";
                if (abrir.ShowDialog() == DialogResult.OK)
                {

                    if (abrir.FileName.Equals("") == false)
                    {


                        SLDocument sl = new SLDocument(abrir.FileName);
                        int celda = 2;

                        while (!string.IsNullOrEmpty(sl.GetCellValueAsString(celda, 1)))
                        {
                            E_PERSONAS_RTU persona = new E_PERSONAS_RTU();
                            persona.nit = sl.GetCellValueAsString(celda, 1);
                            lst.Add(persona);
                            celda++;
                        }
                        dataGrid.DataSource = lst;


                    }
                }
                if (lst.Count > 0)
                {
                    this.btnDescargar.Enabled = true;
                    Procesar();
                    frmExitos.ErrorMensaje("Archvio Cargado con Exito");
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public Boolean FindElementIfExists(IWebDriver driver, string by)
        {
            try
            {
                var elements = driver.FindElement(By.XPath(by));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void Procesar()
        {
            ChromeOptions options = new ChromeOptions();
            E_PERSONAS_RTU rpersona = new E_PERSONAS_RTU();
           
            try
            {
                foreach (var persona in lst)
                {
                    var per = conexion.cargar(persona);

                    if (per.dpi != null)
                    {
                        persona.dpi = per.dpi;
                        persona.primer_nombre = per.primer_nombre;
                        persona.segundo_nombre = per.segundo_nombre;
                        persona.primer_apellido  = per.primer_apellido;
                        persona.segundo_apellido = per.segundo_apellido;
                        persona.apellido_casada = per.apellido_casada;
                        persona.nombre =  per.primer_nombre + " " + per.segundo_nombre + " " + per.primer_apellido + " " + per.segundo_apellido + " " + per.apellido_casada;
                        persona.cedula = per.cedula;
                        persona.fecha = per.fecha;
                        persona.sexo = per.sexo;
                        persona.nacionalidad = per.nacionalidad;
                        persona.estado_civil = per.estado_civil;
                        persona.nombre_comercial = per.nombre_comercial;
                        persona.numero_establecimiento = per.numero_establecimiento;
                        persona.estado_establecimiento = per.estado_establecimiento;
                        persona.ultima_actualizacion = per.ultima_actualizacion;

                        var CAR = conexion.cargar_caracteristicas(persona);
                        //carga las caracteristicas guaradas en base de datos
                        foreach (var a in CAR)
                        {
                            cara.Add(a);
                        }
                        var iva = conexion.cargar_iva(persona);
                        //carga las caracteristicas guaradas en base de datos
                        foreach (var a in iva)
                        {
                            IVA.Add(a);
                        }
                        var isr = conexion.cargar_ISR(persona);
                        //carga las caracteristicas guaradas en base de datos
                        foreach (var a in isr)
                        {
                            ISR.Add(a);
                        }
                    }
                    else {
                        persona.nombre = "No Existen datos en la Base de Datos";
                    }
                }
                      dataGrid.DataSource = lst;
            }
            catch (Exception ex)
            {
                
            }
        }
       
        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            Procesar();
                foreach (var persona in lst)
                {
                        if (persona.primer_nombre.Length > 0)
                        {
                    conexion.Agregar_RTU(persona);
                        }
                }
                if (lst.Count > 0)
                {
                    btnDescargar.Enabled = true;
                    frmExitos.ErrorMensaje("Archivo Procesado con Exito");
                }
            //this.btnDescargar.Enabled = true;
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            if (lst.Count > 0)
            {

                SaveFileDialog csv = new SaveFileDialog();
                // saveFileDialog1.Filter = "Excel Files |*.xlsx";
                csv.Filter = "Excel Files |*.xlsx";
                csv.Title = "Seleccione en donde quiere guardar su archivo";
                if (csv.ShowDialog() == DialogResult.OK)
                {
                    SLDocument sl = new SLDocument();
                    DataTable dt = new DataTable();
                    DataTable dtISR = new DataTable();
                    DataTable dtCaracteristicas = new DataTable();
                    DataTable dtIVA = new DataTable();

                    dtIVA.Columns.Add("NIT");
                    dtIVA.Columns.Add("CODIGO_IMPUESTO");
                    dtIVA.Columns.Add("NOMBRE_IMPUESTO");
                    dtIVA.Columns.Add("TIPO_CONTRIBUYENTE");
                    dtIVA.Columns.Add("CLASIFICACION");
                    dtIVA.Columns.Add("REGIMEN");
                    dtIVA.Columns.Add("PERIODO");
                    dtIVA.Columns.Add("ESTATUS");
                    dtIVA.Columns.Add("FECHA");

                    //columnas de ISR
                    dtISR.Columns.Add("NIT");
                    dtISR.Columns.Add("CODIGO_IMPUESTO");
                    dtISR.Columns.Add("NOMBRE");
                    dtISR.Columns.Add("TIPO_CONTRIBUYENTE");
                    dtISR.Columns.Add("TIPO_RENTA");
                    dtISR.Columns.Add("REGIMEN");
                    dtISR.Columns.Add("FORMA_CALCULO");
                    dtISR.Columns.Add("SISTEMA_INVENTARIOS");
                    dtISR.Columns.Add("SISTEMA_CONTABLE");
                    dtISR.Columns.Add("ESTATUS");
                    dtISR.Columns.Add("FECHA");

                    //Columnos de Caracteristicas
                    dtCaracteristicas.Columns.Add("NIT");
                    dtCaracteristicas.Columns.Add("CARACTERISTICA");
                    dtCaracteristicas.Columns.Add("ESTADO");
                    dtCaracteristicas.Columns.Add("FECHA_ESTATUS");

                    //Columnas Generales del RTU
                    dt.Columns.Add("NIT");
                    dt.Columns.Add("DPI");
                    dt.Columns.Add("NOMBRE_COMPLETO");
                    dt.Columns.Add("PRIMER_NOMBRE");
                    dt.Columns.Add("SEGUNDO_NOMBRE");
                    dt.Columns.Add("PRIMER_APELLIDO");
                    dt.Columns.Add("SEGUNDO_APELLIDO");
                    dt.Columns.Add("APELLIDO_CASADA");
                    dt.Columns.Add("FECHANACIMIENTO");
                    dt.Columns.Add("CEDULA");
                    dt.Columns.Add("NACIONALIDAD");
                    dt.Columns.Add("ESTADO_CIVIL");
                    dt.Columns.Add("NOMBRE_COMERCIAL");
                    dt.Columns.Add("NUMERO_ESTABLECIMIENTO");
                    dt.Columns.Add("ESTADO_ESTABLECIMIENTO");
                    dt.Columns.Add("ULTIMA_ACTUALIZACION");
                    foreach (var dato in lst)
                    {
                        DataRow dr = dt.NewRow();
                        dr["DPI"] = dato.dpi;
                        dr["NIT"] = dato.nit;
                        dr["NOMBRE_COMPLETO"] = dato.nombre;
                        dr["PRIMER_NOMBRE"] = dato.primer_nombre;
                        dr["SEGUNDO_NOMBRE"] = dato.segundo_nombre;
                        dr["PRIMER_APELLIDO"] = dato.primer_apellido;
                        dr["SEGUNDO_APELLIDO"] = dato.segundo_apellido;
                        dr["APELLIDO_CASADA"] = dato.apellido_casada;
                        dr["CEDULA"] = dato.cedula;
                        dr["NACIONALIDAD"] = dato.nacionalidad;
                        dr["ESTADO_CIVIL"] = dato.estado_civil;
                        dr["NOMBRE_COMERCIAL"] = dato.nombre_comercial;
                        dr["NUMERO_ESTABLECIMIENTO"] = dato.numero_establecimiento;
                        dr["ESTADO_ESTABLECIMIENTO"] = dato.estado_establecimiento;
                        dr["FECHANACIMIENTO"] = dato.fecha.ToString("dd/MM/yyyy");
                        dr["ULTIMA_ACTUALIZACION"] = dato.ultima_actualizacion.ToString("dd/MM/yyyy");
                        dt.Rows.Add(dr);
                    }


                    foreach (var a in ISR)
                    {
                        DataRow dr = dtISR.NewRow();
                        dr["NIT"] = a.NIT;
                        dr["CODIGO_IMPUESTO"] = a.CODIGO_IMPUESTO;
                        dr["NOMBRE"] = a.NOMBRE;
                        dr["TIPO_CONTRIBUYENTE"] = a.TIPO_CONTRIBUYENTE;
                        dr["TIPO_RENTA"] = a.TIPO_RENTA;
                        dr["REGIMEN"] = a.REGIMEN;
                        dr["FORMA_CALCULO"] = a.FORMA_CALCULO;
                        dr["SISTEMA_INVENTARIOS"] = a.SISTEMA_INVENTARIOS;
                        dr["SISTEMA_CONTABLE"] = a.SISTEMA_CONTABLE;
                        dr["ESTATUS"] = a.ESTATUS;
                        dr["FECHA"] = a.FECHA.ToString("dd/MM/yyyy");
                        dtISR.Rows.Add(dr);
                    }

                    foreach (var a in IVA)
                    {
                        DataRow dr = dtIVA.NewRow();
                        dr["NIT"] = a.nit;
                        dr["CODIGO_IMPUESTO"] = a.codigo_impuesto;
                        dr["NOMBRE_IMPUESTO"] = a.nombre_impuesto;
                        dr["TIPO_CONTRIBUYENTE"] = a.tipo_contribuyente;
                        dr["CLASIFICACION"] = a.clasificacion;
                        dr["REGIMEN"] = a.regimen;
                        dr["PERIODO"] = a.periodo;
                        dr["ESTATUS"] = a.estatus;
                        dr["FECHA"] = a.fecha.ToString("dd/MM/yyyy");
                        dtIVA.Rows.Add(dr);
                    }

                    foreach (var a in cara)
                    {
                        DataRow dr = dtCaracteristicas.NewRow();
                        dr["NIT"] = a.nit;
                        dr["CARACTERISTICA"] = a.caracteristica;
                        dr["ESTADO"] = a.estado;
                        dr["FECHA_ESTATUS"] = a.fecha_estatus.ToString("dd/MM/yyyy");
                        dtCaracteristicas.Rows.Add(dr);
                    }
                    
                    sl.AddWorksheet("RTU");
                    sl.ImportDataTable(1, 1, dt, true);
                    sl.AddWorksheet("AfiliacionIVA");
                    sl.ImportDataTable(1, 1, dtIVA, true);
                    sl.AddWorksheet("AfiliacionISR");
                    sl.ImportDataTable(1, 1, dtISR, true);
                    sl.AddWorksheet("Caracteristicas");
                    sl.ImportDataTable(1, 1, dtCaracteristicas, true);
                    sl.DeleteWorksheet("Sheet1");
                    sl.SaveAs(csv.FileName);
                    frmExitos.ErrorMensaje("Descarga Realizada con Exito");
                }
            }
        }
    }
}
