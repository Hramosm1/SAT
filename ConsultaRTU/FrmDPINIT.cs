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
    public partial class FrmDPINIT : Form
    {
        List<E_PERSONA> lst = new List<E_PERSONA>();

        public FrmDPINIT()
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
                            E_PERSONA persona = new E_PERSONA();
                            persona.dpi = sl.GetCellValueAsInt64(celda, 1);
                            lst.Add(persona);
                            celda++;
                        }
                        dataGrid.DataSource = lst;


                    }
                }
                if (lst.Count > 0)
                {
                    this.btnProcesar.Enabled = true;
                    frmExitos.ErrorMensaje("Archvio Cargado con Exito");
                }
            }
            catch (Exception ex)
            {

            }
        }


        public void Procesar()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("no-sandbox");
            options.AddArguments("--disable-extensions"); // to disable extension
            options.AddArguments("--disable-notifications"); // to disable notification
            options.AddArguments("--disable-application-cache"); // to disable cache
             
            // options.AddArgument("headless");
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = false;
            // string workingDirectory = Environment.CurrentDirectory;
            //string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;

            IWebDriver driver = new ChromeDriver(service, options, TimeSpan.FromMinutes(5));
            E_PERSONA existe = new E_PERSONA();
            this.btnDescargar.Enabled = false;

            try
            {
                // driver.Manage().Timeouts().PageLoad.Add(TimeSpan.FromSeconds(190));
                driver.Navigate().GoToUrl("https://cdn.c.sat.gob.gt/rtu/consulta-cui-nit");
                Boolean bandera = true;
                Boolean bandera2 = true;
                IWebElement boton;
                IWebElement boton2;

                while (bandera)
                {
                    Thread.Sleep(500);
                    if (driver.PageSource.Contains("Ingresar el Código Único de Identificación CUI"))
                    {
                        bandera = false;
                    }
                }

                foreach (var persona in lst)
                {

                    DBConection conexion = new DBConection();
                    existe = conexion.Existe(persona);
                    if (existe.dpi != 0 && existe.nombre != "Esta consulta no contiene datos")
                    {
                        persona.nit = existe.nit;
                        persona.nombre = existe.nombre;
                        persona.fecha = existe.fecha;
                        persona.fechacreacion = existe.fechacreacion;
                        persona.notificacion2 = existe.notificacion2;
                    }
                    else
                    {
                       
                        bandera2 = true;
                        while (bandera2)
                        {
                            if (driver.PageSource.Contains("Ingresar el Código Único de Identificación CUI"))
                            {
                                bandera2 = false;
                            }
                            Thread.Sleep(500);
                            
                        }
                        
                        var dpi = driver.FindElement(By.XPath("/html/body/app-my-app/app-consulta-cui-nit/div/div/div/div/div/div/div[3]/div[1]/form/div/div/mat-form-field/div/div[1]/div/input"));
                        dpi.SendKeys(persona.dpi.ToString());

                        boton = driver.FindElement(By.XPath("/html/body/app-my-app/app-consulta-cui-nit/div/div/div/div/div/div/div[3]/div[1]/form/button[1]"));
                        boton.Submit();
                       

                        bandera2 = true;
                        int contador;
                        contador = 0;
                        
                        while (bandera2)
                        {
                            contador += 1;
                            if (contador > 99)
                            {
                                bandera2 = false;
                               
                            }
                            if (driver.PageSource.Contains("Esta consulta no contiene datos") || driver.PageSource.Contains("CUI:"))
                            {
                                bandera2 = false;
                            }
                        }

                        if (driver.PageSource.Contains("Esta consulta no contiene datos"))
                        {
                            persona.nombre = "Esta consulta no contiene datos";
                        }
                        else if (persona.nombre != "Esta consulta no contiene datos")
                        {
                            persona.nit = driver.FindElement(By.XPath("/html/body/app-my-app/app-consulta-cui-nit/div/div/div[2]/div/div/div[2]/div/div[2]/div[2]")).Text;
                            persona.nombre = driver.FindElement(By.XPath("/html/body/app-my-app/app-consulta-cui-nit/div/div/div[2]/div/div/div[2]/div/div[3]/div[2]")).Text;
                            string wtel = driver.FindElement(By.XPath("/html/body/app-my-app/app-consulta-cui-nit/div/div/div[2]/div/div/div[2]/div/div[4]/div[2]")).Text;
                            persona.fecha = Convert.ToDateTime(wtel);
                            persona.notificacion2 = driver.FindElement(By.XPath("/html/body/app-my-app/app-consulta-cui-nit/div/div/mat-card/div/table/tbody/tr/td[2]/mat-card-content[2]")).Text;
                            string FechaCreacion = driver.FindElement(By.XPath("/html/body/app-my-app/app-consulta-cui-nit/div/div/div[2]/div/div/div[2]/div/div[6]/div[2]")).Text;
                            persona.fechacreacion = Convert.ToDateTime(FechaCreacion);

                            conexion.Agregar(persona);
                        }
                        boton2 = driver.FindElement(By.XPath("/html/body/app-my-app/app-consulta-cui-nit/div/div/div[1]/div/div/div/div[3]/div[1]/form/button[2]"));

                        
                        boton2.Submit();

                        bandera2 = true;
                        while (bandera2)
                        {
                            if (driver.PageSource.Contains("Ingresar el Código Único de Identificación CUI"))
                            {
                                bandera2 = false;
                            }
                        }
                        
                    }
                }


                //driver.Close();
                //driver.Quit();
                dataGrid.DataSource = lst;
                this.btnDescargar.Enabled = true;
                frmExitos.ErrorMensaje("Archvio Cargado con Exito");
            }
            catch (Exception ex)
            {
                driver.Close();
                driver.Quit();
                dataGrid.DataSource = lst;
                frmFallido.ErrorMensaje("Peticiones bloqueadas");
                this.btnDescargar.Enabled = true;

            }

        }
        
        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            Procesar();
            
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            try
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
                        dt.Columns.Add("DPI");
                        dt.Columns.Add("NIT");
                        dt.Columns.Add("NOMBRE");
                        dt.Columns.Add("FECHANACIMIENTO");
                        dt.Columns.Add("FECHACREACION");
                        dt.Columns.Add("NOTIFICACION2");
                        foreach (var dato in lst)
                        {
                            DataRow dr = dt.NewRow();
                            dr["DPI"] = dato.dpi;
                            dr["NIT"] = dato.nit;
                            dr["NOMBRE"] = dato.nombre;
                            dr["FECHANACIMIENTO"] = dato.fecha.ToString("dd/MM/yyyy");
                            dr["FECHACREACION"] = dato.fechacreacion.ToString("dd/MM/yyyy");
                            dr["NOTIFICACION2"] = dato.notificacion2;

                            dt.Rows.Add(dr);
                        }
                        sl.ImportDataTable(1, 1, dt, true);
                        sl.SaveAs(csv.FileName);
                        frmExitos.ErrorMensaje("Descarga Realizada con Exito");
                    }
                }

            }
            catch
            {
                frmExitos.ErrorMensaje("Descarga Realizada con Exito");
            }
            
        }

  
    }
}
