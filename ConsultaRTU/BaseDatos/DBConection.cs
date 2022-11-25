using ConsultaRTU.Entidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ConsultaRTU.BaseDatos
{
    public class DBConection
    {
        SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["conexion"].ConnectionString);

        public E_PERSONA Existe(E_PERSONA Persona)
        {

            E_PERSONA Respuesta = new E_PERSONA();
            int activo = 0;

            while(activo == 0)
            {
                try
                {
                    try
                    {
                        conexion.Open();
                        activo = 1;
                        string sql;
                        sql = $"SELECT DPI, NIT, NOMBRE_COMPLETO, FECHA_NACIMIENTO, FECHA_CREACION, NOTIFICACION_2 FROM SALUD.PERSONAS_SAT WHERE DPI = { Persona.dpi}";
                        SqlCommand comando = new SqlCommand(sql, conexion);
                        SqlDataReader registro = comando.ExecuteReader();


                        if (registro.Read())
                        {

                            Respuesta.dpi = Int64.Parse(registro["DPI"].ToString());
                            Respuesta.nit = registro["NIT"].ToString();
                            Respuesta.nombre = registro["NOMBRE_COMPLETO"].ToString();
                            Respuesta.fecha = DateTime.Parse(registro["FECHA_NACIMIENTO"].ToString());
                            Respuesta.fechacreacion = DateTime.Parse(registro["FECHA_CREACION"].ToString());
                            Respuesta.notificacion2 = registro["NOTIFICACION_2"].ToString();
                        }

                        conexion.Close();
                        return Respuesta;
                    }
                    catch
                    {
                        

                    }
                }
                catch
                {

                }
            }

            return Respuesta;
        }
        public E_PERSONAS_RTU cargar(E_PERSONAS_RTU Persona)
        {
            E_PERSONAS_RTU Respuesta = new E_PERSONAS_RTU();
            SqlDataReader reader;
            int activo = 0;
            
            while (activo == 0)
            {
                try
                {
                    conexion.Open();
                    SqlCommand cmd = conexion.CreateCommand();
                    activo = 1;

                    string sql;
                    /*sql = "SELECT  NIT, DPI, PRIMER_NOMBRE, SEGUNDO_NOMBRE, PRIMER_APELLIDO, SEGUNDO_APELLIDO, APELLIDO_CASADA, " +
                        " CEDULA, FECHA_NACIMIENTO, SEXO, NACIONALIDAD, ESTADO_CIVIL, NOMBRE_COMERCIAL, NUMERO_ESTABLECIMIENTO, " +
                        " ESTADO_ESTABLECIMIENTO, ULTIMA_ACTUALIZACION " +
                        " FROM SALUD.PERSONAS_RTU WHERE NIT = @NIT";*/
                    sql = $"SELECT isnull(NIT,''), isnull(DPI,''), isnull(PRIMER_NOMBRE,''), isnull(SEGUNDO_NOMBRE,''), isnull(PRIMER_APELLIDO,''), isnull(SEGUNDO_APELLIDO,''), isnull(APELLIDO_CASADA,''),  isnull(CEDULA,''), FECHA_NACIMIENTO, isnull(SEXO,''), isnull(NACIONALIDAD,''), isnull(ESTADO_CIVIL,''), isnull(NOMBRE_COMERCIAL,''), convert(INT,NUMERO_ESTABLECIMIENTO),  isnull(ESTADO_ESTABLECIMIENTO,''), isnull(ULTIMA_ACTUALIZACION,'01/01/1900')  FROM SALUD.PERSONAS_RTU WHERE NIT = '{Persona.nit}'";

                    cmd.CommandText = sql;
                    //cmd.Parameters.AddWithValue("@NIT", Persona.nit);
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Respuesta.nit = reader.GetString(0);
                        Respuesta.dpi = reader.GetString(1);
                        Respuesta.primer_nombre = reader.GetString(2);
                        Respuesta.segundo_nombre = reader.GetString(3);
                        Respuesta.primer_apellido = reader.GetString(4);
                        Respuesta.segundo_apellido = reader.GetString(5);
                        Respuesta.apellido_casada = reader.GetString(6);
                        Respuesta.cedula = reader.GetString(7);
                        Respuesta.fecha = reader.GetDateTime(8);
                        Respuesta.sexo = reader.GetString(9);
                        Respuesta.nacionalidad = reader.GetString(10);
                        Respuesta.estado_civil = reader.GetString(11);
                        Respuesta.nombre_comercial = reader.GetString(12);
                        Respuesta.estado_establecimiento = reader.GetString(14);
                        Respuesta.ultima_actualizacion = reader.GetDateTime(15);
                        Respuesta.numero_establecimiento = reader.GetInt32(13);
                    }
                    conexion.Close();
                    return Respuesta;
                }
                catch (Exception ex)
                {

                }
            }
            
            
            return Respuesta;
        }

        public List<e_caracteristicas> cargar_caracteristicas(E_PERSONAS_RTU Persona)
        {

            List<e_caracteristicas> ListRespuesta = new List<e_caracteristicas>();
            e_caracteristicas Respuesta = new e_caracteristicas();
            SqlDataReader reader;
            int activo = 0;

            while(activo == 0)
            {
                try
                {
                    conexion.Open();
                    SqlCommand cmd = conexion.CreateCommand();
                    activo = 1;

                    string sql = " SELECT NIT, CARACTERISTICAS, ESTADO, FECHA_ESTATUS FROM SALUD.CARACTERISTICAS_RTU WHERE NIT = @NIT";
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@NIT", Persona.nit);

                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Respuesta.nit = reader.GetString(0);
                        Respuesta.caracteristica = reader.GetString(1);
                        Respuesta.estado = reader.GetString(2);
                        Respuesta.fecha_estatus = reader.GetDateTime(3);
                        ListRespuesta.Add(Respuesta);
                    }
                    conexion.Close();
                    return ListRespuesta;
                }
                catch
                {

                }
            }
            
            
            
            return ListRespuesta;
        }

        public List<e_afiliacion> cargar_iva(E_PERSONAS_RTU Persona)
        {
            List<e_afiliacion> ListRespuesta = new List<e_afiliacion>();
            e_afiliacion Respuesta = new e_afiliacion();
            SqlDataReader reader;
            int activo = 0;

            while(activo == 0)
            {
                try
                {
                    conexion.Open();
                    SqlCommand cmd = conexion.CreateCommand();
                    activo = 1;

                    string sql = " SELECT NIT, CODIGO_IMPUESTO, NOMBRE_IMPUESTO, TIPO_CONTRIBUYENTE, CLASIFICACION, REGIMEN, PERiODO, estatus, FECHA FROM SALUD.IvA_RTU WHERE NIT = @NIT";
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@NIT", Persona.nit);

                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Respuesta.nit = reader.GetString(0);
                        Respuesta.codigo_impuesto = reader.GetInt32(1);
                        Respuesta.nombre_impuesto = reader.GetString(2);
                        Respuesta.tipo_contribuyente = reader.GetString(3);
                        Respuesta.clasificacion = reader.GetString(4);
                        Respuesta.regimen = reader.GetString(5);
                        Respuesta.periodo = reader.GetString(6);
                        Respuesta.estatus = reader.GetString(7);
                        Respuesta.fecha = reader.GetDateTime(8);
                        ListRespuesta.Add(Respuesta);
                    }
                    conexion.Close();
                }
                catch (Exception ex)
                {

                }
            }
            
            return ListRespuesta;
        }


        public List<e_isr> cargar_ISR(E_PERSONAS_RTU Persona)
        {

            List<e_isr> ListRespuesta = new List<e_isr>();
            e_isr Respuesta = new e_isr();
            SqlDataReader reader;
            int activo = 0;
            
            while(activo == 0)
            {
                try
                {
                    conexion.Open();
                    SqlCommand cmd = conexion.CreateCommand();
                    activo = 1;

                    string sql = " SELECT NIT, CODIGO_IMPUESTO, NOMBRE_IMPUESTO, TIPO_CONTRIBUYENTE, TIPO_RENTA, REGIMEN, FORMA_CALCULO, SISTEMA_INVENTARIOS, SISTEMA_CONTABLE, ESTATUS, FECHA FROM SALUD.ISR_RTU WHERE NIT = @NIT";
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@NIT", Persona.nit);

                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Respuesta.NIT = reader.GetString(0);
                        Respuesta.CODIGO_IMPUESTO = reader.GetInt32(1);
                        Respuesta.NOMBRE = reader.GetString(2);
                        Respuesta.TIPO_CONTRIBUYENTE = reader.GetString(3);
                        Respuesta.TIPO_RENTA = reader.GetString(4);
                        Respuesta.REGIMEN = reader.GetString(5);
                        Respuesta.FORMA_CALCULO = reader.GetString(6);
                        Respuesta.SISTEMA_INVENTARIOS = reader.GetString(7);
                        Respuesta.SISTEMA_CONTABLE = reader.GetString(8);
                        Respuesta.ESTATUS = reader.GetString(9);
                        Respuesta.FECHA = reader.GetDateTime(10);
                        ListRespuesta.Add(Respuesta);
                    }
                    conexion.Close();
                    return ListRespuesta;
                }
                catch (Exception ex)
                {

                }
            }
            
            return ListRespuesta;
        }

        public void agregar_caracteristica(e_caracteristicas p)
        {

            e_caracteristicas caracteristicas = new e_caracteristicas();
            String sql = "";
            int activo = 0;

            while (activo == 0)
            {
                try
                {
                    conexion.Open();
                    activo = 1;
                    SqlCommand cmd = conexion.CreateCommand();
                    SqlTransaction transaction;
                    transaction = conexion.BeginTransaction();
                    cmd.Connection = conexion;
                    cmd.Transaction = transaction;
                    

                    try
                    {
                        if (p.nit != null)
                        {
                            sql = "insert into SALUD.caracteristicas_rtu (nit, caracteristicas, estado, fecha_estatus) values(@NIT,@CARACTERISTICAS,@ESTADO,@FECHA);";

                            cmd.CommandText = sql;
                            cmd.Parameters.AddWithValue("@NIT", p.nit);
                            cmd.Parameters.AddWithValue("@CARACTERISTICAS", p.caracteristica);
                            cmd.Parameters.AddWithValue("@ESTADO", p.estado);
                            cmd.Parameters.AddWithValue("@FECHA", p.fecha_estatus);
                            cmd.ExecuteNonQuery();

                            transaction.Commit();
                            conexion.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        conexion.Close();
                    }

                }
                catch (Exception ex)
                {

                }
               
            }
            
           
        }
        public void agregar_afiliacion(e_afiliacion p)
        {
            String sql = "";
            int activo = 0;
            while (activo == 0)
            {
                try
                {
                    conexion.Open();
                    activo = 1;
                    SqlCommand cmd = conexion.CreateCommand();
                    SqlTransaction transaction;
                    transaction = conexion.BeginTransaction();
                    cmd.Connection = conexion;
                    cmd.Transaction = transaction;
                    


                    try
                    {
                        if (p.nit != null)
                        {

                            sql = "insert into SALUD.IVA_rtu (nit, codigo_impuesto, nombre_impuesto, tipo_contribuyente, clasificacion, regimen, periodo, estatus, fecha) ";
                            sql = sql + " values(@NIT, @CODIGO_IMPUESTO, @NOMBRE_IMPUESTO, @TIPO_CONTRIBUYENTE, @CLASIFICACION, @REGIMEN, @PERIODO, @ESTATUS, @FECHA)";

                            cmd.CommandText = sql;
                            cmd.Parameters.AddWithValue("@NIT", p.nit);
                            cmd.Parameters.AddWithValue("@CODIGO_IMPUESTO", p.codigo_impuesto);
                            cmd.Parameters.AddWithValue("@NOMBRE_IMPUESTO", p.nombre_impuesto);
                            cmd.Parameters.AddWithValue("@TIPO_CONTRIBUYENTE", p.tipo_contribuyente);
                            cmd.Parameters.AddWithValue("@CLASIFICACION", p.clasificacion);
                            cmd.Parameters.AddWithValue("@REGIMEN", p.regimen);
                            cmd.Parameters.AddWithValue("@PERIODO", p.periodo);
                            cmd.Parameters.AddWithValue("@ESTATUS", p.estatus);
                            cmd.Parameters.AddWithValue("@FECHA", p.fecha);

                            cmd.ExecuteNonQuery();

                            transaction.Commit();
                            conexion.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        conexion.Close();
                    }
                }
                catch(Exception ex)
                {

                }
                
            }
        }

        public void agregar_ISR(e_isr p)
        {
            String sql = "";
            int activo = 0;

            while(activo == 0)
            {
                try
                {
                    conexion.Open();
                    activo = 1;
                    SqlCommand cmd = conexion.CreateCommand();
                    SqlTransaction transaction;
                    transaction = conexion.BeginTransaction();
                    cmd.Connection = conexion;
                    cmd.Transaction = transaction;

                    try
                    {
                        if (p.NIT != null)
                        {

                            sql = "INSERT INTO SALUD.ISR_RTU(NIT, CODIGO_IMPUESTO, NOMBRE_IMPUESTO, TIPO_CONTRIBUYENTE, TIPO_RENTA, REGIMEN, FORMA_CALCULO, SISTEMA_INVENTARIOS, SISTEMA_CONTABLE, ESTATUS, FECHA) ";
                            sql = sql + " values(@NIT, @CODIGO_IMPUESTO, @NOMBRE_IMPUESTO, @TIPO_CONTRIBUYENTE, @TIPO_RENTA, @REGIMEN, @FORMA_CALCULO, @SISTEMA_INVENTARIOS, @SISTEMA_CONTABLE, @ESTATUS,@FECHA); ";

                            cmd.CommandText = sql;
                            cmd.Parameters.AddWithValue("@NIT", p.NIT);
                            cmd.Parameters.AddWithValue("@CODIGO_IMPUESTO", p.CODIGO_IMPUESTO);
                            cmd.Parameters.AddWithValue("@NOMBRE_IMPUESTO", p.NOMBRE);
                            cmd.Parameters.AddWithValue("@TIPO_CONTRIBUYENTE", p.TIPO_CONTRIBUYENTE);
                            cmd.Parameters.AddWithValue("@TIPO_RENTA", p.CODIGO_IMPUESTO);
                            cmd.Parameters.AddWithValue("@REGIMEN", p.REGIMEN);
                            cmd.Parameters.AddWithValue("@FORMA_CALCULO", p.FORMA_CALCULO);
                            cmd.Parameters.AddWithValue("@SISTEMA_INVENTARIOS", p.SISTEMA_INVENTARIOS);
                            cmd.Parameters.AddWithValue("@SISTEMA_CONTABLE", p.SISTEMA_CONTABLE);
                            cmd.Parameters.AddWithValue("@ESTATUS", p.ESTATUS);
                            cmd.Parameters.AddWithValue("@FECHA", p.FECHA);

                            cmd.ExecuteNonQuery();

                            transaction.Commit();
                            conexion.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        conexion.Close();
                    }
                }
                catch(Exception ex)
                {

                }
                
            }
           
            
        }
        public void agregar_ISO(E_ISO p)
        {
            String sql = "";
            int activo = 0;

            while(activo == 0)
            {
                try
                {
                    conexion.Open();
                    activo = 1;
                    SqlCommand cmd = conexion.CreateCommand();
                    SqlTransaction transaction;
                    transaction = conexion.BeginTransaction();
                    cmd.Connection = conexion;
                    cmd.Transaction = transaction;

                    try
                    {
                        if (p.NIT != null)
                        {
                            sql = "INSERT INTO salud.ISO_RTU (NIT, FORMA_ACREDITAMIENTO, FECHA)VALUES(@NIT,@FORMA_ACREDITAMIENTO,@FECHA);";

                            cmd.CommandText = sql;
                            cmd.Parameters.AddWithValue("@NIT", p.NIT);
                            cmd.Parameters.AddWithValue("@FORMA_ACREDITAMIENTO", p.FORMA_ACREDITAMIENTO);
                            cmd.Parameters.AddWithValue("@FECHA", p.FECHA);

                            cmd.ExecuteNonQuery();

                            transaction.Commit();
                            conexion.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        conexion.Close();
                    }

                }
                catch(Exception ex)
                {

                }
                
            }
            
            
        }
        public void Agregar(E_PERSONA Personas)
        {
            E_PERSONA Respuesta = new E_PERSONA();
            String sql;
            int activo = 0;

            while(activo == 0)
            {
                try
                {
                    try
                    {
                        conexion.Open();
                        activo = 1;

                        sql = $"SELECT DPI, NIT, NOMBRE_COMPLETO, FECHA_NACIMIENTO, FECHA_CREACION, NOTIFICACION_2 FROM SALUD.PERSONAS_SAT WHERE DPI = {Personas.dpi}";
                        SqlCommand comando = new SqlCommand(sql, conexion);
                        SqlDataReader registro = comando.ExecuteReader();

                        if (registro.Read())
                        {
                            Respuesta.dpi = Int64.Parse(registro["DPI"].ToString());
                            Respuesta.nit = registro["NIT"].ToString();
                            Respuesta.nombre = registro["NOMBRE_COMPLETO"].ToString();
                            Respuesta.fecha = DateTime.Parse(registro["FECHA_NACIMIENTO"].ToString());
                            Respuesta.fechacreacion = DateTime.Parse(registro["FECHA_CREACION"].ToString());
                            Respuesta.notificacion2 = registro["NOTIFICACION_2"].ToString();
                        }
                        conexion.Close();
                        if (Respuesta.dpi == 0)
                        {
                            conexion.Open();
                            SqlCommand cmd2 = conexion.CreateCommand();
                            SqlTransaction transaction;
                            transaction = conexion.BeginTransaction();
                            cmd2.Connection = conexion;
                            cmd2.Transaction = transaction;
                            try
                            {
                                sql = "insert into salud.personas_sat (DPI, NIT, NOMBRE_COMPLETO, FECHA_NACIMIENTO, FECHA_CREACION, NOTIFICACION_2)" + "VALUES(@DPI,@NIT,@NOMBRE_COMPLETO,@FECHA,@FECHA_CREACION,@NOTIFICACION_2);";
                                //sql = $"insert into salud.personas_sat (DPI, NIT, NOMBRE_COMPLETO, FECHA_NACIMIENTO) VALUES({Personas.dpi},'{Personas.nit}','{Personas.nombre}','{Convert.ToDateTime(Personas.fecha).ToString("MM/dd/YYYY")}');";

                                cmd2.CommandText = sql;
                                cmd2.Parameters.AddWithValue("@DPI", Personas.dpi);
                                cmd2.Parameters.AddWithValue("@NIT", Personas.nit);
                                cmd2.Parameters.AddWithValue("@NOMBRE_COMPLETO", Personas.nombre);
                                cmd2.Parameters.AddWithValue("@FECHA", Personas.fecha);
                                cmd2.Parameters.AddWithValue("@FECHA_CREACION", Personas.fechacreacion);
                                cmd2.Parameters.AddWithValue("@NOTIFICACION_2", Personas.notificacion2);

                                cmd2.ExecuteNonQuery();

                                transaction.Commit();
                                conexion.Close();

                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                conexion.Close();

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        conexion.Close();
                    }
                }
                catch(Exception ex)
                {

                }
            }
        }

        public void Agregar_RTU(E_PERSONAS_RTU Personas)
        {
            SqlDataReader reader;
           
            E_PERSONAS_RTU per = new E_PERSONAS_RTU();
            String sql;
            int Retorno = 0;
            int activo = 0;

            while(activo == 0)
            {
                try
                {
                    conexion.Open();
                    activo = 1;
                    SqlCommand cmd = conexion.CreateCommand();

                    try
                    {
                        sql = "select DPI, NIT,  FECHA_NACIMIENTO from salud.personas_rtu WHERE nit =@NIT;";

                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@NIT", Personas.nit);
                        reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            per.dpi = reader.GetString(0);
                            Retorno = 1;
                        }
                        conexion.Close();
                        if (per.dpi == null)
                        {
                            conexion.Open();
                            SqlCommand cmd2 = conexion.CreateCommand();
                            SqlTransaction transaction;
                            transaction = conexion.BeginTransaction();
                            cmd2.Connection = conexion;
                            cmd2.Transaction = transaction;
                            try
                            {
                                sql = "insert into salud.personas_RTU (NIT, DPI, PRIMER_NOMBRE, SEGUNDO_NOMBRE, PRIMER_APELLIDO, SEGUNDO_APELLIDO, APELLIDO_CASADA, ";
                                sql = sql + "CEDULA, FECHA_NACIMIENTO, SEXO, NACIONALIDAD, ESTADO_CIVIL, NOMBRE_COMERCIAL, NUMERO_ESTABLECIMIENTO, ESTADO_ESTABLECIMIENTO, ULTIMA_ACTUALIZACION, ";
                                sql = sql + "FECHA_COLEGIADO, PROFESION, COLEGIADO_PROFESIONALES, SECTOR_ECONOMICO, NUMERO_COLEGIADO, FECHA_VENCIMIENTO, FECHA_INICIO_OPERACIONES ) ";
                                sql = sql + " VALUES( @NIT, @DPI, @PRIMER_NOMBRE, @SEGUNDO_NOMBRE, @PRIMER_APELLIDO, @SEGUNDO_APELLIDO, @APELLIDO_CASADA,";
                                sql = sql + " @CEDULA, @FECHA_NACIMIENTO, @SEXO, @NACIONALIDAD, @ESTADO_CIVIL, @NOMBRE_COMERCIAL, @NUMERO_ESTABLECIMIENTO, @ESTADO_ESTABLECIMIENTO, @ULTIMA_ACTUALIZACION,";
                                sql = sql + " @FECHA_COLEGIADO, @PROFESION, @COLEGIADO_PROFESIONALES, @SECTOR_ECONOMICO, @NUMERO_COLEGIADO, @FECHA_VENCIMIENTO, @FECHA_INICIO_OPERACIONES); ";

                                cmd2.CommandText = sql;

                                cmd2.Parameters.AddWithValue("@NIT", Personas.nit);
                                cmd2.Parameters.AddWithValue("@DPI", Personas.dpi);
                                cmd2.Parameters.AddWithValue("@PRIMER_NOMBRE", Personas.primer_nombre);
                                if (Personas.segundo_nombre == null)
                                {
                                    cmd2.Parameters.AddWithValue("@SEGUNDO_NOMBRE", DBNull.Value);
                                }
                                else
                                {
                                    cmd2.Parameters.AddWithValue("@SEGUNDO_NOMBRE", Personas.segundo_nombre);
                                }
                                cmd2.Parameters.AddWithValue("@PRIMER_APELLIDO", Personas.primer_apellido);
                                if (Personas.segundo_apellido == null)
                                {
                                    cmd2.Parameters.AddWithValue("@SEGUNDO_APELLIDO", DBNull.Value);
                                }
                                else
                                {
                                    cmd2.Parameters.AddWithValue("@SEGUNDO_APELLIDO", Personas.segundo_apellido);
                                }
                                if (Personas.apellido_casada == null)
                                {
                                    cmd2.Parameters.AddWithValue("@APELLIDO_CASADA", DBNull.Value);
                                }
                                else
                                {
                                    cmd2.Parameters.AddWithValue("@APELLIDO_CASADA", Personas.apellido_casada);
                                }
                                if (Personas.cedula == null)
                                {
                                    cmd2.Parameters.AddWithValue("@CEDULA", DBNull.Value);
                                }
                                else
                                {
                                    cmd2.Parameters.AddWithValue("@CEDULA", Personas.cedula);
                                }
                                cmd2.Parameters.AddWithValue("@FECHA_NACIMIENTO", Personas.fecha);
                                cmd2.Parameters.AddWithValue("@SEXO", Personas.sexo);
                                cmd2.Parameters.AddWithValue("@NACIONALIDAD", Personas.nacionalidad);
                                cmd2.Parameters.AddWithValue("@ESTADO_CIVIL", Personas.estado_civil);
                                if (Personas.nombre_comercial == null)
                                {
                                    cmd2.Parameters.AddWithValue("@NOMBRE_COMERCIAL", DBNull.Value);
                                }
                                else
                                {
                                    cmd2.Parameters.AddWithValue("@NOMBRE_COMERCIAL", Personas.nombre_comercial);
                                }
                                cmd2.Parameters.AddWithValue("@NUMERO_ESTABLECIMIENTO", Personas.numero_establecimiento);
                                if (Personas.estado_establecimiento == null)
                                {
                                    cmd2.Parameters.AddWithValue("@ESTADO_ESTABLECIMIENTO", DBNull.Value);
                                }
                                else
                                {
                                    cmd2.Parameters.AddWithValue("@ESTADO_ESTABLECIMIENTO", Personas.estado_establecimiento);
                                }
                                if (Personas.ultima_actualizacion.ToString("dd/MM/yyyy") == "01/01/0001")
                                {
                                    cmd2.Parameters.AddWithValue("@ULTIMA_ACTUALIZACION", DBNull.Value);
                                }
                                else
                                {
                                    cmd2.Parameters.AddWithValue("@ULTIMA_ACTUALIZACION", Personas.ultima_actualizacion);
                                }
                                //----------------------------------------------------------------
                                if (Personas.Fecha_colegiado.ToString("dd/MM/yyyy") == "01/01/0001")
                                {
                                    cmd2.Parameters.AddWithValue("@FECHA_COLEGIADO", DBNull.Value);
                                }
                                else
                                {
                                    cmd2.Parameters.AddWithValue("@FECHA_COLEGIADO", Personas.Fecha_colegiado);
                                }
                                if (Personas.profesion == null)
                                {
                                    cmd2.Parameters.AddWithValue("@PROFESION", DBNull.Value);
                                }
                                else
                                {
                                    cmd2.Parameters.AddWithValue("@PROFESION", Personas.profesion);
                                }
                                if (Personas.colagiado_profesionales == null)
                                {
                                    cmd2.Parameters.AddWithValue("@COLEGIADO_PROFESIONALES", DBNull.Value);
                                }
                                else
                                {
                                    cmd2.Parameters.AddWithValue("@COLEGIADO_PROFESIONALES", Personas.colagiado_profesionales);
                                }
                                if (Personas.sector_economico == null)
                                {
                                    cmd2.Parameters.AddWithValue("@SECTOR_ECONOMICO", DBNull.Value);
                                }
                                else
                                {
                                    cmd2.Parameters.AddWithValue("@SECTOR_ECONOMICO", Personas.sector_economico);
                                }
                                cmd2.Parameters.AddWithValue("@NUMERO_COLEGIADO", Personas.numero_colegiado);
                                if (Personas.fecha_vencimiento.ToString("dd/MM/yyyy") == "01/01/0001")
                                {
                                    cmd2.Parameters.AddWithValue("@FECHA_VENCIMIENTO", DBNull.Value);
                                }
                                else
                                {
                                    cmd2.Parameters.AddWithValue("@FECHA_VENCIMIENTO", Personas.fecha_vencimiento);
                                }
                                if (Personas.fecha_inicio_operaciones.ToString("dd/MM/yyyy") == "01/01/0001")
                                {
                                    cmd2.Parameters.AddWithValue("@FECHA_INICIO_OPERACIONES", DBNull.Value);
                                }
                                else
                                {
                                    cmd2.Parameters.AddWithValue("@FECHA_INICIO_OPERACIONES", Personas.fecha_inicio_operaciones);
                                }


                                cmd2.ExecuteNonQuery();
                                transaction.Commit();
                                conexion.Close();

                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                conexion.Close();

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        conexion.Close();
                    }
                }
                catch(Exception ex)
                {

                }
            }
        }
    }

}

