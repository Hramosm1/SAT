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
using twoCaptcha;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Globalization;

namespace ConsultaRTU
{
    public partial class FrmRTU : Form
    {
        List<E_PERSONAS_RTU> lst = new List<E_PERSONAS_RTU>();
        List<e_caracteristicas> cara = new List<e_caracteristicas>();
        List<e_afiliacion> IVA = new List<e_afiliacion>();
        List<e_isr> ISR = new List<e_isr>();
        List<E_ISO> ISO = new List<E_ISO>();
        List<e_caracteristicas> Rescara = new List<e_caracteristicas>();
        List<e_afiliacion> ResIVA = new List<e_afiliacion>();
        List<e_isr> ResISR = new List<e_isr>();
        DBConection conexion = new DBConection();
        private string satToken;
        private DateTime tokenExpiration = DateTime.MinValue;

        public FrmRTU()
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
                    this.btnProcesar.Enabled = true;
                    frmExitos.ErrorMensaje("Archivo Cargado con Exito");
                }
            }
            catch (Exception ex)
            {
                frmExitos.ErrorMensaje(ex.ToString());
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
        private void GenerarToken(E_PERSONAS_RTU persona, IWebDriver driver)
        {
            driver.Navigate().GoToUrl("https://cdn.c.sat.gob.gt/rtu/constancia-rtu-portal?nit=" + persona.nit);
            driver.Manage().Timeouts().PageLoad.Add(TimeSpan.FromSeconds(190));
            Thread.Sleep(10000);

            var hcaptchaIframe = driver.FindElement(By.XPath("//iframe[contains(@src, 'hcaptcha.com')]"));
            string src = hcaptchaIframe.GetAttribute("src");
            Uri uri = new Uri(src);
            string fragment = uri.Fragment;
            var fragmentParams = System.Web.HttpUtility.ParseQueryString(fragment.TrimStart('#'));
            string siteKey = fragmentParams.Get("sitekey");
            string pageUrl = driver.Url;

            Solver solver = new Solver();
            string id = solver.postHcaptcha(siteKey, pageUrl);

            ((IJavaScriptExecutor)driver).ExecuteScript(
                "document.querySelector('[name=\"h-captcha-response\"]').style.display = 'inline';");

            string answer;
            while (true)
            {
                answer = solver.get(id.Substring(3));
                if (answer == "CAPCHA_NOT_READY")
                {
                    Thread.Sleep(20000);
                }
                else
                {
                    break;
                }
            }

            if (answer.StartsWith("ERROR_"))
            {
                driver.Quit(); // Saltar a repetir la consulta con el mismo nit para cambiar de captcha
            }
            else
            {
                IWebElement tokenField = driver.FindElement(By.CssSelector("[name='h-captcha-response']"));
                tokenField.SendKeys(answer.Substring(3));
                string captchaToken = answer.Substring(3);

                using (var client = new HttpClient())
                {
                    // SAT API para obtener el access_token
                    string tokenUrl = $"https://svc.c.sat.gob.gt/api/sat-seguridad/tokens/hc/{captchaToken}";

                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    client.DefaultRequestHeaders.Add("Origin", "https://cdn.c.sat.gob.gt");
                    client.DefaultRequestHeaders.Add("Referer", "https://cdn.c.sat.gob.gt/");
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

                    var tokenResponse = client.GetAsync(tokenUrl).Result;

                    if (tokenResponse.IsSuccessStatusCode)
                    {
                        var tokenJson = tokenResponse.Content.ReadAsStringAsync().Result;
                        var tokenObj = Newtonsoft.Json.Linq.JObject.Parse(tokenJson);
                        this.satToken = tokenObj["access_token"]?.ToString();
                        tokenExpiration = DateTime.Now.AddMinutes(59);
                    }
                    else
                    {
                        Console.WriteLine("Error al obtener el access_token SAT: " + tokenResponse.StatusCode);
                    }
                }
            }
        }
        private static bool TrySelect<T>(JToken root, string path, out T value)
        {
            value = default(T);
            if (root == null) return false;

            var token = root.SelectToken(path);
            if (token == null || token.Type == JTokenType.Null || token.Type == JTokenType.Undefined)
                return false;

            try
            {
                value = token.ToObject<T>();
                return true;
            }
            catch
            {
                return false;
            }
        }
        private static bool TrySelectUnixMs(JToken root, string path, out DateTime value)
        {
            value = default(DateTime);
            long ms;
            if (!TrySelect<long>(root, path, out ms)) return false;

            try
            {
                value = DateTimeOffset.FromUnixTimeMilliseconds(ms).DateTime;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void Procesar(E_PERSONAS_RTU persona, IWebDriver driver)
        {
            E_PERSONAS_RTU rpersona = new E_PERSONAS_RTU();

            try
            {

                //if (string.IsNullOrEmpty(satToken) && DateTime.Now >= tokenExpiration)
                //{
                //    GenerarToken(persona, driver);
                //}

                persona.primer_nombre = "NIT consultado se encuentra como fallecido";

                using (var httpClient = new HttpClient())
                {
                    string nit = persona.nit; //4865219
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", satToken);
                    httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    var rtuResponse = httpClient.GetAsync($"https://svc.c.sat.gob.gt/api/sat_rtu/contribuyentes/datos/general/constancia/publica/{nit}").Result;

                    if (rtuResponse.IsSuccessStatusCode)
                    {
                        string json = rtuResponse.Content.ReadAsStringAsync().Result; 
                        var root = JObject.Parse(json);
                        string fallecido;
                        if (TrySelect<string>(root, "data.type", out fallecido))
                        {
                            if (fallecido == "Tiene_Fecha_Fallecimiento")
                            {
                                persona.primer_nombre = "NIT consultado se encuentra como fallecido";

                                dataGrid.DataSource = lst;
                                return;
                            }
                              
                        }

                        
                        //Identificación
                        string primer_nombre;
                        if (TrySelect<string>(root, "data.attributes.datos.contribuyente.persona.primer_Nombre", out primer_nombre))
                        {
                            persona.primer_nombre = primer_nombre;
                        }
                        string segundo_nombre;
                        if (TrySelect<string>(root, "data.attributes.datos.contribuyente.persona.segundo_Nombre", out segundo_nombre))
                        {
                            persona.segundo_nombre = segundo_nombre;
                        }
                        string primer_apellido;
                        if (TrySelect<string>(root, "data.attributes.datos.contribuyente.persona.primer_Apellido", out primer_apellido))
                        {
                            persona.primer_apellido = primer_apellido;
                        }
                        string segundo_apellido;
                        if (TrySelect<string>(root, "data.attributes.datos.contribuyente.persona.segundo_Apellido", out segundo_apellido))
                        {
                            persona.segundo_apellido = segundo_apellido;
                        }
                        string dpi;
                        if (TrySelect<string>(root, "data.attributes.datos.contribuyente.persona.dpi", out dpi))
                        {
                            persona.dpi = dpi;
                        }
                        DateTime fecha_vencimiento;
                        if (TrySelectUnixMs(root, "data.attributes.datos.contribuyente.persona.fecha_Vencimiento_DPI", out fecha_vencimiento))
                        {
                            persona.fecha_vencimiento = fecha_vencimiento;
                        }
                        string cedula;
                        if (TrySelect<string>(root, "data.attributes.datos.contribuyente.persona.numero_Registro_Cedula_Desc", out cedula))
                        {
                            string orden;
                            if (TrySelect<string>(root, "data.attributes.datos.contribuyente.persona.numero_Orden_Cedula", out orden))
                            {
                                persona.cedula = cedula +" "+ orden;
                            }
                                
                        }
                        string nacimientoStr;
                        if (TrySelect<string>(root, "data.attributes.datos.contribuyente.persona.fecha_Nacimiento", out nacimientoStr))
                        {
                            DateTime fechaNac;
                            if (DateTime.TryParse(nacimientoStr, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out fechaNac))
                            {
                                persona.fecha = fechaNac;
                            }
                        }
                        string sexo;
                        if (TrySelect<string>(root, "data.attributes.datos.contribuyente.persona.genero_Desc", out sexo))
                        {
                            persona.sexo = sexo;
                        }
                        string nacionalidad;
                        if (TrySelect<string>(root, "data.attributes.datos.contribuyente.persona.nacionalidad_Desc", out nacionalidad))
                        {
                            persona.nacionalidad = nacionalidad;
                        }
                        string estado_civil;
                        if (TrySelect<string>(root, "data.attributes.datos.contribuyente.persona.estado_Civil_Desc", out estado_civil))
                        {
                            persona.estado_civil = estado_civil;
                        }
                        string sector_economico;
                        if (TrySelect<string>(root, "data.attributes.datos.contribuyente.persona.sectorEconomico.sectorEconomicoDesc", out sector_economico))
                        {
                            persona.sector_economico = sector_economico;
                        }

                        //Información del colegiado
                        int numero_colegiado;
                        if (TrySelect<int>(root, "data.attributes.datos.contribuyente.persona.listaColegiados[0].numeroColegiado", out numero_colegiado))
                        {
                            persona.numero_colegiado = numero_colegiado;
                        }
                        DateTime Fecha_colegiado;
                        if (TrySelectUnixMs(root, "data.attributes.datos.contribuyente.persona.listaColegiados[0].fechaColegiado", out Fecha_colegiado))
                        {
                            persona.Fecha_colegiado = Fecha_colegiado;
                        }
                        string profesion;
                        if (TrySelect<string>(root, "data.attributes.datos.contribuyente.persona.listaColegiados[0].codTituloDesc", out profesion))
                        {
                            persona.profesion = profesion;
                        }
                        string colagiado_profesionales;
                        if (TrySelect<string>(root, "data.attributes.datos.contribuyente.persona.listaColegiados[0].codProfesionDesc", out colagiado_profesionales))
                        {
                            persona.colagiado_profesionales = colagiado_profesionales;
                        }

                        persona.nombre = persona.primer_nombre + " " + persona.segundo_nombre + " " + persona.primer_apellido + " " + persona.segundo_apellido;

                        //ÚLTIMO ESTABLECIMIENTO
                        string nombre_comercial;
                        if (TrySelect<string>(root, "data.attributes.datos.establecimiento[0].nombreEstablecimiento", out nombre_comercial))
                        {
                            persona.nombre_comercial = nombre_comercial;
                        }
                        int numero_establecimiento;
                        if (TrySelect<int>(root, "data.attributes.datos.establecimiento[0].numeroSecuenciaEstablecimiento", out numero_establecimiento))
                        {
                            persona.numero_establecimiento = numero_establecimiento;
                        }
                        DateTime fecha_inicio_operaciones;
                        if (TrySelectUnixMs(root, "data.attributes.datos.establecimiento[0].fechaInicioOperaciones", out fecha_inicio_operaciones))
                        {
                            persona.fecha_inicio_operaciones = fecha_inicio_operaciones;
                        }
                        string estado_establecimiento;
                        if (TrySelect<string>(root, "data.attributes.datos.establecimiento[0].estadoDesc", out estado_establecimiento))
                        {
                            persona.estado_establecimiento = estado_establecimiento;
                        }

                        //OTROS
                        DateTime ultima_actualizacion;
                        if (TrySelectUnixMs(root, "data.attributes.datos.fechaUltimaActualizacionRatificacion", out ultima_actualizacion))
                        {
                            persona.ultima_actualizacion = ultima_actualizacion;
                        }

                        //CARACTERÍSTICAS ESPECIALES
                        e_caracteristicas carac = new e_caracteristicas();
                        string caracteristica;
                        if (TrySelect<string>(root, "data.attributes.datos.caracteristicasEspeciales[0].nombreCaracteristica", out caracteristica))
                        {
                            carac.nit = persona.nit;
                            carac.caracteristica = caracteristica;

                            string estado;
                            if (TrySelect<string>(root, "data.attributes.datos.caracteristicasEspeciales[0].nombreEstado", out estado))
                            {
                                carac.estado = estado;
                            }
                            DateTime fecha_estatus;
                            if (TrySelectUnixMs(root, "data.attributes.datos.caracteristicasEspeciales[0].fechaEstado", out fecha_estatus))
                            {
                                carac.fecha_estatus = fecha_estatus;
                            }
                            cara.Add(carac);
                        }
                        

                        //AFILIACIONES
                        //Impuesto al Valor Agregado
                        e_afiliacion afil = new e_afiliacion();
                        int codigo_impuesto;
                        if (TrySelect<int>(root, "data.attributes.datos.afiliacionImpuesto.iva.codigoImpuesto", out codigo_impuesto))
                        {
                            afil.nit = persona.nit;
                            afil.codigo_impuesto = codigo_impuesto;

                            string nombre_impuesto;
                            if (TrySelect<string>(root, "data.attributes.datos.afiliacionImpuesto.iva.nombreImpuesto", out nombre_impuesto))
                            {
                                afil.nombre_impuesto = nombre_impuesto;
                            }
                            string tipo_contribuyente;
                            if (TrySelect<string>(root, "data.attributes.datos.afiliacionImpuesto.iva.tipoContribuyenteDesc", out tipo_contribuyente))
                            {
                                afil.tipo_contribuyente = tipo_contribuyente;
                            }
                            string clasificacion;
                            if (TrySelect<string>(root, "data.attributes.datos.afiliacionImpuesto.iva.tipoEstablecimientoDesc", out clasificacion))
                            {
                                afil.clasificacion = clasificacion;
                            }
                            string regimen;
                            if (TrySelect<string>(root, "data.attributes.datos.afiliacionImpuesto.iva.codigoRegimenDesc", out regimen))
                            {
                                afil.regimen = regimen;
                            }
                            string periodo;
                            if (TrySelect<string>(root, "data.attributes.datos.afiliacionImpuesto.iva.periodoImpositivoDesc", out periodo))
                            {
                                afil.periodo = periodo;
                            }
                            string estatus;
                            if (TrySelect<string>(root, "data.attributes.datos.afiliacionImpuesto.iva.obligaciones[0].descripcionEstado", out estatus))
                            {
                                afil.estatus = estatus;
                            }
                            DateTime fecha;
                            if (TrySelectUnixMs(root, "data.attributes.datos.afiliacionImpuesto.iva.fechaDesde", out fecha))
                            {
                                afil.fecha = fecha;
                            }

                            IVA.Add(afil);
                        }
                        
                        //Impuesto Sobre la Renta
                        e_isr isr = new e_isr();
                        int CODIGO_IMPUESTO;
                        if (TrySelect<int>(root, "data.attributes.datos.afiliacionImpuesto.isr.codigoImpuesto", out CODIGO_IMPUESTO))
                        {
                            isr.NIT = persona.nit;
                            isr.CODIGO_IMPUESTO = CODIGO_IMPUESTO;

                            string NOMBRE;
                            if (TrySelect<string>(root, "data.attributes.datos.afiliacionImpuesto.isr.nombreImpuesto", out NOMBRE))
                            {
                                isr.NOMBRE = NOMBRE;
                            }
                            string TIPO_CONTRIBUYENTE;
                            if (TrySelect<string>(root, "data.attributes.datos.afiliacionImpuesto.isr.tipoContribuyenteDesc", out TIPO_CONTRIBUYENTE))
                            {
                                isr.TIPO_CONTRIBUYENTE = TIPO_CONTRIBUYENTE;
                            }
                            string TIPO_RENTA;
                            if (TrySelect<string>(root, "data.attributes.datos.afiliacionImpuesto.isr.tipoRentaDesc", out TIPO_RENTA))
                            {
                                isr.TIPO_RENTA = TIPO_RENTA;
                            }
                            string REGIMEN;
                            if (TrySelect<string>(root, "data.attributes.datos.afiliacionImpuesto.isr.regimenDesc", out REGIMEN))
                            {
                                isr.REGIMEN = REGIMEN;
                            }
                            string FORMA_CALCULO;
                            if (TrySelect<string>(root, "data.attributes.datos.afiliacionImpuesto.isr.formaCalculoDesc", out FORMA_CALCULO))
                            {
                                isr.FORMA_CALCULO = FORMA_CALCULO;
                            }
                            string SISTEMA_INVENTARIOS;
                            if (TrySelect<string>(root, "data.attributes.datos.afiliacionImpuesto.isr.sistemaInventarioDesc", out SISTEMA_INVENTARIOS))
                            {
                                isr.SISTEMA_INVENTARIOS = SISTEMA_INVENTARIOS;
                            }
                            string SISTEMA_CONTABLE;
                            if (TrySelect<string>(root, "data.attributes.datos.afiliacionImpuesto.isr.sistemaContableDesc", out SISTEMA_CONTABLE))
                            {
                                isr.SISTEMA_CONTABLE = SISTEMA_CONTABLE;
                            }
                            string ESTATUS;
                            if (TrySelect<string>(root, "data.attributes.datos.afiliacionImpuesto.isr.estatusDeAfiliacionDesc", out ESTATUS))
                            {
                                isr.ESTATUS = ESTATUS;
                            }
                            DateTime FECHA;
                            if (TrySelectUnixMs(root, "data.attributes.datos.afiliacionImpuesto.isr.fechaDesde", out FECHA))
                            {
                                isr.FECHA = FECHA;
                            }

                            ISR.Add(isr);
                        }
  
                        

                    }

                }

                // driver.Close();
                //driver.Quit();
                dataGrid.DataSource = lst;
            }

            catch (Exception ex)
            {
                //     driver.Close();
                //    driver.Quit();
                persona.nombre = "Error en Consulta ";
            }
        }
        public void ProcesarRespaldo(E_PERSONAS_RTU persona, IWebDriver driver)
        {
            E_PERSONAS_RTU rpersona = new E_PERSONAS_RTU();

            try
            {

                if (!string.IsNullOrEmpty(satToken) && DateTime.Now < tokenExpiration)
                {

                    using (var httpClient = new HttpClient())
                    {
                        string nit = persona.nit;
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", satToken);
                        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                        var rtuResponse = httpClient.GetAsync($"https://svc.c.sat.gob.gt/api/sat_rtu/contribuyentes/datos/general/constancia/publica/{nit}").Result;

                        if (rtuResponse.IsSuccessStatusCode)
                        {
                            string json = rtuResponse.Content.ReadAsStringAsync().Result;
                        }
                        else
                        {
                            Console.WriteLine("Error al consultar el NIT: " + rtuResponse.StatusCode);
                        }
                    }

                }
                else
                {
                    // === Resolver Captcha ===
                    driver.Navigate().GoToUrl("https://cdn.c.sat.gob.gt/rtu/constancia-rtu-portal?nit=" + persona.nit);
                    driver.Manage().Timeouts().PageLoad.Add(TimeSpan.FromSeconds(190));
                    Thread.Sleep(10000);

                    var hcaptchaIframe = driver.FindElement(By.XPath("//iframe[contains(@src, 'hcaptcha.com')]"));
                    string src = hcaptchaIframe.GetAttribute("src");
                    Uri uri = new Uri(src);
                    string fragment = uri.Fragment;
                    var fragmentParams = System.Web.HttpUtility.ParseQueryString(fragment.TrimStart('#'));
                    string siteKey = fragmentParams.Get("sitekey");
                    string pageUrl = driver.Url;

                    Solver solver = new Solver();
                    string id = solver.postHcaptcha(siteKey, pageUrl);

                    ((IJavaScriptExecutor)driver).ExecuteScript(
                        "document.querySelector('[name=\"h-captcha-response\"]').style.display = 'inline';");

                    string answer;
                    while (true)
                    {
                        answer = solver.get(id.Substring(3));
                        if (answer == "CAPCHA_NOT_READY")
                        {
                            Thread.Sleep(20000);
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (answer.StartsWith("ERROR_"))
                    {
                        driver.Quit();
                    }
                    else
                    {
                        IWebElement tokenField = driver.FindElement(By.CssSelector("[name='h-captcha-response']"));
                        tokenField.SendKeys(answer.Substring(3));
                        string captchaToken = answer.Substring(3);

                        using (var client = new HttpClient())
                        {
                            // SAT API para obtener el access_token
                            string tokenUrl = $"https://svc.c.sat.gob.gt/api/sat-seguridad/tokens/hc/{captchaToken}";

                            client.DefaultRequestHeaders.Add("Accept", "application/json");
                            client.DefaultRequestHeaders.Add("Origin", "https://cdn.c.sat.gob.gt");
                            client.DefaultRequestHeaders.Add("Referer", "https://cdn.c.sat.gob.gt/");
                            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

                            var tokenResponse = client.GetAsync(tokenUrl).Result;

                            if (tokenResponse.IsSuccessStatusCode)
                            {
                                var tokenJson = tokenResponse.Content.ReadAsStringAsync().Result;
                                var tokenObj = Newtonsoft.Json.Linq.JObject.Parse(tokenJson);
                                satToken = tokenObj["access_token"]?.ToString();
                                tokenExpiration = DateTime.Now.AddMinutes(59);
                            }
                            else
                            {
                                Console.WriteLine("Error al obtener el access_token SAT: " + tokenResponse.StatusCode);
                            }
                        }

                        using (var httpClient = new HttpClient())
                        {
                            string nit = persona.nit;
                            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", satToken);
                            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                            var rtuResponse = httpClient.GetAsync($"https://svc.c.sat.gob.gt/api/sat_rtu/contribuyentes/datos/general/constancia/publica/{nit}").Result;

                            if (rtuResponse.IsSuccessStatusCode)
                            {
                                string json = rtuResponse.Content.ReadAsStringAsync().Result;
                            }
                            else
                            {
                                Console.WriteLine("Error al consultar el NIT: " + rtuResponse.StatusCode);
                            }
                        }
                        //----------------------------------
                    }
                    tokenExpiration = DateTime.Now.AddMinutes(59); // Opcional: 59 para margen de seguridad
                }

                //while (bandera)
                //{
                //    Thread.Sleep(500);
                //    if (driver.PageSource.Contains(" Constancia de inscripción y actualización de datos al Registro Tributario Unificado "))
                //    {
                //        bandera = false;
                //    }
                //}


                //if (driver.PageSource.Contains("El NIT es inválido."))
                //{
                //    persona.nombre = "El NIT es inválido.";
                //}

                //else if (driver.PageSource.Contains("El NIT consultado no se encuentra activo."))
                //{
                //    persona.nombre = "El NIT consultado no se encuentra activo.";
                //}
                //else if (driver.PageSource.Contains("El NIT es inválido."))
                //{
                //    persona.nombre = "El NIT es inválido.";
                //}
                //else if (driver.PageSource.Contains("El NIT consultado se encuentra registrado como fallecido"))
                //{
                //    persona.nombre = "El NIT consultado se encuentra registrado como fallecido";
                //}
                //else
                //{
                //    Boolean bandera2 = true;
                //    Boolean bandera3 = true;
                //    int i = 1;
                //    int j = 2;
                //    Thread.Sleep(500);
                //    while (bandera3)
                //    {
                //        Boolean val = FindElementIfExists(driver, "/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[1]/h6");
                //        if (val)
                //        {
                //            if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[1]/h6")).Text.ToUpper().Contains("IDENTIFICACIÓN"))
                //            {
                //                while (bandera2)
                //                {
                //                    val = FindElementIfExists(driver, "/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div/div[" + i + "]/div[1]");
                //                    if (val)
                //                    {

                //                        if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div/div[" + i + "]/div[1]")).Text.Contains("Primer nombre"))
                //                        {
                //                            persona.primer_nombre = driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div/div[" + i + "]/div[2]")).Text;
                //                        }
                //                        else if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div/div[" + i + "]/div[1]")).Text.Contains("Segundo nombre"))
                //                        {
                //                            persona.segundo_nombre = driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div/div[" + i + "]/div[2]")).Text;
                //                        }
                //                        else if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div/div[" + i + "]/div[1]")).Text.Contains("Primer apellido"))
                //                        {
                //                            persona.primer_apellido = driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div/div[" + i + "]/div[2]")).Text;
                //                        }
                //                        else if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div/div[" + i + "]/div[1]")).Text.Contains("Segundo apellido"))
                //                        {
                //                            persona.segundo_apellido = driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div/div[" + i + "]/div[2]")).Text;
                //                        }
                //                        else if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div/div[" + i + "]/div[1]")).Text.Contains("Código Único de Identificación"))
                //                        {
                //                            persona.dpi = driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div/div[" + i + "]/div[2]")).Text;
                //                        }
                //                        else if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div/div[" + i + "]/div[1]")).Text.Contains("Fecha de vencimiento"))
                //                        {
                //                            persona.fecha_vencimiento = Convert.ToDateTime(driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div/div[" + i + "]/div[2]")).Text);
                //                        }
                //                        else if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div/div[" + i + "]/div[1]")).Text.Contains("Cédula de vecindad"))
                //                        {
                //                            persona.cedula = driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div/div[" + i + "]/div[2]")).Text;
                //                        }
                //                        else if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div/div[" + i + "]/div[1]")).Text.Contains("Fecha de Nacimiento"))
                //                        {
                //                            persona.fecha = Convert.ToDateTime(driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div/div[" + i + "]/div[2]")).Text);
                //                        }
                //                        else if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div/div[" + i + "]/div[1]")).Text.Contains("Sexo"))
                //                        {
                //                            persona.sexo = driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div/div[" + i + "]/div[2]")).Text;
                //                        }
                //                        else if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div/div[" + i + "]/div[1]")).Text.Contains("Nacionalidad"))
                //                        {
                //                            persona.nacionalidad = driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div/div[" + i + "]/div[2]")).Text;
                //                        }
                //                        else if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div/div[" + i + "]/div[1]")).Text.Contains("Estado civil"))
                //                        {
                //                            persona.estado_civil = driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[2]/div/div[2]/div/div[" + i + "]/div[2]")).Text;
                //                        }
                //                        else if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div/div[" + i + "]/div[1]")).Text.Contains("Sector economico"))
                //                        {
                //                            persona.sector_economico = driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[2]/div/div[2]/div/div[" + i + "]/div[2]")).Text;
                //                        }
                //                        else if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div/div[" + i + "]/div[1]")).Text.Contains("Información del colegiado:"))
                //                        {
                //                            persona.numero_colegiado = Convert.ToInt32(driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[2]/div/div[2]/div/div[16]/div[2]/table/tbody/tr/td[1]")).Text);
                //                            persona.Fecha_colegiado = Convert.ToDateTime(driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[2]/div/div[2]/div/div[16]/div[2]/table/tbody/tr/td[2]")).Text);
                //                            persona.profesion = driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[2]/div/div[2]/div/div[16]/div[2]/table/tbody/tr/td[3]")).Text;
                //                            persona.colagiado_profesionales = driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[2]/div/div[2]/div/div[16]/div[2]/table/tbody/tr/td[4]")).Text;
                //                        }

                //                        i++;
                //                    }
                //                    else
                //                    { bandera2 = false; }
                //                }
                //                persona.nombre = persona.primer_nombre + " " + persona.segundo_nombre + " " + persona.primer_apellido + " " + persona.segundo_apellido;
                //            }
                //            else if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[1]/h6")).Text.ToUpper().Contains("ÚLTIMO ESTABLECIMIENTO"))
                //            {
                //                int x = 1;
                //                bandera2 = true;
                //                while (bandera2)
                //                {
                //                    val = FindElementIfExists(driver, "/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div/div[" + x + "]/div[1]");
                //                    if (val)
                //                    {
                //                        if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div/div[" + x + "]/div[1]")).Text.Contains("Nombre Comercial"))
                //                        {
                //                            persona.nombre_comercial = driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div/div[" + x + "]/div[2]")).Text;
                //                        }
                //                        else if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div/div[" + x + "]/div[1]")).Text.Contains("Número de secuencia de establecimiento"))
                //                        {
                //                            persona.numero_establecimiento = Convert.ToInt32(driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[3]/div/div[2]/div/div[" + x + "]/div[2]")).Text);
                //                        }
                //                        else if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[3]/div/div[2]/div/div[" + x + "]/div[1]")).Text.Contains("Fecha Inicio de Operaciones"))
                //                        {
                //                            persona.fecha_inicio_operaciones = Convert.ToDateTime(driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[3]/div/div[2]/div/div[4]/div[2]")).Text);
                //                        }
                //                        else if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div/div[" + x + "]/div[1]")).Text.Contains("Estado del establecimiento"))
                //                        {
                //                            persona.estado_establecimiento = driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div/div[" + x + "]/div[2]")).Text;
                //                        }
                //                    }
                //                    else { bandera2 = false; }
                //                    x++;
                //                }
                //            }
                //            else if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[1]/h6")).Text.ToUpper().Contains("OTROS"))
                //            {
                //                int x = 2;
                //                bandera2 = true;
                //                while (bandera2)
                //                {
                //                    val = FindElementIfExists(driver, "/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[" + x + "]/div/div/div[1]");
                //                    if (val)
                //                    {
                //                        if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[" + x + "]/div/div/div[1]")).Text.Contains("Fecha última actualización"))
                //                        {
                //                            persona.ultima_actualizacion = Convert.ToDateTime(driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[" + x + "]/div/div/div[2]")).Text);
                //                        }
                //                    }
                //                    else { bandera2 = false; }
                //                    x++;
                //                }
                //            }
                //            else if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[1]/h6")).Text.ToUpper().Contains("CARACTERÍSTICAS ESPECIALES"))
                //            {
                //                //caracteristicas
                //                Boolean columnas = true;
                //                Boolean filas = true;
                //                int x = 1;
                //                int col = 1;
                //                int row = 2;
                //                e_caracteristicas p = new e_caracteristicas();
                //                while (filas)
                //                {
                //                    val = FindElementIfExists(driver, "/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[" + row + "]/div/table/tbody/tr/td[" + col + "]");

                //                    if (val)
                //                    {
                //                        columnas = true;
                //                        while (columnas)
                //                        {
                //                            val = FindElementIfExists(driver, "/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[" + row + "]/div/table/tbody/tr/td[" + col + "]");

                //                            if (val)
                //                            {

                //                                if (col == 1)
                //                                {
                //                                    p.nit = persona.nit;
                //                                    p.caracteristica = driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[" + row + "]/div/table/tbody/tr/td[" + col + "]")).Text;
                //                                }
                //                                else if (col == 2)
                //                                {
                //                                    p.estado = driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[" + row + "]/div/table/tbody/tr/td[" + col + "]")).Text;
                //                                }
                //                                else if (col == 3)
                //                                {
                //                                    p.fecha_estatus = Convert.ToDateTime(driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[" + row + "]/div/table/tbody/tr/td[" + col + "]")).Text);
                //                                    cara.Add(p);
                //                                    columnas = false;
                //                                }
                //                                col++;
                //                            }
                //                            else
                //                            {
                //                                filas = false;
                //                            }
                //                        }
                //                        row++;
                //                    }
                //                    else
                //                    {
                //                        filas = false;
                //                    }
                //                }

                //            }
                //            else if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[1]/h6")).Text.ToUpper().Contains("AFILIACIONES"))
                //            {
                //                //AFILIACIONES
                //                Boolean columnas = true;
                //                Boolean filas = true;
                //                int x = 1;
                //                int row = 1;
                //                while (columnas)
                //                {
                //                    int col = 1;
                //                    val = FindElementIfExists(driver, "/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/h5[" + x + "]");

                //                    if (val)
                //                    {
                //                        if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/h5[" + x + "]")).Text.Contains("Impuesto al Valor Agregado"))
                //                        {
                //                            e_afiliacion p = new e_afiliacion();
                //                            p.nit = persona.nit;
                //                            filas = true;
                //                            while (filas)
                //                            {
                //                                val = FindElementIfExists(driver, "/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[1]");

                //                                if (val)
                //                                {
                //                                    if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[1]")).Text.Contains("Código de Impuesto"))
                //                                    {
                //                                        p.codigo_impuesto = Convert.ToInt32(driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[2]")).Text);
                //                                    }
                //                                    else if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[1]")).Text.Contains("Nombre de Impuesto"))
                //                                    {
                //                                        p.nombre_impuesto = driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[2]")).Text;
                //                                    }
                //                                    else if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[1]")).Text.Contains("Tipo de contribuyente"))
                //                                    {
                //                                        p.tipo_contribuyente = driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[2]")).Text;
                //                                    }
                //                                    else if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[1]")).Text.Contains("Clasificación del establecimiento"))
                //                                    {
                //                                        p.clasificacion = driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[2]")).Text;
                //                                    }
                //                                    else if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[1]")).Text.Contains("Régimen"))
                //                                    {
                //                                        p.regimen = driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[2]")).Text;
                //                                    }
                //                                    else if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[1]")).Text.Contains("Periodo impositivo"))
                //                                    {
                //                                        p.periodo = driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[2]")).Text;
                //                                    }
                //                                    else if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[1]")).Text.Contains("Estatus de la afiliación"))
                //                                    {
                //                                        p.estatus = driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[2]")).Text;
                //                                    }
                //                                    else if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[1]")).Text.Contains("Fecha desde"))
                //                                    {
                //                                        p.fecha = Convert.ToDateTime(driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[2]")).Text);
                //                                    }
                //                                    col++;
                //                                }
                //                                else
                //                                {
                //                                    filas = false;
                //                                }
                //                            }

                //                            IVA.Add(p);
                //                        }
                //                        else if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/h5[" + x + "]")).Text.Contains("Impuesto Sobre la Renta"))
                //                        {
                //                            e_isr p = new e_isr();
                //                            p.NIT = persona.nit;
                //                            filas = true;
                //                            while (filas)
                //                            {
                //                                val = FindElementIfExists(driver, "/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[1]");

                //                                if (val)
                //                                {
                //                                    if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[1]")).Text.Contains("Código de Impuesto"))
                //                                    {
                //                                        p.CODIGO_IMPUESTO = Convert.ToInt32(driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[2]")).Text);
                //                                    }
                //                                    else if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[1]")).Text.Contains("Nombre de Impuesto"))
                //                                    {
                //                                        p.NOMBRE = driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[2]")).Text;
                //                                    }
                //                                    else if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[1]")).Text.Contains("Tipo de contribuyente"))
                //                                    {
                //                                        p.TIPO_CONTRIBUYENTE = driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[2]")).Text;
                //                                    }

                //                                    else if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[1]")).Text.Contains("Tipo de Renta"))
                //                                    {
                //                                        p.TIPO_RENTA = driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[2]")).Text;
                //                                    }
                //                                    else if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[1]")).Text.Contains("Régimen por tipo de renta"))
                //                                    {
                //                                        p.REGIMEN = driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[2]")).Text;
                //                                    }
                //                                    else if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[1]")).Text.Contains("Forma de Cálculo"))
                //                                    {
                //                                        p.FORMA_CALCULO = driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[2]")).Text;
                //                                    }
                //                                    else if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[1]")).Text.Contains("Sistema de valuación de inventarios"))
                //                                    {
                //                                        p.SISTEMA_INVENTARIOS = driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[2]")).Text;
                //                                    }
                //                                    else if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[1]")).Text.Contains("Sistema Contable"))
                //                                    {
                //                                        p.SISTEMA_CONTABLE = driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[2]")).Text;
                //                                    }
                //                                    else if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[1]")).Text.Contains("Estatus de la afiliación"))
                //                                    {
                //                                        p.ESTATUS = driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[2]")).Text;
                //                                    }
                //                                    else if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[1]")).Text.Contains("Fecha desde"))
                //                                    {
                //                                        p.FECHA = Convert.ToDateTime(driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[2]")).Text);
                //                                    }
                //                                    col++;
                //                                }
                //                                else
                //                                {
                //                                    filas = false;
                //                                }
                //                            }

                //                            ISR.Add(p);
                //                        }
                //                        else if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/h5[" + x + "]")).Text.Contains("Impuesto de Solidaridad"))
                //                        {
                //                            //E_ISO p = new E_ISO();
                //                            //p.NIT = persona.nit;
                //                            //filas = true;
                //                            //while (filas)
                //                            //{
                //                            //    val = FindElementIfExists(driver, "/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[1]");

                //                            //    if (val)
                //                            //    {
                //                            //        if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[1]")).Text.Contains("Formas de Acreditamiento"))
                //                            //        {
                //                            //            p.FORMA_ACREDITAMIENTO = driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[2]")).Text;
                //                            //        }
                //                            //        else if (driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[1]")).Text.Contains("Fecha desde"))
                //                            //        {
                //                            //            p.FECHA = Convert.ToDateTime(driver.FindElement(By.XPath("/html/body/app-my-app/app-constancia-rtu/div/div/div/div[" + j + "]/div/div[2]/div[" + row + "]/div[" + col + "]/div[2]")).Text);
                //                            //        }
                //                            //        col++;
                //                            //    }
                //                            //    else
                //                            //    {
                //                            //        filas = false;
                //                            //    }
                //                            //}

                //                            //ISO.Add(p);
                //                        }
                //                        row++;
                //                    }
                //                    else
                //                    {
                //                        columnas = false;
                //                    }
                //                    x++;
                //                }
                //            }
                //            j++;
                //        }
                //        else { bandera3 = false; }
                //    }
                //}
                Thread.Sleep(500);



                // driver.Close();
                //driver.Quit();
                //dataGrid.DataSource = lst;
            }

            catch (Exception ex)
            {
                //     driver.Close();
                //    driver.Quit();
                persona.nombre = "Error en Consulta ";
            }
        }
        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            ChromeOptions options = new ChromeOptions();
            E_PERSONAS_RTU rpersona = new E_PERSONAS_RTU();
            options.AddArgument("no-sandbox");

            options.AddArguments("--disable-extensions"); // to disable extension
            options.AddArguments("--disable-notifications"); // to disable notification
            options.AddArguments("--disable-application-cache"); // to disable cache

            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = false;

            IWebDriver driver = new ChromeDriver(service, options, TimeSpan.FromMinutes(5));

            try
            {
                foreach (var persona in lst)
                {

                    cara.Clear();
                    IVA.Clear();
                    ISR.Clear();
                    var per = conexion.cargar(persona);
                    if (per.dpi != null)
                    {

                        persona.dpi = per.dpi;
                        persona.primer_nombre = per.primer_nombre;
                        persona.segundo_nombre = per.segundo_nombre;
                        persona.primer_apellido = per.primer_apellido;
                        persona.segundo_apellido = per.segundo_apellido;
                        persona.apellido_casada = per.apellido_casada;
                        persona.nombre = per.primer_nombre + " " + per.segundo_nombre + " " + per.primer_apellido + " " + per.segundo_apellido + " " + per.apellido_casada;
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
                    else
                    {
                        Procesar(persona, driver);

                        if (persona.primer_nombre != null)
                        {
                            conexion.Agregar_RTU(persona);
                            foreach (var c in cara)
                            {
                                conexion.agregar_caracteristica(c);
                                Rescara.Add(c);
                            }
                            foreach (var a in IVA)
                            {
                                conexion.agregar_afiliacion(a);
                                ResIVA.Add(a);
                            }
                            foreach (var a in ISR)
                            {
                                conexion.agregar_ISR(a);
                                ResISR.Add(a);
                            }
                        }
                    }

                }
                // driver.Close();
                //driver.Quit();
            }
            catch (Exception ex)
            {
                // driver.Close();
                //driver.Quit();
            }
            if (lst.Count > 0)
            {
                btnDescargar.Enabled = true;

                driver.Close();
                driver.Quit();
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

                    dt.Columns.Add("FECHA_COLEGIADO");
                    dt.Columns.Add("PROFESION");
                    dt.Columns.Add("COLEGIADO_PROFESIONALES");
                    dt.Columns.Add("SECTOR_ECONOMICO");
                    dt.Columns.Add("NUMERO_COLEGIADO");
                    dt.Columns.Add("FECHA_VENCIMIENTO");
                    dt.Columns.Add("FECHA_INICIO_OPERACIONES");


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
                        if (dato.ultima_actualizacion.ToString("dd/MM/yyyy") == "01/01/0001")
                        {
                            dr["ULTIMA_ACTUALIZACION"] = "";
                        }
                        else
                        {
                            dr["ULTIMA_ACTUALIZACION"] = dato.ultima_actualizacion.ToString("dd/MM/yyyy");
                        }
                        if (dato.Fecha_colegiado.ToString("dd/MM/yyyy") == "01/01/0001")
                        {
                            dr["FECHA_COLEGIADO"] = "";
                        }
                        else
                        {
                            dr["FECHA_COLEGIADO"] = dato.Fecha_colegiado.ToString("dd/MM/yyyy");
                        }
                        dr["PROFESION"] = dato.profesion;
                        dr["COLEGIADO_PROFESIONALES"] = dato.colagiado_profesionales;
                        dr["SECTOR_ECONOMICO"] = dato.sector_economico;
                        dr["NUMERO_COLEGIADO"] = dato.numero_colegiado;
                        if (dato.fecha_vencimiento.ToString("dd/MM/yyyy") == "01/01/0001")
                        {
                            dr["FECHA_VENCIMIENTO"] = "";
                        }
                        else
                        {
                            dr["FECHA_VENCIMIENTO"] = dato.fecha_vencimiento.ToString("dd/MM/yyyy");
                        }
                        if (dato.fecha_inicio_operaciones.ToString("dd/MM/yyyy") == "01/01/0001")
                        {
                            dr["FECHA_INICIO_OPERACIONES"] = "";
                        }
                        else
                        {
                            dr["FECHA_INICIO_OPERACIONES"] = dato.fecha_inicio_operaciones.ToString("dd/MM/yyyy");
                        }





                        dt.Rows.Add(dr);
                    }


                    foreach (var a in ResISR)
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

                    foreach (var a in ResIVA)
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

                    foreach (var a in Rescara)
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
