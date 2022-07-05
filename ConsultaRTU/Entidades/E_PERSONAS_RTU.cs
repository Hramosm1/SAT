using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultaRTU.Entidades
{
    public class e_caracteristicas
    {
        public string nit { get; set; }
        public string caracteristica { get; set; }
        public string estado { get; set; }
        public DateTime fecha_estatus { get; set; }
    }
    public class E_ISO
    {
        public string NIT { get; set; }
        public string FORMA_ACREDITAMIENTO { get; set; }
        public DateTime FECHA { get; set; }
    }
        public class e_isr
    {
        public string NIT { get; set; }
        public  int CODIGO_IMPUESTO { get; set; }
        public string NOMBRE { get; set; }
        public string TIPO_CONTRIBUYENTE { get; set; }
        public string TIPO_RENTA { get; set; }
        public string REGIMEN { get; set; }
        public string FORMA_CALCULO { get; set; }
        public string SISTEMA_INVENTARIOS { get; set; }
        public string SISTEMA_CONTABLE { get; set; }
        public string ESTATUS { get; set; }
        public DateTime FECHA { get; set; }
    }
    public class e_afiliacion
    {
        public string nit { get; set; }
        public int codigo_impuesto { get; set; }
        public string  nombre_impuesto { get; set; }
        public string tipo_contribuyente { get; set; }
        public string clasificacion { get; set; }
        public string regimen { get; set; }
        public string  periodo { get; set; }
        public string  estatus { get; set; }
        public DateTime fecha { get; set; }
    }
    public class E_PERSONAS_RTU
    {
        public string nit { get; set; }
        public string dpi { get; set; }
        public string nombre { get; set; }
        public string primer_nombre { get; set; }
        public string segundo_nombre { get; set; }
        public string primer_apellido { get; set; }
        public string segundo_apellido { get; set; }
        public string apellido_casada { get; set; }
        public string cedula { get; set; }
        public DateTime fecha { get; set; }
        public DateTime Fecha_colegiado { get; set; }
        public string sexo { get; set; }
        public string nacionalidad { get; set; }
        public string profesion { get; set; }
        public string colagiado_profesionales { get; set; }
        public string estado_civil { get; set; }
        public string sector_economico { get; set; }
        public string nombre_comercial { get; set; }
        public int numero_establecimiento { get; set; }
        public int numero_colegiado { get; set; }
        public string estado_establecimiento { get; set; }
        public DateTime fecha_vencimiento { get; set; }
        public DateTime fecha_inicio_operaciones { get; set; }


        public DateTime ultima_actualizacion { get; set; }
    }
}
